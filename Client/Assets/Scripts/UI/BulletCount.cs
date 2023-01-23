using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    public CharacterMainControllerVR controller;
    public WeaponController weaponController;
    public TextMeshProUGUI countText;

    // Update is called once per frame
    void Update()
    {
        if (weaponController == null || weaponController._weaponData.IsThrowable == true)
            return;
        

        UpdateUI();

    }

    void UpdateUI()
    {
            if (weaponController._currentState == Define.GunState.Ready)
            {
                countText.text = $"{weaponController.curBulletCount.ToString()}/{weaponController._weaponData.ammoBulletCount.ToString()}";
            }else if(weaponController._currentState == Define.GunState.Empty)
            {
                countText.text = "Empty";
            }
            else if(weaponController._currentState == Define.GunState.Reloading)
            {
                countText.text = "Reloading";
            }

    }
}
