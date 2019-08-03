using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreySpawner : MonoBehaviour
{

    public float size = 450f;
    public GameObject prey;
    public int preyCount;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < preyCount; i++)
            Instantiate(prey, CalculateSpawnPosition(), Quaternion.identity, this.transform);
    }
    Vector3 CalculateSpawnPosition()
    {
        Vector3 rayStartPos = new Vector3(Random.Range(this.transform.position.x - size/2, this.transform.position.x + size / 2), 500f, 
                                          Random.Range(this.transform.position.x - size / 2, this.transform.position.x + size / 2));
        RaycastHit hit;
        if (Physics.Raycast(rayStartPos, Vector3.down * 1000f, out hit))
            return hit.point;
        else
            return Vector3.zero;
    }
}
