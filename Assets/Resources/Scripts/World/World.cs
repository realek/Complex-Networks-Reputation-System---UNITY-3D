using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

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
    public Dictionary<Faction,Network<Npc>> factionMembershipNetwork
    {
        get
        {
            return m_factionsNetwork;
        }
    }
    Network<Settlement> m_settlementNetwork;
    public Network<Settlement> settlementNetwork
    {
        get
        {
            return m_settlementNetwork;
        }
    }

    [SerializeField]
    private QuestManager m_quests;
    public QuestManager quests
    {
        get
        {
            return m_quests;
        }
    }
    private static World m_instance;
    public static World instance
    {
        get
        {
            return m_instance;
        }
    }

    public void HndQuestCmpl(Quest questToBeCompleted, out bool questsRemoved)
    {
        var x = questToBeCompleted.CompleteKillQuestObjective();
        questsRemoved = false;
        if (x != null)
        {
            //handle kill objective
            for (int i = 0; i < x.Length; i++)
            {
                var net = factionMembershipNetwork[x[i].faction];
                for (int j = 0; j < net.Connections.Count; j++)
                    if(net.Connections[j].First.data == x[i] || net.Connections[j].Second.data == x[i])
                        net.Connections.Remove(net.Connections[j]);
                for(int j = 0; j < net.Nodes.Count; j++)
                {
                    if (net.Nodes[j].data == x[i])
                    {
                        net.Nodes.Remove(net.Nodes[j]);
                        break;
                    }
                }
                for(int j = 0; j < settlementNetwork.Nodes.Count; j++)
                {
                    var settlement = settlementNetwork.Nodes[j].data;
                    if (!settlement.inhabitands.Contains(x[i]))
                        continue;
                    for (int k = 0; k < settlement.localNPCNetwork.Connections.Count; k++)
                        if (settlement.localNPCNetwork.Connections[k].First.data == x[i] ||
                            settlement.localNPCNetwork.Connections[k].Second.data == x[i])
                            settlement.localNPCNetwork.Connections.Remove(settlement.localNPCNetwork.Connections[k]);
                    
                    for(int k = 0; k < settlement.localNPCNetwork.Nodes.Count; k++)
                        if(settlement.localNPCNetwork.Nodes[k].data == x[i])
                        {
                            settlement.localNPCNetwork.Nodes.Remove(settlement.localNPCNetwork.Nodes[k]);
                            break;
                        }

                }

                var quests = m_quests.IsNpcInvolvedInQuest(x[i]);
                for(int j = 0; j < quests.Count;j++)
                {
                        m_quests.generatedQuests.Remove(quests[i]);
                }
                quests.Clear();
                Destroy(x[i].gameObject);
            }
            questsRemoved = true;
            return;
        }

        if (questToBeCompleted.CompleteQuestCollectObjective())
        {
            m_quests.generatedQuests.Remove(questToBeCompleted);
            return;
        }
        var y = questToBeCompleted.GetQuestDeliverObjective();
        if(y!=null)
        {
            m_quests.generatedQuests.Remove(questToBeCompleted);
            //handle deliver target, no reputation change done at the current moment

            return;
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

        for (int i = 0; i < m_allFactions.Count; i++)
        {
            for (int j = 0; j < m_allFactions.Count; j++)
            {
                if (i == j || m_allFactions[i].HasRep(m_allFactions[j]))
                    continue;
                int rep = 0;
                rep += m_allFactions[i].CreateReputationTowards(m_allFactions[j]);
                rep += m_allFactions[j].CreateReputationTowards(m_allFactions[i]);
                rep = Mathf.Clamp(rep, -1, 1); // clamp so that value is 1 , 0 , -1
                m_allFactions[i].SetRelationship(m_allFactions[j], (Relationship)rep);
                m_allFactions[j].SetRelationship(m_allFactions[i], (Relationship)rep);

            }
        }

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

        //Set npc relationships within each faction
        for (int i = 0; i < m_allFactions.Count; i++)
        {

            if (m_factionsNetwork[m_allFactions[i]] == null)
                continue;
            var conn = m_factionsNetwork[m_allFactions[i]].Connections;

            for (int j = 0; j < conn.Count; j++)
            {
                if (conn[j].First.data.GetRelationship(conn[j].Second.data) != Relationship.None)
                    continue;

                int rep = conn[i].First.data.CreateRelationshipTowards(conn[i].Second.data) + 1; //since they are both the same faction +1 rep
                rep = Mathf.Clamp(rep, -1, 1);
                conn[i].First.data.SetRelationshipWithNpc(conn[i].Second.data, (Relationship)rep);
                conn[i].Second.data.SetRelationshipWithNpc(conn[i].First.data, (Relationship)rep);
            }
        }

        //Generate Quests based on settlements
        m_quests = new QuestManager();

        for (int i = 0; i < settlements.Count; i++)
        {

            var conn = settlements[i].localNPCNetwork.Connections;
            for (int j = 0; j < conn.Count; j++)
            {
                if (Random.value <= QuestManager.QUEST_GENERATION_RATE)
                {
                    var rel = conn[j].First.data.GetRelationship(conn[j].Second.data);
                    switch (rel)
                    {
                        case Relationship.Friendly:
                            int ch = Random.Range(0, 2);
                            switch (ch)
                            {
                                case 0:  //make deliver quest
                                    m_quests.GenerateDeliverQuest(conn[j]);
                                    break;

                                case 1: // make collect quest
                                    m_quests.GenerateCollectQuest(conn[j]);
                                    break;
                            }
                            break;

                        case Relationship.Hostile:
                            m_quests.GenerateKillQuest(conn[j]);
                            break;

                        case Relationship.Neutral:
                            ch = Random.Range(0, 3);

                            switch (ch)
                            {
                                case 0: //deliver
                                    m_quests.GenerateDeliverQuest(conn[j]);
                                    break;

                                case 1: //collect
                                    m_quests.GenerateCollectQuest(conn[j]);
                                    break;
                                case 2: //kill
                                    m_quests.GenerateKillQuest(conn[j]);
                                    break;
                            }

                            break;
                    }
                }
            }
        }



    }
}
