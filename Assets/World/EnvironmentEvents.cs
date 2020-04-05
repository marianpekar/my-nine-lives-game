using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class EnvironmentEvents
{
    List<Action> timeChangedActions = new List<Action>();
    public void RegisterTimeChangedAction(Action action) { timeChangedActions.Add(action); }
    public void InvokeTimeChangedActions() { InvokeActions(timeChangedActions); }
    private void InvokeActions(List<Action> actions)
    {
        foreach (Action action in actions)
            action.Invoke();
    }

    private static EnvironmentEvents instance;

    public static EnvironmentEvents Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new EnvironmentEvents();
            }

            return instance;
        }
    }
}
