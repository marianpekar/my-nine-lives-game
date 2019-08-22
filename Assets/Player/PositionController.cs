using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    public Vector2 spawnLocation = new Vector2(500,500);
    public Vector3 offset = new Vector3(0,0.33f,0);
    public Vector3 cameraSpawnOffset = new Vector3(0, 3, -6);

    public float worldRaius = 200;
    public float cameraStopFollowRadius = 190;
    public GameObject player;
    public GameObject cam;

    Vector3 cameraSpawnPos;
    Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = CalculateSpawnHit().point + offset;
        Spawn(spawnPosition);
    }

    public void Spawn(Vector3 position)
    {
        player.transform.position = position;

        cameraSpawnPos = position + cameraSpawnOffset;
        cam.transform.position = cameraSpawnPos;
    }

    public void Respawn()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;

        player.transform.position = spawnPosition;

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;

        cam.transform.position = cameraSpawnPos;
        cam.GetComponent<FollowCamera>().enabled = true;
    }

    public void Update()
    {
        Debug.Log("Distance from spawnPoint: " + Vector3.Distance(player.transform.position, spawnPosition));

        if(Vector3.Distance(player.transform.position, spawnPosition) > cameraStopFollowRadius)
            cam.GetComponent<FollowCamera>().enabled = false;

        if (Vector3.Distance(player.transform.position, spawnPosition) > worldRaius)
            Respawn();
    }

    RaycastHit CalculateSpawnHit()
    {
        Vector3 rayStartPos = new Vector3(spawnLocation.x, 500f, spawnLocation.y);
        RaycastHit hit;
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, Mathf.Infinity))
            return hit;
        else
            return new RaycastHit();
    }
}
