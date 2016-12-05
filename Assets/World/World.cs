using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    [SerializeField]
    private string settlementTag;
    [SerializeField]
    private List<Faction> m_allFactions;
    public List<Faction> factions
    {
        get
        {
            return m_allFactions;
        }
    }
    [SerializeField]
    private int m_independentID = 0;
    public int independentID
    {
        get
        {
            return m_independentID;
        }
    }
    Dictionary<Faction,Network<Npc>> m_factionsNetwork;
    Network<Settlement> m_settlementNetwork;
    private static World m_instance;
    public static World instance
    {
        get
        {
            return m_instance;
        }
    }
    // Use this for initialization
    void Awake ()
    {
        //Ruberband singleton
        if (m_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            m_instance = this;
    }

    private void Start()
    {
        NetworkGenerator<Settlement> settlementGen = new NetworkGenerator<Settlement>();
        NetworkGenerator<Npc> npcFactionNetwork = new NetworkGenerator<Npc>();

        ///Settlements link only if they have the same faction
        settlementGen.LoadNodeLinkCondition((Settlement first, Settlement second) =>
        {
            if (first.controllingfaction == second.controllingfaction)
                return true;
            else if (first.controllingfaction == m_allFactions[m_independentID] || second.controllingfaction == m_allFactions[m_independentID])
                return true;
            else
                return false;
        });
        List<Settlement> settlements = new List<Settlement>(FindObjectsOfType<Settlement>());

        //init settlements - work around to circumvent the need of a faction serialized class
        for (int i = 0; i < settlements.Count; i++)
            settlements[i].Init();


        Settlement[] nsettlements = new Settlement[2];
        nsettlements[0] = settlements[0];
        nsettlements[1] = settlements[1];
        settlements.RemoveAt(0);
        settlements.RemoveAt(0);
        m_settlementNetwork = settlementGen.Startup(NetworkModel.Barabasi_Albert, nsettlements[0], nsettlements[1])
            .MultipleStepNetwork(settlements.ToArray());

        ///Create faction networks
        m_factionsNetwork = new Dictionary<Faction, Network<Npc>>();
        for (int i = 0; i < m_allFactions.Count; i++)
        {
            Npc[] npcs = null;
            Network<Npc> network = null;
            if (m_allFactions[i].members.Count == 0)
            {
                npcs = new Npc[m_allFactions[i].members.Count];
                m_factionsNetwork.Add(m_allFactions[i], network);
                continue;
            }

            npcs = new Npc[m_allFactions[i].members.Count - 2];
            m_allFactions[i].members.CopyTo(2, npcs, 0, npcs.Length);
            network = npcFactionNetwork
                 .Startup(NetworkModel.Barabasi_Albert, m_allFactions[i].members[0], m_allFactions[i].members[1])
                  .MultipleStepNetwork(npcs);
            m_factionsNetwork.Add(m_allFactions[i], network);
        }
    }

    private void OnDrawGizmos()
    {

        if (m_settlementNetwork != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < m_settlementNetwork.Connections.Count; i++)
            {
                Gizmos.DrawLine(m_settlementNetwork.Connections[i].First.data.transform.position,
                    m_settlementNetwork.Connections[i].Second.data.transform.position);
            }
        }

        if (m_factionsNetwork != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < m_allFactions.Count; i++)
            {
                var network = m_factionsNetwork[m_allFactions[i]];
                if (network == null)
                    continue;
                for (int j = 0; j < network.Connections.Count; j++)
                {
                    Gizmos.DrawLine(network.Connections[j].First.data.transform.position,
                        network.Connections[j].Second.data.transform.position);
                }
            }
        }

    }

}
