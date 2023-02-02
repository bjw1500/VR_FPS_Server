using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;
        ServerSession serverSession = session as ServerSession;

        //플레이어 본인의 캐릭터를 생성.

        Debug.Log($"{enterGamePacket.Player.Name} EnterGame");
        Managers.Object.Add(enterGamePacket.Player, true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leavePacket = packet as S_LeaveGame;
        ServerSession serverSession = session as ServerSession;

        Managers.Object.RemoveMyPlayer();
    }


    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;
        ServerSession serverSession = session as ServerSession;

        foreach (ObjectInfo info in spawnPacket.Info)
        {
            Managers.Object.Add(info);
        }
    }

    public static void S_ItemSpawnHandler(PacketSession session, IMessage packet)
    {
        S_ItemSpawn spawnPacket = packet as S_ItemSpawn;
        ServerSession serverSession = session as ServerSession;

        foreach (ItemInfo info in spawnPacket.Infos)
        {
            Managers.Object.AddItem(info);
        }
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;
        ServerSession serverSession = session as ServerSession;

        foreach (var objectId in despawnPacket.ObjectIds)
        {
            Managers.Object.Remove(objectId);
        }
    }

    public static void S_ObjectSpawnHandler(PacketSession session, IMessage packet)
    {
        S_ObjectSpawn spawnPacket = packet as S_ObjectSpawn;
        ServerSession serverSession = session as ServerSession;

      
        foreach (ObjectInfo info in spawnPacket.Infos)
        {
            Managers.Object.AddObject(info);
        }
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;
        ServerSession serverSession = session as ServerSession;

        GameObject go = Managers.Object.Find(movePacket.PlayerId);
        if (go == null)
            return;

        CharacterRemoteControllerVR rc = go.GetComponent<CharacterRemoteControllerVR>();
        if (rc == null)
            return;
        rc.Info.MovementInfo = movePacket.MovementInfo;
    }

    public static void S_SkillHandler(PacketSession session, IMessage packet)
    {
        S_Skill skillPacket = packet as S_Skill;
        ServerSession serverSession = session as ServerSession;

        GameObject go = Managers.Object.Find(skillPacket.Info.ObjectId);
        if (go == null)
            return;

        CharacterRemoteControllerVR cc = go.GetComponent<CharacterRemoteControllerVR>();
        if (cc == null)
            return;

        cc.UseSkill(skillPacket);
    }

    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changeHpPacket = packet as S_ChangeHp;
        ServerSession serverSession = session as ServerSession;

        GameObject go = Managers.Object.Find(changeHpPacket.ObjectId);
        if (go == null)
            return;

        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return;

        bc.Hp = changeHpPacket.Hp;

    }


    public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;
        ServerSession serverSession = session as ServerSession;

        //죽으면 배그처럼 기절한 상태로 만들어준다.

        GameObject go = Managers.Object.Find(diePacket.ObjectId);
        if (go == null)
            return;
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return;
        bc.OnDead(diePacket.Attacker); 
    }

    public static void S_ChangeWeaponHandler(PacketSession session, IMessage packet)
    {
        S_ChangeWeapon changePacket = packet as S_ChangeWeapon;
        ServerSession serverSession = session as ServerSession;

        GameObject go = Managers.Object.Find(changePacket.PlayerId);
        if (go == null)
            return;

        CharacterRemoteControllerVR cc = go.GetComponent<CharacterRemoteControllerVR>();
        if (cc == null)
            return;

        cc.ChangeWeapon(changePacket.Slot);

    }

    public static void S_GetWeaponHandler(PacketSession session, IMessage packet)
    {
        S_GetWeapon getWeaponPacket = packet as S_GetWeapon;
        ServerSession serverSession = session as ServerSession;


        GameObject go = Managers.Object.Find(getWeaponPacket.PlayerId);
        if (go == null)
            return;

        CharacterRemoteControllerVR cc = go.GetComponent<CharacterRemoteControllerVR>();
        if (cc == null)
            return;

        Item item = null;
        if (Managers.Object._items.TryGetValue(getWeaponPacket.Info.ObjectId, out item) == false)
            return;

        cc.GetWeapon(item);

    }

    public static void S_UpdatePlayerInfoHandler(PacketSession session, IMessage packet)
    {
        S_UpdatePlayerInfo UpdatePacket = packet as S_UpdatePlayerInfo;
        ServerSession serverSession = session as ServerSession;

        foreach(ObjectInfo info in UpdatePacket.Infos)
        {
            Managers.Object.UpdatePlayerInfo(info);
        }

    }





    //Pre - Game


    public static void S_EnterWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        S_EnterWaitingRoom enterPacket = packet as S_EnterWaitingRoom;
        ServerSession serverSession = session as ServerSession;

        GameObject go = GameObject.Find("LobbyUI");
        if (go == null)
            return;

        UI_Lobby ui = go.GetComponent<UI_Lobby>();
        if (ui == null)
            return;

        ui.UpdateRoom(enterPacket);

    }

    public static void S_LeaveWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        S_LeaveWaitingRoom diePacket = packet as S_LeaveWaitingRoom;
        ServerSession serverSession = session as ServerSession;
    }

    public static void S_StartGameHandler(PacketSession session, IMessage packet)
    {
        S_StartGame startPacket = packet as S_StartGame;
        ServerSession serverSession = session as ServerSession;

        GameObject go = GameObject.Find("LobbyUI");

        if (go == null)
            return;

        UI_Lobby ui = go.GetComponent<UI_Lobby>();

        if (ui == null)
            return;

        ui.LoadScene(startPacket.MapId);
        //Managers.Object.slotNumber = startGamePacket.Slot;

        Debug.Log("게임을 시작합니다!");
    }

    public static void S_EnterLobbyOkHandler(PacketSession session, IMessage packet)
    {
        S_EnterLobbyOk okPacket = packet as S_EnterLobbyOk;
        ServerSession serverSession = session as ServerSession;

        GameObject go = GameObject.Find("LoginUI");

        if (go == null)
            return;

        UI_Login ui = go.GetComponent<UI_Login>();

        if (ui == null)
            return;

        ui.LoadLobby();

        //로비에 입장했다면?
    }

    public static void S_SelectCharacterHandler(PacketSession session, IMessage packet)
    {
        S_SelectCharacter SelectPacket = packet as S_SelectCharacter;
        ServerSession serverSession = session as ServerSession;

        GameObject go = GameObject.Find("LobbyUI");

        if (go == null)
            return;

        UI_Lobby ui = go.GetComponent<UI_Lobby>();

        if (ui == null)
            return;

        ui.UpdateSlot(SelectPacket);
    }

    public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {
        S_Connected connectedPacket = packet as S_Connected;
        ServerSession serverSession = session as ServerSession;

        Debug.Log("서버 접속에 성공했습니다...");
        //C_Login loginPacket = new C_Login();
        //loginPacket.UniqueId = SystemInfo.deviceUniqueIdentifier;
        //Managers.Network.Send(loginPacket);

        GameObject go = GameObject.Find("LoginUI");

        if (go == null)
            return;

        UI_Login ui = go.GetComponent<UI_Login>();

        if (ui == null)
            return;
        ui._informationText.text = "서버 접속에 성공했습니다.";
        ui.ConnectSuccess();
        Managers.Network._connect = true;
    }

    public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        S_Login loginPacket = packet as S_Login;
        ServerSession serverSession = session as ServerSession;

        Debug.Log($"LoginOk: {loginPacket.LoginOk})");

        //로비에 입장시키기.
        GameObject go = GameObject.Find("LoginUI");

        if (go == null)
            return;

        UI_Login ui = go.GetComponent<UI_Login>();

        if (ui == null)
            return;
        ui._informationText.text = "로그인 성공";
    }


    public static void S_LoginFailedHandler(PacketSession session, IMessage packet)
    {
        S_LoginFailed loginFailedPacket = packet as S_LoginFailed;
        ServerSession serverSession = session as ServerSession;

        //팝업창을 띄울까?

        GameObject go = GameObject.Find("LoginUI");

        if (go == null)
            return;

        UI_Login ui = go.GetComponent<UI_Login>();

        if (ui == null)
            return;
        ui._informationText.text = $"로그인 실패: {loginFailedPacket.Information})";
        Debug.Log($"로그인 실패: {loginFailedPacket.Information})");

    }

    public static void S_FailedCreateAccountHandler(PacketSession session, IMessage packet)
    {
        S_FailedCreateAccount failedCreate = packet as S_FailedCreateAccount;
        ServerSession serverSession = session as ServerSession;

        Debug.Log($"LoginOk: {failedCreate.Information})");

        GameObject go = GameObject.Find("LoginUI");

        if (go == null)
            return;

        UI_Login ui = go.GetComponent<UI_Login>();

        if (ui == null)
            return;
        ui._informationText.text = $"아이디 생성 실패: {failedCreate.Information})";
        Debug.Log($"아이디 생성 실패: {failedCreate.Information})");
    }

    public static void S_SuccessCreateAccountHandler(PacketSession session, IMessage packet)
    {
        S_SuccessCreateAccount successCreate = packet as S_SuccessCreateAccount;
        ServerSession serverSession = session as ServerSession;

        Debug.Log($"LoginOk: {successCreate.Information})");

        GameObject go = GameObject.Find("CreateAccountUI");

        if (go == null)
            return;

        UI_CreateAccount ui = go.GetComponent<UI_CreateAccount>();

        if (ui == null)
            return;

        ui._loginUI._informationText.text = $"{successCreate.Information})";
        Debug.Log($"{successCreate.Information})");

        ui.SuccessCreateAccount();
    }
}
