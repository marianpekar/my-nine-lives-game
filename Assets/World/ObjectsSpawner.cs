using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectsSpawner : MonoBehaviour
{
    public float size = 800;
    public float offset = 5f;
    public GameObject[] gameObjects;
    public int count;
    public float maxSteepAngle = 40f;
    public bool isNavMeshObstacle = true;
    public float lowerOffset = 0.22f;

    public Vector2 blankSpaceCenterPosition = new Vector2(500, 500);
    public float blankSpaceRadius = 10f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            RaycastHit hit = CalculateSpawnHit();
            if (Vector2.Distance(new Vector2(hit.point.x, hit.point.z), blankSpaceCenterPosition) < blankSpaceRadius)
                continue;

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain") && Vector3.Angle(hit.normal, Vector3.up) < maxSteepAngle)
            {
                Debug.DrawRay(hit.point, hit.normal, Color.green, Mathf.Infinity);
                GameObject gameObject = Instantiate(gameObjects[Random.Range(0, gameObjects.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), this.transform) as GameObject;
                gameObject.transform.Rotate(transform.right, -90);
                gameObject.transform.localPosition += new Vector3(0, -lowerOffset, 0);

                if(isNavMeshObstacle)
                {
                    gameObject.AddComponent<NavMeshObstacle>().carving = true;
                }

            } else
            {
                i--;
            }
        }   
    }

    public RaycastHit CalculateSpawnHit()
    {
        Vector3 rayStartPos = new Vector3(Random.Range(this.transform.position.x - size / 2, this.transform.position.x + size / 2), 500f,
                                          Random.Range(this.transform.position.x - size / 2, this.transform.position.x + size / 2));
        RaycastHit hit;
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, Mathf.Infinity))
            return hit;
        else
            return new RaycastHit();
    }
}
