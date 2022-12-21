using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damage;
    BoxCollider boxC;
    public bool isSwing;
    public Transform bloodPos;
    public GameObject blood;

    private void Awake()
    {
        boxC = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isSwing)
        {
            boxC.enabled = true;
        }
        else {
            boxC.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1") {
            GameObject shotBlood =  Instantiate(blood, bloodPos.position, bloodPos.rotation);
            Destroy(shotBlood, 2f);

        }
    }

}
