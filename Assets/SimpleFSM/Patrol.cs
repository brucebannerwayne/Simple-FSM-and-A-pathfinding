using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : FSMstate
{
    private GameObject[] path;
    private float speed = 2.0f;
    private Transform player;
    
    private int currentIndex = 0;
    public Patrol(FSMsystem fsm) : base(fsm) {
        stateID = StateID.Patrol;
        path = GameObject.FindGameObjectsWithTag("Path");
        player = GameObject.FindWithTag("Player").transform;
       
    }
    public override void Act(GameObject obj)
    {
        obj.transform.LookAt(path[currentIndex].transform.position);
        obj.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(Vector3.Distance(obj.transform.position,path[currentIndex].transform.position)< 0.5)
        {
            currentIndex++;
            currentIndex %= path.Length;
        }
    }

    public override void Reason(GameObject obj)
    {
        if(Vector3.Distance(player.position,obj.transform.position)< 2)
        {
            fsm.DoTransition(Transition.SpotPlayer);
        }
    }
}
