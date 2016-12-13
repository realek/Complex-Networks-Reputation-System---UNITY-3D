using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayNetwork : MonoBehaviour {

    public World world;
    public int currentDisplayFaction = 0;
    public int currentDisplaySettlement = 0;
    public int currentDisplayQuest = 0;
    private bool m_displaySettlementsNetwork;
    private bool m_displayLocalSettlementNetwork;
    private bool m_displayNpcFactionNetwork;
    private Faction m_currentlySelectedFaction;
    private Settlement m_currentlySelectedSettlement;

    public Faction SelectedFaction
    {
        get
        {
            return m_currentlySelectedFaction;
        }
    }
    public Settlement SelectedSettlement
    {
        get
        {
            return m_currentlySelectedSettlement;
        }
    }

    public void ToggleSettlements()
    {
        m_displaySettlementsNetwork = !m_displaySettlementsNetwork;
    }

    public void ToggleInhabitants()
    {
        m_displayLocalSettlementNetwork = !m_displayLocalSettlementNetwork;
    }

    public void ToggleFactions()
    {
        m_displayNpcFactionNetwork = !m_displayNpcFactionNetwork;
    }

    public void PreviousSettlement()
    {
        currentDisplaySettlement--;
        currentDisplaySettlement = Mathf.Clamp(currentDisplaySettlement, 0, world.settlementNetwork.Nodes.Count - 1);
    }
    public void NextSettlement()
    {
        currentDisplaySettlement++;
        currentDisplaySettlement = Mathf.Clamp(currentDisplaySettlement, 0, world.settlementNetwork.Nodes.Count - 1);
    }
    public void PreviousFaction()
    {
        currentDisplayFaction--;
        currentDisplayFaction = Mathf.Clamp(currentDisplayFaction, 0, world.factions.Count - 1);
    }
    public void NextFaction()
    {
        currentDisplayFaction++;
        currentDisplayFaction = Mathf.Clamp(currentDisplayFaction, 0, world.factions.Count - 1);
    }
    public void NextQuest()
    {
        if(world.quests.generatedQuests!=null)
            if(world.quests.generatedQuests.Count-1 > currentDisplayQuest)
                currentDisplayQuest++;
        
    }
    public void PreviousQuest()
    {
        if (world.quests.generatedQuests != null)
            if (currentDisplayQuest > 1)
                currentDisplayQuest--;
    }
    public void CompleteCurrentQuest()
    {
        if (world.quests.generatedQuests != null)
            if (!world.quests.generatedQuests[currentDisplayQuest].Completed())
            {
                bool resetQuestPosition;
                world.HndQuestCmpl(world.quests.generatedQuests[currentDisplayQuest], out resetQuestPosition);
                if (resetQuestPosition)
                    currentDisplayQuest = 0;
            }
    }
    private void OnDrawGizmos()
    {
        if (world != null)
        {
            

            if (world.settlementNetwork != null)
            {
                currentDisplaySettlement = Mathf.Clamp(currentDisplaySettlement, 0, world.settlementNetwork.Nodes.Count - 1);
                if (m_displayLocalSettlementNetwork)
                {
                    Gizmos.color = Color.green;
                    var connections = world.settlementNetwork.Nodes[currentDisplaySettlement].data.localNPCNetwork.Connections;

                    for (int i = 0; i < connections.Count; i++)
                    {
                        Gizmos.DrawLine(connections[i].First.data.transform.position,
                            connections[i].Second.data.transform.position);
                    }
                }
                if (m_displaySettlementsNetwork)
                {
                    Gizmos.color = Color.red;
                    for (int i = 0; i < world.settlementNetwork.Connections.Count; i++)
                    {
                        Gizmos.DrawLine(world.settlementNetwork.Connections[i].First.data.transform.position,
                            world.settlementNetwork.Connections[i].Second.data.transform.position);
                    }
                }

            }
            currentDisplayFaction = Mathf.Clamp(currentDisplayFaction, 0, world.factions.Count - 1);
            if (world.factionMembershipNetwork != null && m_displayNpcFactionNetwork)
            {

                Gizmos.color = Color.yellow;
                var network = world.factionMembershipNetwork[world.factions[currentDisplayFaction]];
                if (network == null)
                    return;
                for (int j = 0; j < network.Connections.Count; j++)
                {
                    Gizmos.DrawLine(network.Connections[j].First.data.transform.position,
                        network.Connections[j].Second.data.transform.position);
                }
            }
        }
    }

}
