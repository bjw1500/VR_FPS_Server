using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
    }

    protected void Init() 
    {
        GameMng.I.PointerIn += PointerInside;
        GameMng.I.PointerOut += PointerOutside;
    }

    protected virtual void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            Debug.Log("pointer is inside this object : " + e.target.name);
        }
    }
    protected virtual void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            Debug.Log("pointer is outside this object : " + e.target.name);
        }
    }

    protected virtual void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            Debug.Log("pointer is click this object : " + e.target.name);
        }
    }
}
