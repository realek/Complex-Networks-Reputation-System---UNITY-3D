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

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_settlementNetwork != null)
            for (int i = 0; i < m_settlementNetwork.Connections.Count; i++)
            {
                Gizmos.DrawLine(m_settlementNetwork.Connections[i].First.data.transform.position,
                    m_settlementNetwork.Connections[i].Second.data.transform.position);
            }
    }

}
