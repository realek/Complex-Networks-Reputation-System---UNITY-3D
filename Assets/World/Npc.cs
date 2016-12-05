using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Morality
{
    None = 0,
    TrueNeutral = 1,
    NeutralGood = 2,
    LawfulGood = 3,
    LawfulNeutral = 4,
    NeutralEvil = 5,
    LawfulEvil = 6,
    ChaoticGood = 7,
    ChaoticEvil = 8,
    ChaoticNeutral = 9
}

public enum Race
{
    None = 0, 
    Human = 1,
    Elf = 2 ,
    Undead = 3,
    Goblin = 4,
    Orc = 5,
    HalfElf = 6,
    Dwarf = 7,
    Troll = 8,
    Gnome = 9
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
    public Faction faction
    {
        get
        {
            return m_faction;
        }
    }
    private Dictionary<Npc, NpcRelationship> m_npcRelationShips;
    private void SetName(string firstName, string lastName)
    {
        m_firstName = firstName;
        m_lastName = lastName;
    }
    public void SetFaction(Faction fac)
    {
        m_faction = fac;
    }

    public void GenerateSelf(Race race, Morality morals)
    {
        m_npcRelationShips = new Dictionary<Npc, NpcRelationship>();
        m_race = race;
        m_morality = morals;
        SetName("FIRSTNAME "+race.ToString() + GetInstanceID(), "LASTNAME "+race.ToString() + GetInstanceID());
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
    /// Indoctrinate the npc, thus changing its morality
    /// </summary>
    /// <param name="morals"></param>
    public void Indoctrinate(Morality morals)
    {
        m_morality = morals;
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
