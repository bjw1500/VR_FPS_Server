using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//플레이어 슬롯은 캐릭터 모델링과 플레이어 정보를 담는다.
//Lobby는 그런 플레이어 슬롯을 관리한다.
//플레이어가 입장할 떄마다 슬롯 리스트에 추가한다.
//추가후에는 슬롯 설정을 해준다. 게임 캐릭터 생성 및 플레이어 정보

public class UI_Lobby : UI_Base
{

    public int MapId { get; set; } = 0;

    public Dictionary<int, UI_PlayerLobbySlot> _players = new Dictionary<int, UI_PlayerLobbySlot>();
    public UI_PlayerLobbySlot mySlot;
    public GridLayoutGroup lobbySlots;

    //버튼
    //텍스트 연결
   

    enum Buttons
    {
        GameStart,
        NextMapButton,
        PreviousMapButton,
    }

    enum Images
    {
        MapImage,
    }

    Button _gameStart;
    Button _nextMapButton;
    Button _previousMapButton;

    const int _mapCount = 2;
    Image _mapImage;
    public Sprite[] _imageSlot = new Sprite[_mapCount];

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        _gameStart = Get<Button>((int)Buttons.GameStart);
        _nextMapButton = Get<Button>((int)Buttons.NextMapButton);
        _previousMapButton = Get<Button>((int)Buttons.PreviousMapButton);
        _mapImage = Get<Image>((int)Images.MapImage);

        BindEvent(_gameStart.gameObject, GameStart, Define.UIEvent.Click);
        BindEvent(_nextMapButton.gameObject, NextMap, Define.UIEvent.Click);
        BindEvent(_previousMapButton.gameObject, PreviousMap, Define.UIEvent.Click);

        GameObject go = transform.Find("Grid").gameObject;
        lobbySlots = go.GetComponent<GridLayoutGroup>();
        foreach (Transform child in lobbySlots.transform)
        {
            Destroy(child.gameObject);
        }


        _mapImage.sprite = _imageSlot[MapId];
    }


    public void NextMap(PointerEventData eventData)
    {

        //누르면 다음 맵으로 이동. 
        MapId++;
        if(MapId > _mapCount - 1)
        {
            MapId = 0;
        }
        _mapImage.sprite = _imageSlot[MapId];


        //맵 이름을 바꿔줘야 할까? 
    }

    public void PreviousMap(PointerEventData eventData)
    {
        MapId--;
        if (MapId < 0)
        {
            MapId = _mapCount - 1;
        }
        _mapImage.sprite = _imageSlot[MapId];
    }



    public void UpdateRoom(S_EnterWaitingRoom enterGamePacket)
    {

        GameObject go = Managers.Resource.Instantiate("UI/PlayerLobbySlot", lobbySlots.transform);

        UI_PlayerLobbySlot newPlayer = go.GetComponent<UI_PlayerLobbySlot>();
        newPlayer._playerInfo = enterGamePacket.Info.Player;
        newPlayer._slot = lobbySlots.transform.childCount;

        //지금은 디폴트값을 플레이어로 하고 있지만,
        //나중에는 플레이어가 직접 선택할 수 있도록 수정해주자.
        newPlayer._player = Managers.Resource.Instantiate("Player/LobbyPlayer", go.transform);
        if(enterGamePacket.MyPlayer == true)
            mySlot = newPlayer;
        newPlayer._player.transform.localScale *= 100;
        //Rigidbody rb = newPlayer._player.GetComponent<Rigidbody>();
        //rb.isKinematic = true;

        newPlayer.Refresh();


        _players.Add(enterGamePacket.Info.ObjectId, newPlayer);
    }

    public void LeaveGame(S_LeaveWaitingRoom leaveGamePacket)
    {

        UI_PlayerLobbySlot leavePlayer = null;
        if(_players.TryGetValue(leaveGamePacket.Info.ObjectId, out leavePlayer) == false)
        {
            Debug.Log("Lobby의 LeaveGame에 잘못된 ObjectId가 전달되었습니다.");
            return;
        }

        _players.Remove(leaveGamePacket.Info.ObjectId);
        Managers.Resource.Destroy(leavePlayer.gameObject);

    }

    public void GameStart(PointerEventData eventData)
    {
        C_StartGame start = new C_StartGame();

        
        //나중에 캐릭터 선택이 가능해질때를 위한 코드.
        foreach(UI_PlayerLobbySlot player in _players.Values)
        {
            start.Players.Add(player._playerInfo);
        }
        //나중에는 로비창에서 MapId 직접 고르게 하자.
        start.MapId = MapId;
        Managers.Network.Send(start);
    }

    public void LoadScene(int mapId)
    {
        //Managers.Scene.LoadScene(Define.Scene.Game);
        Managers.Scene.LoadMap(mapId);

    }


}
