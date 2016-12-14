using UnityEngine;
using System;
using System.Collections.Generic;

public enum QuestTarget
{
    Bandit,
    Guard,
    Skeleton,
    Spider,
    Wolves,
    Rabits,
    Poachers,
    Cultists,
    NamedNpc
}

public enum QuestItem
{
    Provisions,
    Gold,
    Trinkets,
    Potions,
    Documents,
    Herbs,
    Weapon,
    Armor,
    Equipment,
    Metals,
    Jewels
}

public enum QuestObjectiveType
{
    Kill,
    Collect,
    Deliver
}

[Serializable]
public class Quest {

    /// <summary>
    /// Base quest objective class
    /// </summary>
    private abstract class QuestObjective
    {
        public abstract bool isComplete();
        public abstract void UpdateObjective();
        public abstract void ForceComplete();
    }

    /// <summary>
    /// Derived kill objective class
    /// </summary>
    [Serializable]
    private sealed class QuestObjectiveKill : QuestObjective
    {
        private int m_cKillCount = 0;
        private int m_neededKillCount;
        private QuestTarget m_targetType;
        private Npc[] m_targets;
        public Npc[] targets
        {
            get
            {
                return m_targets;
            }
        }
        public QuestObjectiveKill(int neededKills, QuestTarget questTargetType, params Npc[] targets)
        {
            m_neededKillCount = neededKills;
            m_targetType = questTargetType;
            m_targets = targets;
        }

        public override bool isComplete()
        {
            return m_cKillCount == m_neededKillCount;
        }

        public override void UpdateObjective()
        {
            if(m_cKillCount < m_neededKillCount)
                m_cKillCount++;
        }

        public override string ToString()
        {
            string targetNames = "";
            for (int i = 0; i < m_targets.Length; i++)
            {
                targetNames += m_targets[i].name + " ";
            }
            return "Kill the following targets : " + targetNames;
        }

        public override void ForceComplete()
        {
            m_cKillCount = m_neededKillCount;
        }
    }
    /// <summary>
    /// derived collect objective class
    /// </summary>
    [Serializable]
    private sealed class QuestObjectiveCollect : QuestObjective
    {
        private int m_cCollectCount = 0;
        private int m_neededCollectCount;

        private QuestItem[] m_collectables;

        public QuestObjectiveCollect(int neededCollectables, params QuestItem[] collectables)
        {
            m_neededCollectCount = neededCollectables;
            m_collectables = collectables;
        }

        public override bool isComplete()
        {
            return m_cCollectCount == m_neededCollectCount;
        }

        public override void UpdateObjective()
        {
            if (m_cCollectCount < m_neededCollectCount)
                m_cCollectCount++;
        }

        public override string ToString()
        {
            string targetNames = "";
            for (int i = 0; i < m_collectables.Length; i++)
            {
                targetNames += m_collectables[i].ToString() + " ";
            }
            return "Collect the following items : " + targetNames;
        }

        public override void ForceComplete()
        {
            m_cCollectCount = m_neededCollectCount;
        }
    }
    /// <summary>
    /// derived deliver objective class
    /// </summary>
    [Serializable]
    private sealed class QuestObjectiveDeliver : QuestObjective
    {
        private bool m_delivered = false;
        private QuestItem m_deliverables;
        private Npc m_deliverTarget;
        public Npc deliverTarget
        {
            get
            {
                return m_deliverTarget;
            }
        }

        public QuestObjectiveDeliver(QuestItem item, Npc deliverTarget)
        {
            m_deliverables = item;
        }

        public override bool isComplete()
        {
            return m_delivered;
        }

        public override void UpdateObjective()
        {
            m_delivered = true;
        }

        public override string ToString()
        {
            return "Deliver the following item : " + m_deliverables.ToString();
        }

        public override void ForceComplete()
        {
            m_delivered = true;
        }
    }

    public Npc QuestGiver
    {
        get
        {
            return m_questStarter;
        }
    }

    public Npc QuestEnder
    {
        get
        {
            return m_questReturn;
        }
    }

    [SerializeField]
    private Npc m_questStarter;
    [SerializeField]
    private Npc m_questReturn;

    [SerializeField]
    private bool m_started;

    public string name
    {
        get
        {
            return m_name;
        }
    }
    [SerializeField]
    private string m_name;
    public string description
    {
        get
        {
            return m_description;
        }
    }

    [SerializeField]
    private string m_description;
    private Dictionary<QuestObjectiveType, QuestObjective> m_objectives;
    [SerializeField]
    private Quest m_nextChainQuest;

