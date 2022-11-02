using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;

public class LobbyScene : BaseScene
{
    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        //로비에 입장했다. 
        //이 안에는 플레이어들의 목록이 있어야 한다.

        //플레이어는 로비에 입장하면 서버에 플레이어의 목록을 요청한다.
        //그리고 자신의 슬롯 번호를 받는다.
        SceneType = Define.Scene.Lobby;
        Debug.Log("Lobby에 입장하셨습니다.");
        C_EnterLobbyOk enterOk = new C_EnterLobbyOk();
        Managers.Network.Send(enterOk);
    }

    public override void Clear()
    {
        
    }
}
