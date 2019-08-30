using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawnManager : MonoBehaviour
{
    public EnvironmentManager environmentManager;

    public ObjectsSpawner[] objectsSpawners;
    public PreySpawner[] preySpawners;

    public GroundSelector groundSelector;

    // Start is called before the first frame update
    public void Start()
    {
        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            objectsSpawner.Spawn();

            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == environmentManager.CurrentEnvironmentType)
            {
                if (objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.EnvironmentEpoch.All ||
                objectsSpawner.thisEnvironmentEpoch == environmentManager.CurrentEnvironmentEpoch)
                {
                    objectsSpawner.SetAllActive();
                }
            }
        }
    }

    public void Respawn()
    {
        groundSelector.SelectNextGround();

        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == environmentManager.CurrentEnvironmentType)
            {
                if (objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.EnvironmentEpoch.All ||
                objectsSpawner.thisEnvironmentEpoch == environmentManager.CurrentEnvironmentEpoch)
                {
                    objectsSpawner.RepositionAll();
                    objectsSpawner.SetAllActive();
                } else
                {
                    objectsSpawner.SetAllInactive();
                }
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
