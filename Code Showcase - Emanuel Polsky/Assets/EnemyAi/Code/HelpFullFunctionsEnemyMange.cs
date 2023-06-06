using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.AI.StateMechine
{
    public static class HelpFullFunctionsEnemyMange 
    {

        public static Vector3[] ChangeGameObjectsToPosition(this GameObject[] gameObjects)
        {
            Vector3[] transformArray = new Vector3[gameObjects.Length];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                transformArray[i] = gameObjects[i].transform.position;
            }
            return transformArray;
        }
        public static Transform[] ChangeGameObjectsToTransforms(this GameObject[] gameObjects)
        {
            Transform[] transformArray = new Transform[gameObjects.Length];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                transformArray[i] = gameObjects[i].transform;
            }
            return transformArray;
        }
    }
}
