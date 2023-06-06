using UnityEngine;

namespace GarmentButton.AI.StateMechine
{
    [CreateAssetMenu(menuName = "EnemyAi/FSM/General Enemy Data", fileName = "General Enemy Data")]
    public class EnemyDuckStats : ScriptableObject
    {

        public float MinDistanceRunningFromPlayer = 5f;
        public float MaxDistanceRunningFromPlayer = 5f;


        [Tooltip("Check If Reached Limits World")]
        public LayerMask EndWorldObjectsLayer;

        [Tooltip("Place He Can Hide")]
        public LayerMask HideLayer;
        public float RadioSphereToDetecCloseHideSpot;




        [Header("Speed Character ")]
        public float SpeedCharacter = 1f;
        public float WhenToRunBackToShip = 20f;

        [Header("AnimationsData")]
        [Tooltip("How many second it will take to the enemy to react to the player")]
        public float HowMuchTimeItTakesToRun = 0.6f;

        [Tooltip("How many second it will take to the Animation of get Spooked To Finish")]
        public float HowMuchTimeItTakesToSpook = 1f;


        [HideInInspector]
        public Transform[] HidePositions;

        [HideInInspector]
        public Vector3[] BackUpPosition;

        [HideInInspector]
        public Vector3[] SpaceShipPositions;


    }
}