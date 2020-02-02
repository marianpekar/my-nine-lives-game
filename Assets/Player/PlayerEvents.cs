using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerEvents
{
    List<Action> preyCatchedActions = new List<Action>();
    List<Action> feedLevelChangedActions = new List<Action>();
    List<Action> lifeRemovedActions = new List<Action>();
    List<Action> playerDiedActions = new List<Action>();
    List<Action> playerChasedStartActions = new List<Action>();
    List<Action> playerChasedEndActions = new List<Action>();
    List<Action> staminaChangedActions = new List<Action>();
    List<Action> scoreChangedActions = new List<Action>();
    List<Action> lifeAddedActions = new List<Action>();
    List<Action> pausedActions = new List<Action>();
    List<Action> stealthStartActions = new List<Action>();
    List<Action> stealthEndActions = new List<Action>();

    private void InvokeActions(List<Action> actions)
    {
        foreach (Action action in actions)
            action.Invoke();
    }

    public void RegisterFeedLevelChangedActons(Action action) { feedLevelChangedActions.Add(action); }
    public void RegisterPreyCatchAction(Action action) { preyCatchedActions.Add(action); }
    public void RegisterLifeRemovedActions(Action action) { lifeRemovedActions.Add(action); }
    public void RegisterPlayerDiedActions(Action action) { playerDiedActions.Add(action); }
    public void RegisterPlayerChasedStartActions(Action action) { playerChasedStartActions.Add(action); }
    public void RegisterPlayerChasedEndActions(Action action) { playerChasedEndActions.Add(action); }
    public void RegisterStaminaChangedActions(Action action) { staminaChangedActions.Add(action); }
    public void RegisterScoreChangedActions(Action action) { scoreChangedActions.Add(action); }
    public void RegisterLifeAddedActions(Action action) { lifeAddedActions.Add(action); }
    public void RegisterPausedActions(Action action) { pausedActions.Add(action); }
    public void RegisterStealthStartActions(Action action ) { stealthStartActions.Add(action); }
    public void RegisterStealthEndActions(Action action) { stealthEndActions.Add(action); }

    public void InvokeLifeRemovedActions() { InvokeActions(lifeRemovedActions); }
    public void InvokePreyCatchedActions() { InvokeActions(preyCatchedActions); }
    public void InvokeFeedLevelChangedActions() { InvokeActions(feedLevelChangedActions); }
    public void InvokePlayerDiedActions() { InvokeActions(playerDiedActions); }
    public void InvokePlayerChasedStartActions() { InvokeActions(playerChasedStartActions); }
    public void InvokePlayerChasedEndActions() { InvokeActions(playerChasedEndActions); }
    public void InvokeStaminaChangedActions() { InvokeActions(staminaChangedActions); }
    public void InvokeScoreChangedActions() { InvokeActions(scoreChangedActions); }
    public void InvokeLifeAddedActions() { InvokeActions(lifeAddedActions); }
    public void InvokePauseActions() { InvokeActions(pausedActions); }
    public void InvokeStealthStartActions() { InvokeActions(stealthStartActions); }
    public void InvokeStealthEndActions() { InvokeActions(stealthEndActions); }

    public void ClearAllActionLists()
    {
        preyCatchedActions.Clear();
        feedLevelChangedActions.Clear();
        lifeRemovedActions.Clear();
        lifeAddedActions.Clear();
        playerDiedActions.Clear();
        playerChasedStartActions.Clear();
        playerChasedEndActions.Clear();
        staminaChangedActions.Clear();
        scoreChangedActions.Clear();
        pausedActions.Clear();
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
