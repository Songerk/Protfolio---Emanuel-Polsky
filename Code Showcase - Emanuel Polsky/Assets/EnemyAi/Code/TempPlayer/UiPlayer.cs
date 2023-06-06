using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _camera;

    [SerializeField] private RectTransform _imageRectTransform;
    [SerializeField] private RectTransform _canvasRectTransform;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        updatePosition();
    }
    [ContextMenu("Move Image")]
    void updatePosition()
    {
        // Convert the world position of the object to canvas space
        Vector2 viewportPosition = _camera.WorldToViewportPoint(_target.position);
        Vector2 canvasPosition = new Vector2(
            (viewportPosition.x * _canvasRectTransform.sizeDelta.x) - (_canvasRectTransform.sizeDelta.x * 0.5f),
            (viewportPosition.y * _canvasRectTransform.sizeDelta.y) - (_canvasRectTransform.sizeDelta.y * 0.5f)
        );

        // Set the position of the image in canvas space
        _imageRectTransform.anchoredPosition = canvasPosition;
    }
}
