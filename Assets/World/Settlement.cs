using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField]
    private List<Race> m_populations;
    [HideInInspector]
    public Faction controllingfaction;
    public GameObject npcPrefab;
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

    private static Color s_settlementColor = new Color(0, 0.5f, 0, 0.5f);
    private static int defaultcubesperPlane = 10;

    private void OnDrawGizmos()
    {
        Gizmos.color = s_settlementColor;
        Gizmos.DrawCube(transform.position, transform.localScale * defaultcubesperPlane);
    }


}
