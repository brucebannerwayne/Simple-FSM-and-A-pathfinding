using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Transition
{
    NullTransition = 0,
    SpotPlayer,
    LostPlayer,
}
public enum StateID
{
    NullStateID = 0,
    Patrol,
    Chase,
}
public abstract class FSMstate
{
    protected StateID stateID;
    public StateID ID { get { return stateID; } }
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected FSMsystem fsm;

    public FSMstate(FSMsystem fsm)
    {
        this.fsm = fsm;
    }

    public void AddTransition(Transition trans,StateID id)
    {
        if(trans == Transition.NullTransition)
        {
            Debug.LogError("NullTransition");
            return;
        }
        if(id == StateID.NullStateID)
        {
            Debug.LogError("NullStateID");
            return;
        }
        if (map.ContainsKey(trans))
        {
            Debug.LogError("duplicated transition");
            return;
        }
        map.Add(trans, id);
    }
    public void DeleteTransition(Transition trans)
    {
        if(trans == Transition.NullTransition)
        {
            Debug.LogError("NullTransition");
            return;
        }
        if (map.ContainsKey(trans) == false)
        {
            Debug.LogError("transition not found");
            return;
        }
        map.Remove(trans);
    }
    public StateID GetOutPutState(Transition trans)//get current state
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }
    public virtual void DoBeforeEnter() { }//get the action beforing entering this state
    public virtual void DoAfterLeave() { }//get the action of the next state
    public abstract void Act(GameObject obj);//action of the state
    public abstract void Reason(GameObject obj);//determination of the transition
}
