using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LifeIcon : MonoBehaviour
{
    private LifeIconState IconState = LifeIconState.Full;
    [SerializeField] private Image icon;
    [SerializeField] private float _speedChangeUI;


    public void ChangeToEmpty()
    {
        IconState = LifeIconState.Empty;
        transform.DOShakeRotation(1);
        icon.DOColor(Color.black, _speedChangeUI);
    }
    public void ChangeToFull()
    {
        IconState = LifeIconState.Full;
        icon.DOColor(Color.white, _speedChangeUI);
    }
    public LifeIconState GetState()
    {
        return IconState;
    }

}
public enum LifeIconState { Full, Empty}
