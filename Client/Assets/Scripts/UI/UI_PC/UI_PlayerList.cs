using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerList : UI_Base
{
    //혹시라도 나중에 오류가 나면 그리드 연결은 프리팹에서 해주자.
    public GridLayoutGroup redTeam;
    public GridLayoutGroup blueTeam;
   
    const int maxPlayer = 4;
    List<UI_PlayerSlot> playerSlots = new List<UI_PlayerSlot>();

    public override void Init()
    {
        GameObject redTeamList = transform.Find("RedPlayerList").gameObject;
        GameObject blueTeamList = transform.Find("BluePlayerList").gameObject;
        redTeam = redTeamList.GetComponent<GridLayoutGroup>();
        blueTeam = blueTeamList.GetComponent<GridLayoutGroup>();

        Managers.Object.PlayerList = this;
    }
    
    public void SetList()
    {
        //ObjectManager에서 유저 정보를 받아와서 리스트에 표시한다.
        //유저가 접속하면 슬롯을 알아서 만들어주게 할까?
        //팀 Id에 따라 Blue 그리드와 Red 그리드에 나누어준다.

        foreach(Transform child in redTeam.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in blueTeam.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ObjectInfo player in Managers.Object._players.Values)
        {

            UI_PlayerSlot slot;

            //0 Red | 1 Blue
            if (player.TeamId == 0)
            {
                //플레이어가 레드팀이라면, 레드팀 리스트에 텍스트 생성 후 이어준다.
                GameObject go = Managers.Resource.Instantiate("UI/PlayerList/PlayerSlot", redTeam.transform);
                slot = go.GetComponent<UI_PlayerSlot>();
                slot.info = player;
            }
            else
            {
                //플레이어가 블루팀이라면,
                GameObject go = Managers.Resource.Instantiate("UI/PlayerList/PlayerSlot", blueTeam.transform);
                slot = go.GetComponent<UI_PlayerSlot>();
                slot.info = player;
            }

            playerSlots.Add(slot);
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        //나중에 완성되면 누군가 킬을 할때나 죽을 때마다 RefreshUI 호출하게 하자.
        foreach(UI_PlayerSlot player in playerSlots)
        {
            player.Refresh();
        }
    }
}

/*
//플레이어 닉네임 가져오기
public class userName : MonoBehaviourPunCallbacks, IPunObservable {
    //TMP
    public TextMeshProUGUI nickName;
 
    void Start () {
        //입력해서 저장된 userid지우는 코드 
        PlayerPrefs.DeleteAll();

        //플레이어의 닉네임 저장
        nickName.text = photonView.Owner.NickName;
    }

  //플레이어 닉네임 출력-> player수 만큼 버튼 배정(동적할당?)
  public override void OnJoinedRoom()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
   //플레이어 수 만큼 button 배정
            return p.Nickname;
        }
    }

    //방에 들어왔을 때 목록에 업데이트하는 함수 ->해당 플레이어 담을 버튼 생성
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Add a button for the new player.
        // You can access the new player's nick name by using newPlayer.NickName.
    }
    //방에서 나갔을때 해당플레이어 버튼 삭제되게 하기
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Remove a button that is related to the player who just left the room.
    }

}
 */