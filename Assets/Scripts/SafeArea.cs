using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2 minAnchor;
    Vector2 maxAnchor;
    ScreenOrientation screenOrientation;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        SetSafeArea();
    }

    void OnRectTransformDimensionsChange()
    {
        // Update safe area when screen dimensions change (ex. device rotates)
        if (screenOrientation != Screen.orientation)
        {
            SetSafeArea();
        }
    }

    void SetSafeArea()
    {
        // Tutorial by Hooson - https://youtu.be/VprqsEsFb5w
        screenOrientation = Screen.orientation;

        minAnchor = Screen.safeArea.position;
        maxAnchor = Screen.safeArea.size + minAnchor;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }
}
