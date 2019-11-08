using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : FSMstate
{
    private Transform player;
    private float speed = 3.0f;
    public Chase(FSMsystem fsm): base(fsm)
    {
        stateID = StateID.Chase;
        player = GameObject.FindWithTag("Player").transform;
    }
    public override void Act(GameObject obj)
    {
        obj.transform.LookAt(player.position);
        obj.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public override void Reason(GameObject obj)
    {
        if (Vector3.Distance(player.position, obj.transform.position) > 6)
        {
            fsm.DoTransition(Transition.LostPlayer);
        }
    }
}   
