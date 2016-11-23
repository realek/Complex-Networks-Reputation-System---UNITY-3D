[System.Serializable]
public class Node<T>
{
    private int m_id;
    private int m_degree;
    private T m_nodeData;

    public T data
    {
        get
        {
            return m_nodeData;
        }
    }

    public int ID
    {
        get
        {
            return m_id;
        }
    }

    public void LoadData(T data)
    {
        m_nodeData = data;
    }

    public Node(int id,int degree)
    {
        m_id = id;
        m_degree = degree;
    }

    public Node(int id,int degree,T nodeData)
    {
        m_id = id;
        m_degree = degree;
        m_nodeData = nodeData;
    }
}
