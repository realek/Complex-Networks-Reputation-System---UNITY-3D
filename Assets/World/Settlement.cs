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
    public List<Npc> inhabitands
    {
        get
        {
            return m_inhabitants;
        }
    }
    private static Color s_settlementColor = new Color(0, 0.5f, 0, 0.5f);
    private static int defaultcubesperPlane = 10;

    private void Awake()
    {
        m_inhabitants = new List<Npc>();
        int count = (int)m_category;
        Vector3 extents = GetComponent<Collider>().bounds.extents;

        ///No time to make this look pretty....
        for (int i = 0; i < count; i++)
        {
            int tryCount = 25;
            Vector3 nPOs = Vector3.zero;
            bool found = false;
            while (tryCount > 0)
            {
                if (m_inhabitants.Count == 0)
                {
                    nPOs = GeneratePoint(extents);
                    found = true;
                    break;
                }
                for (int j = 0; j < m_inhabitants.Count; j++)
                {
                    nPOs = GeneratePoint(extents);
                    if (!m_inhabitants[j].GetComponentInChildren<Collider>().bounds.Contains(nPOs))
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
                tryCount--;
            }

            if (!found)
                continue;
            m_inhabitants.Add(((GameObject)Instantiate(npcPrefab,nPOs,Quaternion.identity)).GetComponent<Npc>());
            m_inhabitants[m_inhabitants.Count - 1].transform.SetParent(gameObject.transform.GetChild(0));
            m_inhabitants[m_inhabitants.Count - 1]
                .GenerateSelf(m_populations[Random.Range(0,m_populations.Count)],(Morality)Random.Range(1,10));

        }
    }

    private Vector3 GeneratePoint(Vector3 extents)
    {
        Vector3 lowerLeft = transform.position - extents;
        Vector3 upperRight = transform.position + extents;

        return new Vector3(Random.Range(lowerLeft.x, upperRight.x), transform.position.y, Random.Range(lowerLeft.z, upperRight.z));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = s_settlementColor;
        Gizmos.DrawCube(transform.position, transform.localScale * defaultcubesperPlane);
    }


}
