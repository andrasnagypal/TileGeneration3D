using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagand
{
    public struct TriangleProperties
    {
        
        public List<GameObject> Neighbours;
        public string Name;
        public Vector3 Position;
    }


    public class SettlementCreator : MonoBehaviour
    {
        public GameObject RedTriangle;
        public int HowManyTriangle;

       public List<GameObject> InQueue = new List<GameObject>();
       public List<GameObject> Finished = new List<GameObject>();


        private void Awake()
        {
            
        }

        
    }
}
