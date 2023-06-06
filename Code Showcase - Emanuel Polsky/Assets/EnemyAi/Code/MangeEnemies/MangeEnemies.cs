using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GarmentButton.AI.StateMechine
{
    public class MangeEnemies : MonoBehaviour
    {
        [SerializeField] private EnemyDuckStats _enemiesData;
        [SerializeField] private GameObject[] _allEnemies;
        [SerializeField] private Transform[] _hidePosition;
        [SerializeField] private Vector3[] _backUpPosition;
        [SerializeField] private Vector3[] _spaceShipPositions;



        [ContextMenu("Find All Position")]
        private void FindAllDestinations()
        {
            var hideSpots = GameObject.FindGameObjectsWithTag("Hide");
            _hidePosition = hideSpots.ChangeGameObjectsToTransforms();

            var backUpPosition = GameObject.FindGameObjectsWithTag("BackUp");
            _backUpPosition = backUpPosition.ChangeGameObjectsToPosition();

            var spaceShipPosition = GameObject.FindGameObjectsWithTag("End");
            _spaceShipPositions = spaceShipPosition.ChangeGameObjectsToPosition();

        }

        [ContextMenu("Find All Enemies")]
        private void GetAllEnemies()
        {
            _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        }

        private void Awake()
        {
            if (_enemiesData == null)
                return;
            _enemiesData.HidePositions = _hidePosition;
            _enemiesData.BackUpPosition = _backUpPosition;
            _enemiesData.SpaceShipPositions = _spaceShipPositions;

            for (int i = 0; i < _allEnemies.Length; i++)
                _allEnemies[i].SetActive(true);
        }

    }
}
