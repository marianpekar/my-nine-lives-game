using UnityEngine;
public class FollowCamera : MonoBehaviour
{
    public GameObject target;

    Vector3 offset;
    Vector3 initialOffset;
    Vector3 addToOffset = Vector3.zero;
    int initialDistanceToGround;

    const float minYOffset = -3f;
    const float maxYOffset = -1f;

    float sideRaysDist;

    void Start()
    {
        offset = PointOneUpThePlayer() - transform.position;
        initialOffset = offset;
        initialDistanceToGround = (int)DistanceToGround();
    }

    void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime);

        if ((int)DistanceToGround() < initialDistanceToGround || offset.y > maxYOffset)
            offset.y -= 0.01f;
        else if ((int)DistanceToGround() > initialDistanceToGround || offset.y < minYOffset)
            offset.y += 0.01f;



        if ((PlayerStates.Singleton.IsWalking && !PlayerStates.Singleton.IsWalkingBackward && PlayerStates.Singleton.IsGrounded) || IsInPlaneOfLenght(1.25f))
            SetCloserCamera();
        else
            SetOriginalCamera();

        CalculateOffsetToAvoidObstacle();

        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.forward), Color.magenta);
        transform.LookAt(PointOneUpThePlayer() + target.transform.TransformDirection(Vector3.forward));

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = Vector3.Lerp(transform.position, PointOneUpThePlayer() - (rotation * (offset + addToOffset)), Time.deltaTime);
    }

    void SetCloserCamera()
    {
        offset.y = offset.y / 3f;
        offset.z = offset.z / 2f;
        sideRaysDist = 1f;
    }

    void SetOriginalCamera()
    {
        offset = initialOffset;
        sideRaysDist = 2f;
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
    const float sideRaysSpread = 0.9f;

    readonly Vector3 forwardOffsetAddition = new Vector3(0.66f, 0f, 0f);
    readonly Vector3 sideOffsetAddition = new Vector3(0.33f, 0f, 0f);

    void CalculateRaycastsDirections()
    {
        leftRayDir = (sideRaysSpread * transform.TransformDirection(-Vector3.right + Vector3.forward));
        rightRayDir = (sideRaysSpread * transform.TransformDirection(Vector3.right + Vector3.forward));
    }

    float DistanceTo(Vector3 point)
    {
        return Vector3.Distance(point, transform.position);
    }

    Vector3 DirectionTo(Vector3 point)
    {
        return point - transform.position;
    }

    Vector3 PointOneUpThePlayer()
    {
        return target.transform.position + target.transform.TransformDirection(Vector3.up);
    }

    void CalculateOffsetToAvoidObstacle()
    {
        CalculateRaycastsDirections();
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.left + Vector3.down) * 2f, Color.yellow);
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(-Vector3.left + Vector3.down) * 2f, Color.yellow);

        if (IsPointOccluded(PointOneUpThePlayer()))
            if (Vector3.Distance(GetHitPoint(DirectionTo(PointOneUpThePlayer()), DistanceTo(PointOneUpThePlayer()) * 0.75f), 
                GetHitPoint(PointOneUpThePlayer(), target.transform.TransformDirection(-Vector3.left + Vector3.down), 2f))
                < Vector3.Distance(GetHitPoint(DirectionTo(PointOneUpThePlayer()), DistanceTo(PointOneUpThePlayer()) * 0.75f), 
                GetHitPoint(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.left + Vector3.down), 2f)))
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

    bool IsPointOccluded(Vector3 point)
    {
        Debug.DrawRay(transform.position, point - transform.position, Color.blue);
        return Physics.Raycast(transform.position, point - transform.position, Vector3.Distance(point, transform.position));
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

    bool IsInPlaneOfLenght(float lenght)
    {
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.left) * lenght, Color.red);
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(-Vector3.left) * lenght, Color.red);

        Ray rayLeft= new Ray(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.left));
        Ray rayRight = new Ray(PointOneUpThePlayer(), target.transform.TransformDirection(-Vector3.left));

        return Physics.Raycast(rayLeft, lenght) && Physics.Raycast(rayRight, lenght);
    }
}