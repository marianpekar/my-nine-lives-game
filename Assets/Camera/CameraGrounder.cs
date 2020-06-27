using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGrounder : MonoBehaviour
{
    public float OffsetY = 2f;
    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.height, true);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity)) {
            transform.position = hit.point + new Vector3(0, OffsetY, 0);
        }
    }

}
