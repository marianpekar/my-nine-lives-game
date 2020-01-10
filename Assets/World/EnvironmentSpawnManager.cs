using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawnManager : MonoBehaviour
{
    public EnvironmentManager environmentManager;

    public ObjectsSpawner[] objectsSpawners;
    public AISpawner[] aiSpawners;

    public GroundSelector groundSelector;

    // Start is called before the first frame update
    public void Start()
    {
        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            objectsSpawner.Spawn();

            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == EnvironmentManager.CurrentEnvironmentType)
            {
                if (objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.EnvironmentEpoch.All ||
                objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.CurrentEnvironmentEpoch)
                {
                    objectsSpawner.SetAllActive();
                }
            }
        }
    }

    public void Respawn()
    {
        groundSelector.SelectNextGround();
        environmentManager.SetRandomEnvironmentType();
        environmentManager.SetRandomEnvironmentEpoch();
        environmentManager.SetRandomTime();
        environmentManager.SetEnvironment();

        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == EnvironmentManager.CurrentEnvironmentType)
            {
                if (objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.EnvironmentEpoch.All ||
                objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.CurrentEnvironmentEpoch)
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

        foreach (AISpawner preySpawner in aiSpawners)
        {
            preySpawner.RespawnAll();
        }
    }
}
