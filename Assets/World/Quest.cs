using UnityEngine;
using System.Collections;
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

[System.Serializable]
public class Quest {

    private abstract class QuestObjective
    {
        public abstract bool isComplete();
        public abstract void UpdateObjective();
    }

    private sealed class QuestObjectiveKill : QuestObjective
    {
        private int m_cKillCount = 0;
        private int m_neededKillCount;
        private QuestTarget m_targetType;
        private GameObject[] m_targets;
        public QuestObjectiveKill(int neededKills, QuestTarget questTargetType, params GameObject[] targets)
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
    }

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
    }

    private sealed class QuestObjectiveDeliver : QuestObjective
    {
        private bool m_delivered = false;
        private QuestItem m_deliverables;

        public QuestObjectiveDeliver(QuestItem item)
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
    [SerializeField]
    private bool m_isComplete;

    public string name
    {
        get
        {
            return m_name;
        }
    }
    [SerializeField]
    private string m_name;
    private Dictionary<Type,QuestObjective> m_objectives;
    [SerializeField]
    private Quest m_nextChainQuest;

    public Quest(string name, Npc questStarter, Npc questReturn, Quest nextQuest = null)
    {
        m_objectives = new Dictionary<Type, QuestObjective>();
        m_nextChainQuest = nextQuest;
        m_started = false;
        m_name = name;
        m_questStarter = questStarter;
        m_questReturn = questReturn;
    }

    public void AddKillObjective(int count,QuestTarget targetType,params GameObject[] targets)
    {
        m_objectives.Add(typeof(QuestObjectiveKill), new QuestObjectiveKill(count, targetType,targets));
    }

    public void AddCollectObjective(int count, params QuestItem[] items)
    {
        m_objectives.Add(typeof(QuestObjectiveCollect), new QuestObjectiveCollect(count, items));
    }

    public void AddDeliverObjective(QuestItem item)
    {
        m_objectives.Add(typeof(QuestObjectiveDeliver), new QuestObjectiveDeliver(item));
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

    public bool Completed()
    {
        foreach (KeyValuePair<Type, QuestObjective> pair in m_objectives)
        {
            if (!pair.Value.isComplete())
            {
                return false;
            }
        }
        return true;
    }
}
