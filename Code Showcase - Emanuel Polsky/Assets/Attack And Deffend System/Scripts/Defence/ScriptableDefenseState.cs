using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableDefenseState : ScriptableObject
{

    [Tooltip("Allows the player to Defense")]
    public bool AllowDefense = true;

    [Tooltip("Allows the player to Counter Attack")]
    public bool AllowToCounterAttack = true;

    [Tooltip("The fixed frame cooldown of your players basic Defense")]
    public int DefenseFrameCooldown = 15;

    [Tooltip("The fixed frame Lasting of your players basic Defense")]
    public int DefenseFrameActive = 15;
}
