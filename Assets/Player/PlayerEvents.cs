using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerEvents
{
    List<Action> preyCatchedActions = new List<Action>();
    List<Action> feedLevelChangedActions = new List<Action>();
    List<Action> liveRemovedActions = new List<Action>();
    List<Action> playerDiedActions = new List<Action>();

    private void InvokeActions(List<Action> actions)
    {
        foreach (Action action in actions)
            action.Invoke();
    }

    public void RegisterFeedLevelChangedActons(Action action) { feedLevelChangedActions.Add(action); }
    public void RegisterPreyCatchAction(Action action) { preyCatchedActions.Add(action); }
    public void RegisterLifeRemovedActions(Action action) { liveRemovedActions.Add(action); }
    public void RegisterPlayerDiedActions(Action action) { playerDiedActions.Add(action); }

    public void InvokeLiveRemovedActions() { InvokeActions(liveRemovedActions); }
    public void InvokePreyCatchedActions() { InvokeActions(preyCatchedActions); }
    public void InvokeFeedLevelChangedActions() { InvokeActions(feedLevelChangedActions); }
    public void InvokePlayerDiedActions() { InvokeActions(playerDiedActions); } 

    public void ClearAllActionLists()
    {
        preyCatchedActions.Clear();
        feedLevelChangedActions.Clear();
        liveRemovedActions.Clear();
        playerDiedActions.Clear();
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
