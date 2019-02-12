﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagand
{
    public class TriangleController : MonoBehaviour
    {
        public PLAINTRIANGLE AttributesOfTheTriangle;

      
        public void SetUp()
        {
            transform.position = new Vector3(AttributesOfTheTriangle.PositionParameters[0],
                AttributesOfTheTriangle.PositionParameters[1],
                AttributesOfTheTriangle.PositionParameters[2]);
           
            transform.Rotate(new Vector3(AttributesOfTheTriangle.RotationParameters[(int)RotationNames.RotX],
                    AttributesOfTheTriangle.RotationParameters[(int)RotationNames.RotY],
                    AttributesOfTheTriangle.RotationParameters[(int)RotationNames.RotZ]));
            gameObject.name = "TRIANGLE" + AttributesOfTheTriangle.IDNumberForTriangle;
        }
    }
}