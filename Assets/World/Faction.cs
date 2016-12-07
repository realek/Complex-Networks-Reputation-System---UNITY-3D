using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum FactionState
{
    Operating,
    Expanding,
    Destroyed
}

[System.Serializable]
public class Faction
{
    public string name;
    public FactionState state;
    public List<Morality> allowedAlignments
    {
        get
        {
            return m_AllowedAlignments;
        }
    }
    [SerializeField]
    private List<Morality> m_AllowedAlignments;
    [SerializeField]
    private List<Race> m_AllowedRaces;
    [SerializeField]
    private List<Npc> m_members;
    public List<Npc> members
    {
        get
        {
            return m_members;
        }
    }

    private Dictionary<Faction, Relationship> m_factionRep;

    public Faction()
    {
        state = FactionState.Operating;
        m_members = new List<Npc>();
        m_factionRep = new Dictionary<Faction, Relationship>();
    }

    public void SetRelationship(Faction target, Relationship relationship)
    {
        if (!m_factionRep.ContainsKey(target))
            m_factionRep.Add(target, relationship);
        else
            m_factionRep[target] = relationship;
    }

    private bool Evaluate(Npc potentialMember)
    {
        if (m_AllowedAlignments.Contains(potentialMember.morality) && m_AllowedRaces.Contains(potentialMember.race))
            return true;
        else
            return false;
    }
    public bool AddMember(Npc newMember)
    {
        if (Evaluate(newMember))
            m_members.Add(newMember);
        else
            return false;

        return true;
    }

    public bool RemoveMember(Npc member)
    {
       return m_members.Remove(member);
    }

    public void UpdateMemberStatus()
    {
        for (int i = 0; i < m_members.Count; i++)
            if (!m_members[i].isAlive)
                m_members.Remove(m_members[i]);
    }

    /// <summary>
    /// returns 1 if friendly
    /// return 0  if neutral
    /// returns -1 if hostile
    /// </summary>
    /// <param name="fac"></param>
    /// <returns></returns>
    public int CreateReputationTowards(Faction fac)
    {
        int alignments = 0;
        int races = 0;

        for (int i = 0; i < fac.m_AllowedAlignments.Count; i++)
        {
            if (m_AllowedAlignments.Contains(fac.m_AllowedAlignments[i]))
                alignments++;
            else
                alignments--;
        }

        for (int i = 0; i < fac.m_AllowedRaces.Count; i++)
        {
            if (m_AllowedRaces.Contains(fac.m_AllowedRaces[i]))
                races++;
            else
                races--;
        }

        int rating = alignments + races;
        if(rating > 0)
            return 1;
        else if(rating == 0)
            return 0;
        else
            return -1;
    }

    public int GetReputationAsInt(Faction fac)
    {
        
        Relationship rep = Relationship.None;
        if (fac == this)
            rep = Relationship.Friendly;
        else if (m_factionRep.TryGetValue(fac, out rep) == false)
            throw new System.Exception("Unable to get faction rep");
        return (int)rep;
    }

    public Relationship GetReputation(Faction fac)
    {
        Relationship rep = Relationship.None;
        if (fac == this)
            rep = Relationship.Friendly;
        else if (m_factionRep.TryGetValue(fac, out rep) == false)
            throw new System.Exception("Unable to get faction rep");
        return rep;
    }

    public bool HasRep(Faction fac)
    {
        return m_factionRep.ContainsKey(fac);
    }
}


