using GarmentButton.InputControl;
using UnityEngine;



namespace Attack
{


    public class MangeAttackType : MonoBehaviour
    {
        #region Reference Variables
        [SerializeField] private AttackTypeData _attackTypeData;
        [SerializeField] private Animation _animation;
        [SerializeField] private MineInputController _controller;

        [Tooltip("Reorder them in the order of the enum" +
            " 0 = water," +
            " 1 = fire" +
            "2 = wind" +
            "3 = earth")]
        [SerializeField] private GameObject[] Icons;

        #endregion

        #region Setup And subscription Functions
        private void OnEnable()
        {
            if (_animation == null)
                SetAnimation();
            _controller.PressedDpad += ChangeTypeMange;
            ChangeTypeData(_attackTypeData.GetElementType());
        }
        private void OnDisable()
        {
            _controller.PressedDpad -= ChangeTypeMange;
        }
        private void SetAnimation()
        {
            var ss = GameObject.FindGameObjectWithTag("Elements");
            _animation = ss.GetComponent<Animation>();
            for (int i = 0; i < Icons.Length; i++)
            {
                Icons[i] = ss.transform.GetChild(i).gameObject;
            }
        }

        #endregion

        #region Private Functions
        private void ChangeTypeMange(Dpad dPad)
        {
            switch (dPad)
            {
                case Dpad.Up:
                    ChangeTypeData(AttackType.Water);
                    break;
                case Dpad.Down:
                    ChangeTypeData(AttackType.Fire);
                    break;
                case Dpad.Left:
                    break;
                case Dpad.Right:
                    break;
            }

        }
        private void ChangeTypeData(AttackType type)
        {
            for (int i = 0; i < Icons.Length; i++)
            {
                if (Icons[i].activeSelf)
                    Icons[i].SetActive(false);
            }
            Icons[(int)type].SetActive(true);
            _attackTypeData.ChangeType(type);
            _animation.Play();
        }
        #endregion

    }
}
