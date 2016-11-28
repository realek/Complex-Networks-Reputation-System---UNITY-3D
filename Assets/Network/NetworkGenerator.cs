using UnityEngine;
using System.Collections.Generic;
using System;

public enum NetworkModel
{
    None,
    Barabasi_Albert,
    ER
}

public class NetworkGenerator<T> {

    private NetworkModel m_currentModel;
    private static Network<T> m_storedNetwork;
    private Func<T,T,bool> linkCondition;
    private int m_maxLinksOnAdd;
    private const int DEFAULT_MAX_LINKS_ON_ADD = 5;
    private const float DEFAULT_ER_LINK_PROBABILITY = 0.5f;
    private int degSumOfConnected;

    public NetworkGenerator()
    {
        m_maxLinksOnAdd = DEFAULT_MAX_LINKS_ON_ADD;
        m_storedNetwork = new Network<T>();
    }

    public NetworkGenerator(int maxLinksOnNodeAdd)
    {
        m_maxLinksOnAdd = maxLinksOnNodeAdd;
       
    }

    public void LoadNodeLinkCondition(Func<T,T,bool> nodeLinkCondition)
    {
        linkCondition = nodeLinkCondition;
    }

    public NetworkGenerator<T> Startup(NetworkModel model,T fnData, T snData)
    {
        m_storedNetwork = new Network<T>();
        m_currentModel = model;
        StartModel(fnData, snData);
        return this;

    }

    private void StartModel(T fnData,T snData)
    {
       int fId =  m_storedNetwork.AddNode(fnData);
       int sId = m_storedNetwork.AddNode(snData); 
        //hardcode first & second node degree to 1
        m_storedNetwork.GetNodeByID(fId).degree = 1;
        m_storedNetwork.GetNodeByID(sId).degree = 1;
        m_storedNetwork.AddConnection(fId, sId);
        if (m_currentModel == NetworkModel.Barabasi_Albert)
            m_storedNetwork.AddConnection(fId, sId);
        else if (m_currentModel == NetworkModel.ER)
            if (UnityEngine.Random.value < DEFAULT_ER_LINK_PROBABILITY)
                m_storedNetwork.AddConnection(fId, sId);


    }

    private int ComputeConnectedSum()
    {
        int sum = 0;
        foreach(Node<T> node in m_storedNetwork.Nodes)
        {
            sum += 2*node.degree;
        }

        return sum;
    }

    private void StepNetworkModel(T newNodeData)
    {
        int nId = m_storedNetwork.AddNode(newNodeData);

        if (m_currentModel == NetworkModel.ER)
            if (UnityEngine.Random.value > DEFAULT_ER_LINK_PROBABILITY)
            {
                m_storedNetwork.GetNodeByID(nId).degree = 0; // no connections for this node
                return;
            }


        int links = m_maxLinksOnAdd;
        links = Mathf.Min(links, m_storedNetwork.Nodes.Count);

        List<int> linktargets = FindLinkTargets(links);

        foreach(int i in linktargets)
        {
            m_storedNetwork.AddConnection(nId, i);
            m_storedNetwork.GetNodeByID(i).degree += 1;
        }

        m_storedNetwork.GetNodeByID(nId).degree = links;

    }

    private List<int> FindLinkTargets(int links)
    {
        List<int> ret = new List<int>();
        int availableDegree = ComputeConnectedSum();
        for (int i = 0; i < links;i++)
        {
            if (availableDegree <= 0)
                break;
            int rand = UnityEngine.Random.Range(0, availableDegree);
            int id = 0;
            int its = 0;
            while (its <= rand)
            {
                if (id == m_storedNetwork.Nodes.Count)
                    break;
                if (!ret.Contains(id))
                    its += m_storedNetwork.GetNodeByID(id).degree;
                id++;

            }
            id--;
            ret.Add(id);
            availableDegree -= m_storedNetwork.GetNodeByID(id).degree;
        }
        return ret;
    }


    public NetworkGenerator<T> MultipleStepNetwork(params T[] newNodesData)
    {
        for (int i = 0; i < newNodesData.Length; i++)
            StepNetwork(newNodesData[i]);
        return this;
    }

    public NetworkGenerator<T> StepNetwork(T newNodeData)
    {
        StepNetworkModel(newNodeData);
        return this;
    }

    public static implicit operator Network<T>(NetworkGenerator<T> generator)
    {
        Network<T> ret = m_storedNetwork;
        m_storedNetwork = null;
        return ret;
    }


}
