using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMPro.TMP_Text LogText;
    [SerializeField] private TMPro.TMP_Text _roomName;
    public static string selectedWeapon;

    void Start()
    {
        Log("Votre nom est " + PhotonNetwork.NickName);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Log("Connecté au serveur");

        PhotonNetwork.JoinLobby(); // Il faut peut etre l'enlever
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_roomName.text, options, TypedLobby.Default);
        //PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions{MaxPlayers = 4});
    }

    public void Revenir()
    {
        PhotonNetwork.NetworkStatisticsReset();
        Debug.Log("Deconnecte du serveur");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Log("Room " + PhotonNetwork.CurrentRoom.Name + " a été créée.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log("La création de la room a échoué: " + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Log("Le joueur " + newPlayer.NickName + " a rejoint la room.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Log("Le joueur " + otherPlayer.NickName + " a quitté la room.");
    }

    public override void OnJoinedRoom()
    {
        Log("Vous avez rejoint la room " + PhotonNetwork.CurrentRoom.Name);
        //PhotonNetwork.LoadLevel("Jeu justoin");
    }

    

    public void Log(string message)
    {
        Debug.Log(message);
        LogText.text += "\n";
        LogText.text += message;
    }
    
    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            selectedWeapon = "0";  // marteau
        }
        if (val == 1)
        {
            selectedWeapon = "1";  // claymore
        }
        if (val == 2)
        {
            selectedWeapon = "2";  // lance
        }
        if (val == 3)
        {
            selectedWeapon = "3"; // double_lames
        }
        
    }

}
