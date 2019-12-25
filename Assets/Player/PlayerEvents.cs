using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerEvents
{
    List<Action> preyCatchedActions = new List<Action>();
    List<Action> feedLevelChangedActions = new List<Action>();
    public void RegisterFeedLevelChangedActons(Action action)
    {
        feedLevelChangedActions.Add(action);
    }

    public void RegisterPreyCatchAction(Action action)
    {
        preyCatchedActions.Add(action);
    }

    public void ProcessPreyCatchedActions()
    {
        foreach (Action preyCatchedAction in preyCatchedActions)
        {
            preyCatchedAction.Invoke();
        }
    }

    public void ProcessFeedLevelChangedActions()
    {
        foreach (Action feedLevelChangedAction in feedLevelChangedActions)
        {
            feedLevelChangedAction.Invoke();
        }
    }

    public void ResetAllActionLists()
    {
        preyCatchedActions.Clear();
        feedLevelChangedActions.Clear();
    }

    private static PlayerEvents instance;

    public static PlayerEvents Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerEvents();
            }

            return instance;
        }
    }
}
