
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector3 position;
    public Vector3 angle;
    public float length;

    public Node(Vector3 point, Vector3 angle, float len)
    {
        this.position = point;
        this.angle = angle;
        length = len;
    }
}
