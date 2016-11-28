﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum QuestType
{
    Kill,
    Collect,
    Deliver,
    Chain
}

public enum QuestTarget
{
    Bandit,
    Guard,
    Skeleton,
    Spider,
    Wolves,
    Rabits,
    Poachers,
    Cultists
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
        private QuestTarget[] m_targetTypes;
        public QuestObjectiveKill(int neededKills, params QuestTarget[] questTargets)
        {
            m_neededKillCount = neededKills;
            m_targetTypes = questTargets;
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



    public QuestType type
    {
        get
        {
            return m_type;
        }
    }
    private QuestType m_type;

    public GameObject QuestGiver
    {
        get
        {
            return m_questStarter;
        }
    }

    public GameObject QuestEnder
    {
        get
        {
            return m_questReturn;
        }
    }

    private GameObject m_questStarter;
    private GameObject m_questReturn;

    private bool m_started;
    private bool m_isComplete;

    public string name
    {
        get
        {
            return m_name;
        }
    }
    private string m_name;
    private Dictionary<Type,QuestObjective> m_objectives;
    private Quest m_nextChainQuest;

    public Quest(QuestType type, string name, GameObject questStarter, GameObject questReturn, Quest nextQuest = null)
    {
        m_objectives = new Dictionary<Type, QuestObjective>();
        m_nextChainQuest = nextQuest;
        m_started = false;
        m_type = type;
        m_name = name;
        m_questStarter = questStarter;
        m_questReturn = questReturn;
    }

    public void AddKillObjective(int count, params QuestTarget[] targets)
    {
        m_objectives.Add(typeof(QuestObjectiveKill), new QuestObjectiveKill(count, targets));
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