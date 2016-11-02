using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

public class SystemRunner : MonoBehaviour {

   
    public GameObject prefab;
    private string m_input = "F";
    private string output;
    private Dictionary<char, string> m_rules;
    [SerializeField]
    private List<Node> m_nodes;
    [SerializeField]
    private List<GameObject> m_branches;
    [ReadOnly(true)]
    public string result;
	// Use this for initialization
	void Awake ()
    {
        m_nodes = new List<Node>();
        m_branches = new List<GameObject>();
        m_rules = new Dictionary<char, string>();
        output = m_input;
        AddRules();

        for (int i = 0; i < 4; i++)
        {
            output = IterateLiterals(output);

        }

        CreateStructure(output, 1.0f);

    }

    void AddRules()
    {
        m_rules.Add('F', "[FF][[-F]F[+F]][[+F]F[+F]][[+F]F[-F]]");
    }


    private string IterateLiterals(string inputStr)
    {
        StringBuilder sb = new StringBuilder();

        foreach (char ch in inputStr)
        {
            if (m_rules.ContainsKey(ch))
            {
                sb.Append(m_rules[ch]);
            }
            else
            {
                sb.Append(ch);
            }
        }

        return sb.ToString();
    }

    Vector3 pivot(Vector3 aPos, Vector3 bPos, Vector3 angles)
    {
        Vector3 dir = aPos - bPos;
        dir = Quaternion.Euler(angles) * dir;
        return (dir + bPos);
    }

    private void CreateStructure(string input, float segmentLength)
    {
        Stack<Node> values = new Stack<Node>();
        Node lN = new Node(Vector3.zero, Vector3.zero, segmentLength);
        values.Push(lN);

        foreach (char c in input)
        {
            switch (c)
            {
                case 'F': // Draw line of length lastBranchLength, in direction of lastAngle

                    m_nodes.Add(lN);
                    Node nN = new Node(lN.position + new Vector3(0, lN.length, 0), lN.angle, segmentLength);
                    nN.length = lN.length - 0.02f;
                    if (nN.length <= 0.0f)
                        nN.length = 0.001f;

                    nN.angle.y = lN.angle.y + UnityEngine.Random.Range(-30, 30);

                    nN.position = pivot(nN.position, lN.position, new Vector3(nN.angle.x, 0, 0));
                    nN.position = pivot(nN.position, lN.position, new Vector3(0, nN.angle.y, 0));

                    m_nodes.Add(nN);
                    lN = nN;
                    break;
                case '+': // Rotate +30
                    lN.angle.x += 30.0f;
                    break;
                case '[': // Save State
                    values.Push(lN);
                    break;
                case '-': // Rotate -30
                    lN.angle.x += -30.0f;
                    break;
                case ']': // Load Saved State
                    lN = values.Pop();
                    break;
            }
        }

        for (int i = 0; i < m_nodes.Count; i += 2)
        {
            CreateTreeBranch(m_nodes[i], m_nodes[i + 1], 0.1f);
        }
    }

    private void CreateTreeBranch(Node from, Node to, float radius)
    {
        GameObject branch = Instantiate(prefab);
        float length = Vector3.Distance(to.position, from.position);
        radius = radius * length;

        Vector3 scale = new Vector3(radius, length/2.0f, radius);
        branch.transform.localScale = scale;

        branch.transform.position = from.position;
        branch.transform.Rotate(to.angle);

        branch.transform.parent = transform;

        m_branches.Add(branch);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_nodes.Count; i += 2)
        {
          //  Debug.DrawLine(m_nodes[i].position, m_nodes[i + 1].position, Color.black);
        }
    }
}
