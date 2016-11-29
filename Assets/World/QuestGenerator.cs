using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager {
    List<Quest> m_quests;

    public QuestManager()
    {
        m_quests = new List<Quest>();
    }
    /// <summary>
    /// Generate a quest based on the provided node connection
    /// </summary>
    /// <param name="nodeConnection">Node connection between two npcs</param>
    public void GenerateKillQuest(Connection<Npc> nodeConnection)
    {
        Quest nQ = new Quest(QuestType.Kill,"Random Kill Quest #"+Random.value, nodeConnection.First.data.gameObject, nodeConnection.First.data.gameObject);
        nQ.AddKillObjective(1, QuestTarget.NamedNpc, nodeConnection.Second.data.gameObject);
    }

}
