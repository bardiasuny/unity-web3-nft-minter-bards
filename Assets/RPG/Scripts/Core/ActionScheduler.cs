using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IACTION currentAction;
        public void StartAction(IACTION action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;    
        }
    }
}
