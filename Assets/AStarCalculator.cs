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

        

        public void AStarCalc()
        {
            if ( IsAvailableForCalc)
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
            //-1ezni kell indexben mert kifut a listből pl. 258 idjű utsú elem a 257. indexen van!!!
            for (int i = 0; i < RoadForAStar.Length; i++)
            {
                if (!IDNumberForTriangles.Contains(RoadForAStar[i].IDNumberForTriangles[0]))
                     IDNumberForTriangles.Add(RoadForAStar[i].IDNumberForTriangles[0]);
                if (!IDNumberForTriangles.Contains(RoadForAStar[i].IDNumberForTriangles[1]))
                    IDNumberForTriangles.Add(RoadForAStar[i].IDNumberForTriangles[1]);
                Debug.Log("Index: " + i);
                //berakni a roadot az egyik settlementbe
                for (int j = 0; j < 3; j++)
                {
                    Debug.Log("Triangle index 0: " + RoadForAStar[i].IDNumberForTriangles[0]);
                    Debug.Log("Triangle index 0 from list: " + TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[0]-1]);
                    Debug.Log("Roads to settlement 0: " + TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[0]-1].RoadsToTheSettlement[j]);
                    
                    if (TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[0]-1].RoadsToTheSettlement[j]<1)
                    {
                        TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[0]-1].RoadsToTheSettlement[j] = RoadForAStar[i].IDNumberForRoad;
                        break;
                    }
                }
                //berakni a roadot a másik settlementbe
                for (int j = 0; j < 3; j++)
                {
                    Debug.Log("Triangle index 1: " + RoadForAStar[i].IDNumberForTriangles[1]);
                    Debug.Log("Triangle index 1 from list: " + TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[1]-1]);
                    Debug.Log("Roads to settlement 1: " + TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[1]-1].RoadsToTheSettlement[j]);

                    if (TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[1]-1].RoadsToTheSettlement[j] <1)
                    {
                        TrianglesForAStar[RoadForAStar[i].IDNumberForTriangles[1]-1].RoadsToTheSettlement[j] = RoadForAStar[i].IDNumberForRoad;
                        break;
                    }
                }
                AStarNode newNode;
                newNode.IDNumberForPossiblePaths = RoadForAStar[i].IDNumberForTriangles;
                newNode.GCost = RoadForAStar[i].LevelOfTheRoad;
                newNode.HCost = 0;
                NodesToCalculcate[i]=newNode;
                
            }
            for (int i = 0; i < TrianglesForAStar.Length; i++)
            {
                AStarNode newNode;
                newNode.IDNumberForPossiblePaths = new int[3]
                    {
                        TrianglesForAStar[IDNumberForTriangles[i]-1].RoadsToTheSettlement [0],
                        TrianglesForAStar[IDNumberForTriangles[i]-1].RoadsToTheSettlement [1],
                       TrianglesForAStar[IDNumberForTriangles[i]-1].RoadsToTheSettlement [2]
                    };
                newNode.GCost = 6;
                newNode.HCost = 0;
                NodesToCalculcate[i+ RoadForAStar.Length] = newNode;
            }
            for (int i = 0; i < IDNumberForTriangles.Count; i++)
            {
                Debug.Log("Settlement id: " + TrianglesForAStar[IDNumberForTriangles[i] - 1].IDNumberForTriangle);
                Debug.Log("/nFirst: "+TrianglesForAStar[IDNumberForTriangles[i]-1].RoadsToTheSettlement[0]+
                       "/nSecond: "+TrianglesForAStar[IDNumberForTriangles[i]-1].RoadsToTheSettlement [1]+
                        "/nThird:"+ TrianglesForAStar[IDNumberForTriangles[i]-1].RoadsToTheSettlement [2]);
            }
        }
    }
}
