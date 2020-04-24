using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Complete
{
    [Serializable]
    public class AITankManager : TankManager
    {
        public override void Setup()
        {
            // Set up AI brain
        }

        public override void DisableControl()
        {
            base.DisableControl();
        }

        public override void EnableControl()
        {
            base.EnableControl();
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}