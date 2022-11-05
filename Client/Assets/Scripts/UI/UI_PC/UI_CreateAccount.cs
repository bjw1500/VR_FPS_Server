using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CreateAccount : UI_Base
{
    enum Buttons
    {
        CheckButton,
    }

    enum InputFields
    {
        IDText,
        PasswordText,
    }
    [SerializeField] Button _checkButton;
    [SerializeField] InputField _idText;
    [SerializeField] InputField _passwordText;
    public UI_Login _loginUI;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<InputField>(typeof(InputFields));

        _checkButton = Get<Button>((int)Buttons.CheckButton);
        _idText = Get<InputField>((int)InputFields.IDText);
        _passwordText = Get<InputField>((int)InputFields.PasswordText);
        _loginUI = this.transform.parent.GetComponent<UI_Login>();

        BindEvent(_checkButton.gameObject, CheckSignUP, Define.UIEvent.Click);

        //_serverIPText.placeholder.GetComponent<>().text = "127.0.0.1";
        //_serverIPText.text = "127.0.0.1";
        //_nameText.placeholder.GetComponent<TextMeshProUGUI>().text = "Name";
    }

    public void CheckSignUP(PointerEventData data)
    {
        Debug.Log("SignUP!");

        //작성한 ID와 PassWord 서버로 보내기.
        C_CreateAccount newAccount = new C_CreateAccount();
        newAccount.Id = _idText.text;
        newAccount.Password = _passwordText.text;
        Managers.Network.Send(newAccount);

        //SeverSession OnConnected에서 성공시 LoadLobbyScene 실행됨.
    }

    public void SuccessCreateAccount()
    {
        //서버에서 아이디 생성이 성공적으로 끝났을때 실행되는 함수
        _loginUI.Active();
    }
}
