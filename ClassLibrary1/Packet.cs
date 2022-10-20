using System;
using System.Net;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Packets
{
    public enum PacketType
    {
        ChatMessage = 0,
        PrivateMessage,
        ClientName,
        ConnectToGame,
        MovePacket,
        LoginPacket,
        ClientListPacket,
        GroupMessage,
        CreateGroup,
        Disconnect
    };

    [Serializable]
    public class Packet
    {
        public PacketType packetType { get; protected set; }
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string message;
        public string sender;
        public ChatMessagePacket(string _message)
        {
            message = _message;
            packetType = PacketType.ChatMessage;
        }
    }

    [Serializable]
    public class ClientNamePacket : Packet
    {
        public string name;
        public ClientNamePacket(string _name)
        {
            name = _name;
            packetType = PacketType.ClientName;
        }
    }

    [Serializable]
    public class PrivateMessage : Packet
    {
        public string message;
        public string recipient;
        public string sender;
        public PrivateMessage(string _message, string _recipient, string _sender)
        {
            message = _message;
            recipient = _recipient;
            sender = _sender;
            packetType = PacketType.PrivateMessage;
        }
    }

    [Serializable]
    public class ConnectToGame : Packet
    {
        public bool isHost;
        public string hostName = null;
        public string playerName;
        public string message;
        public ConnectToGame(bool _isHost, string _hostName)
        {
            isHost = _isHost;
            hostName = _hostName;
            packetType = PacketType.ConnectToGame;
        }
    }

    [Serializable]
    public class MovePacket : Packet
    {
        public float x;
        public float y;
        public MovePacket(float _x, float _y)
        {
            x = _x;
            y = _y;
            packetType = PacketType.MovePacket;
        }
    }

    [Serializable]
    public class LoginPacket : Packet
    {
        public IPEndPoint endPoint;
        public LoginPacket(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
            packetType = PacketType.LoginPacket;
        }
    }

    [Serializable]
    public class CreateGroup : Packet
    {
        public string GroupName;
        public List<string> recipients;
        public string sender;
        public CreateGroup()
        {
            packetType = PacketType.CreateGroup;
        }
    }

    [Serializable]
    public class GroupMessage : Packet
    {
        public string GroupName;
        public List<string> recipients;
        public string sender;
        public string message;
        public GroupMessage()
        {
            packetType = PacketType.GroupMessage;
        }
    }

    [Serializable]
    public class Disconnect : Packet
    {
        public Disconnect()
        {
            packetType = PacketType.Disconnect;
        }
    }
}
