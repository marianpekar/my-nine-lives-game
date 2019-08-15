using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    public Vector2 spawnLocation = new Vector2(500,500);
    public Vector3 offset = new Vector3(0,1,0);

    Vector3 spawnPosition;

    void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        this.transform.position = CalculateSpawnHit().point + offset;
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
