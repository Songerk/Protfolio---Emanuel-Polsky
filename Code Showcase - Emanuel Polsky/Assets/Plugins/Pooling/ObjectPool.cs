using System;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.Pooling
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        //this script it object that will handle the pooling and the stroge behaind the scenes
        private System.Action<T> pullObject;
        private System.Action<T> pushObject;
        private Stack<T> pooledObjects = new Stack<T>();
        private GameObject prefab;
        private bool _debug = false;
        public int pooledCount
        {
            get
            {
                return pooledObjects.Count;
            }
        }
        public ObjectPool(GameObject pooledObject, int numberSpawn = 0)
        {
            prefab = pooledObject;
            Spawn(numberSpawn);
        }
        public ObjectPool(GameObject prefab, Action<T> pullObject, Action<T> pushObject, int numberSpawn = 0)
        {
            this.pullObject = pullObject;
            this.pushObject = pushObject;
            this.prefab = prefab;
        }

        private void Spawn(int numberSpawn)
        {
            T t;
            for (int i = 0; i < numberSpawn; i++)
            {
                t = GameObject.Instantiate(prefab).GetComponent<T>();
                pooledObjects.Push(t);
                t.gameObject.SetActive(false);
            }
        }

        #region Pull Function
        public T Pull()
        {
            T t;
            if (pooledCount > 0)
                t = pooledObjects.Pop();
            else
                t = GameObject.Instantiate(prefab).GetComponent<T>();
            t.gameObject.SetActive(true);
            t.BoolPulled(true);
            t.Initialize(Push);
            if(_debug)Debug.Log($"Enabled, Name:{t.gameObject.name}, Position: {t.transform.GetSiblingIndex()} ");

            pullObject?.Invoke(t);

            return t;
        }

        public T Pull(Vector3 position)
        {
            T t = Pull();
            t.transform.position = position;
            return t;
        }

        public T Pull(Vector3 position, Quaternion rotation)
        {
            T t = Pull();
            t.transform.position = position;
            t.transform.rotation = rotation;
            return t;
        }

        public T Pull(Transform parent)
        {
            T t = Pull();
            t.transform.SetParent(parent, false);
            return t;
        }

        public GameObject PullGameObject()
        {
            return Pull().gameObject;
        }

        public GameObject PullGameObject(Vector3 position)
        {
            GameObject go = Pull().gameObject;
            go.transform.position = position;
            return go;
        }

        public GameObject PullGameObject(Vector3 position, Quaternion rotation)
        {
            GameObject go = Pull().gameObject;
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }

        #endregion




        public void Push(T t)
        {
            pooledObjects.Push(t);

            pushObject?.Invoke(t);
            t.gameObject.SetActive(false);
            t.BoolPulled(false);
            if (_debug) Debug.Log($"Disaables, Name:{t.gameObject.name}, Position: {t.transform.GetSiblingIndex()} ");
        }


    }
}
