using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private FSMsystem fsm;
    // Start is called before the first frame update
    void Start()
    {
        InitFSM();
    }
    private void InitFSM()
    {
        fsm = new FSMsystem();
        FSMstate patrol = new Patrol(fsm);
        patrol.AddTransition(Transition.SpotPlayer, StateID.Chase);
        fsm.AddState(patrol);
        FSMstate chase = new Chase(fsm);
        chase.AddTransition(Transition.LostPlayer, StateID.Patrol);
        fsm.AddState(chase);
    }
    // Update is called once per frame
    void Update()
    {
        fsm.Update(gameObject);
    }
}
