using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;

namespace Server
{
	public class ClientSession : PacketSession
	{
		public Player MyPlayer { get; set; }
		public int SessionId { get; set; }

		public void Send(IMessage packet)
        {
			string msgName = packet.Descriptor.Name.Replace("_", string.Empty);

			MsgId msgid = (MsgId)Enum.Parse(typeof(MsgId), msgName);

			ushort size = (ushort)packet.CalculateSize();
			byte[] sendBuffer = new byte[size + 4];
			Array.Copy(BitConverter.GetBytes((ushort)size + 4), 0, sendBuffer, 0, sizeof(ushort));
			Array.Copy(BitConverter.GetBytes((ushort)msgid), 0, sendBuffer, 2, sizeof(ushort));
			Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

			Send(new ArraySegment<byte>(sendBuffer));

		}

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");

            {
				S_Connected connectedPacket = new S_Connected();
				Send(connectedPacket);
            }

			/*
			 *현재 제작중인 접속 방식
			 * 클라이언트가 아이디와 비번을 보낸다.
			 * 서버에서는 계정을 확인후 loginOk 패킷을 보내고
			 * LoginHandler에서 플레이어 생성후 로비로 입장시킨다.
			 * 
			 */
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint)
		{

			if (MyPlayer != null)
			{
				if (MyPlayer.Room != null)
				{
					GameRoom gameRoom = MyPlayer.Room;
					gameRoom.Push(gameRoom.LeaveGame, MyPlayer.Info.ObjectId);
				}

				if (MyPlayer.LobbyRoom != null)
				{
					LobbyRoom waitRoom = MyPlayer.LobbyRoom;
					waitRoom.LeaveGame(MyPlayer.Info.ObjectId);
				}
			}

            SessionManager.Instance.Remove(this);

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
