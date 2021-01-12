using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

public class TempNetManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();

        PhotonPeer.RegisterType(typeof(PieceInfo), (byte)'W', SerializePiece, DeserializePiece);
    }

    public static readonly byte[] memPiece = new byte[4 * 4];
    private static short SerializePiece(StreamBuffer outStream, object customobject)
    {
        PieceInfo pieceinfo = (PieceInfo)customobject;
        lock (memPiece)
        {
            byte[] bytes = memPiece;
            int index = 0;
            Protocol.Serialize((int)pieceinfo.playerType, bytes, ref index);
            Protocol.Serialize((int)pieceinfo.chessPiece, bytes, ref index);
            Protocol.Serialize(pieceinfo.Index.x, bytes, ref index);
            Protocol.Serialize(pieceinfo.Index.y, bytes, ref index);
            outStream.Write(bytes, 0, 4 * 4);
        }

        return 4 * 4;
    }
    private static object DeserializePiece(StreamBuffer inStream, short length)
    {
        PieceInfo pieceinfo = new PieceInfo();
        lock (memPiece)
        {
            inStream.Read(memPiece, 0, 4 * 4);
            int index = 0;
            int value;
            Protocol.Deserialize(out value, memPiece, ref index);
            pieceinfo.playerType = (PLAYERTYPE)value;
            Protocol.Deserialize(out value, memPiece, ref index);
            pieceinfo.chessPiece = (CHESSPIECE)value;
            Protocol.Deserialize(out pieceinfo.Index.x, memPiece, ref index);
            Protocol.Deserialize(out pieceinfo.Index.y, memPiece, ref index);
        }

        return pieceinfo;
    }



    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);

    public override void OnJoinedRoom() 
    {
        //PhotonNetwork.Instantiate("ChessSet", Vector3.zero, Quaternion.identity);
    }



}
