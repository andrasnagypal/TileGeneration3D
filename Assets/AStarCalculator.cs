using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Nagand
{
    public class AStarCalculator : MonoBehaviour
    {
        public GameObject ParentOfRoads;
        public GameObject ParentOfTriangles;

        ROAD[] RoadForAStar;
        PLAINTRIANGLE[] TrianglesForAStar;
        List<int> IDNumberForTriangles = new List<int>();
        int rnd;
        AStarNode[] NodesToCalculcate;
        List<AStarPath> PathToWalk = new List<AStarPath>();
        List<AStarNode> PathsForChecking = new List<AStarNode>();
        int PointA, PointB;

        bool IsAvailableForCalc = true;

        

        public void AStarCalc()
        {
            if ( IsAvailableForCalc)
            {
                IsAvailableForCalc = !IsAvailableForCalc;
                
                RoadForAStar = GetComponent<RoadsContainer>().RoadsOfTheGame;
                TrianglesForAStar = GetComponent<TrianglesContainer>().PlainTriangles;
                rnd = UnityEngine.Random.Range(0, RoadForAStar.Length);
                NodesToCalculcate = new AStarNode[RoadForAStar.Length + TrianglesForAStar.Length];
               
                CreatingNodes();                
                //HCostCalc();
                

            }
        }

        private void CreatingNodes()
        {
            int i;
            //Triangle-k berakása
            for (i = 0; i < TrianglesForAStar.Length; i++)
            {
                AStarNode newNode;
                newNode.IDNumberForPossiblePaths = new int[3] { 0, 0, 0 };                    
                newNode.GCost = 2;
                newNode.HCost = 0;
                newNode.IDNumberOfNode = TrianglesForAStar[i].IDNumberForTriangle;
                NodesToCalculcate[i] = newNode;
            }
            //roadok berakása
            for (i=0; i < RoadForAStar.Length; i++)
            {
                AStarNode newNode;
                newNode.IDNumberForPossiblePaths = RoadForAStar[i].IDNumberForTriangles;
                newNode.GCost = RoadForAStar[i].LevelOfTheRoad;
                newNode.HCost = 0;
                newNode.IDNumberOfNode = RoadForAStar[i].IDNumberForRoad;
                NodesToCalculcate[i+ TrianglesForAStar.Length] = newNode;
            }
            NodesToCalculcate = NodesToCalculcate.OrderBy(enemy => enemy.IDNumberOfNode).ToArray();
            //Trianglek roadokhoz való hozzácsatolása
            for ( i = NodesToCalculcate.Length-1; TrianglesForAStar.Length-1 < i; i--)
            {
                for (int j = 0; j < NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]-1].IDNumberForPossiblePaths.Length; j++)
                {
                    if (NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]-1].IDNumberForPossiblePaths[j]==0)
                    {
                        NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]-1].IDNumberForPossiblePaths[j] = NodesToCalculcate[i].IDNumberOfNode;
                        break;
                    }
                }
                for (int j = 0; j < NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]-1].IDNumberForPossiblePaths.Length; j++)
                {
                    if (NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[1]-1].IDNumberForPossiblePaths[j] == 0)
                    {
                        NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[1]-1].IDNumberForPossiblePaths[j] = NodesToCalculcate[i].IDNumberOfNode;
                        break;
                    }
                }
            }
            for ( i = 0; i < NodesToCalculcate.Length; i++)
            {
                Debug.Log("ID number: " + NodesToCalculcate[i].IDNumberOfNode);
                for (int j = 0; j < NodesToCalculcate[i].IDNumberForPossiblePaths.Length; j++)
                {
                    Debug.Log("Possible path " + (j + 1) + ": " + NodesToCalculcate[i].IDNumberForPossiblePaths[j]);
                }
            }
        }        

        private void HCostCalc()
        {
            for (int i = 0; i < NodesToCalculcate.Length; i++)
            {
                Vector3 temppos = Vector3.down;
                if (IDNumberForTriangles.Contains(i + 1))
                {
                    for (int j = 0; j < TrianglesForAStar.Length; j++)
                    {
                        if (TrianglesForAStar[j].IDNumberForTriangle == i + 1)
                        {
                            temppos = new Vector3(
                               TrianglesForAStar[j].PositionParameters[0],
                                TrianglesForAStar[j].PositionParameters[1],
                                 TrianglesForAStar[j].PositionParameters[2]);
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < RoadForAStar.Length; j++)
                    {
                        if (RoadForAStar[j].IDNumberForRoad == i)
                        {
                            temppos = new Vector3(
                               RoadForAStar[j].PositionParameters[0],
                                RoadForAStar[j].PositionParameters[1],
                                 RoadForAStar[j].PositionParameters[2]);
                            break;
                        }
                    }
                }
                NodesToCalculcate[i].HCost = Vector3.Distance(
                    new Vector3(
                        RoadForAStar[PointB].PositionParameters[0],
                        RoadForAStar[PointB].PositionParameters[1],
                        RoadForAStar[PointB].PositionParameters[2]),
                    temppos) * 6;
                Debug.Log("Index of " + i + " HCost is " + NodesToCalculcate[i].HCost);
            }
        }

        
    }
}
