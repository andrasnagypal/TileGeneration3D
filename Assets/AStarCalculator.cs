using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nagand
{
    public class AStarCalculator : MonoBehaviour
    {
        ROAD[] RoadForAStar;
        PLAINTRIANGLE[] TrianglesForAStar;
        List<int> IDNumberForTriangles = new List<int>();
        int rnd;
        AStarNode[] NodesToCalculcate;
        AStarPath[] PathToWalk;

        bool IsAvailableForCalc = true;

        private void Update()
        {
            AStarCalc();
        }

        private void AStarCalc()
        {
            if (Input.GetKey(KeyCode.Space) && IsAvailableForCalc)
            {
                IsAvailableForCalc = !IsAvailableForCalc;
                Debug.Log("Valami");
                RoadForAStar = GetComponent<RoadsContainer>().RoadsOfTheGame;
                TrianglesForAStar = GetComponent<TrianglesContainer>().PlainTriangles;
                rnd = UnityEngine.Random.Range(0, RoadForAStar.Length);
                NodesToCalculcate = new AStarNode[RoadForAStar.Length + TrianglesForAStar.Length];
                MakingNodes();
                Debug.Log("The road " + RoadForAStar[rnd].IDNumberForRoad + " has these settlements: " + RoadForAStar[rnd].IDNumberForTriangles[0] + " + " + RoadForAStar[rnd].IDNumberForTriangles[1]);
                Debug.Log("Nodes: " + NodesToCalculcate.Length);
            }
        }

        private void MakingNodes()
        {
            for (int i = 0; i < RoadForAStar.Length; i++)
            {
                if (!IDNumberForTriangles.Contains(RoadForAStar[i].IDNumberForTriangles[0]))
                     IDNumberForTriangles.Add(RoadForAStar[i].IDNumberForTriangles[0]);
                if (!IDNumberForTriangles.Contains(RoadForAStar[i].IDNumberForTriangles[1]))
                    IDNumberForTriangles.Add(RoadForAStar[i].IDNumberForTriangles[1]);

                AStarNode newNode;
                newNode.IDNumberForPossiblePaths = RoadForAStar[i].IDNumberForTriangles;
                newNode.GCost = RoadForAStar[i].LevelOfTheRoad;
                newNode.HCost = 0;
                NodesToCalculcate[i]=newNode;
                Debug.Log(newNode);
            }
            //for (int i = RoadForAStar.Length; i < RoadForAStar.Length+ TrianglesForAStar.Length; i++)
            //{
            //    AStarNode newNode;
            //    newNode.IDNumberForPossiblePaths = new int[3]
            //    {
            //        TrianglesForAStar[IDNumberForTriangles[i- RoadForAStar.Length]].RoadsToTheSettlement [0],
            //        TrianglesForAStar[IDNumberForTriangles[i- RoadForAStar.Length]].RoadsToTheSettlement [1],
            //        TrianglesForAStar[IDNumberForTriangles[i- RoadForAStar.Length]].RoadsToTheSettlement [2]
            //    };
            //    newNode.GCost = 6;
            //    newNode.HCost = 0;
            //    NodesToCalculcate[i] = newNode;
            //}
        }
    }
}
