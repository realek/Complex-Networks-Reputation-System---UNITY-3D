using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Morality
{
    None,
    TrueNeutral,
    NeutralGood,
    LawfulGood,
    LawfulNeutral,
    NeutralEvil,
    LawfulEvil,
    ChaoticGood,
    ChaoticEvil
}

public enum Race
{
    None,
    Human,
    Elf,
    Undead,
    Goblin,
    Orc,
    HalfElf,
    Dwarf,
    Troll,
    Gnome
}

public enum NpcRelationship
{
    None,
    Friendly,
    Neutral,
    Hostile
}

public class Npc : MonoBehaviour
{
    public string fName
    {
        get
        {
            return m_firstName;
        }
    }
    public string lName
    {
        get
        {
            return m_lastName;
        }
    }
    public bool isAlive
    {
        get
        {
            return m_alive;
        }
    }

    [SerializeField]
    private string m_firstName;
    [SerializeField]
    private string m_lastName;
    private bool m_alive = true;

    public Race race
    {
        get
        {
            return m_race;
        }
    }

    public Morality morality
    {
        get
        {
            return m_morality;
        }
    }

    [SerializeField]
    private Race m_race;
    [SerializeField]
    private Morality m_morality;
    [SerializeField]
    private Faction m_faction;
    private Dictionary<Npc, NpcRelationship> m_npcRelationShips;
    public void SetName(string firstName, string lastName)
    {
        m_firstName = firstName;
        m_lastName = lastName;
    }
    public void SetFaction(Faction fac)
    {
        m_faction = fac;
    }

    /// <summary>
    /// If npc doesnt exist in the relationship database it will be added
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="relationship"></param>
    public void SetRelationshipWithNpc(Npc npc, NpcRelationship relationship)
    {
        NpcRelationship rel;
        if (m_npcRelationShips.TryGetValue(npc, out rel))
            m_npcRelationShips[npc] = relationship;
        else
            m_npcRelationShips.Add(npc, relationship);
    }

    /// <summary>
    /// Returns none if the provided npc is not known, otherwise returns the current relationship between the npcs
    /// </summary>
    public NpcRelationship GetRelationship(Npc npc)
    {
        NpcRelationship rel;
        if (m_npcRelationShips.TryGetValue(npc, out rel))
            return rel;
        else
            return NpcRelationship.None;
    }
    public void Die()
    {
        m_alive = false;
    }
}
