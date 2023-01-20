using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempP : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        if (h != 0) {
            rigid.velocity = new Vector3(h * speed, 0, 0);
        }
        if (v != 0) {
            rigid.velocity = new Vector3(0, 0, v * speed);
        }
        if (Input.GetButton("Jump")) {
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }
}
