using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagers : MonoBehaviour
{
    //오브젝트 생성할 땐, ObjectManager 호출해서 MakeObj() 호출하고, 안에 타입 입력하면 됨. 타입은 checkType 함수 참고하고.
    //오브젝트 위치 정하는건, 해당 스크립트에서 생성한 오브젝트 transform에 직접 넣으면 됨.
    //오브젝트 지울땐, 그냥 SetActive로 하고, 시간 정할거면, Invoke나 코루틴으로 알아서 해야 됨.
    //GetPool 함수는 타입을 입력받아서 해당 오브젝트의 배열 전부를 return하는 함수

    public GameObject machinegun_P;
    public GameObject sniper_P;
    public GameObject handgun_P;
    public GameObject machinegunBullet_P;
    public GameObject sniperBullet_P;
    public GameObject handgunBullet_P;
    public GameObject granade_P;

    GameObject[] machinegun;
    GameObject[] sniper;
    GameObject[] handgun;
    GameObject[] machinegunBullet;
    GameObject[] sniperBullet;
    GameObject[] handgunBullet;
    GameObject[] granade;

    GameObject[] targetPool;

    private void Awake()
    {
        machinegun = new GameObject[10];
        sniper = new GameObject[10];
        handgun = new GameObject[10];
        machinegunBullet = new GameObject[1000];
        sniperBullet = new GameObject[300];
        handgunBullet = new GameObject[500];
        granade = new GameObject[300];

        Generate();
    }

    void Generate() 
    {
        for (int i = 0; i < machinegun.Length; i++) {
            machinegun[i] = Instantiate(machinegun_P);
            machinegun[i].SetActive(false);
        }
        for (int i = 0; i < sniper.Length; i++)
        {
            sniper[i] = Instantiate(sniper_P);
            sniper[i].SetActive(false);
        }
        for (int i = 0; i < handgun.Length; i++)
        {
            handgun[i] = Instantiate(handgun_P);
            handgun[i].SetActive(false);
        }
        for (int i = 0; i < machinegunBullet.Length; i++)
        {
            machinegunBullet[i] = Instantiate(machinegunBullet_P);
            machinegunBullet[i].SetActive(false);
        }
        for (int i = 0; i < sniperBullet.Length; i++)
        {
            sniperBullet[i] = Instantiate(sniperBullet_P);
            sniperBullet[i].SetActive(false);
        }
        for (int i = 0; i < handgunBullet.Length; i++)
        {
            handgunBullet[i] = Instantiate(handgunBullet_P);
            handgunBullet[i].SetActive(false);
        }
        for (int i = 0; i < granade.Length; i++)
        {
            granade[i] = Instantiate(granade_P);
            granade[i].SetActive(false);
        }

 
    }

    public GameObject MakeObj(string type)
    {
        CheckType(type);

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        CheckType(type);
        return targetPool;
    }

    void CheckType(string type)
    {
        switch (type)
        {
            case "Machinegun":
                targetPool = machinegun;
                break;
            case "Sniper":
                targetPool = sniper;
                break;
            case "Handgun":
                targetPool = handgun;
                break;
            case "MachinegunBullet":
                targetPool = machinegunBullet;
                break;
            case "SniperBullet":
                targetPool = sniperBullet;
                break;
            case "HandgunBullet":
                targetPool = handgunBullet;
                break;
            case "Granade":
                targetPool = granade;
                break;
        }
    }
}
