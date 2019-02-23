﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nagand
{
    public class RoadController : MonoBehaviour, ShowPath
    {
        public ROAD AttributesOfTheRoad;

        

        public void SetUp()
        {
            transform.position = new Vector3(AttributesOfTheRoad.PositionParameters[0],
                AttributesOfTheRoad.PositionParameters[1],
                AttributesOfTheRoad.PositionParameters[2]);
            transform.Rotate(AttributesOfTheRoad.RotationParameters[0], 
                AttributesOfTheRoad.RotationParameters[1], 
                AttributesOfTheRoad.RotationParameters[2]);
            gameObject.name = "Road" + AttributesOfTheRoad.IDNumberForRoad;
        }

        public void TurnToColor()
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        public void TurnBackToOriginalColor()
        {
            Debug.Log("RoadRecolor");
            gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
