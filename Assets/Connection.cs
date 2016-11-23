[System.Serializable]
public class Connection<T> {

    private Node<T> m_first;
    private Node<T> m_second;

    public Node<T> First
    {
        get
        {
            return m_first;
        }
    }
    public Node<T> Second
    {
        get
        {
            return m_second;
        }
    }

    public Connection(Node<T> firstNode, Node<T> secondNode)
    {
        m_first = firstNode;
        m_second = secondNode;
    }


}
