﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public Vector2 spawnLocation = new Vector2(500,500);
    public Vector3 offset = new Vector3(0,0.33f,0);
    public Vector3 cameraSpawnOffset = new Vector3(0, 3, -6);
    public float outsideWorldLimit = 3f;

    public float worldRaius = 200;
    public float cameraStopFollowRadius = 190;
    public GameObject player;
    public GameObject cam;
    public Image overlay;

    Vector3 cameraSpawnPos;
    Vector3 spawnPosition;

    public EnvironmentSpawnManager environmentSpawnManager;

    void Start()
    {
        spawnPosition = CalculateSpawnHit().point + offset;
        Spawn(spawnPosition);

        PlayerEvents.Singleton.RegisterLifeRemovedActions(Respawn);
    }

    public void Spawn(Vector3 position)
    {
        player.transform.position = position;

        cameraSpawnPos = position + cameraSpawnOffset;
        cam.transform.position = cameraSpawnPos;
    }

    public void Respawn()
    {
        environmentSpawnManager.Respawn();

        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;

        spawnPosition = CalculateSpawnHit().point + offset;
        Spawn(spawnPosition);

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;

        cam.GetComponent<FollowCamera>().ResetFollowSpeed();

        PlayerStates.Singleton.IsJumping = false;
    }

    public void Update()
    {
        //Debug.Log("Distance from spawnPoint: " + Vector3.Distance(player.transform.position, spawnPosition));

        float playerDistanceFromSpawnPoint = Vector3.Distance(player.transform.position, spawnPosition);

        if (playerDistanceFromSpawnPoint > cameraStopFollowRadius)
        {
            float normalizedOverlayIntensityUnit = (worldRaius - cameraStopFollowRadius) / 128f;
            float overlayIntensity = (playerDistanceFromSpawnPoint - cameraStopFollowRadius) * normalizedOverlayIntensityUnit; 
            overlay.color = new Color(1, 1, 1, overlayIntensity);
            cam.GetComponent<FollowCamera>().SetFollowSpeed(0.01f);
        }
        else
        {
            overlay.color = new Color(1, 1, 1, 0);
            cam.GetComponent<FollowCamera>().ResetFollowSpeed();
        }


        if (playerDistanceFromSpawnPoint > worldRaius + outsideWorldLimit)
            Respawn();
    }

    RaycastHit CalculateSpawnHit()
    {
        Vector3 rayStartPos = new Vector3(spawnLocation.x, 1000f, spawnLocation.y);
        RaycastHit hit;
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, 2000f))
            return hit;
        else
            return new RaycastHit();
    }
}
