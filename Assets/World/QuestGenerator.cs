using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuestManager {

    [SerializeField]
    List<Quest> m_generatedQuests;
    public List<Quest> generatedQuests
    {
        get
        {
            return m_generatedQuests;
        }
    }
    public static float QUEST_GENERATION_RATE = 0.05f; //chance to make a quest
    public QuestManager()
    {
        m_generatedQuests = new List<Quest>();
    }
    /// <summary>
    /// Generate a quest based on the provided node connection
    /// </summary>
    /// <param name="nodeConnection">Node connection between two npcs</param>
    public void GenerateKillQuest(Connection<Npc> nodeConnection)
    {
        Quest nQ = new Quest("Random Kill Quest #"+1+((Random.value+Random.value)/Random.value*Random.value), nodeConnection.First.data);
        nQ.SetQuestReturn(nodeConnection.First.data);
        nQ.AddKillObjective(1, QuestTarget.NamedNpc, nodeConnection.Second.data);
        m_generatedQuests.Add(nQ);
    }

    /// <summary>
    /// Generate a quest where you will deliver an random item to the destination
    /// </summary>
    /// <param name="nodeConnection"></param>
    public void GenerateDeliverQuest(Connection<Npc> nodeConnection)
    {
        Quest nQ = new Quest("Random Deliver Quest #" + 1 + ((Random.value + Random.value) / Random.value * Random.value), nodeConnection.First.data);
        nQ.AddDeliverObjective((QuestItem)Random.Range(0, 11),nodeConnection.Second.data);
        m_generatedQuests.Add(nQ);
    }

    /// <summary>
    /// Generate a quest where you will collect provisions for the quest giver
    /// </summary>
    /// <param name="nodeConnection"></param>
    public void GenerateCollectQuest(Connection<Npc> nodeConnection)
    {
        Quest nQ = new Quest("Random Collect Quest #" + 1 + ((Random.value + Random.value) / Random.value * Random.value), nodeConnection.First.data);
        nQ.SetQuestReturn(nodeConnection.Second.data);
        nQ.AddCollectObjective(25, QuestItem.Provisions);
        m_generatedQuests.Add(nQ);

    }

    /// <summary>
    /// Returns a list of quests where the npc is involved
    /// </summary>
    /// <param name="npc"></param>
    /// <returns></returns>
    public List<Quest> IsNpcInvolvedInQuest(Npc npc)
    {
        List<Quest> foundQ = new List<Quest>();
        for (int i = 0; i < m_generatedQuests.Count; i++)
        {
            if (m_generatedQuests[i].QuestEnder || m_generatedQuests[i].QuestEnder)
            {
                foundQ.Add(m_generatedQuests[i]);
                continue;
            }
            var kNpcs = m_generatedQuests[i].GetKillObjectiveTargets();
            var dNpc = m_generatedQuests[i].GetQuestDeliverObjective();
            if (kNpcs != null)
            {
                foreach(Npc n in kNpcs)
                    if(n==npc)
                    {
                        foundQ.Add(m_generatedQuests[i]);
                        continue;
                    }
            }

            if(dNpc !=null)
                if(dNpc == npc)
                    foundQ.Add(m_generatedQuests[i]);
        }

        return foundQ;

    }
}
