using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : Test
{
    void Start()
    {
        base.Init();
    }

    protected override void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            Debug.Log("in" + e.target.name);
        }
    }
    protected override void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            Debug.Log("out" + e.target.name);
        }
    }
}
