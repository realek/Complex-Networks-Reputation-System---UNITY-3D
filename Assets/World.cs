﻿using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    [SerializeField] private Network<NpcEntry> m_network;


    //Generate position test func
    public NpcEntry GenerateNpcEntry()
    {
        return new NpcEntry() { alive = true, name = "trollface" + Random.value + Random.value,
            position = new Vector3(Random.Range(-1000.0f, 2000.0f), Random.Range(-4000.0f, 1000.0f), Random.Range(-500.0f, 1500.0f)) };
    }

    // Use this for initialization
    void Start ()
    {
        NetworkGenerator<NpcEntry> generator = new NetworkGenerator<NpcEntry>();
        generator.LoadNodeLinkCondition(() => {
            if(true) // if distance is whatever
                return true;
        });

        NpcEntry[] npcs = new NpcEntry[2000];
        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i] = GenerateNpcEntry();
        }
        m_network = generator.Startup(NetworkModel.Barabasi_Albert, GenerateNpcEntry(), GenerateNpcEntry())

            .MultipleStepNetwork(npcs);


    }

    private void DrawNetwork(Network<NpcEntry> network)
    {
        foreach(Connection<NpcEntry> con in network.Connections)
        {
            Debug.DrawLine(con.First.data.position, con.Second.data.position);
        }
    }

    // Update is called once per frame
	void Update ()
    {
        DrawNetwork(m_network);
      //  Debug.DrawLine(gameObject.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z+50),Color.red);
	}
}
