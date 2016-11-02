using UnityEngine;
using System.Collections;

public class SystemRunner : MonoBehaviour {

    [SerializeField]
    private L_System.SystemGenerator m_system;
    public GameObject prefab;
	// Use this for initialization
	void Start () {

        m_system = new L_System.SystemGenerator();
        m_system.Generate();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            m_system.Assemble(gameObject.transform, Instantiate(prefab));
        }
	
	}
}
