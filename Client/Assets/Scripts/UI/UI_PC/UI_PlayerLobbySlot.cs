using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerLobbySlot : UI_Base
{

    public int _slot;

    //플레이어 정보
    public PlayerInfo _playerInfo;
    //플레이어가 픽한 캐릭터 번호
    public int _characterSelectNumber;

    //플레이어의 이름.
    public TextMeshProUGUI _playerNameText;



    public override void Init()
    {
        
    }


    public void Refresh()
    {
        _playerNameText.text = _playerInfo.Name;
        
    }


}