    public Quest(string name, Npc questStarter, Quest nextQuest = null)
    {
        m_objectives = new Dictionary<QuestObjectiveType, QuestObjective>();
        m_nextChainQuest = nextQuest;
        m_started = false;
        m_name = name;
        m_questStarter = questStarter;
    }

    /// <summary>
    /// Add kill objective to quest, one objective of this type per quest
    /// </summary>
    /// <param name="count"></param>
    /// <param name="targetType"></param>
    /// <param name="targets"></param>
    public void AddKillObjective(int count,QuestTarget targetType,params Npc[] targets)
    {
        m_objectives.Add(QuestObjectiveType.Kill, new QuestObjectiveKill(count, targetType,targets));
        m_description += m_objectives[QuestObjectiveType.Kill].ToString();
    }

    /// <summary>
    /// add collect objective to quest, one objective of this type per quest
    /// </summary>
    /// <param name="count"></param>
    /// <param name="items"></param>
    public void AddCollectObjective(int count, params QuestItem[] items)
    {
        m_objectives.Add(QuestObjectiveType.Collect, new QuestObjectiveCollect(count, items));
        m_description += m_objectives[QuestObjectiveType.Collect].ToString();
    }

    /// <summary>
    /// add deliver objective to quest, one objective of this type per quest.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="deliverTarget"></param>
    public void AddDeliverObjective(QuestItem item, Npc deliverTarget)
    {
        m_objectives.Add(QuestObjectiveType.Deliver, new QuestObjectiveDeliver(item,deliverTarget));
        m_description += m_objectives[QuestObjectiveType.Deliver].ToString();
    }

    /// <summary>
    /// Set quest return/handin npc, can be the same as the quest giver or another npc
    /// </summary>
    /// <param name="questReturn"></param>
    public void SetQuestReturn(Npc questReturn)
    {
        m_questReturn = questReturn;
    }

    /// <summary>
    /// Complete kill objective and return kill targets for network deletion
    /// </summary>
    /// <returns></returns>
    public Npc[] CompleteKillQuestObjective()
    {
        QuestObjective obj;
        if(m_objectives.TryGetValue(QuestObjectiveType.Kill, out obj))
        {
            QuestObjectiveKill killObj = (QuestObjectiveKill)obj;
            killObj.ForceComplete();
            return killObj.targets;
        }
        return null;

    }

    /// <summary>
    /// returns true if collect objective was found and completed
    /// </summary>
    /// <returns></returns>
    public bool CompleteQuestCollectObjective()
    {
        QuestObjective obj;
        if (m_objectives.TryGetValue(QuestObjectiveType.Collect, out obj))
        {
            QuestObjectiveCollect cObj = (QuestObjectiveCollect)obj;
            cObj.ForceComplete();
            return true;
        }
        return false;
    }

    /// <summary>
    /// returns the target npc for the completed deliver objective, returns null if no objective was found
    /// </summary>
    /// <returns></returns>
    public Npc GetQuestDeliverObjective()
    {
        QuestObjective obj;
        if (m_objectives.TryGetValue(QuestObjectiveType.Deliver, out obj))
        {
            QuestObjectiveDeliver dObj = (QuestObjectiveDeliver)obj;
            dObj.ForceComplete();
            return dObj.deliverTarget;
        }
        return null;
    }

    /// <summary>
    /// Returns the current deliver objective target
    /// </summary>
    /// <returns></returns>
    public Npc GetDeliverObjectiveTarget()
    {
        QuestObjective obj;
        if (m_objectives.TryGetValue(QuestObjectiveType.Deliver, out obj))
        {
            QuestObjectiveDeliver dObj = (QuestObjectiveDeliver)obj;
            return dObj.deliverTarget;
        }
        return null;
    }

    /// <summary>
    /// returns the current kill objective targets
    /// </summary>
    /// <returns></returns>
    public Npc[] GetKillObjectiveTargets()
    {
        QuestObjective obj;
        if (m_objectives.TryGetValue(QuestObjectiveType.Kill, out obj))
        {
            QuestObjectiveKill killObj = (QuestObjectiveKill)obj;
            return killObj.targets;
        }
        return null;
    }

    public bool Start()
    {
        if (!m_started)
        {
            m_started = true;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Retruns true if all objectives within the quest are completed
    /// </summary>
    /// <returns></returns>
    public bool Completed()
    {
        foreach (KeyValuePair<QuestObjectiveType, QuestObjective> pair in m_objectives)
        {
            if (!pair.Value.isComplete())
            {
                return false;
            }
        }
        return true;
    }
}
