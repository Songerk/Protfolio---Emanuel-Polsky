using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.Pooling
{
    public class SampleObjectPooling : MonoBehaviour
    {
        //this script who will mange the pull and push, meaning from here it will be called the spawn

        public static ObjectPool<PoolObject> objectPool;
        [SerializeField] private GameObject objectPrefab;

        private void Awake()
        {
            //this function will just creat the pooling need for this secific prefab
            objectPool = new ObjectPool<PoolObject>(objectPrefab, CallOnPull, CallOnPush);
        }

        private void SomeFunction()
        {
            //this function will actully pull the object, make it spawn
            PoolObject newObject = objectPool.Pull();
        }

        private void CallOnPull(PoolObject pooObject)
        {
            //something that will happend when pull
        }

        private void CallOnPush(PoolObject pooObject)
        {
            //something that will happend when push
        }
    }
}
