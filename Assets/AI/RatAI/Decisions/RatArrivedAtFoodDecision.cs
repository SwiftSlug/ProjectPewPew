﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/RatArrivedAtFoodDecision")]
public class RatArrivedAtFoodDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return RatArrivedAtFood(controller);
    }

    private bool RatArrivedAtFood(StateController controller)
    {
        //Debug.Log(controller.navMeshAgent.remainingDistance);

        //Debug.Log("Stopping Distance = " + stoppingDistance);

        if (controller.navMeshAgent.remainingDistance < controller.stopDistance)
        //if ((controller.transform.position - controller.target.transform.position).magnitude < stoppingDistance)
        {
            //Debug.Log("CloseEnough");
            return true;
        }
        else
        {
            //Debug.Log("StillTooFar");
            return false;
        }
    }
}
