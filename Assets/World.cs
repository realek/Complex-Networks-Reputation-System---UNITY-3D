using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    [SerializeField] private Network<NpcEntry> m_network;
	// Use this for initialization
	void Start ()
    {
        NetworkGenerator<NpcEntry> generator = new NetworkGenerator<NpcEntry>();
        generator.LoadNodeLinkCondition(() => {
            if(true) // if distance is whatever
                return true;
        });

        m_network = generator.Startup(NetworkModel.Barabasi_Albert)
            .MultipleStepNetwork(10)
            .StepNetwork()
            .StepNetwork()
            .StepNetwork();

	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawLine(gameObject.transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z+50),Color.red);
	}
}
