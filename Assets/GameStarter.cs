using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagand
{
    public class GameStarter : MonoBehaviour
    {
        public GameObject StartingTile;
        HexagonController script;
        public HexagonController[] lista ;
        public List<HexagonController> NotAllNeighbourIsSet = new List<HexagonController>();
        public int HowManyIteration;
        void Start()
        {
            for (int i = 0; i < HowManyIteration; i++)
            {
                CreateHexagons();
            }
            foreach (HexagonController con in StartingTile.GetComponentsInChildren<HexagonController>())
            {
                con.SetUpRoads();
            }
            foreach (HexagonController con in StartingTile.GetComponentsInChildren<HexagonController>())
            {
                con.ConnectAllTilesTogether();
            }
        }

        void CreateHexagons()
        {
            lista = StartingTile.GetComponentsInChildren<HexagonController>();
            for (int i = 0; i < lista.Length; i++)
            {
                if (!lista[i].IsFullyNeighboured())
                    NotAllNeighbourIsSet.Add(lista[i]);
            }

            int rnd = UnityEngine.Random.Range(0, NotAllNeighbourIsSet.Count);
            NotAllNeighbourIsSet[rnd].PopulateNeighbours();
            NotAllNeighbourIsSet.Clear();
        }

    }
}
