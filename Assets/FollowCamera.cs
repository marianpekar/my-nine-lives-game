using UnityEngine;
public class FollowCamera : MonoBehaviour
{
    public GameObject target;

    Vector3 offset;
    Vector3 addToOffset = Vector3.zero;
    int initialDistanceToGround;

    const float minYOffset = -5f;

    void Start()
    {
        offset = target.transform.position - transform.position;
        initialDistanceToGround = (int)DistanceToGround();
    }
    void LateUpdate()
    {
        Debug.Log(offset);
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime);

        if ((int)DistanceToGround() < initialDistanceToGround)
            offset.y--;
        else if ((int)DistanceToGround() > initialDistanceToGround)
            offset.y++;

        if (offset.y < minYOffset)
            offset.y++;

        CalculateOffsetToAvoidObstacle();

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = Vector3.Lerp(transform.position, target.transform.position - (rotation * (offset + addToOffset)), Time.deltaTime);
        transform.LookAt(target.transform);
    }

    float DistanceToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.distance;
        }
        return -1f;
    }

    // Calculate Vector3 as addition to offset to avoid obstacles if there's any, if not addition is Vector3.zero 
    Vector3 forwardRayDir;
    const float forwardRayDist = 4f;

    Vector3 leftRayDir;
    Vector3 rightRayDir;
    const float sideRaysDist = 2f;

    readonly Vector3 forwardOffsetAddition = new Vector3(0.16f, 0f, 0f);
    readonly Vector3 sideOffsetAddition = new Vector3(0.66f, 0f, 0f);
    void CalculateRaycastVectors()
    {
        forwardRayDir = (transform.forward + transform.up) * 0.7f;
        leftRayDir = (0.8f * -transform.right + transform.forward + transform.up) * 0.5f;
        rightRayDir = (0.8f * transform.right + transform.forward + transform.up) * 0.5f;
    }

    void CalculateOffsetToAvoidObstacle()
    {
        CalculateRaycastVectors();

        if (IsCloseToObstacle(forwardRayDist, forwardRayDir))
            if (Vector3.Distance(GetHitPoint(forwardRayDist, forwardRayDir), -transform.right) < Vector3.Distance(GetHitPoint(forwardRayDist, forwardRayDir), transform.right))
                addToOffset += forwardOffsetAddition;
            else
                addToOffset -= forwardOffsetAddition;

        if (IsCloseToObstacle(sideRaysDist, leftRayDir))
            addToOffset -= sideOffsetAddition;
        else if (IsCloseToObstacle(sideRaysDist, rightRayDir))
            addToOffset += sideOffsetAddition;
        else
            addToOffset = Vector3.zero;
    }

    bool IsCloseToObstacle(float distance, Vector3 direction)
    {
        Debug.DrawRay(transform.position, (direction) * distance, Color.green);
        return Physics.Raycast(transform.position, (direction), distance);
    }

    Vector3 GetHitPoint(float distance, Vector3 direction)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction * distance);
        if (Physics.Raycast(ray, out hit))
            return hit.transform.position;
        else
            return Vector3.zero;
    }
}
