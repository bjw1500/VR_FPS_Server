using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class UI_SingleLobby : UI_Base
{
    public int MapId { get; set; } = 0;

    public Dictionary<int, UI_PlayerLobbySlot> _players = new Dictionary<int, UI_PlayerLobbySlot>();
    public UI_PlayerLobbySlot mySlot;
    [SerializeField] List<GameObject> charactersolots;
    [Header("캐릭터 선택 아이콘")]
    [SerializeField] GameObject characterSelectParent;

    [SerializeField] Sprite[] characterImg;     // <! 캐릭터 슬롯 이미지   
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
    [SerializeField] List<Button> _characterIcon;

    const int _mapCount = 4;
    Image _mapImage;
    [Header("맵")]
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

        foreach (Transform child in characterSelectParent.transform)
            _characterIcon.Add(child.GetComponent<Button>());

        for (int i = 0; i < _characterIcon.Count; i++)
        {
            BindEvent(_characterIcon[i].gameObject, SelectedCharacter, Define.UIEvent.Click);
        }

        GameObject go = transform.Find("UserGrid").gameObject;
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);      // <! 시작할 때 다 꺼주기
            charactersolots.Add(child.gameObject);
        }

        // slotInfo.Add(0, charactersolots[0]);        // <! 테스트 용
        _mapImage.sprite = _imageSlot[MapId];
        CreateSinglePlayer();
    }

    public void NextMap(PointerEventData eventData)
    {
        //누르면 다음 맵으로 이동. 
        MapId++;
        if (MapId > _mapCount - 1)
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

    public void CreateSinglePlayer()
    {
        foreach(GameObject go in charactersolots)
        {
            UI_PlayerLobbySlot slot = go.GetComponent<UI_PlayerLobbySlot>();
            if(slot == null)
            {
                Debug.Log("UI_Lobby Error. UpdateRoom");
                return;
            }
            if (slot._playerInfo != null)
                continue;

            slot._playerInfo = new PlayerInfo();
            slot._playerInfo.Name = "SinglePlayer";
            slot._playerInfo.Death = 0;
            slot._playerInfo.Kill = 0;
            slot._playerInfo.ChracterId = 0;
            slot._playerInfo.ObjectId = 99999;
            slot._characterSelectNumber = 0;
            go.GetComponent<Image>().sprite = characterImg[0];
            mySlot = slot;

            //슬롯의 상태 업데이트.
            slot.Refresh();

            //로비 창 안에서 관리할 플레이어 목록 업데이트
            _players.Add(slot._playerInfo.ObjectId, slot);

            break;
        }
    }

    public void UpdateSlot(int number)
    {

        mySlot.transform.GetComponent<Image>().sprite = characterImg[number];
        mySlot._playerInfo.ChracterId = number;
    }

    public void GameStart(PointerEventData eventData)
    {
        Managers.Object._singlePlayer = mySlot._playerInfo;
        Managers.Object._mapId = MapId;

        Managers.Scene.LoadMap(MapId);
    }

    public void SelectedCharacter(PointerEventData eventData)
    {
        string extractInt = Regex.Replace(eventData.pointerClick.name, @"[^0-9]", "");

        int number = int.Parse(extractInt);

        UpdateSlot(number);
    }
}
