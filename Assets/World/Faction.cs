using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum FactionState
{
    Operating,
    Expanding,
    Destroyed
}

public enum FactionRelationship
{
    Allied,
    Subservient,
    War,
    Neutral
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

    private Dictionary<Faction, FactionRelationship> m_factionRep;

    public Faction()
    {
        state = FactionState.Operating;
        m_members = new List<Npc>();
        m_factionRep = new Dictionary<Faction, FactionRelationship>();
    }

    public void SetRelationship(Faction target, FactionRelationship relationship)
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


}


