using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerLobbySlot : UI_Base
{

    public int _slot;
    public PlayerInfo _playerInfo;
    public GameObject _player;
    public TextMeshProUGUI _playerNameText;



    public override void Init()
    {
        
    }


    public void Refresh()
    {
        _playerNameText.text = _playerInfo.Name;
    }


}
