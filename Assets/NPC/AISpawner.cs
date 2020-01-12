using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{

    public float size = 300f;
    public Vector2 blankSpaceCenterPosition = new Vector2(500, 500);
    public float blankSpaceRadius = 30f;

    public GameObject agent;
    List<GameObject> agents = new List<GameObject>();
    public int agentsCount = 30;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < agentsCount; i++)
        {
            RaycastHit spawnHit = CalculateSpawnHit();

            if (Vector2.Distance(new Vector2(spawnHit.point.x, spawnHit.point.z), blankSpaceCenterPosition) < blankSpaceRadius)
                continue;

            if (spawnHit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                GameObject currentAgent = (Instantiate(agent, spawnHit.point, Quaternion.identity, this.transform));
                currentAgent.name = agent.name + "_" + (i + 1);
                agents.Add(currentAgent);
            }
            else
                i--;
            }      
    }

    public void Respawn(GameObject agent)
    {       
        agent.GetComponent<NavMeshAgent>().enabled = false;

        RaycastHit spawnHit = CalculateSpawnHit();

        if (Vector2.Distance(new Vector2(spawnHit.point.x, spawnHit.point.z), blankSpaceCenterPosition) < blankSpaceRadius 
            || spawnHit.point == Vector3.zero 
            || spawnHit.transform.gameObject.layer != LayerMask.NameToLayer("Terrain"))
        {
            Respawn(agent);
            return;
        }

        agent.transform.position = spawnHit.point;
        agent.GetComponent<AIController>().SpawnPosition = spawnHit.point;

        agent.GetComponent<NavMeshAgent>().enabled = true;

        agent.GetComponent<AIController>().PerformStuckCheck();
    }

    public void RespawnAll()
    {
        foreach (GameObject agent in agents)
            Respawn(agent);
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
