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
            Debug.Log("Valami 1");
            Debug.Log(PathList.Count);
            if (IndexesOfPathToUse.Count > 0)
            {
                Debug.Log("Valami 2");
                for (int i = 0; i < IndexesOfPathToUse.Count; i++)
                {
                    int index = IndexesOfPathToUse[i];
                    InterfacesOfPaths[index].TurnBackToOriginalColor();
                    yield return null;
                }
                Debug.Log(IndexesOfPathToUse.Count);
            }
           
            IndexesOfPathToUse.Clear();
            yield return null;
            Debug.Log("Valami 3");
            Debug.Log(PathList.Count);
            Debug.Log(IndexesOfPathToUse.Count);
            for (int i = 0; i < PathList.Count; i++)
            {
                IndexesOfPathToUse.Add(PathList[i]);
                InterfacesOfPaths[PathList[i]].TurnToColor();

            }
        }

        //public void StartShowingPath(List<int> PathList)
        //{
        //    Debug.Log("Valami 1");
        //    Debug.Log(PathList.Count);
        //    if (IndexesOfPathToUse.Count>0)
        //    {
        //        Debug.Log("Valami 2");
        //        for (int i = 0; i < IndexesOfPathToUse.Count; i++)
        //        {
        //            int index = IndexesOfPathToUse[i];
        //            InterfacesOfPaths[index].TurnBackToOriginalColor();
        //        }
        //        Debug.Log(IndexesOfPathToUse.Count);
        //    }
        //    IndexesOfPathToUse.Clear();
        //    Debug.Log("Valami 3");
        //    Debug.Log(PathList.Count);
        //    Debug.Log(IndexesOfPathToUse.Count);
        //    for (int i = 0; i < PathList.Count; i++)
        //    {
        //        IndexesOfPathToUse.Add(PathList[i]);
        //        InterfacesOfPaths[PathList[i]].TurnToColor();

        //    }
        //}

    }
}
