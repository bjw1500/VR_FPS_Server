using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Data;
using Server.DB;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PacketHandler
{
	public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		C_Move MovePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;


		//멀티쓰레드 오류 방지
		//어디선가 MyPlayer을 Null로 밀어줘도, player는 계속 참조하고 있기 때문에 크래쉬 문제 해결에 도움이 됨.
		Player player = clientSession.MyPlayer;
		if (player == null)
			return;
		GameRoom gameRoom = clientSession.MyPlayer.Room;
		if (gameRoom == null)
			return;

	//	Console.WriteLine($"Player{player.Info.ObjectId} " +
	//		$"MoveDir {MovePacket.MovementInfo.CameraPosInfo.PosX}," +
	//		$"{MovePacket.MovementInfo.CameraPosInfo.PosY}," +
	//		$"{MovePacket.MovementInfo.CameraPosInfo.PosZ}");
	//	Console.WriteLine($"Player{player.Info.ObjectId} " +
	//$"transformPosition {MovePacket.MovementInfo.PlayerPosInfo.PosX}," +
	//$"{MovePacket.MovementInfo.PlayerPosInfo.PosY}," +
	//$"{MovePacket.MovementInfo.PlayerPosInfo.PosZ}");

		gameRoom.HandleMove(player, MovePacket);


	}

	public static void C_SkillHandler(PacketSession session, IMessage packet)
	{
		C_Skill skillPacket = packet as C_Skill;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;
		GameRoom gameRoom = clientSession.MyPlayer.Room;
		if (gameRoom == null)
			return;

		gameRoom.Push(gameRoom.HandleSkill, player, skillPacket);
	}

	public static void C_ChangeHpHandler(PacketSession session, IMessage packet)
	{

		C_ChangeHp changeHpPacket = packet as C_ChangeHp;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom gameRoom = clientSession.MyPlayer.Room;
		if (gameRoom == null)
			return;

		gameRoom.Push(gameRoom.HandleHp, player, changeHpPacket);
	}

	public static void C_SpawnHandler(PacketSession session, IMessage packet)
	{
		C_Spawn SpawnPacket = packet as C_Spawn;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;
		GameRoom gameRoom = clientSession.MyPlayer.Room;
		if (gameRoom == null)
			return;

		foreach (var unit in SpawnPacket.Info)
		{
			Console.WriteLine(unit.Name + "이 생성 되었습니다.");
		}


		gameRoom.Push(gameRoom.HandleSpawn, player, SpawnPacket);

	}

	public static void C_DespawnHandler(PacketSession session, IMessage packet)
	{
		C_Despawn DespawnPacket = packet as C_Despawn;
		ClientSession clientSession = session as ClientSession;


		Player player = clientSession.MyPlayer;
		if (player == null)
			return;
		GameRoom gameRoom = clientSession.MyPlayer.Room;
		if (gameRoom == null)
			return;

		foreach(ObjectInfo info in DespawnPacket.Info)
        {
			gameRoom.Push(gameRoom.LeaveGame, info.ObjectId);
        }


	}

	public static void C_ChangeWeaponHandler(PacketSession session, IMessage packet)
	{
		C_ChangeWeapon changePacket = packet as C_ChangeWeapon;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;
		GameRoom gameRoom = clientSession.MyPlayer.Room;
		if (gameRoom == null)
			return;

		gameRoom.Push(gameRoom.HandleChangeWeapon, player, changePacket);

	}

	public static void C_GetWeaponHandler(PacketSession session, IMessage packet)
	{
		C_GetWeapon GetPacket = packet as C_GetWeapon;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;
		GameRoom gameRoom = clientSession.MyPlayer.Room;
		if (gameRoom == null)
			return;

		//플레이어가 무기를 먹었다.
		gameRoom.Push(gameRoom.HandleGetWeapon, player, GetPacket);
	}





	/// Pre - Game
	/// 

	public static void C_StartGameHandler(PacketSession session, IMessage packet)
	{
		C_StartGame startPacket = packet as C_StartGame;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		LobbyRoom room = player.LobbyRoom;
		if (room == null)
			return;


		room.StartGame(startPacket);
	}

	public static void C_MapLoadingFinishHandler(PacketSession session, IMessage packet)
    {
		C_MapLoadingFinish finishPacket = packet as C_MapLoadingFinish;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		LobbyRoom room = player.LobbyRoom;
		if (room == null)
			return;

		room.LoadingFinish(player.ObjectId);

	}

	public static void C_EnterLobbyOkHandler(PacketSession session, IMessage packet)
	{
		C_EnterLobbyOk startPacket = packet as C_EnterLobbyOk;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		LobbyRoom room = player.LobbyRoom;
		if (room == null)
			return;
		//player.Info.Slot = startPacket.Slot;

		room.EnterLobbyRoom(player);

	}

	public static void C_SelectCharacterHandler(PacketSession session, IMessage packet)
	{
		C_SelectCharacter selectInfo = packet as C_SelectCharacter;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		LobbyRoom room = player.LobbyRoom;
		if (room == null)
			return;

		//player.Info.Slot = startPacket.Slot;

		room.SelectCharacter(selectInfo);

	}


	///////////////////////////////////////////로그인
	/*
	 * 
	 * 클라이언트 회원가입창 만듬
	 * 확인 버튼을 누르면 클라에서 비번과 아이디를 보냄.
	 * 서버에서는 날라온 패킷에서 아이디를 확인후 중복된 값이 있는지 찾음.
	 * 없으면 새로운 계정을 생성해줌.
	 * 
	 */



	public static void C_CreateAccountHandler(PacketSession session, IMessage packet)
	{
		C_CreateAccount createAccountPacket = packet as C_CreateAccount;
		ClientSession clientSession = session as ClientSession;

		//플레이어가 회원가입을 통해 Id와 password를 패킷으로 보낸다.
		//그러면 서버가 해야할 역할은?
		//데베에서 아이디를 검색해보고 없으면 생성, 있으면 불가능패킷을 보낸다.

		using(AppDbContext db = new AppDbContext())
        {
			AccountDb findAccount = db.Accounts.
				Where(a => a.AccountName == createAccountPacket.Id).FirstOrDefault();

			//만약 사용자가 만들려고 하는 아이디가 존재한다면,
			if(findAccount != null)
            {
				S_FailedCreateAccount failedPacket = new S_FailedCreateAccount();
				failedPacket.Information = "이미 존재하는 아이디입니다.";
				clientSession.Send(failedPacket);
				return;
				//실패 패킷 보내기
            }


            //통과했으면 새로운 아이디를 만들어준다.

            AccountDb newAccount = new AccountDb()
            {
                AccountName = createAccountPacket.Id,
				AccountPassword = createAccountPacket.Password,
            };
            db.Accounts.Add(newAccount);
            db.SaveChanges();
			Console.WriteLine($"새로운 계정 {createAccountPacket.Id}가 생성되었습니다.");

			//생성 성공 패킷 보내기.
			S_SuccessCreateAccount successPacket = new S_SuccessCreateAccount();
			successPacket.Information = "계정이 성공적으로 생성되었습니다.";
			clientSession.Send(successPacket);


		}


	}


	public static void C_LoginHandler(PacketSession session, IMessage packet)
	{
		C_Login loginPacket = packet as C_Login;
		ClientSession clientSession = session as ClientSession;

		Console.WriteLine($"Login id{loginPacket.LoginId}");





		//로그인 성공 패킷
		Console.WriteLine($"{loginPacket.LoginId}가 로그인했습니다.");
		S_Login loginOk = new S_Login();
		clientSession.Send(loginOk);

		//성공했으니 로비로 입장시키자.
		clientSession.MyPlayer = ObjectManager.Instance.Add<Player>();
		{
			clientSession.MyPlayer.Info.Player.Name = $"Player_{loginPacket.LoginId}";
			clientSession.MyPlayer.Session = clientSession;

		}

		//로비에 넣는다.
		LobbyRoom lobby = RoomManager.Instance.WaitRoomFind(1);
		lobby.EnterLobbyScene(clientSession.MyPlayer);




		////보안 체크를 만들어야 할까?


		////아이디와 비밀번호를 확인하게한 이후,

		////로비창으로 이동시킨다.

		//using (AppDbContext db = new AppDbContext())
  //      {
		//	AccountDb findAccount = db.Accounts.
		//		Where(a => a.AccountName == loginPacket.LoginId).FirstOrDefault();
		
		//	//아이디 맞을시 진입.
		//	if(findAccount != null)
  //          {
		//		//비번이 틀리다면.
		//		if (findAccount.AccountPassword != loginPacket.LoginPassword)
		//		{
		//			Console.WriteLine($"{loginPacket.LoginId}의 비번이 틀렸습니다.");
		//			//TODO로그인 실패 패킷 만들기

		//			S_LoginFailed failed = new S_LoginFailed();
		//			failed.Information = "비번이 틀립니다.";
		//			clientSession.Send(failed);

		//			return;
		//		}
		//		//로그인 성공 패킷
		//		Console.WriteLine($"{loginPacket.LoginId}가 로그인했습니다.");
		//		S_Login loginOk = new S_Login();
		//		clientSession.Send(loginOk);

		//		//성공했으니 로비로 입장시키자.
		//		clientSession.MyPlayer = ObjectManager.Instance.Add<Player>();
  //              {
		//			clientSession.MyPlayer.Info.Player.Name = $"Player_{loginPacket.LoginId}";
		//			clientSession.MyPlayer.Session = clientSession;
					
		//		}

  //              //로비에 넣는다.
  //              LobbyRoom lobby = RoomManager.Instance.WaitRoomFind(1);
  //              lobby.EnterLobbyScene(clientSession.MyPlayer);




  //          }
		//	else
  //          {
		//		//입력된 아이디가 DB에 없는 상황.
		//		//로그인 실패 패킷 보내기.
		//		S_LoginFailed failed = new S_LoginFailed();
		//		failed.Information = "아이디가 틀립니다.";
		//		clientSession.Send(failed);

		//		//AccountDb newAccount = new AccountDb()
		//		//{
		//		//	AccountName = loginPacket.LoginId,
		//		//};
		//		//db.Accounts.Add(newAccount);
		//		//db.SaveChanges();

		//		//S_Login loginOk = new S_Login() { LoginOk = 1};
		//		//clientSession.Send(loginOk);



		//	}


  //      }

	}






}
