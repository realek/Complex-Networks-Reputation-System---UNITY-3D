using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Network<T>
{
    public string name;
    private int m_baseID = 0;
    List<Node<T>> m_nodes;
    List<Connection<T>> m_connections;
    private Func<T, T, bool> m_linkCondition = null;
    public List<Node<T>> Nodes
    {
        get
        {
            return m_nodes;
        }
    }


    public void SetLinkCondition(Func<T, T, bool> condition)
    {
        m_linkCondition = condition;
    }

    public List<Connection<T>> Connections
    {
        get
        {
            return m_connections;
        }
    }

    public Network()
    {
        m_nodes = new List<Node<T>>();
        m_connections = new List<Connection<T>>();
    }

    public Network(string name)
    {
        this.name = name;
        m_nodes = new List<Node<T>>();
        m_connections = new List<Connection<T>>();
    }

    public int AddNode(T nodeData)
    {
        m_nodes.Add(new Node<T>(m_baseID,nodeData));
        m_baseID++;
        return m_nodes.Count - 1;
    }

    public Node<T> GetNodeByID(int nodeID)
    {
        int id = m_nodes.FindIndex(node => node.ID == nodeID);
        if (id == -1)
            return null;
        else return m_nodes[id];
    }

    public void AddConnection(int firstNodeId, int secondNodeId)
    {
       int fIdx = m_nodes.FindIndex(node => node.ID == firstNodeId);
        if (fIdx == -1)
            throw new System.Exception("Node with id: "+firstNodeId+" Was not found");
        int SIdx = m_nodes.FindIndex(node => node.ID == secondNodeId);
        if (SIdx == -1)
            throw new System.Exception("Node with id: " + secondNodeId + " Was not found");
        if(m_linkCondition!=null)
            if (!m_linkCondition(m_nodes[fIdx].data, m_nodes[SIdx].data))
                return;
        Connection<T> con = new Connection<T>(m_nodes[fIdx], m_nodes[SIdx]);
        if (!m_connections.Contains(con))
            m_connections.Add(con);
        else
            Debug.Log("Connection between nodes with ids "+firstNodeId+" and "+secondNodeId+" already exists");
    }

}
