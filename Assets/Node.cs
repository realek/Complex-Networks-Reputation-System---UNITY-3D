[System.Serializable]
public class Node<T>
{
    private int m_id;
    private int m_degree;
    private T m_nodeData;

    public T Data
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

    public Node(int id)
    {
        m_id = id;
    }

    public Node(int id,T nodeData)
    {
        m_id = id;
        m_nodeData = nodeData;
    }
}
