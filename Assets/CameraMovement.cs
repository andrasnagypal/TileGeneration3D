using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Camera;
    public float Speed;
    Vector3 CurrentPos;

    
    void Update()
    {
        CurrentPos = Camera.position;
        if (Input.anyKey||Input.mouseScrollDelta.y!=0)
        {
            Camera.position = CurrentPos + new Vector3(Input.GetAxisRaw("Horizontal") * Speed*Time.deltaTime, Input.GetAxisRaw("Vertical") * Speed * Time.deltaTime, Camera.position.z < -10f|| Input.mouseScrollDelta.y <0? Input.mouseScrollDelta.y * Speed  * Time.deltaTime:0);
            Debug.Log(Input.GetAxisRaw("Mouse ScrollWheel"));
        }
    }
}
