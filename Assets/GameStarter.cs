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
        void Start()
        {
            for (int i = 0; i < 250; i++)
            {
                CreateHexagons();
            }
           
        }

        void CreateHexagons()
        {
            lista = StartingTile.GetComponentsInChildren<HexagonController>();
            int rnd = UnityEngine.Random.Range(0, lista.Length);
            lista[rnd].PopulateNeighbours();
           
        }

    }
}
