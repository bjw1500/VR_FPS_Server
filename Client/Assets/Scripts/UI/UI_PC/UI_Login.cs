using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Login : UI_Base
{
    enum Buttons
    {
        EnterButton,
        ConnectButton,
        SignUpButton,
        EnterSingleGameButton,
    }

    enum InputFields
    {
        ServerIPText,
        IDText,
        PasswordText,
    }

    enum Texts
    {
        InformationText,
        VRTypeText,
    }

    enum Toggles
    {
        VRTypeToggle,
    }

    [SerializeField] Button _enterButton;
    [SerializeField] Button _connectButton;
    [SerializeField] Button _signUpButton;
    [SerializeField] Button _enterSingleGameButton;
    [SerializeField] InputField _serverIPText;
    [SerializeField] InputField _idText;
    [SerializeField] InputField _passwordText;
    [SerializeField] Toggle _toggle;
    [SerializeField] public Text _informationText;
    [SerializeField] UI_CreateAccount _signUP;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<InputField>(typeof(InputFields));
        Bind<Text>(typeof(Texts));
        Bind<Toggle>(typeof(Toggles));


        _enterButton = Get<Button>((int)Buttons.EnterButton);
        _connectButton = Get<Button>((int)Buttons.ConnectButton);
        _signUpButton = Get<Button>((int)Buttons.SignUpButton);
        _enterSingleGameButton = Get<Button>((int)Buttons.EnterSingleGameButton);
        _serverIPText = Get<InputField>((int)InputFields.ServerIPText);
        _idText = Get<InputField>((int)InputFields.IDText);
        _passwordText = Get<InputField>((int)InputFields.PasswordText);
        _informationText = Get<Text>((int)Texts.InformationText);
        _toggle = Get<Toggle>((int)Toggles.VRTypeToggle);

        BindEvent(_enterButton.gameObject, TryLogin, Define.UIEvent.Click);
        BindEvent(_connectButton.gameObject, ConnectToServer, Define.UIEvent.Click);
        BindEvent(_signUpButton.gameObject, LoadSignUpPage, Define.UIEvent.Click);
        BindEvent(_enterSingleGameButton.gameObject, EnterSingleGame, Define.UIEvent.Click);
        BindEvent(_toggle.gameObject, ChangeVRType, Define.UIEvent.Click);

        VrBindEvent(_enterButton.gameObject, TryLogin);
        VrBindEvent(_connectButton.gameObject, ConnectToServer);
        VrBindEvent(_signUpButton.gameObject, LoadSignUpPage);
        VrBindEvent(_enterSingleGameButton.gameObject, EnterSingleGame);
        VrBindEvent(_toggle.gameObject, ChangeVRType);

        _signUpButton.gameObject.SetActive(false);
        _enterButton.gameObject.SetActive(false);
        _idText.gameObject.SetActive(false);
        _passwordText.gameObject.SetActive(false);

        //_serverIPText.placeholder.GetComponent<>().text = "127.0.0.1";
        //_serverIPText.text = "127.0.0.1";
        //_nameText.placeholder.GetComponent<TextMeshProUGUI>().text = "Name";
    }

    public void ConnectToServer(PointerEventData data)
    {
        Debug.Log("Try Connect Server!");
        Debug.Log(_serverIPText.text);
        Managers.Network.Connect(_serverIPText.text);
        //연결이 되면 서버에서 Connect 패킷을 보낸다.
        //Connect 패킷을 받으면 아이디와 비번 입력 진행
    }

    public void TryLogin(PointerEventData data)
    {
        Debug.Log("로그인 시도");

        //EnterLobyyPacket 보내기.
        C_Login login = new C_Login();
        login.LoginId = _idText.text;
        login.LoginPassword = _passwordText.text;
        Managers.Network.Send(login);

        //실패시 다시 시도하게 하기.

        //SeverSession OnConnected에서 성공시 LoadLobbyScene 실행됨.
    }

    public void LoadSignUpPage(PointerEventData data)
    {
        _signUP.gameObject.SetActive(true);
    }

    public void EnterSingleGame(PointerEventData data)
    {
        Debug.Log("싱글게임 시작");
        GameMng.I.SingleGame = true;
        Managers.Scene.LoadScene(Define.Scene.SingleGame);
        //GameScene gs = Managers.Scene.CurrentScene as GameScene;
        //gs.Play();
    }

    public void ChangeVRType(PointerEventData data)
    {
        if (_toggle.isOn == true)
        {
            GameMng.I.VR_Type = Define.VRType.Oculus;
            Debug.Log("현재 설정 중인 VR은 Oculus입니다.");
        }
        else
        {
            GameMng.I.VR_Type = Define.VRType.Vive;
            Debug.Log("현재 설정 중인 VR은 Vive입니다.");
        }
    }

    public void ConnectSuccess()
    {
        //서버에 접속 성공했을 때 실행되는 함수.
        _serverIPText.gameObject.SetActive(false);
        _connectButton.gameObject.SetActive(false);

        _signUpButton.gameObject.SetActive(true);
        _enterButton.gameObject.SetActive(true);
        _idText.gameObject.SetActive(true);
        _passwordText.gameObject.SetActive(true);
    }

    public void Active()
    {
        _signUP.gameObject.SetActive(false);
        _enterButton.gameObject.SetActive(true);
        _idText.gameObject.SetActive(true);
        _passwordText.gameObject.SetActive(true);
    }

    public void LoadLobby()
    {
        Debug.Log("LoadLobby");
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }

    // vr 
 public void ConnectToServer()
    {
        Debug.Log("Try Connect Server!");
        Debug.Log(_serverIPText.text);
        Managers.Network.Connect(_serverIPText.text);
        //연결이 되면 서버에서 Connect 패킷을 보낸다.
        //Connect 패킷을 받으면 아이디와 비번 입력 진행
    }

    public void TryLogin()
    {
        Debug.Log("로그인 시도");

        //EnterLobyyPacket 보내기.
        C_Login login = new C_Login();
        login.LoginId = _idText.text;
        login.LoginPassword = _passwordText.text;
        Managers.Network.Send(login);

        //실패시 다시 시도하게 하기.

        //SeverSession OnConnected에서 성공시 LoadLobbyScene 실행됨.
    }

    public void LoadSignUpPage()
    {
        _signUP.gameObject.SetActive(true);
    }

    public void EnterSingleGame()
    {
        Debug.Log("싱글게임 시작");
        GameMng.I.SingleGame = true;
        Managers.Scene.LoadScene(Define.Scene.SingleGame);
        //GameScene gs = Managers.Scene.CurrentScene as GameScene;
        //gs.Play();
    }

    public void ChangeVRType()
    {
        _toggle.isOn = _toggle.isOn ? false : true;
        if (_toggle.isOn == true)
        {
            GameMng.I.VR_Type = Define.VRType.Oculus;
            Debug.Log("현재 설정 중인 VR은 Oculus입니다.");
        }
        else
        {
            GameMng.I.VR_Type = Define.VRType.Vive;
            Debug.Log("현재 설정 중인 VR은 Vive입니다.");
        }
    }
}