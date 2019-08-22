using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreySpawner : MonoBehaviour
{

    public float size = 450f;
    public GameObject prey;
    public int preyCount;

    public Vector2 blankSpaceCenterPosition = new Vector2(500, 500);
    public float blankSpaceRadius = 10f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <= preyCount; i++)
        {
            RaycastHit hit = CalculateSpawnHit();

            if (Vector2.Distance(new Vector2(hit.point.x, hit.point.z), blankSpaceCenterPosition) < blankSpaceRadius)
                continue;

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                Instantiate(prey, hit.point, Quaternion.identity, this.transform);
            else
                i--;
        }      
    }

    public void Spawn()
    {
        RaycastHit hit = CalculateSpawnHit();
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            Instantiate(prey, hit.point, Quaternion.identity, this.transform);
        else
            Spawn();
    }

    public void Respawn(GameObject prey)
    {
        prey.transform.position = CalculateSpawnHit().point;
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
