using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempKn : MonoBehaviour
{
    Animator anim;
    bool isSwing;
    public GameObject kn;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Swing") && !isSwing) {
            anim.SetTrigger("Swing");
            isSwing = true;
            kn.GetComponent<Knife>().isSwing = true;
            Invoke("SwingCha", 1f);
        }
    }

    void SwingCha() {
        isSwing = false;
        kn.GetComponent<Knife>().isSwing = false;
    }
}
