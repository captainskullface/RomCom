using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// this is to be put on the obj the draggedobj is going to be placed on


public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
       if (eventData.pointerDrag != null) // if the dragged obj exists, set the dragged objs pos to this game object's pos
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        
        //if gameObj tag = trash / accept / table
            //to statManager

        
        
        }
    }
}
