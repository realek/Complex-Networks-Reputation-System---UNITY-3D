using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayNetwork : MonoBehaviour {

    public QuestUI questUI;
    public World world;
    public int currentDisplayFaction = 0;
    public int currentDisplaySettlement = 0;
    public int currentDisplayQuest = 0;
    private bool m_displaySettlementsNetwork;
    private bool m_displayLocalSettlementNetwork;
    private bool m_displayNpcFactionNetwork;

    private static Material s_LineMaterial;
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
        if (world.quests.generatedQuests != null)
        {
            if (world.quests.generatedQuests.Count - 1 > currentDisplayQuest)
                currentDisplayQuest++;
            questUI.DisplayQuest(world.quests.generatedQuests[currentDisplayQuest]);
        }
        
    }
    public void PreviousQuest()
    {
        if (world.quests.generatedQuests != null)
        {
            if (currentDisplayQuest > 1)
                currentDisplayQuest--;
            questUI.DisplayQuest(world.quests.generatedQuests[currentDisplayQuest]);
        }
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
                questUI.DisplayQuest(world.quests.generatedQuests[currentDisplayQuest]);
            }
    }

    private void Start()
    {
        if (world != null && world.quests.generatedQuests.Count > 0)
            questUI.DisplayQuest(world.quests.generatedQuests[currentDisplayQuest]);

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

    //Draw Object
    private void OnRenderObject()
    {
        if (world != null)
        {
            //create line render shader
            if (s_LineMaterial == null)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                s_LineMaterial = new Material(shader);
                s_LineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                s_LineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                s_LineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                s_LineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                s_LineMaterial.SetInt("_ZWrite", 0);
            }

            s_LineMaterial.SetPass(0);
            //Push world matrix
            GL.PushMatrix();



            GL.Begin(GL.LINES);
            if (world.settlementNetwork != null)
            {
                currentDisplaySettlement = Mathf.Clamp(currentDisplaySettlement, 0, world.settlementNetwork.Nodes.Count - 1);
                if (m_displayLocalSettlementNetwork)
                {
                    var connections = world.settlementNetwork.Nodes[currentDisplaySettlement].data.localNPCNetwork.Connections;

                    for (int i = 0; i < connections.Count; i++)
                    {
                        GL.Color(Color.green);
                        GL.Vertex3(connections[i].First.data.transform.position.x, connections[i].First.data.transform.position.y, connections[i].First.data.transform.position.z);
                        GL.Vertex3(connections[i].Second.data.transform.position.x, connections[i].Second.data.transform.position.y, connections[i].Second.data.transform.position.z);
                    }
                }
                if (m_displaySettlementsNetwork)
                {

                    for (int i = 0; i < world.settlementNetwork.Connections.Count; i++)
                    {
                        GL.Color(Color.red);
                        GL.Vertex3(world.settlementNetwork.Connections[i].First.data.transform.position.x, world.settlementNetwork.Connections[i].First.data.transform.position.y, world.settlementNetwork.Connections[i].First.data.transform.position.z);
                        GL.Vertex3(world.settlementNetwork.Connections[i].Second.data.transform.position.x, world.settlementNetwork.Connections[i].Second.data.transform.position.y, world.settlementNetwork.Connections[i].Second.data.transform.position.z);
                    }
                }

            }
            currentDisplayFaction = Mathf.Clamp(currentDisplayFaction, 0, world.factions.Count - 1);
            if (world.factionMembershipNetwork != null && m_displayNpcFactionNetwork)
            {
                var network = world.factionMembershipNetwork[world.factions[currentDisplayFaction]];
                if (network == null)
                    return;

                for (int j = 0; j < network.Connections.Count; j++)
                {
                    GL.Color(Color.yellow);
                    GL.Vertex3(network.Connections[j].First.data.transform.position.x, network.Connections[j].First.data.transform.position.y, network.Connections[j].First.data.transform.position.z);
                    GL.Vertex3(network.Connections[j].Second.data.transform.position.x, network.Connections[j].Second.data.transform.position.y, network.Connections[j].Second.data.transform.position.z);
                }
            }






            //End line draw
            GL.End();
            //pop matrix
            GL.PopMatrix();
        }

    }
}
