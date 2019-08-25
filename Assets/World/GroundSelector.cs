using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSelector : MonoBehaviour
{
    public GameObject[] grounds;
    // Start is called before the first frame update
    void Awake()
    {
        SelectRandomGround();
    }

    // Update is called once per frame
    public void SelectRandomGround()
    {
        foreach(GameObject ground in grounds)
            ground.SetActive(false);

        grounds[Random.Range(0, grounds.Length)].SetActive(true);
    }
}
