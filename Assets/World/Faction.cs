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

public class Faction
{
    public string name;
    public FactionState state;
    public Morality factionMorality
    {
        get
        {
            return m_factionMorality;
        }
    }
    private Morality m_factionMorality;
    private List<Npc> m_members;
    private Dictionary<Faction, FactionRelationship> m_factionRep;

    public Faction(Morality morality)
    {
        m_factionMorality = morality;
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

    public void AddMember(Npc newMember)
    {
        m_members.Add(newMember);
    }

    public void RemoveMember(Npc member)
    {
        m_members.Remove(member);
    }

    public void UpdateMemberStatus()
    {
        for (int i = 0; i < m_members.Count; i++)
            if (!m_members[i].isAlive)
                m_members.Remove(m_members[i]);
    }


}


