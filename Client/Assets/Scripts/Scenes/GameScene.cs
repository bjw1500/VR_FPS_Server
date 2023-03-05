using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using Valve.VR;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        Screen.SetResolution(1920, 1080, false);
        GameMng.I._isVR = XRGeneralSettings.Instance.Manager.activeLoader;
        Debug.Log($"VR :{GameMng.I._isVR} 입니다.");
        GameMng.I.bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
        Play();
    }

    public void Play()
    {
        if (GameMng.I.SingleGame == true)
        {
            ObjectInfo Info = new ObjectInfo();
            Info.ObjectId = 1;
            Info.Name = $"SinglePlayer_{01}";
            Info.MovementInfo = new MovementInfo();
            Info.MovementInfo.PlayerPosInfo = new PositionInfo();
            Info.MovementInfo.PlayerPosInfo.PosX = 0;
            Info.MovementInfo.PlayerPosInfo.PosY = 0;
            Info.MovementInfo.PlayerPosInfo.PosZ = 0;
            Info.MovementInfo.Ground = true;
            Info.MovementInfo.Running = false;
            Info.Player = Managers.Object._singlePlayer;



            Managers.Object.AddSingleGame(Info, GameMng.I._isVR);

            //앞에 로비창을 만들어서 싱글게임과 서버 접속을 나누어주자.
            //서버 연동할 때는?
        }
        else
        {
            Debug.Log($"현재 Single 게임 모드가 아닙니다. " +
                $"현재 서버 연동이 안되어 있다면 Single Game을 활성화해주세요.");
            //EnterGame Packet 보내기?
        }
    }

    public override void Clear()
    {

    }
}
