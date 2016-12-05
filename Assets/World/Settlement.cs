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

    private const float MAX_NPC_DISTANCE = 2f;
    private const float SAME_FACTION_LINK_RATE = 0.25f;
    private const float NPC_PROXIMITY_LINK_RATE = 0.5f;

    private List<Npc> m_inhabitants;
    public List<Npc> inhabitands
    {
        get
        {
            return m_inhabitants;
        }
    }
    private Network<Npc> m_npcNetwork;
    public Network<Npc> localNPCNetwork
    {
        get
        {
            return m_npcNetwork;
        }
    }
    private static Color s_settlementColor = new Color(0, 0.5f, 0, 0.5f);
    private static int defaultcubesperPlane = 10;

    private void Start()
    {
       
    }

    public void Init()
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
            m_inhabitants.Add(((GameObject)Instantiate(npcPrefab, nPOs, Quaternion.identity)).GetComponent<Npc>());
            m_inhabitants[m_inhabitants.Count - 1].transform.SetParent(gameObject.transform.GetChild(0));
            m_inhabitants[m_inhabitants.Count - 1]
                .GenerateSelf(m_populations[Random.Range(0, m_populations.Count)], (Morality)Random.Range(1, 10));

        }
        AssignFaction();
        //create generator and load condition
        NetworkGenerator<Npc> generator = new NetworkGenerator<Npc>();
        generator.LoadNodeLinkCondition((Npc first, Npc second) =>
        {
            //if they are in the same faction there is a chance they met
            if (first.faction == second.faction && Random.value <= SAME_FACTION_LINK_RATE)
                return true;
            //if they are close enough and have the chance to meet create link
            else if (Random.value <= NPC_PROXIMITY_LINK_RATE &&
            Vector3.Distance(first.transform.position, second.transform.position) <= MAX_NPC_DISTANCE)
                return true;
            else
                return false;
        });
        //create npc network
        Npc[] ninhabitants = new Npc[m_inhabitants.Count - 2];
        // take out the first two inhabitants as they are used
        //in the startup step
        m_inhabitants.CopyTo(2, ninhabitants, 0, ninhabitants.Length);
        m_npcNetwork = generator.Startup(NetworkModel.Barabasi_Albert, m_inhabitants[0], m_inhabitants[1])
            .MultipleStepNetwork(ninhabitants);

    }

    private void AssignFaction()
    {
        Dictionary<Faction, int> m_factions = new Dictionary<Faction, int>();


        for (int i = 0; i < m_inhabitants.Count; i++)
        {
            bool factionSet = false;
            for (int j = World.instance.independentID + 1; j < World.instance.factions.Count; j++)
            {
                if (World.instance.factions[j].AddMember(m_inhabitants[i]))
                {
                    m_inhabitants[i].SetFaction(World.instance.factions[j]);

                    if (m_factions.ContainsKey(World.instance.factions[j]))
                        m_factions[World.instance.factions[j]]++;
                    else
                        m_factions.Add(World.instance.factions[j], 1);
                    factionSet = true;
                    break;
                }
            }

            if (!factionSet)
            {
                World.instance.factions[World.instance.independentID].AddMember(m_inhabitants[i]);
                m_inhabitants[i].SetFaction(World.instance.factions[World.instance.independentID]);

                if (m_factions.ContainsKey(World.instance.factions[World.instance.independentID]))
                    m_factions[World.instance.factions[World.instance.independentID]]++;
                else
                    m_factions.Add(World.instance.factions[World.instance.independentID], 1);
            }

            //set settlement faction
            int dominantFactionID = World.instance.independentID;
            int bestSize = 0;
            for (int j = 0; j < World.instance.factions.Count; j++)
            {
                if (m_factions.ContainsKey(World.instance.factions[j]))
                {
                    if (bestSize < m_factions[World.instance.factions[j]])
                    {
                        dominantFactionID = j;
                        bestSize = m_factions[World.instance.factions[j]];
                    }
                }

            }
            controllingfaction = World.instance.factions[dominantFactionID];
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
      //  Gizmos.color = s_settlementColor;
      //  Gizmos.DrawCube(transform.position, transform.localScale * defaultcubesperPlane);
        //if (m_npcNetwork != null)
        //{
        //    Gizmos.color = Color.blue;

        //    for(int i = 0; i < m_npcNetwork.Connections.Count;i++)
        //    {
        //        Gizmos.DrawLine(m_npcNetwork.Connections[i].First.data.transform.position,
        //            m_npcNetwork.Connections[i].Second.data.transform.position);
        //    }
        //    //Gizmos.DrawLine()
        //}
    }


}
