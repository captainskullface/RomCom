using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    RectTransform rect;

    Vector2 windowSize;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        windowSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        rect.sizeDelta = windowSize;
    }
}
