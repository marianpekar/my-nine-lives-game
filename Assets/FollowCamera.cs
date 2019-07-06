using UnityEngine;
public class FollowCamera : MonoBehaviour
{
    public GameObject target;

    Vector3 offset;
    Vector3 addToOffset = Vector3.zero;
    int initialDistanceToGround;

    void Start()
    {
        offset = target.transform.position - transform.position;
        initialDistanceToGround = (int)DistanceToGround();
    }
    void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime);

        if ((int)DistanceToGround() < initialDistanceToGround)
            offset.y--;
        else if ((int)DistanceToGround() > initialDistanceToGround)
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

    void CalculateOffsetToAvoidObstacle()
    {
        if (IsCloseToObstacle(2.5f, transform.forward + transform.up, 0.7f))

            if (Vector3.Distance(GetHitPoint(3f, transform.forward + transform.up, 0.7f), -transform.right) < Vector3.Distance(GetHitPoint(2f, transform.forward, 0.7f), transform.right))
                addToOffset += new Vector3(0.33f, 0, -0.06f);
            else
                addToOffset -= new Vector3(0.33f, 0, -0.06f);

        else if (IsCloseToObstacle(3f, -transform.right + transform.forward + transform.up, 0.5f))
            addToOffset.x -= 0.16f;
        else if (IsCloseToObstacle(3f, transform.right + transform.forward + transform.up, 0.5f))
            addToOffset.x += 0.16f;
        else
            addToOffset = Vector3.zero;
    }

    bool IsCloseToObstacle(float distance, Vector3 direction, float multiplier = 1f)
    {
        Debug.DrawRay(transform.position, (direction * multiplier) * distance, Color.green);
        return Physics.Raycast(transform.position, (direction * multiplier), distance);
    }

    Vector3 GetHitPoint(float distance, Vector3 direction, float multiplier = 1f)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, (direction * multiplier) * distance);
        if (Physics.Raycast(ray, out hit))
            return hit.transform.position;
        else
            return Vector3.zero;

    }
}
