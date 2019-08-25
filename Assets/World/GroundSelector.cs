using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSelector : MonoBehaviour
{
    public GameObject[] grounds;

    public int currentGroundIndex;
    
    // Start is called before the first frame update
    void Awake()
    {
        SelectRandomGround();
    }

    // Update is called once per frame
    void SelectRandomGround()
    {
        currentGroundIndex = Random.Range(0, grounds.Length);
        grounds[currentGroundIndex].SetActive(true);
    }

    public void SelectNextGround()
    {
        grounds[currentGroundIndex].SetActive(false);

        currentGroundIndex++;
        if (currentGroundIndex > grounds.Length - 1)
            currentGroundIndex = 0;

        grounds[currentGroundIndex].SetActive(true);
    }
}
