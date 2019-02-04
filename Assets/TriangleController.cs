using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Nagand
{
    public class TriangleController : MonoBehaviour
    {
        TriangleProperties properties;
        
        public void SetProperties(TriangleProperties p)
        {
            properties = p;
        }
        public TriangleProperties GetProperties()
        {
            return properties;
        }

    }
}
