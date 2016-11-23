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
    private Func<bool> linkCondition;
    private int m_maxLinksOnAdd;
    private const int DEFAULT_MAX_LINKS_ON_ADD = 3;
    private int degSumOfConnected;

    public NetworkGenerator()
    {
        m_maxLinksOnAdd = DEFAULT_MAX_LINKS_ON_ADD;
        m_storedNetwork = new Network<T>();
    }

    public NetworkGenerator(int maxLinksOnNodeAdd)
    {
        m_maxLinksOnAdd = maxLinksOnNodeAdd;
        m_storedNetwork = new Network<T>();
    }

    public void LoadNodeLinkCondition(Func<bool> nodeLinkCondition)
    {
        linkCondition = nodeLinkCondition;
    }

    public NetworkGenerator<T> Startup(NetworkModel model,T fnData, T snData)
    {
        m_currentModel = model;
        switch (model)
        {
            case NetworkModel.Barabasi_Albert:
                {
                    BarabasiAlberModel(fnData,snData);
                    break;
                }

            case NetworkModel.ER:
                {
                    ER();
                    break;
                }

        }
        return this;

    }

    private void BarabasiAlberModel(T fnData,T snData)
    {
       int fId =  m_storedNetwork.AddNode(fnData);
       int sId = m_storedNetwork.AddNode(snData); 
        //hardcode first & second node degree to 1
        m_storedNetwork.GetNodeByID(fId).degree = 1;
        m_storedNetwork.GetNodeByID(sId).degree = 1;
        m_storedNetwork.AddConnection(fId, sId);

    }

    private int ComputeConnectedSum()
    {
        int sum = 0;
        foreach(Node<T> node in m_storedNetwork.Nodes)
        {
            sum += node.degree;
        }

        return sum;
    }

    private void StepBarabasiAlberModel(T newNodeData)
    {
        int nId = m_storedNetwork.AddNode(newNodeData);
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
        int remainingLinks = ComputeConnectedSum();
        for (int i = 0; i < links;i++)
        {

            int rand = UnityEngine.Random.Range(0, remainingLinks);
            int it = 0;
            int its = 0;
            while (its <= rand)
            {
                if (it == m_storedNetwork.Nodes.Count)
                    break;
                if (!ret.Contains(it))
                    its += m_storedNetwork.GetNodeByID(it).degree;
                it++;

            }
            it--;
            ret.Add(it);
            remainingLinks -= m_storedNetwork.GetNodeByID(it).degree;
        }
        return ret;
    }

    private void ER()
    {
      
    }

    public NetworkGenerator<T> MultipleStepNetwork(params T[] newNodesData)
    {
        for (int i = 0; i < newNodesData.Length; i++)
            StepNetwork(newNodesData[i]);
        return this;
    }

    public NetworkGenerator<T> StepNetwork(T newNodeData)
    {
        switch (m_currentModel)
        {
            case NetworkModel.Barabasi_Albert:
                StepBarabasiAlberModel(newNodeData);
                break;
        }
        return this;
    }

    public static implicit operator Network<T>(NetworkGenerator<T> generator)
    {
        Network<T> ret = m_storedNetwork;
        m_storedNetwork = null;
        return ret;
    }


}
