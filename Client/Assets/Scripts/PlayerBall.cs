using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    public float jumpPower;
    public int itemCount;
    bool isJump;
    //public GameManagerLogic manager;
    Rigidbody rigid;
    //AudioSource audio;
    void Awake()
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
        //audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJump) {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
        
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") {
            isJump = false;
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            itemCount++;
            //audio.Play();
            other.gameObject.SetActive(false);
            manager.GetItem(itemCount);
        }
        else if (other.tag == "Finish") {
            if (itemCount == manager.totalItemCount)
            {
                //Game Clear
                SceneManager.LoadScene(manager.stage + 1);
            }
            else {
                //Restart
                SceneManager.LoadScene(manager.stage);
            }
        }
    }*/
}
