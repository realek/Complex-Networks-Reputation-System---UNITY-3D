using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayNetwork : MonoBehaviour {

    public World world;
    public int currentDisplayFaction = 0;
    public int currentDisplaySettlement = 0;
    public bool displaySettlementsNetwork;
    public bool displayLocalSettlementNetwork;
    public bool displayNpcFactionNetwork;



    private void OnDrawGizmos()
    {
        
        if (world != null)
        {
            

            if (world.settlementNetwork != null)
            {
                currentDisplaySettlement = Mathf.Clamp(currentDisplaySettlement, 0, world.settlementNetwork.Nodes.Count - 1);
                if (displayLocalSettlementNetwork)
                {
                    Gizmos.color = Color.green;
                    var connections = world.settlementNetwork.Nodes[currentDisplaySettlement].data.localNPCNetwork.Connections;

                    for (int i = 0; i < connections.Count; i++)
                    {
                        Gizmos.DrawLine(connections[i].First.data.transform.position,
                            connections[i].Second.data.transform.position);
                    }
                }
                if (displaySettlementsNetwork)
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
            if (world.factionMembershipNetwork != null && displayNpcFactionNetwork)
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
