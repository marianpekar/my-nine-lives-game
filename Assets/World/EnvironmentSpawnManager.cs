using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawnManager : MonoBehaviour
{
    public EnvironmentManager environmentManager;

    public ObjectsSpawner[] objectsSpawners;
    public PreySpawner[] preySpawners;

    public GroundSelector groundSelector;

    EnvironmentManager.EnvironmentType currentEnvironmentType;

    // Start is called before the first frame update
    public void Start()
    {
        currentEnvironmentType = environmentManager.CurrentEnvironmentType;

        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            objectsSpawner.Spawn();

            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == environmentManager.CurrentEnvironmentType)
            {
                objectsSpawner.SetAllActive();
            }
        }
    }

    // Update is called once per frame
    public void Respawn()
    {
        groundSelector.SelectNextGround();

        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == environmentManager.CurrentEnvironmentType)
            {
                objectsSpawner.RepositionAll();
                objectsSpawner.SetAllActive();
            }
            else
                objectsSpawner.SetAllInactive();
        }

        foreach (PreySpawner preySpawner in preySpawners)
        {
            preySpawner.RespawnAll();
        }

        environmentManager.SetRandomDayTime();
    }
}
