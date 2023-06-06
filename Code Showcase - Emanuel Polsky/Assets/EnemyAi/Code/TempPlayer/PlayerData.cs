using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarmentButton.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        public bool IsCrouching;
        public float CharacterTotalVelocty => characterController.velocity.magnitude;



    }
}
