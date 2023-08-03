using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LifeMechanics
{


    public class ChangeUiLifeMeter : MonoBehaviour, IUiLife
    {
        [SerializeField] Life lifeScript;
        [SerializeField] Image lifeImages;
        [Tooltip("The speed of the fade out&in ui life")]
        [SerializeField] float speedChangeUI;

        private void OnEnable()
        {
            lifeScript.LifeChanged += UpdateUi;
        }
        private void OnDisable()
        {
            lifeScript.LifeChanged -= UpdateUi;
        }
        public void UpdateUi(float currentLife, float maxLife, bool addLife)
        {
            lifeImages.DOFillAmount(currentLife / maxLife, speedChangeUI);
        }
    }
}
