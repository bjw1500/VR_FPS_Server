using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler {get; set;}

	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.CSpawn, MakePacket<C_Spawn>);
		_handler.Add((ushort)MsgId.CSpawn, PacketHandler.C_SpawnHandler);		
		_onRecv.Add((ushort)MsgId.CDespawn, MakePacket<C_Despawn>);
		_handler.Add((ushort)MsgId.CDespawn, PacketHandler.C_DespawnHandler);		
		_onRecv.Add((ushort)MsgId.CMove, MakePacket<C_Move>);
		_handler.Add((ushort)MsgId.CMove, PacketHandler.C_MoveHandler);		
		_onRecv.Add((ushort)MsgId.CSkill, MakePacket<C_Skill>);
		_handler.Add((ushort)MsgId.CSkill, PacketHandler.C_SkillHandler);		
		_onRecv.Add((ushort)MsgId.CChangeHp, MakePacket<C_ChangeHp>);
		_handler.Add((ushort)MsgId.CChangeHp, PacketHandler.C_ChangeHpHandler);		
		_onRecv.Add((ushort)MsgId.CStartGame, MakePacket<C_StartGame>);
		_handler.Add((ushort)MsgId.CStartGame, PacketHandler.C_StartGameHandler);		
		_onRecv.Add((ushort)MsgId.CEnterLobbyOk, MakePacket<C_EnterLobbyOk>);
		_handler.Add((ushort)MsgId.CEnterLobbyOk, PacketHandler.C_EnterLobbyOkHandler);		
		_onRecv.Add((ushort)MsgId.CLogin, MakePacket<C_Login>);
		_handler.Add((ushort)MsgId.CLogin, PacketHandler.C_LoginHandler);		
		_onRecv.Add((ushort)MsgId.CCreateAccount, MakePacket<C_CreateAccount>);
		_handler.Add((ushort)MsgId.CCreateAccount, PacketHandler.C_CreateAccountHandler);		
		_onRecv.Add((ushort)MsgId.CChangeWeapon, MakePacket<C_ChangeWeapon>);
		_handler.Add((ushort)MsgId.CChangeWeapon, PacketHandler.C_ChangeWeaponHandler);		
		_onRecv.Add((ushort)MsgId.CGetWeapon, MakePacket<C_GetWeapon>);
		_handler.Add((ushort)MsgId.CGetWeapon, PacketHandler.C_GetWeaponHandler);		
		_onRecv.Add((ushort)MsgId.CMapLoadingFinish, MakePacket<C_MapLoadingFinish>);
		_handler.Add((ushort)MsgId.CMapLoadingFinish, PacketHandler.C_MapLoadingFinishHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
		
		if(CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{

		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			action.Invoke(session, pkt);

		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}