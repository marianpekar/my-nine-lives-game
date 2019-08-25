using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            RaycastHit spawnHit = CalculateSpawnHit();

            if (Vector2.Distance(new Vector2(spawnHit.point.x, spawnHit.point.z), blankSpaceCenterPosition) < blankSpaceRadius)
                continue;

            if (spawnHit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                GameObject currentPrey = (Instantiate(prey, spawnHit.point, Quaternion.identity, this.transform));
                currentPrey.name = prey.name + "_" + (i + 1);
                preys.Add(currentPrey);
            }
            else
                i--;
            }      
    }

    public void Respawn(GameObject prey)
    {
        prey.GetComponent<NavMeshAgent>().enabled = false;

        RaycastHit spawnHit = CalculateSpawnHit();

        if (Vector2.Distance(new Vector2(spawnHit.point.x, spawnHit.point.z), blankSpaceCenterPosition) < blankSpaceRadius 
            || spawnHit.point == Vector3.zero 
            || spawnHit.transform.gameObject.layer != LayerMask.NameToLayer("Terrain"))
        {
            Respawn(prey);
            return;
        }

        prey.transform.position = spawnHit.point;
        prey.GetComponent<AIController>().SpawnPosition = spawnHit.point;

        prey.GetComponent<NavMeshAgent>().enabled = true;

        prey.GetComponent<AIController>().PerformStuckCheck();
    }

    public void RespawnAll()
    {
        foreach (GameObject prey in preys)
            Respawn(prey);
    }

    public RaycastHit CalculateSpawnHit()
    {
        Vector3 rayStartPos = new Vector3(Random.Range(this.transform.position.x - size / 2, this.transform.position.x + size / 2), 1000f,
                                          Random.Range(this.transform.position.x - size / 2, this.transform.position.x + size / 2));
        RaycastHit hit;
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, 2000f))
            return hit;
        else
        {
            hit.point = Vector3.zero;
            return hit;
        }
    }
}
