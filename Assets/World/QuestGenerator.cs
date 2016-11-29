using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuestManager {

    [SerializeField]
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
        Quest nQ = new Quest("Random Kill Quest #"+1+((Random.value+Random.value)/Random.value*Random.value), nodeConnection.First.data, nodeConnection.First.data);
        nQ.AddKillObjective(1, QuestTarget.NamedNpc, nodeConnection.Second.data.gameObject);
        m_quests.Add(nQ);
    }

    /// <summary>
    /// Generate a quest where you will deliver an random item to the destination
    /// </summary>
    /// <param name="nodeConnection"></param>
    public void GenerateDeliverQuest(Connection<Npc> nodeConnection)
    {
        Quest nQ = new Quest("Random Deliver Quest #" + 1 + ((Random.value + Random.value) / Random.value * Random.value), nodeConnection.First.data, nodeConnection.Second.data);
        nQ.AddDeliverObjective((QuestItem)Random.Range(0, 11));
        m_quests.Add(nQ);
    }

    /// <summary>
    /// Generate a quest where you will collect provisions for the quest giver
    /// </summary>
    /// <param name="nodeConnection"></param>
    public void GenerateCollectQuest(Connection<Npc> nodeConnection)
    {
        Quest nQ = new Quest("Random Collect Quest #" + 1 + ((Random.value + Random.value) / Random.value * Random.value), nodeConnection.First.data, nodeConnection.First.data);
        nQ.AddCollectObjective(25, QuestItem.Provisions);
        m_quests.Add(nQ);

    }

    /// <summary>
    /// Generates a chain quest where you will first have to collect weapons, and afterwards deliver the scavenged material from the weapons 
    /// </summary>
    /// <param name="nodeConnection"></param>
    public void GenerateChainQuest(Connection<Npc> nodeConnection)
    {
        Quest nQ = new Quest("Random Collect Quest #" + 1 + ((Random.value + Random.value) / Random.value * Random.value), nodeConnection.First.data, nodeConnection.Second.data);
        nQ.AddCollectObjective(10, QuestItem.Weapon);
        Quest nnQ = new Quest("Random Collect Quest #" + 1 + ((Random.value + Random.value) / Random.value * Random.value), nodeConnection.First.data, nodeConnection.First.data, nQ);
        nnQ.AddDeliverObjective(QuestItem.Metals);
        m_quests.Add(nQ);
    }
}
