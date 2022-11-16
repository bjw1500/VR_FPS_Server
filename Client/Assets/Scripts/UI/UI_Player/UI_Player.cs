using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Google.Protobuf.Protocol;

public class UI_Player : UI_Base
{
    enum Buttons
    {
        PlayerList,
        Setting,
        Quit,
    }

    const string scene = "Login";
    public GameObject menuPrefab;
    public GameObject settingPrefab;
    public Button playerlist;
    public Button setting;
    public Button quit;

    public override void Init()
    {
        VrBindEvent(playerlist.gameObject, ControllPlayerList);
        VrBindEvent(setting.gameObject, ControllSetting);
        VrBindEvent(quit.gameObject, GotoLogin);
    }

    public void ControllPlayerList()
    {
        menuPrefab.SetActive(menuPrefab.activeSelf ? false : true);
        playerlist.gameObject.SetActive(playerlist.gameObject.activeSelf ? false : true);
    }

    public void ControllSetting()
    {
        settingPrefab.transform.localPosition = new Vector3(0f, 0f, 0.15f);
        settingPrefab.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        settingPrefab.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        menuPrefab.SetActive(menuPrefab.activeSelf ? false : true);
        settingPrefab.gameObject.SetActive(settingPrefab.gameObject.activeSelf ? false : true);
    }

    public void GotoLogin()
    {
        SceneManager.LoadScene(scene);

        //TODO
        //서버에서 목록 비워주게 해주기.
        C_Despawn despawn = new C_Despawn();
        despawn.Info.Add(Managers.Object.MyPlayer.Info);
       
    }
}
