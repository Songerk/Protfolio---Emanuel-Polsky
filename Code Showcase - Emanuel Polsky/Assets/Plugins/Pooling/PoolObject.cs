using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.Pooling
{
    public class PoolObject : MonoBehaviour, IPoolable<PoolObject>
    {
        // Basic Object Pooling
        // the script needed to be on the prefab that we would like to pull
        // The name Class needed to be too after the "IPoolable" and in the actions

        private Action<PoolObject> returnToPool;

        bool isPooled;

        public void BoolPulled(bool isPoolledBack)
        {
            isPooled = isPoolledBack;
        }

        public void Initialize(Action<PoolObject> returnAction)
        {
            returnToPool = returnAction;
        }
        public void ReturnToPool()
        {
            returnToPool?.Invoke(this);
        }


    }
}
