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

public enum Relationship
{
    None = -2,
    Friendly = 1,
    Neutral = 0,
    Hostile = -1
}

public class Npc : MonoBehaviour
{
    public static readonly List<Morality> Neutrals = new List<Morality>()
    { Morality.TrueNeutral,Morality.ChaoticNeutral,Morality.LawfulNeutral };
    public static readonly List<Morality> Goods = new List<Morality>()
    { Morality.LawfulGood, Morality.NeutralGood, Morality.ChaoticGood };
    public static readonly List<Morality> Evils = new List<Morality>()
    { Morality.LawfulEvil, Morality.NeutralEvil, Morality.ChaoticEvil };

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
    private Dictionary<Npc, Relationship> m_npcRelationShips;
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
        m_npcRelationShips = new Dictionary<Npc, Relationship>();
        m_race = race;
        m_morality = morals;
        SetName("FIRSTNAME "+race.ToString() + GetInstanceID(), "LASTNAME "+race.ToString() + GetInstanceID());
    }
    /// <summary>
    /// If npc doesnt exist in the relationship database it will be added
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="relationship"></param>
    public void SetRelationshipWithNpc(Npc npc, Relationship relationship)
    {
        Relationship rel;
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
    public Relationship GetRelationship(Npc npc)
    {
        Relationship rel;
        if (m_npcRelationShips.TryGetValue(npc, out rel))
            return rel;
        else
            return Relationship.None;
    }
    /// <summary>
    /// returns 1 if friendly
    /// returns 0 if neutral
    /// returns -1 if hostile
    /// </summary>
    /// <param name="npc"></param>
    /// <returns></returns>
    public int CreateRelationshipTowards(Npc npc)
    {
        if (m_morality == npc.morality)
            return 1;

        if ((Goods.Contains(m_morality) && Goods.Contains(npc.morality)) ||
            (Neutrals.Contains(m_morality) && Neutrals.Contains(npc.morality)) ||
            (Evils.Contains(m_morality) && Evils.Contains(npc.morality)))
            return 1;

        if (Goods.Contains(m_morality) && Neutrals.Contains(npc.morality) ||
            Goods.Contains(npc.morality) && Neutrals.Contains(m_morality) ||
            Evils.Contains(m_morality) && Neutrals.Contains(npc.morality) ||
            Evils.Contains(npc.morality) && Neutrals.Contains(m_morality))
            return 0;

        if (Evils.Contains(m_morality) && Goods.Contains(npc.morality) ||
            Evils.Contains(npc.morality) && Goods.Contains(m_morality))
            return -1;

        throw new System.Exception("Morality not set on either npc");
    }

    public void Die()
    {
        m_alive = false;
    }
}
