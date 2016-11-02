using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace L_System
{
    [System.Serializable]
    public class SystemGenerator
    {
        private string m_inputString;
        public float m_sizeValue = 15f;
        public float m_sizeGrowth = -1.5f;
        public float m_angleValue = 90f;
        public float m_angleGrowth = 0f;
        public Dictionary<char, string> m_rules;
        [SerializeField]
        public Node[,] m_nodes;
        private Stack<SystemState> m_states;
        private SystemState m_state;
        public int m_SysWidth = 80;
        public int m_SysHeight = 80;

        public string Input
        {
            set
            {
                m_inputString = value;
            }
        }



        public SystemGenerator()
        {
            m_rules = new Dictionary<char, string>();
            m_states = new Stack<SystemState>();
            m_nodes = new Node[m_SysWidth, m_SysHeight];
            for (int i = 0; i < m_SysWidth; i++)
            {
                for (int j = 0; j < m_SysHeight; j++)
                {
                    m_nodes[i, j] = new Node();
                }
            }

            m_rules.Add('L', "|-S!L!Y");
            m_rules.Add('S', "[F[FF-YS]F)G]+");
            m_rules.Add('Y', "--[F-)<F-FG]-");
            m_rules.Add('G', "FGF[Y+>F]+Y");
        }


        public string IterateLiterals(string inputStr)
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

    public void Generate()
        {
            m_state = new SystemState();
            m_state.direction = 0;
            m_state.angle = m_angleValue;
            m_state.size = m_sizeValue;
            m_state.gridPos = new Vector2(15, 15);
            m_inputString = "GYLS";
            m_inputString = IterateLiterals(m_inputString);
            foreach (char ch in m_inputString)
            {
                switch (ch)
                {
                    case 'F':
                        float newX = m_state.gridPos.x + m_state.size * Mathf.Cos(m_state.direction * Mathf.PI / 180);
                        float newY = m_state.gridPos.y + m_state.size * Mathf.Sin(m_state.direction * Mathf.PI / 180);

                        Debug.Log(newX + " " + newY);
                        if (newX >= m_SysWidth || newX < 0 || newY >= m_SysHeight || newY < 0)
                            break;

                        m_nodes[Mathf.RoundToInt(m_state.gridPos.x), Mathf.RoundToInt(m_state.gridPos.y)].use = true;
                        m_nodes[Mathf.RoundToInt(newX), Mathf.RoundToInt(newY)].use = true;


                        m_state.gridPos.x = newX;
                        m_state.gridPos.y = newY;
                        break;
                    case '+':
                        m_state.direction += m_state.angle;
                        break;
                    case '-':
                        m_state.direction-= m_state.angle;
                        break;
                    case '>':
                        m_state.size *= (1 - m_sizeGrowth);
                        break;
                    case '<':
                        m_state.size *= (1 + m_sizeGrowth);
                        break;
                    case ')':
                        m_state.angle *= (1 + m_angleGrowth);
                        break;
                    case '(':
                        m_state.angle *= (1 - m_angleGrowth);
                        break;
                    case '[':
                        m_states.Push(m_state.Clone());
                        break;
                    case ']':
                        m_state = m_states.Pop();
                        break;
                    case '!':
                        m_state.angle *= -1;
                        break;
                    case '|':
                        m_state.direction += 180;
                        break;
                }
            }

            Debug.Log(m_nodes.Length);
        }

        public void Assemble(Transform root,GameObject newChild)
        {
            for (int x = 0; x < m_SysWidth; x++)
            {
                for (int y = 0; y < m_SysHeight; y++)
                {
                    if (m_nodes[x, y].use)
                    {
                        newChild.transform.position = new Vector3(x, y, 0);
                        newChild.transform.rotation = Quaternion.identity;
                        newChild.transform.parent = root;
                        newChild.name = "Node ID"+"["+x+" "+y+"]";
                    }
                }
            }
        }
    }


}
