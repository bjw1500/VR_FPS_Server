////플레이어 닉네임 가져오기
//public class userName : MonoBehaviourPunCallbacks, IPunObservable
//{
//    //TMP
//    public TextMeshProUGUI nickName;

//    void Start()
//    {
//        //입력해서 저장된 userid지우는 코드 
//        PlayerPrefs.DeleteAll();

//        //플레이어의 닉네임 저장
//        nickName.text = photonView.Owner.NickName;
//    }

//    //플레이어 닉네임 출력-> player수 만큼 버튼 배정(동적할당?)
//    public override void OnJoinedRoom()
//    {
//        foreach (Player p in PhotonNetwork.PlayerList)
//        {
//            //플레이어 수 만큼 button 배정
//            return p.Nickname;
//        }
//    }

//    //방에 들어왔을 때 목록에 업데이트하는 함수 ->해당 플레이어 담을 버튼 생성
//    public override void OnPlayerEnteredRoom(Player newPlayer)
//    {
//        // Add a button for the new player.
//        // You can access the new player's nick name by using newPlayer.NickName.
//    }
//    //방에서 나갔을때 해당플레이어 버튼 삭제되게 하기
//    public override void OnPlayerLeftRoom(Player otherPlayer)
//    {
//        // Remove a button that is related to the player who just left the room.
//    }

//}