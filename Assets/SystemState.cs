using UnityEngine;
using System.Collections;


namespace L_System
{
    public class SystemState
    {
        public float size;
        public float angle;
        public float direction;
        public Vector2 gridPos;

        public SystemState()
        {
            
        }

        public SystemState Clone()
        {
            return (SystemState)MemberwiseClone();
        }


    }
}
