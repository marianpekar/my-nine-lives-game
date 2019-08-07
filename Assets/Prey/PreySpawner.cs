﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreySpawner : MonoBehaviour
{

    public float size = 450f;
    public GameObject prey;
    public int preyCount;

    int terrainLayerMask = 9;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < preyCount; i++)
        {
            RaycastHit hit = CalculateSpawnHit();
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                Instantiate(prey, hit.point, Quaternion.identity, this.transform);
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