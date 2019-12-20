using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectsSpawner : MonoBehaviour
{
    public EnvironmentManager.EnvironmentType thisEnvironmentType;
    public EnvironmentManager.EnvironmentEpoch thisEnvironmentEpoch;

    public float size = 800;
    public float offset = 5f;
    public Vector2 blankSpaceCenterPosition = new Vector2(500, 500);
    public float blankSpaceRadius = 10f;
    public bool fixRotation = true;

    public GameObject[] gameObjects;
    List<GameObject> instantiatedObjects = new List<GameObject>();
    public int count;
    public float maxSteepAngle = 40f;
    public float lowerOffset = 0.22f;

    public bool isNavMeshObstacle = true;

    void Show()
    {
       
    }

    // Start is called before the first frame update
    public void Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            RaycastHit hit = CalculateSpawnHit();
            if (Vector2.Distance(new Vector2(hit.point.x, hit.point.z), blankSpaceCenterPosition) < blankSpaceRadius)
                continue;

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain") && Vector3.Angle(hit.normal, Vector3.up) < maxSteepAngle)
            {
                //Debug.DrawRay(hit.point, hit.normal, Color.green, Mathf.Infinity);
                GameObject prefab = gameObjects[Random.Range(0, gameObjects.Length)];
                GameObject gameObject = Instantiate(prefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), this.transform) as GameObject;

                gameObject.transform.Rotate(transform.right, fixRotation ? -90 : 0);
                gameObject.transform.localPosition += new Vector3(0, -lowerOffset, 0);
                gameObject.name = prefab.name + "_" + (i + 1);

                if (isNavMeshObstacle)
                {
                    gameObject.AddComponent<NavMeshObstacle>().carving = true;
                }

                instantiatedObjects.Add(gameObject);
            } else
            {
                i--;
            }
        }

        SetAllInactive();
    }

    public void SetAllActive()
    {
        foreach (GameObject gameObject in instantiatedObjects)
            gameObject.SetActive(true);
    }

    public void SetAllInactive()
    {
        foreach (GameObject gameObject in instantiatedObjects)
            gameObject.SetActive(false);
    }

    public void RepositionAll()
    {
        foreach (GameObject gameObject in instantiatedObjects)
            Reposition(gameObject);
    }

    public void Reposition(GameObject gameObject)
    {
        RaycastHit hit = CalculateSpawnHit();
        if (Vector2.Distance(new Vector2(hit.point.x, hit.point.z), blankSpaceCenterPosition) < blankSpaceRadius)
        {
            Reposition(gameObject);
            return;
        }

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain") && Vector3.Angle(hit.normal, Vector3.up) < maxSteepAngle)
        {
            gameObject.transform.position = hit.point;
            gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            gameObject.transform.Rotate(transform.right, fixRotation ? -90 : 0);
            gameObject.transform.localPosition += new Vector3(0, -lowerOffset, 0);
        }
        else
        {
            Reposition(gameObject);
        }
    }

    public RaycastHit CalculateSpawnHit()
    {
        Vector3 rayStartPos = new Vector3(Random.Range(this.transform.position.x - size / 2, this.transform.position.x + size / 2), 1000f,
                                          Random.Range(this.transform.position.x - size / 2, this.transform.position.x + size / 2));
        RaycastHit hit;
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, 2000f))
            return hit;
        else
            return new RaycastHit();
    }
}
