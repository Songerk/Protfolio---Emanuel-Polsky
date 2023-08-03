using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.Pooling
{
    public interface IPool<T>
    {

        T Pull();

        void Push(T t);
    }
    public interface IPoolable<T>
    {
        void Initialize(System.Action<T> returnAction);
        void BoolPulled(bool isPoolledBack);
        void ReturnToPool();
    }
}