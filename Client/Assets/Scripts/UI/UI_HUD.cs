using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : UI_Base
{
    enum Texts
    {
        ActionText,
    }

    public Text _actionText;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        _actionText = Get<Text>((int)Texts.ActionText);
    }
}
