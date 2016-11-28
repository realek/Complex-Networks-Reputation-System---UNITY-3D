using UnityEngine;
using System.Collections;


public enum SettlementCategory
{
    Outpost,
    Village,
    Hamlet,
    Stronghold,
    City

}

public enum SettlementStatus
{
    Occupied,
    Deserted,
    Destroyed
}
public class Settlement : MonoBehaviour {

    [SerializeField]
    private string m_name;
    [SerializeField]
    private SettlementCategory m_category;
    [SerializeField]
    private SettlementStatus m_status;
    private Faction m_controllingfaction;
    private bool m_controlled;

    public bool independent
    {
        get
        {
            return !m_controlled;
        }
    }
    public SettlementCategory category
    {
        get
        {
            return m_category;
        }
    }
    public SettlementStatus status
    {
       get
        {
            return m_status;
        }
    }
    public void SetControllingFaction(Faction fac)
    {
        if (fac != null)
            m_controlled = false;
        else
            m_controlled = true;
        m_controllingfaction = fac;

    }


}
