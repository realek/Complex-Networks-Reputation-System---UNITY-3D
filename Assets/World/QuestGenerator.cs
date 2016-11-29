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
    /// <typeparam name="Npc">Npc class passed as restricted type</typeparam>
    /// <param name="nodeConnection">Node connection between two npcs</param>
    public void GenerateQuest<Npc>(Connection<Npc> nodeConnection)
    {

    }

}
