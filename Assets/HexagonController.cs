using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagand
{
    public enum TileDirections
    {
        West, NorthWest, NorthEast, East, SouthEast, SouthWest
    }
    public class HexagonController : MonoBehaviour
    {
        public GameObject prefab;
        public GameObject[] NeighbouringTiles = new GameObject[6];
        public GameObject[] Roads=new GameObject[6];
        public GameObject parentObject;
        public GameObject RoadPrefab;
        public int iteration = 0;
        public static int currentIteration=0;



        public void PopulateNeighbours()
        {
           
            prefab = Resources.Load<GameObject>("PREFAB_GrassTILE1.0fSide");
            Debug.Log(prefab);
            currentIteration++;
            #region West Handling
            if (NeighbouringTiles[(int)TileDirections.West] == null)
            {
                NeighbouringTiles[(int)TileDirections.West] = Instantiate(prefab, transform.position - new Vector3(2.032f, 0, 0), Quaternion.identity);
                NeighbouringTiles[(int)TileDirections.West].transform.Rotate(0, 0, 90);
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.East] = gameObject;
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().parentObject = parentObject;
                NeighbouringTiles[(int)TileDirections.West].transform.parent = parentObject.transform;
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().iteration= currentIteration;               
           }
           
            #endregion
            #region NorthWest Handling
            if (NeighbouringTiles[(int)TileDirections.NorthWest] == null)
            {
                NeighbouringTiles[(int)TileDirections.NorthWest] = Instantiate(prefab, transform.position - new Vector3(1.015f, -1.76f, 0), Quaternion.identity);
                NeighbouringTiles[(int)TileDirections.NorthWest].transform.Rotate(0, 0, 90);
                NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthEast] = gameObject;
                NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().parentObject = parentObject;
                NeighbouringTiles[(int)TileDirections.NorthWest].transform.parent = parentObject.transform;
                NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().iteration = currentIteration;
            }
            
            #endregion
            #region NorthEast Handling
            if (NeighbouringTiles[(int)TileDirections.NorthEast] == null)
            {
                NeighbouringTiles[(int)TileDirections.NorthEast] = Instantiate(prefab, transform.position + new Vector3(1.015f, 1.76f, 0), Quaternion.identity);
                NeighbouringTiles[(int)TileDirections.NorthEast].transform.Rotate(0, 0, 90);
                NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthWest] = gameObject;
                NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().parentObject = parentObject;
                NeighbouringTiles[(int)TileDirections.NorthEast].transform.parent = parentObject.transform;
                NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().iteration = currentIteration;
            }
          
            #endregion
            #region East Handling
            if (NeighbouringTiles[(int)TileDirections.East] == null)
            {
                NeighbouringTiles[(int)TileDirections.East] = Instantiate(prefab, transform.position + new Vector3(2.032f, 0, 0), Quaternion.identity);
                NeighbouringTiles[(int)TileDirections.East].transform.Rotate(0, 0, 90);
                NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.West] = gameObject;
                NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().parentObject = parentObject;
                NeighbouringTiles[(int)TileDirections.East].transform.parent = parentObject.transform;
                NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().iteration = currentIteration;
            }
           
            #endregion
            #region SouthEast Handling
            if (NeighbouringTiles[(int)TileDirections.SouthEast] == null)
            {
                NeighbouringTiles[(int)TileDirections.SouthEast] = Instantiate(prefab, transform.position + new Vector3(1.015f, -1.76f, 0), Quaternion.identity);
                NeighbouringTiles[(int)TileDirections.SouthEast].transform.Rotate(0, 0, 90);
                NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthWest] = gameObject;
                NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().parentObject = parentObject;
                NeighbouringTiles[(int)TileDirections.SouthEast].transform.parent = parentObject.transform;
                NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().iteration = currentIteration;
            }
            
            #endregion
            #region SouthWest Handling
            if (NeighbouringTiles[(int)TileDirections.SouthWest] == null)
            {
                NeighbouringTiles[(int)TileDirections.SouthWest] = Instantiate(prefab, transform.position - new Vector3(1.015f, 1.76f, 0), Quaternion.identity);
                NeighbouringTiles[(int)TileDirections.SouthWest].transform.Rotate(0, 0, 90);
                NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthEast] = gameObject;
                NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().parentObject = parentObject;
                NeighbouringTiles[(int)TileDirections.SouthWest].transform.parent = parentObject.transform;
                NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().iteration = currentIteration;
            }


            #endregion
            ConnectAllTilesTogether();



}
        public void SetUpRoads()
        {
            RoadPrefab = Resources.Load<GameObject>("PREFAB_Road");
            if (Roads[(int)TileDirections.West] == null)
            {
                Roads[(int)TileDirections.West] = Instantiate(RoadPrefab, transform.position - new Vector3(1.015f, 0, 0), Quaternion.identity);
                Roads[(int)TileDirections.West].transform.Rotate(0, 0, 90);
                if (NeighbouringTiles[(int)TileDirections.West]!=null)
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().Roads[(int)TileDirections.East] = Roads[(int)TileDirections.West];
            }
            if (Roads[(int)TileDirections.NorthWest] == null)
            {
                Roads[(int)TileDirections.NorthWest] = Instantiate(RoadPrefab, transform.position - new Vector3(.5f, -.866f, 0), Quaternion.identity);
                Roads[(int)TileDirections.NorthWest].transform.Rotate(0, 0, 30);
                if (NeighbouringTiles[(int)TileDirections.NorthWest] != null)
                    NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().Roads[(int)TileDirections.SouthEast] = Roads[(int)TileDirections.NorthWest];
            }
            if (Roads[(int)TileDirections.NorthEast] == null)
            {
                Roads[(int)TileDirections.NorthEast] = Instantiate(RoadPrefab, transform.position + new Vector3(.5f, .866f, 0), Quaternion.identity);
                Roads[(int)TileDirections.NorthEast].transform.Rotate(0, 0, -30);
                if (NeighbouringTiles[(int)TileDirections.NorthEast] != null)
                    NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().Roads[(int)TileDirections.SouthWest] = Roads[(int)TileDirections.NorthEast];
            }
            if (Roads[(int)TileDirections.East] == null)
            {
                Roads[(int)TileDirections.East] = Instantiate(RoadPrefab, transform.position + new Vector3(1.015f, 0, 0), Quaternion.identity);
                Roads[(int)TileDirections.East].transform.Rotate(0, 0, 90);
                if (NeighbouringTiles[(int)TileDirections.East] != null)
                    NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().Roads[(int)TileDirections.West] = Roads[(int)TileDirections.East];
            }
            if (Roads[(int)TileDirections.SouthEast] == null)
            {
                Roads[(int)TileDirections.SouthEast] = Instantiate(RoadPrefab, transform.position + new Vector3(.5f, -.866f, 0), Quaternion.identity);
                Roads[(int)TileDirections.SouthEast].transform.Rotate(0, 0, 30);
                if (NeighbouringTiles[(int)TileDirections.SouthEast] != null)
                    NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().Roads[(int)TileDirections.NorthWest] = Roads[(int)TileDirections.SouthEast];
            }
            if (Roads[(int)TileDirections.SouthWest] == null)
            {
                Roads[(int)TileDirections.SouthWest] = Instantiate(RoadPrefab, transform.position + new Vector3(-.5f, -.866f, 0), Quaternion.identity);
                Roads[(int)TileDirections.SouthWest].transform.Rotate(0, 0, -30);
                if (NeighbouringTiles[(int)TileDirections.SouthWest] != null)
                    NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().Roads[(int)TileDirections.NorthEast] = Roads[(int)TileDirections.SouthWest];
            }
        }

        public void ConnectAllTilesTogether()
        {
            //Connecting West to NW and SW
            if (NeighbouringTiles[(int)TileDirections.NorthWest] != null&& NeighbouringTiles[(int)TileDirections.West]!=null)
            {
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthEast] = NeighbouringTiles[(int)TileDirections.NorthWest];
                NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthWest] = NeighbouringTiles[(int)TileDirections.West];
            }
            if (NeighbouringTiles[(int)TileDirections.SouthWest] != null && NeighbouringTiles[(int)TileDirections.West] != null)
            {
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthEast] = NeighbouringTiles[(int)TileDirections.SouthWest];
                NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthWest] = NeighbouringTiles[(int)TileDirections.West];
            }
            //Connecting NorthWest to W and NE
            if (NeighbouringTiles[(int)TileDirections.West] != null && NeighbouringTiles[(int)TileDirections.NorthWest] != null)
            {
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthEast] = NeighbouringTiles[(int)TileDirections.NorthWest];
                NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthWest] = NeighbouringTiles[(int)TileDirections.West];
            }
            if (NeighbouringTiles[(int)TileDirections.NorthEast] != null && NeighbouringTiles[(int)TileDirections.NorthWest] != null)
            {
                NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.West] = NeighbouringTiles[(int)TileDirections.NorthWest];
                NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.East] = NeighbouringTiles[(int)TileDirections.NorthEast];
            }
            //Connecting NorthEast to NW and E
            if (NeighbouringTiles[(int)TileDirections.East] != null && NeighbouringTiles[(int)TileDirections.NorthEast] != null)
            {
                NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthEast] = NeighbouringTiles[(int)TileDirections.East];
                NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthWest] = NeighbouringTiles[(int)TileDirections.NorthEast];
            }
            if (NeighbouringTiles[(int)TileDirections.NorthWest] != null && NeighbouringTiles[(int)TileDirections.NorthEast] != null)
            {
                NeighbouringTiles[(int)TileDirections.NorthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.East] = NeighbouringTiles[(int)TileDirections.NorthEast];
                NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.West] = NeighbouringTiles[(int)TileDirections.NorthWest];
            }
            //Connecting East to NE and SE
            if (NeighbouringTiles[(int)TileDirections.NorthEast] != null && NeighbouringTiles[(int)TileDirections.East] != null)
            {
                NeighbouringTiles[(int)TileDirections.NorthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthEast] = NeighbouringTiles[(int)TileDirections.East];
                NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthWest] = NeighbouringTiles[(int)TileDirections.NorthEast];
            }
            if (NeighbouringTiles[(int)TileDirections.SouthEast] != null && NeighbouringTiles[(int)TileDirections.East] != null)
            {
                NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthEast] = NeighbouringTiles[(int)TileDirections.East];
                NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthWest] = NeighbouringTiles[(int)TileDirections.SouthEast];
            }
            //Connecting SouthEast to E and SW
            if (NeighbouringTiles[(int)TileDirections.SouthWest] != null && NeighbouringTiles[(int)TileDirections.SouthEast] != null)
            {
                NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.West] = NeighbouringTiles[(int)TileDirections.SouthWest];
                NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.East] = NeighbouringTiles[(int)TileDirections.SouthEast];
            }
            if (NeighbouringTiles[(int)TileDirections.East] != null && NeighbouringTiles[(int)TileDirections.SouthEast] != null)
            {
                NeighbouringTiles[(int)TileDirections.East].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthWest] = NeighbouringTiles[(int)TileDirections.SouthEast];
                NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthEast] = NeighbouringTiles[(int)TileDirections.East];
            }
            //Connecting SouthWest to W and SE
            if (NeighbouringTiles[(int)TileDirections.SouthEast] != null && NeighbouringTiles[(int)TileDirections.SouthWest] != null)
            {
                NeighbouringTiles[(int)TileDirections.SouthEast].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.West] = NeighbouringTiles[(int)TileDirections.SouthWest];
                NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.East] = NeighbouringTiles[(int)TileDirections.SouthEast];
            }
            if (NeighbouringTiles[(int)TileDirections.West] != null && NeighbouringTiles[(int)TileDirections.SouthWest] != null)
            {
                NeighbouringTiles[(int)TileDirections.West].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.SouthEast] = NeighbouringTiles[(int)TileDirections.SouthWest];
                NeighbouringTiles[(int)TileDirections.SouthWest].GetComponent<HexagonController>().NeighbouringTiles[(int)TileDirections.NorthWest] = NeighbouringTiles[(int)TileDirections.West];
            }
        }
        public bool IsFullyNeighboured()
        {
            bool result = true;
            for (int i = 0; i < NeighbouringTiles.Length; i++)
            {
                if (NeighbouringTiles[i]==null)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}


