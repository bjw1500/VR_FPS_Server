using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    public Action action = null;

    private void OnEnable()
    {
        GameMng.I.PointerIn += PointerInside;
        GameMng.I.PointerOut += PointerOutside;
        GameMng.I.PointerClick += PointerClick;
    }
    private void OnDisable() 
    {
        GameMng.I.PointerIn -= PointerInside;
        GameMng.I.PointerOut -= PointerOutside;
        GameMng.I.PointerClick -= PointerClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.button);
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    protected virtual void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            // Debug.Log("pointer in : " + e.target.name);
        }
    }
    protected virtual void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            // Debug.Log("pointer out : " + e.target.name);
        }
    }

    protected virtual void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            action();
            // Debug.Log("pointer click : " + e.target.name);
        }
    }
}
