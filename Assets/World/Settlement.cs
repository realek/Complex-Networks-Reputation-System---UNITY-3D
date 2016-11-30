using UnityEngine;
using System.Collections.Generic;

public enum SettlementCategory
{
    Outpost = 5,
    Village = 15,
    Hamlet = 25,
    Stronghold = 50,
    City = 100

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
    private List<Npc> m_inhabitants;
    private static Color s_settlementColor = new Color(0, 0.5f, 0, 0.5f);
    private static int defaultcubesperPlane = 10;

    private void Awake()
    {
        m_inhabitants = new List<Npc>();
        int count = (int)m_category;
        for (int i = 0; i < count; i++)
        {
            m_inhabitants.Add(((GameObject)Instantiate(npcPrefab, gameObject.transform.GetChild(0), true)).GetComponent<Npc>());
            m_inhabitants[m_inhabitants.Count - 1]
                .GenerateSelf(m_populations[Random.Range(0,m_populations.Count)],(Morality)Random.Range(1,9));

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = s_settlementColor;
        Gizmos.DrawCube(transform.position, transform.localScale * defaultcubesperPlane);
    }


}
