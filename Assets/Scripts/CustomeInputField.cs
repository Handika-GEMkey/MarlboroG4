using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomeInputField : InputField, IPointerClickHandler
{

    public UnityAction onClicked; 

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClicked != null)
        {
            onClicked.Invoke();
        }
    }
}