using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{

    public static class HelpFullFunctionEnemies
    {
        public static bool HasChaneToHappend(int percentageChance)
        {
            int randomNumber = Random.Range(0, 101);

            if (randomNumber < percentageChance)
                return true;
            return false;
        }
    }
}
