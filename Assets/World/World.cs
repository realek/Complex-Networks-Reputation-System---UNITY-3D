using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    [SerializeField]
    List<Faction> m_allFactions;
    [SerializeField]
    List<Settlement> m_allSettlements;
    Network<Faction> m_factionsInteractions;
    Network<Settlement> m_settlementsInteractions;
    Dictionary<Settlement, Network<Npc>> m_npcsInteractions;

    // Use this for initialization
    void Start ()
    {
        NetworkGenerator<Npc> npcGen = new NetworkGenerator<Npc>();
        NetworkGenerator<Settlement> settlementGen = new NetworkGenerator<Settlement>();
        NetworkGenerator<Faction> factionGen = new NetworkGenerator<Faction>();

//        generator.LoadNodeLinkCondition((Npc first,Npc second) => {
//            if (second == first) // if distance is whatever
//                return true;
//            else
//                return false;
//        });

//        Npc[] npcs = new Npc[20];

//        for (int i = 0; i < npcs.Length; i++)
//        {
//            npcs[i] = GenerateNpcEntry(firstNetwork.transform.position);
//        }
//        m_network = generator.Startup(NetworkModel.Barabasi_Albert, GenerateNpcEntry(firstNetwork.transform.position),
//            GenerateNpcEntry(firstNetwork.transform.position))

//            .MultipleStepNetwork(npcs);

//        for (int i = 0; i < npcs.Length; i++)
//        {
//            npcs[i] = GenerateNpcEntry(secondNetwork.transform.position);
//        }

//        m_network1 = generator.Startup(NetworkModel.ER, GenerateNpcEntry(secondNetwork.transform.position),
//            GenerateNpcEntry(secondNetwork.transform.position))
//            .MultipleStepNetwork(npcs);

//        colors = new Color[m_network.Connections.Count];
//        for (int i = 0; i < colors.Length; i++)
//        {
//;
//            colors[i] = new Color(Random.value, Random.value, Random.value);
//        }

//        //Testing Higher layer
//        m_mainNetwork = new Network<Network<Npc>>();
//        m_mainNetwork.AddNode(m_network);
//        m_mainNetwork.AddNode(m_network1);
//        m_mainNetwork.AddConnection(0, 1);
    }

    //private void DrawNetwork(Network<Npc> network, Network<Npc> network1)
    //{
    //    Debug.DrawLine(firstNetwork.transform.position, secondNetwork.transform.position, Color.black);

    //    for (int i = 0; i < network.Connections.Count; i++)
    //    {
    //        Debug.DrawLine(network.Connections[i].First.data.transform.position, network.Connections[i].Second.data.transform.position,colors[i]);
    //    }

    //    for (int i = 0; i < network1.Connections.Count; i++)
    //    {
            
    //        Debug.DrawLine(network1.Connections[i].First.data.transform.position, network1.Connections[i].Second.data.transform.position, colors[i]);
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    if (m_network != null && m_network1 != null)
    //    {
    //        Gizmos.color = Color.cyan;
    //        foreach (Node<Npc> node in m_network.Nodes)
    //            Gizmos.DrawSphere(node.data.transform.position, 100.0f);

    //        Gizmos.color = Color.green;
    //        foreach (Node<Npc> node in m_network1.Nodes)
    //            Gizmos.DrawSphere(node.data.transform.position, 100.0f);
    //    }

    //}

    // Update is called once per frame
    void Update ()
    {
      //  DrawNetwork(m_network,m_network1);
      //  Debug.DrawLine(gameObject.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z+50),Color.red);
	}
}
