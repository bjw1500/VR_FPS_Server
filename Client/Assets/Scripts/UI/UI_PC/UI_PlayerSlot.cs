using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerSlot : UI_Base
{

    enum Texts
    {
        PlayerNameText,
        KillCountText,
        DeathCountText
    }

    public TextMeshProUGUI playerNameText;    //이름      //
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI deathCountText;

    //슬롯에 연동된 플레이어의 정보
    public int OjbectId;

    public override void Init()
    {

    }

    public void Awake()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));

        playerNameText = Get<TextMeshProUGUI>((int)Texts.PlayerNameText);
        killCountText = Get<TextMeshProUGUI>((int)Texts.KillCountText);
        deathCountText = Get<TextMeshProUGUI>((int)Texts.DeathCountText);
    }

    public void Refresh()
    {
        //name 왜 넓값인지 확인하기.

        ObjectInfo info = null;
        if (Managers.Object._players.TryGetValue(OjbectId, out info) == false)
            return;

        playerNameText.text = info.Player.Name;
        killCountText.text = info.Player.Kill.ToString();
        deathCountText.text = info.Player.Death.ToString();
        //혹시 필요하다면 체력이나 총 데미지량? 대충 이런 것들도 추가 가능.
    }

}
