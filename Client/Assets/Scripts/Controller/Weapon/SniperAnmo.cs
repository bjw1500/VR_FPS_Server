using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAnmo : MonoBehaviour
{
    public GameObject[] bulletReady;
    public Sniper gun;

    private void Update()
    {
        if (gun.curBulletCount == gun._weaponData.ammoBulletCount)
        {
            for (int i = 0; i < bulletReady.Length; i++)
            {
                bulletReady[i].SetActive(true);
            }
        }

        else {
            for (int i = gun._weaponData.ammoBulletCount - 1; i>=gun.curBulletCount; i--) {
                bulletReady[i].SetActive(false);
            }
        }
    }

}
