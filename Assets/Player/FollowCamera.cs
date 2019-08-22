using UnityEngine;
public class FollowCamera : MonoBehaviour
{
    public GameObject target;

    Vector3 offset;
    Vector3 initialOffset;
    Vector3 addToOffset = Vector3.zero;

    Vector3 pointToLookAtFront;
    Vector3 pointToLookAtBack;
    Vector3 pointToLookAt;

    int initialDistanceToGround;

    const float minYOffset = -3f;
    const float maxYOffset = -2f;

    const float minXRotation = -0.2f;
    const float maxXRotation = 0.15f;

    float rotationSpeed = 4f;
    float followSpeed = 1f;

    float sideRaysDist;
    const float sideRayDistSmall = 0.5f;
    const float sideRayDistOriginal = 1f;

    const float obstaclesOnBothSidesRayLength = 1.25f;

    void Start()
    {
        offset = DirectionTo(PointOneUpThePlayer());
        initialOffset = offset;
        initialDistanceToGround = (int)DistanceToGround();
    }

    public void SetFollowSpeed(float speed)
    {
        followSpeed = speed;
    }

    public void ResetFollowSpeed()
    {
        followSpeed = 1f;
    }

    void LateUpdate()
    {
        KeepDistanceFromGround();
        SetBetweenCloserAndOriginalCamera();
        CalculateOffsetToAvoidObstacle();
        SetPointToLookAt();
        SetPositionAndRotation();
    }
    void KeepDistanceFromGround()
    {
        if ((int)DistanceToGround() < initialDistanceToGround || offset.y > maxYOffset)
            offset.y -= 0.01f;
        else if ((int)DistanceToGround() > initialDistanceToGround || offset.y < minYOffset)
            offset.y += 0.01f;
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

    void SetPointToLookAt()
    {
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.forward), Color.magenta);
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(-Vector3.forward), Color.cyan);

        pointToLookAtFront = PointOneUpThePlayer() + DirectionTo(target.transform.TransformDirection(Vector3.forward));
        pointToLookAtBack = PointOneUpThePlayer() + DirectionTo(target.transform.TransformDirection(-Vector3.forward));

        if (PlayerStates.Singleton.IsWalkingBackward)
            pointToLookAt = pointToLookAtBack;
        else
            pointToLookAt = pointToLookAtFront;
    }

    void SetBetweenCloserAndOriginalCamera()
    {
        if ((PlayerStates.Singleton.IsWalking && !PlayerStates.Singleton.IsWalkingBackward && PlayerStates.Singleton.IsGrounded) || ObstaclesFromBothSides(obstaclesOnBothSidesRayLength))
            SetCloserCamera();
        else
            SetOriginalCamera();
    }

    void SetCloserCamera()
    {
        offset.y = offset.y / 3f;
        offset.z = offset.z / 2f;
        sideRaysDist = sideRayDistSmall;
    }

    void SetOriginalCamera()
    {
        offset = initialOffset;
        sideRaysDist = sideRayDistOriginal;
    }

    void SetPositionAndRotation()
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pointToLookAt), Time.deltaTime * rotationSpeed);
        Quaternion clampedRotation = new Quaternion(Mathf.Clamp(rotation.x, minXRotation, maxXRotation), rotation.y, rotation.z, rotation.w);
        transform.rotation = clampedRotation;
        transform.position = Vector3.Lerp(transform.position, PointOneUpThePlayer() - (clampedRotation * (offset + addToOffset)), followSpeed * Time.deltaTime);
    }

    // Calculate Vector3 as addition to offset to avoid obstacles if there's any, if not addition is Vector3.zero 
    Vector3 leftRayDir;
    Vector3 rightRayDir;
    const float sideRaysSpread = 2f;

    Vector3 leftRayDir2;
    Vector3 rightRayDir2;
    const float sideRaysSpread2 = 0.7f;

    readonly Vector3 forwardOffsetAddition = new Vector3(0.66f, 0f, 0f);
    readonly Vector3 sideOffsetAddition = new Vector3(0.16f, 0f, 0f);

    void CalculateRaycastsDirections()
    {
        leftRayDir = transform.TransformDirection(-Vector3.right * sideRaysSpread + Vector3.forward);
        rightRayDir = transform.TransformDirection(Vector3.right * sideRaysSpread + Vector3.forward);
        leftRayDir2 = transform.TransformDirection(-Vector3.right * sideRaysSpread2 + Vector3.forward);
        rightRayDir2 = transform.TransformDirection(Vector3.right * sideRaysSpread2 + Vector3.forward);
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
        return target.transform.position + target.transform.TransformDirection(Vector3.up); ;
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
                addToOffset += Vector3.Lerp(Vector3.zero, forwardOffsetAddition, Time.deltaTime * 2f);
            else
                addToOffset -= forwardOffsetAddition;

        if (IsCloseToObstacle(sideRaysDist, leftRayDir) || IsCloseToObstacle(sideRaysDist, leftRayDir2))
            addToOffset -= Vector3.Lerp(Vector3.zero, sideOffsetAddition, Time.deltaTime * 2f);
        else if (IsCloseToObstacle(sideRaysDist, rightRayDir) || IsCloseToObstacle(sideRaysDist, rightRayDir2))
            addToOffset += Vector3.Lerp(Vector3.zero, sideOffsetAddition, Time.deltaTime * 2f);
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
        Debug.DrawRay(transform.position, DirectionTo(point), Color.blue);
        return Physics.Raycast(transform.position, DirectionTo(point), Vector3.Distance(point, transform.position));
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

    bool ObstaclesFromBothSides(float rayLenght)
    {
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.left) * rayLenght, Color.red);
        Debug.DrawRay(PointOneUpThePlayer(), target.transform.TransformDirection(-Vector3.left) * rayLenght, Color.red);

        Ray rayLeft= new Ray(PointOneUpThePlayer(), target.transform.TransformDirection(Vector3.left));
        Ray rayRight = new Ray(PointOneUpThePlayer(), target.transform.TransformDirection(-Vector3.left));

        return Physics.Raycast(rayLeft, rayLenght) && Physics.Raycast(rayRight, rayLenght);
    }
}