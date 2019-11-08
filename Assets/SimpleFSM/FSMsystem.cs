using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMsystem 
{
    private Dictionary<StateID,FSMstate> states = new Dictionary<StateID, FSMstate>();
    private StateID currentID;
    private FSMstate currentState;
    public void Update(GameObject obj)
    {
        currentState.Act(obj);
        currentState.Reason(obj);
    }

    public void AddState(FSMstate s)
    {
        if(s== null)
        {
            Debug.LogError("FSM state is empty");
            return;
        }
        if(currentState == null)
        {
            currentState = s;
            currentID = s.ID;
        }
        if (states.ContainsKey(s.ID))
        {
            Debug.LogError("state already exist");
            return;
        }
        states.Add(s.ID, s);
    }
    public void DeleteState(StateID id)
    {
        if(id == StateID.NullStateID)
        {
            Debug.LogError("State is empty");
            return;
        }
        if(states.ContainsKey(id) == false)
        {
            Debug.LogError("state does not exist");
            return;
        }
        states.Remove(id);
    }
    public void DoTransition(Transition trans)//transit state
    {
        if(trans == Transition.NullTransition)
        {
            Debug.LogError("empty transition");
            return;
        }
        StateID id = currentState.GetOutPutState(trans);
        if(id == StateID.NullStateID)
        {
            Debug.LogWarning("Unable to perform transition");
            return;
        }
        if(states.ContainsKey(id) == false)
        {
            Debug.LogError("Cannot find this state");
            return;
        }
        FSMstate state = states[id];
        currentState.DoAfterLeave();
        currentState = state;
        currentID = id;
        currentState.DoBeforeEnter();
    }
}
