using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nagand
{
    public class PathColorManager : MonoBehaviour
    {
        public static PathColorManager Instance;
        public List<ShowPath> InterfacesOfPaths = new List<ShowPath>();

        List<int> IndexesOfPathToUse = new List<int>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

        }

        public void AddInterfaces(List<GameObject> RoadsOrTriangles)
        {
            for (int i = 0; i < RoadsOrTriangles.Count; i++)
            {
                ShowPath tempInterface = RoadsOrTriangles[i].GetComponent<ShowPath>();
                InterfacesOfPaths.Add(tempInterface);
            }
           
        
        }
        public IEnumerator StartShowingPath(List<int> PathList)
        {
            
            
            if (IndexesOfPathToUse.Count > 0)
            {
               
                for (int i = 0; i < IndexesOfPathToUse.Count; i++)
                {
                    int index = IndexesOfPathToUse[i];
                    InterfacesOfPaths[index].TurnBackToOriginalColor();
                   
                }
                Debug.Log(IndexesOfPathToUse.Count);
            }
           
            IndexesOfPathToUse.Clear();
            yield return null;
            Debug.Log("Start pathcoloring: ");
           
            for (int i = 0; i < PathList.Count; i++)
            {
                IndexesOfPathToUse.Add(PathList[i]);
                InterfacesOfPaths[PathList[i]].TurnToColor();
                Debug.Log(PathList[i]);
            }
            GetComponent<AStarCalculator>().IsAvailableForCalc = true;
        }       

    }
}
