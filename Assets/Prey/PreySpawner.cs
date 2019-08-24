using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreySpawner : MonoBehaviour
{

    public float size = 450f;
    public Vector2 blankSpaceCenterPosition = new Vector2(500, 500);
    public float blankSpaceRadius = 30f;

    public GameObject prey;
    List<GameObject> preys = new List<GameObject>();
    public int preyCount;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < preyCount; i++)
        {
            RaycastHit spawnPosition = CalculateSpawnHit();

            if (Vector2.Distance(new Vector2(spawnPosition.point.x, spawnPosition.point.z), blankSpaceCenterPosition) < blankSpaceRadius)
                continue;

            if (spawnPosition.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                GameObject currentPrey = (Instantiate(prey, spawnPosition.point, Quaternion.identity, this.transform));
                currentPrey.name = prey.name + "_" + (i + 1);
                preys.Add(currentPrey);
            }
            else
                i--;
            }      
    }

    public void Respawn(GameObject prey)
    {
        Vector3 spawnPosition = CalculateSpawnHit().point;

        if (Vector2.Distance(new Vector2(spawnPosition.x, spawnPosition.z), blankSpaceCenterPosition) < blankSpaceRadius)
        {
            Respawn(prey);
            return;
        }

        prey.transform.position = spawnPosition;
        prey.GetComponent<AIController>().SpawnPosition = spawnPosition;
        prey.GetComponent<AIController>().PerformStuckCheck();
    }

    public void RespawnAll()
    {
        foreach (GameObject prey in preys)
            Respawn(prey);
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
