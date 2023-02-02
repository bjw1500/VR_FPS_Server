using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Google.Protobuf;

public class NetworkManager
{
	ServerSession _session = new ServerSession();
	public bool _connect = false;
	

	public void Send(ArraySegment<byte> packet)
	{
		_session.Send(packet);
	}

	public void Send(IMessage packet)
	{
		_session.Send(packet);
	}

	public void Init()
	{
		

		// DNS (Domain Name System)
		//string host = Dns.GetHostName();
		//IPHostEntry ipHost = Dns.GetHostEntry(host);
		//IPAddress ipAddr = IPAddress.Parse(IpAddress);
		//IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

		//Connector connector = new Connector();

		//connector.Connect(endPoint,
		//	() => { return _session; },
		//	1);
	}

	public void Connect(string IpAddress)
	{

        Debug.Log($"Connect to {IpAddress}");
        IPAddress ipAddr = IPAddress.Parse(IpAddress);
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 12345);
        Connector connector = new Connector();
        connector.Connect(endPoint,
            () => { return _session; },
            1);
    }

	public void LocalConnect()
    {
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = ipHost.AddressList[0];
		IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

		Connector connector = new Connector();

		connector.Connect(endPoint,
			() => { return _session; },
			1);
	}


	public void Update()
	{
		List<PacketMessage> list = PacketQueue.Instance.PopAll();
		foreach (PacketMessage packet in list)
		{
			Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
			if (handler != null)
				handler.Invoke(_session, packet.Message);
		}
	}
}
