using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSlot
{
    public int _slot;
    public PlayerInfo _playerInfo;

    //플레이어 입장.
    //슬롯 변경
    //만약 플레이어가 나가면, 슬롯 비우고 플레이어 위치 변경?
}

public class UI_Lobby : UI_Base
{
    public int playerCount = 4;
    public Dictionary<int, PlayerSlot> _players = new Dictionary<int, PlayerSlot>();
    public PlayerSlot mySlot;

    enum Texts
    {
        PlayerText1,
        PlayerText2,
        PlayerText3,
        PlayerText4,
    }

    enum Buttons
    {
        GameStart
    }

    List<Text> _player = new List<Text>();
    Text _player1;
    Text _player2;
    Text _player3;
    Text _player4;

    Button _gameStart;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        for (int i = 0; i < playerCount; i++)
        {
            _player.Add(Get<Text>(i));
            _player[i].text = "NULL";
        }

        _gameStart = Get<Button>((int)Buttons.GameStart);

        BindEvent(_gameStart.gameObject, GameStart, Define.UIEvent.Click);
    }

    public void UpdateRoom(S_EnterWaitingRoom enterGamePacket)
    {
        PlayerSlot newPlayer = new PlayerSlot();
        newPlayer._playerInfo = enterGamePacket.Info.Player;

        for (int i = 0; i < playerCount; i++)
        {
            //NULL이 아니면 이미 플레이어 슬롯이 존재한다는 소리니 스킵.
            if (_player[i].text != "NULL")
                continue;
            else
            {
                //슬롯이 비어있다면 한칸씩 당겨준다.
                newPlayer._slot = i;
                _player[i].text = enterGamePacket.Info.Player.Name;
                //if (enterGamePacket.MyPlayer == true)
                //    mySlot = newPlayer;
                break;
            }
        }
        _players.Add(enterGamePacket.Info.ObjectId, newPlayer);
    }

    public void LeaveGame(S_LeaveWaitingRoom leaveGamePacket)
    {
        _players.Remove(leaveGamePacket.Info.ObjectId);

        //플레이어 이름 찾아서 제거하기?
        for (int i = 0; i < playerCount; i++)
        {
            //순회하고, 이름 비교
            if (_player[i].text == leaveGamePacket.Info.Player.Name)
            {
                //슬롯에 채워진 이름이 방을 나가는 플레이어의 이름과 같다면,
                _player[i].text = "NULL";
            }
        }
    }

    public void GameStart(PointerEventData eventData)
    {
        // Managers.SceneManager.LoadScene(Define.Scene.Game);

        C_StartGame start = new C_StartGame();
        //start.Slot = mySlot._slot;
        Managers.Network.Send(start);

        //Start 패킷을 보낸다.
        //슬롯값을 같이 딸려보낸다.
    }

    public void LoadScene()
    {
        Managers.Scene.LoadScene(Define.Scene.Game);
    }
}
