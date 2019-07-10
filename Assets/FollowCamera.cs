using UnityEngine;
public class FollowCamera : MonoBehaviour
{
    public GameObject target;

    Vector3 offset;
    Vector3 addToOffset = Vector3.zero;
    int initialDistanceToGround;

    const float minYOffset = -5f;
    const float maxYOffset = -2f;

    float raysDistMultiplier;

    void Start()
    {
        offset = target.transform.position - transform.position;
        initialDistanceToGround = (int)DistanceToGround();
    }

    void LateUpdate()
    {
        if (IsInPlaneofSize(3f))
            raysDistMultiplier = 0.5f;
        else
            raysDistMultiplier = 1f;

        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime);

        if ((int)DistanceToGround() < initialDistanceToGround || offset.y > maxYOffset)
            offset.y -= 0.01f;
        else if ((int)DistanceToGround() > initialDistanceToGround || offset.y < minYOffset)
            offset.y += 0.01f;

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
    Vector3 leftRayDir;
    Vector3 rightRayDir;
    const float sideRaysDist = 2f;
    const float sideRaysSpread = 0.9f;

    readonly Vector3 forwardOffsetAddition = new Vector3(0.66f, 0f, 0f);
    readonly Vector3 sideOffsetAddition = new Vector3(0.33f, 0f, 0f);

    void CalculateRaycastsDirections()
    {
        leftRayDir = (sideRaysSpread * -transform.right + transform.forward + transform.up) * raysDistMultiplier;
        rightRayDir = (sideRaysSpread * transform.right + transform.forward + transform.up) * raysDistMultiplier;
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(target.transform.position, transform.position) * 0.75f;
    }

    Vector3 DirectionToPlayer()
    {
        return target.transform.position - transform.position;
    }

    Vector3 PointOneUpThePlayer()
    {
        return target.transform.position + Vector3.up;
    }

    void CalculateOffsetToAvoidObstacle()
    {
        CalculateRaycastsDirections();
        Debug.DrawRay(PointOneUpThePlayer(), (Vector3.left + Vector3.down) * 2f, Color.yellow);
        Debug.DrawRay(PointOneUpThePlayer(), (-Vector3.left + Vector3.down) * 2f, Color.yellow);

        if (IsPlayerOccluded())
            if (Vector3.Distance(GetHitPoint(DirectionToPlayer(), DistanceToPlayer()), GetHitPoint(PointOneUpThePlayer(), -Vector3.left + Vector3.down, 2f))
                < Vector3.Distance(GetHitPoint(DirectionToPlayer(), DistanceToPlayer()), GetHitPoint(PointOneUpThePlayer(), Vector3.left + Vector3.down, 2f)))
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

    bool IsPlayerOccluded()
    {
        Debug.DrawRay(transform.position, 0.75f * (target.transform.position - transform.position), Color.blue);
        return Physics.Raycast(transform.position, target.transform.position - transform.position, 0.75f * Vector3.Distance(target.transform.position, transform.position));
    }

    Vector3 GetHitPoint(Vector3 direction, float distance)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction * distance);
        if (Physics.Raycast(ray, out hit))
            return hit.transform.position;
        else
            return Vector3.zero;
    }

    Vector3 GetHitPoint(Vector3 startPos, Vector3 direction, float distance)
    {
        RaycastHit hit;
        Ray ray = new Ray(startPos, direction * distance);
        if (Physics.Raycast(ray, out hit))
            return hit.transform.position;
        else
            return Vector3.zero;
    }

    bool IsInPlaneofSize(float size)
    {
        Debug.DrawRay(transform.position, (Vector3.forward - Vector3.left) * size, Color.red);
        Debug.DrawRay(transform.position, (Vector3.forward + Vector3.left) * size, Color.red);
        Debug.DrawRay(transform.position, -(Vector3.forward - Vector3.left) * size, Color.red);
        Debug.DrawRay(transform.position, -(Vector3.forward + Vector3.left) * size, Color.red);

        Ray rayForward = new Ray(transform.position, Vector3.forward - Vector3.left);
        Ray rayBackward = new Ray(transform.position, Vector3.forward + Vector3.left);
        Ray rayLeft = new Ray(transform.position, -(Vector3.forward - Vector3.left));
        Ray rayRight = new Ray(transform.position, -(Vector3.forward + Vector3.left));

        return Physics.Raycast(rayForward, size) ||
               Physics.Raycast(rayBackward, size) ||
               Physics.Raycast(rayLeft, size) ||
               Physics.Raycast(rayRight, size);
    }
}