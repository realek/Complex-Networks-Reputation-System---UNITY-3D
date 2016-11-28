using UnityEngine;
using System.Collections;


public enum Morality
{
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

    public void SetName(string firstName, string lastName)
    {
        m_firstName = firstName;
        m_lastName = lastName;
    }

    public void Die()
    {
        m_alive = false;
    }
}
