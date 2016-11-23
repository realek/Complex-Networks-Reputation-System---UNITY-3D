using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Network<T>
{
    public string name;
    private int m_baseID = 0;
    List<Node<T>> m_nodes;
    List<Connection<T>> m_connections;

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

    public void AddNode(T nodeData)
    {
        m_nodes.Add(new Node<T>(m_baseID,nodeData));
        m_baseID++;
    }

    public void AddConnection(int firstNodeId, int secondNodeId)
    {
       int fIdx = m_nodes.FindIndex(node => node.ID == firstNodeId);
        if (fIdx == -1)
            throw new System.Exception("Node with id: "+firstNodeId+" Was not found");
        int SIdx = m_nodes.FindIndex(node => node.ID == secondNodeId);
        if (SIdx == -1)
            throw new System.Exception("Node with id: " + secondNodeId + " Was not found");

        Connection<T> con = new Connection<T>(m_nodes[fIdx], m_nodes[SIdx]);
        if (!m_connections.Contains(con))
            m_connections.Add(con);
        else
            Debug.Log("Connection between nodes with ids "+firstNodeId+" and "+secondNodeId+" already exists");
    }

}
