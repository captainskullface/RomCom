using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    RectTransform rect;

    Vector2 windowSize;

    bool dragged;

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

    //called while beginning the drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        //good place to put any feedback anims/noises
        dragged = true;
        transform.SetAsLastSibling();
    }

    //Called when clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        //good place to put any feedback anims/noises
        transform.SetAsLastSibling();
    }



    //called every frame mid drag
    public void OnDrag(PointerEventData eventData)
    {
        //rect.anchoredPosition += (eventData.delta / canvas.scaleFactor); //moves the rectPos along with the .delta (how the mouse has moved since the last frame) / the scale of the canvas
        rect.anchoredPosition += (eventData.delta); //moves the rectPos along with the .delta (how the mouse has moved since the last frame) / the scale of the canvas
    }


    //called after letting go
    public void OnEndDrag(PointerEventData eventData)
    {
        dragged = false;

        float edge = 50;

        Vector3 adjustedPos = rect.anchoredPosition;
        Vector3 rectSize = rect.rect.size * rect.localScale.x;

        if (rect.anchoredPosition.x > (Screen.width - edge + (rectSize.x / 2)))
        {
            adjustedPos = new Vector3(Screen.width + ((rectSize.x / 2) - (edge - 5)), adjustedPos.y, adjustedPos.z);
        }
        else if (rect.anchoredPosition.x < edge - (rectSize.x / 2))
        {
            adjustedPos = new Vector3((edge + 5) - (rectSize.x / 2), adjustedPos.y, adjustedPos.z);
        }

        if (rect.anchoredPosition.y > 0)
        {
            adjustedPos = new Vector3(adjustedPos.x, 0 - 5, adjustedPos.z);
        }
        else if (rect.anchoredPosition.y < -Screen.height + (edge * 4))
        {
            adjustedPos = new Vector3(adjustedPos.x, -Screen.height + ((edge * 4) + 5), adjustedPos.z);
        }
        rect.anchoredPosition = adjustedPos;
    }
}
