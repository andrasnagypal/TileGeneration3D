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
        if (Input.anyKey)
        Camera.position = CurrentPos+ new Vector3(Input.GetAxisRaw("Horizontal")*Speed, Input.GetAxisRaw("Vertical")*Speed, 0);
    }
}
