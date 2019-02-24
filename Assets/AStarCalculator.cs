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
      
        AStarNode[] NodesToCalculcate;
        List<AStarPath> PathToWalk = new List<AStarPath>();
        List<AStarNode> PathsForChecking = new List<AStarNode>();
        List<int> IDForPathNodes = new List<int>();
        int PointA, PointB;

        public bool IsAvailableForCalc = true;





        public void AStarCalc(int pointa,int pointb)
        {
           if (IsAvailableForCalc)
            {
                IsAvailableForCalc = !IsAvailableForCalc;                
              
                
               
                
                PointA = pointa;
                PointB = pointb;
                HCostCalc();
                StartPath();
                
                
            }
        }

       

        private void StartPath()
        {
           
            AStarPath PathToWalkOn = new AStarPath
            {
                FCost = NodesToCalculcate[PointA].GCost + NodesToCalculcate[PointA].HCost,
                IDNumberOfNode = NodesToCalculcate[PointA].IDNumberOfNode
           };
            Debug.Log("Point A " + NodesToCalculcate[PointA].IDNumberOfNode);
            Debug.Log("Point B " + NodesToCalculcate[PointB].IDNumberOfNode);          
            float CurrentPathGCost = NodesToCalculcate[PointA].GCost;
            int IndexOFLowestCostNode;
            IDForPathNodes.Clear();
            PathToWalk.Clear();
            PathToWalk.Add(PathToWalkOn);
            PathsForChecking.Clear();
            for (int i = 0; i < NodesToCalculcate[PointA].IDNumberForPossiblePaths.Length; i++)
            {
                if (NodesToCalculcate[PointA].IDNumberForPossiblePaths[i] > -1)
                    PathsForChecking.Add(NodesToCalculcate[NodesToCalculcate[PointA].IDNumberForPossiblePaths[i]]);
            }
            bool IsFinished = NodesToCalculcate[PointA].HCost != 0 ? true : false;
            //PathToWalk[PathToWalk.Count - 1].IDNumberOfNode != NodesToCalculcate[PointB].IDNumberOfNode
            while (IsFinished)
            {
                if (PathsForChecking[0].IDNumberOfNode > 0)
                    IndexOFLowestCostNode = PathsForChecking[0].IDNumberOfNode;
                else IndexOFLowestCostNode = PathsForChecking[1].IDNumberOfNode;
                for (int i =1; i < PathsForChecking.Count; i++)
                {
                    
                    
                    if (NodesToCalculcate[IndexOFLowestCostNode].HCost+ NodesToCalculcate[IndexOFLowestCostNode].GCost + CurrentPathGCost
                        > PathsForChecking[i].HCost + PathsForChecking[i].GCost + CurrentPathGCost)
                    {
                        IndexOFLowestCostNode = PathsForChecking[i].IDNumberOfNode;
                    }
                }
                CurrentPathGCost += NodesToCalculcate[IndexOFLowestCostNode].GCost;
                IsFinished = NodesToCalculcate[IndexOFLowestCostNode].HCost != 0 ? true : false;
                PathsForChecking.Clear();
                PathToWalkOn = new AStarPath
                {
                    FCost = NodesToCalculcate[IndexOFLowestCostNode].GCost + NodesToCalculcate[PointA].HCost,
                    IDNumberOfNode = NodesToCalculcate[IndexOFLowestCostNode].IDNumberOfNode
                };
                PathToWalk.Add(PathToWalkOn);
                for (int i = 0; i < NodesToCalculcate[PathToWalkOn.IDNumberOfNode].IDNumberForPossiblePaths.Length; i++)
                {
                    if(NodesToCalculcate[PathToWalkOn.IDNumberOfNode].IDNumberForPossiblePaths[i]>-1)
                    PathsForChecking.Add(NodesToCalculcate[NodesToCalculcate[PathToWalkOn.IDNumberOfNode].IDNumberForPossiblePaths[i]]);
                }               
            }
            for (int i = 0; i < PathToWalk.Count; i++)
            {
                IDForPathNodes.Add(PathToWalk[i].IDNumberOfNode);
            }
            Debug.Log(IDForPathNodes.Count);
            StartCoroutine( PathColorManager.Instance.StartShowingPath(IDForPathNodes));
            
          
        }

      

        public void CreatingNodes()
        {
            RoadForAStar = GetComponent<RoadsContainer>().RoadsOfTheGame;
            TrianglesForAStar = GetComponent<TrianglesContainer>().PlainTriangles;
            NodesToCalculcate = new AStarNode[RoadForAStar.Length + TrianglesForAStar.Length];
            int i;
            //Triangle-k berakása
            for (i = 0; i < TrianglesForAStar.Length; i++)
            {
                AStarNode newNode;
                newNode.IDNumberForPossiblePaths = new int[3] { -1, -1, -1 };                    
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
                for (int j = 0; j < NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]].IDNumberForPossiblePaths.Length; j++)
                {
                    if (NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]].IDNumberForPossiblePaths[j]==-1)
                    {
                        NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]].IDNumberForPossiblePaths[j] = NodesToCalculcate[i].IDNumberOfNode;
                        break;
                    }
                }
                for (int j = 0; j < NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[0]].IDNumberForPossiblePaths.Length; j++)
                {
                    if (NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[1]].IDNumberForPossiblePaths[j] == -1)
                    {
                        NodesToCalculcate[NodesToCalculcate[i].IDNumberForPossiblePaths[1]].IDNumberForPossiblePaths[j] = NodesToCalculcate[i].IDNumberOfNode;
                        break;
                    }
                }
            }          
        }        

        private void HCostCalc()
        {
            Vector3 PointBpos = new Vector3(
                PointB< TrianglesForAStar.Length? TrianglesForAStar[PointB].PositionParameters[0]: RoadForAStar[PointB- TrianglesForAStar.Length].PositionParameters[0],
                PointB < TrianglesForAStar.Length ? TrianglesForAStar[PointB].PositionParameters[1] : RoadForAStar[PointB - TrianglesForAStar.Length].PositionParameters[1],
                PointB < TrianglesForAStar.Length ? TrianglesForAStar[PointB].PositionParameters[2] : RoadForAStar[PointB - TrianglesForAStar.Length].PositionParameters[2]
                );
            for (int i = 0; i < NodesToCalculcate.Length; i++)
            {
                Vector3 temppos = Vector3.down;
                if (i< TrianglesForAStar.Length)
                {
                    for (int j = 0; j < TrianglesForAStar.Length; j++)
                    {
                        if (TrianglesForAStar[j].IDNumberForTriangle == i )
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

                NodesToCalculcate[i].HCost = Vector3.Distance(PointBpos, temppos) * 6;
               
            }
        }

        
    }
}
