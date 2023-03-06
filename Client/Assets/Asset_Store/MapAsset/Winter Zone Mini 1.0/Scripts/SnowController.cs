using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowController : MonoBehaviour
{
    public Transform playerOrCamera;
    public GameObject[] sg1;
    public GameObject[] sg2;
    public GameObject[] sg3;
    float snowTime = 0;
    bool snowOn = false;
    bool bigSnowOn = false;
    bool snowCoolDown = false;
    bool smallSnowOn = false;

    float randTime;
    float coolDownTime;
    int small = 0;

    private void Awake()
    {
        randTime = Random.Range(10f, 30f);
        coolDownTime = Random.Range(5f, 20f);
    }

    private void Update()
    {
        snowTime += Time.deltaTime;

        if (snowTime >= randTime && !snowOn) {
            StopCoroutine(BlizzardOn(sg2));
            StartCoroutine(BlizzardOn(sg2));
            snowOn = true;
            snowTime = 0;
            int big = Random.Range(1, 4);
            if (big == 1 && !bigSnowOn) {
                StartCoroutine(BlizzardOn(sg3));
                bigSnowOn = true;
            }
        }

        if (snowTime >= 15 && snowOn && !snowCoolDown) {
            Debug.Log("snow off");
            StopCoroutine(BlizzardOff(sg2));
            StartCoroutine(BlizzardOff(sg2));
            snowTime = 0;
            if (bigSnowOn) {
                StartCoroutine(BlizzardOff(sg3));
                bigSnowOn = false;
            }
            
            snowCoolDown = true;
            coolDownTime = Random.Range(5f, 20f);
            small = Random.Range(1, 5);
        }

        
        if (small == 1 && snowCoolDown && !smallSnowOn) {
            StartCoroutine(BlizzardOff(sg1));
            smallSnowOn = true;
        }

        if (snowCoolDown && snowTime >= 20) {
            snowOn = false;
            snowCoolDown = false;
            snowTime = 0;
            if (smallSnowOn) {
                StartCoroutine(BlizzardOn(sg1));
                smallSnowOn = false;
            }
            randTime = Random.Range(10f, 30f);
        }
    }

    IEnumerator BlizzardOn(GameObject[] sg)
    {
        for (int i = 0; i < sg.Length; i++)
        {
            sg[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

    }

    IEnumerator BlizzardOff(GameObject[] sg) {
        for (int i = 0; i < sg.Length; i++) {
            sg[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
