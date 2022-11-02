using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;

        Debug.Log("로그인 화면에 입장하셨습니다.");

    }
    public override void Clear()
    {
        
    }
}
