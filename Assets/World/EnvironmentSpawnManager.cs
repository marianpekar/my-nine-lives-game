using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawnManager : MonoBehaviour
{
    public EnvironmentManager environmentManager;

    public ObjectsSpawner[] objectsSpawners;
    public AISpawner[] preySpawners;

    public GroundSelector groundSelector;

    // Start is called before the first frame update
    public void Start()
    {
        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            objectsSpawner.Spawn();

            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == EnvironmentPreserver.EnvironmentType)
            {
                if (objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.EnvironmentEpoch.All ||
                objectsSpawner.thisEnvironmentEpoch == EnvironmentPreserver.EnvironmentEpoch)
                {
                    objectsSpawner.SetAllActive();
                }
            }
        }
    }

    public void Respawn()
    {
        groundSelector.SelectNextGround();
        environmentManager.SetRandomDayTime();
        environmentManager.SetRandomEnvironmentType();
        environmentManager.SetRandomEnvironmentEpoch();
        environmentManager.SetEnvironment();

        foreach (ObjectsSpawner objectsSpawner in objectsSpawners)
        {
            if (objectsSpawner.thisEnvironmentType == EnvironmentManager.EnvironmentType.All ||
                objectsSpawner.thisEnvironmentType == EnvironmentPreserver.EnvironmentType)
            {
                if (objectsSpawner.thisEnvironmentEpoch == EnvironmentManager.EnvironmentEpoch.All ||
                objectsSpawner.thisEnvironmentEpoch == EnvironmentPreserver.EnvironmentEpoch)
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

        foreach (AISpawner preySpawner in preySpawners)
        {
            preySpawner.RespawnAll();
        }
    }
}
