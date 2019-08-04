using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    public float size = 800;
    public GameObject[] gameObjects;
    public int count;
    public float maxSteepAngle = 40f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            RaycastHit hit = CalculateSpawnHit();

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain") && Vector3.Angle(hit.normal, Vector3.up) < maxSteepAngle)
            {
                Debug.DrawRay(hit.point, hit.normal, Color.green, Mathf.Infinity);
                GameObject treeInstance = Instantiate(gameObjects[Random.Range(0, gameObjects.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), this.transform) as GameObject;
                treeInstance.transform.Rotate(transform.right, -90);
                treeInstance.transform.Translate(0, -0.22f, 0);
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
