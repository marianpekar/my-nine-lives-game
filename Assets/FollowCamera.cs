using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    Vector3 offset;
    float initialGroundDistance;

    void Start()
    {
        offset = target.transform.position - transform.position;
        initialGroundDistance = GetFlooredGroundDistance();
    }
    void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime);

        if (GetFlooredGroundDistance() < initialGroundDistance)
            offset.y--;
        else if (GetFlooredGroundDistance() > initialGroundDistance)
            offset.y++;
  
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = Vector3.Lerp(transform.position, target.transform.position - (rotation * offset), Time.deltaTime);
      
        transform.LookAt(target.transform);
    }

    float GetGroundDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.distance;
        }
        return -1f;
    }

    float GetFlooredGroundDistance()
    {
       return Mathf.Floor(GetGroundDistance());
    }
}
