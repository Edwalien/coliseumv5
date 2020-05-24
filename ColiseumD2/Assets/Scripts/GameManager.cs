using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public GameObject SansPantalon;
    public GameObject AvecPantalon;
    public GameObject claymore;
    public GameObject double_lames;
    public GameObject lance;
    public GameObject marteau;
    public static GameObject weapon;
    public static Vector3 FreePos;
    
    //La fin de la partie
    public static bool End;
    public GameObject EndMenu;
    
    //Le tableau avec les stats
    public Transform ui_leaderboard;
    
    //Pour les kills et deaths
    public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
    public int myind;
    
    public enum EventCodes : byte
    {
        NewPlayer,
        UpdatePlayers,
        ChangeStat
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        End = false; //La fin de la partie
        
        InitializeUI();
        NewPLayer_S(PhotonNetwork.NickName);
        weapon = marteau;
        if (LobbyManager.selectedWeapon == "0")
            weapon = marteau;
        else if (LobbyManager.selectedWeapon == "1")
            weapon = claymore;
        else if (LobbyManager.selectedWeapon == "2")
            weapon = lance;
        else if (LobbyManager.selectedWeapon == "3")
            weapon = double_lames;
        InitialSpawnPoint(); //spawn du joueur
        PhotonNetwork.Instantiate(weapon.name, FreePos, Quaternion.identity);
        
        Invoke("EndGame", 900); //Arreter la partie apres 40 secondes
    }

    private void Update()
    {
        if (!End)
        {
            if (!PauseMenu.GameIsPaused)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (ui_leaderboard.gameObject.activeSelf) ui_leaderboard.gameObject.SetActive(false);
                    else Leaderboard(ui_leaderboard);
                }
            }
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        End = true;
        EndMenu.SetActive(true);
        if (ui_leaderboard.gameObject.activeSelf) ui_leaderboard.gameObject.SetActive(false);
        else Leaderboard(ui_leaderboard);
    }

    public void InitialSpawnPoint()
    {
        Random rndposx = new Random();
        Random rndposz = new Random();
        Random addvalue = new Random();
        
        int posx = rndposx.Next(-30, 30);
        int posz = rndposz.Next(-30, 30);
        int toAdd = addvalue.Next(-40, 40);

        while ((posx < 5 && posx > -5)&&(posz < 5 && posz > -5))
        {
            Random flipcoin = new Random();
            int fliped = flipcoin.Next(-10,10);
            if (fliped < 0)
            {
                posx = posx + toAdd;
            }
            else
            {
                posz = posz + toAdd;
            }
        }

        
        FreePos = new Vector3(posx, 1, posz);
    }
    
    public static void RespawnPoint()
    {
        if (!Physics.Raycast(new Vector3(10, 1, 0), Vector3.up,20))
        {
            FreePos = new Vector3(10, 1, 0);
        }

        else
        {
            if (!Physics.Raycast(new Vector3(-10, 1, 0), Vector3.up,20))
            {
                FreePos = new Vector3(-10, 0, 0);
            }
            else
            {
                if (!Physics.Raycast(new Vector3(0, 1, 10), Vector3.up,20))
                {
                    FreePos = new Vector3(0, 0, 10);
                }
                else
                {
                    if (!Physics.Raycast(new Vector3(0, 1, -10), Vector3.up,20))
                    {
                        FreePos = new Vector3(0, 0, -10);
                    }
                }
            }
        }
    }

    //Fonctions pour le tableau des scores
    private void InitializeUI()
    {
        ui_leaderboard = GameObject.Find("Scoreboard").transform.Find("PlayerList").transform;
    }

    private void Leaderboard(Transform p_1b)
    {
        // nettoyer
        for (int i = 2; i < p_1b.childCount; i++)
        {
            Destroy(p_1b.GetChild(i).gameObject);
        }
        
        //cacher le prefab
        GameObject playercard = p_1b.GetChild(1).gameObject;
        playercard.SetActive(false);
        
        //trier
        List<PlayerInfo> sorted = SortPlayers(playerInfo);
        
        //afficher
        foreach (PlayerInfo a in sorted)
        {
            GameObject newcard = Instantiate(playercard, p_1b) as GameObject;

            newcard.transform.Find("Name").GetComponent<Text>().text = a.nickname;
            newcard.transform.Find("Deaths").GetComponent<Text>().text = a.deaths.ToString();
            newcard.transform.Find("Kills").GetComponent<Text>().text = a.kills.ToString();
            
            newcard.SetActive(true);
        }
        
        //activer
        p_1b.gameObject.SetActive(true);

    }

    private List<PlayerInfo> SortPlayers(List<PlayerInfo> p_info)
    {
        List<PlayerInfo> sorted = new List<PlayerInfo>();

        while (sorted.Count < p_info.Count)
        {
            //set defaults
            short highest = -1;
            PlayerInfo selection = p_info[0];
            
            //grab next highest player
            foreach (PlayerInfo a in p_info)
            {
                if (sorted.Contains(a)) continue;
                if (a.kills > highest)
                {
                    selection = a;
                    highest = a.kills;
                }
            }
            
            //add player
            sorted.Add(selection);
        }

        return sorted;
    }
    
    
    //Pour connaitre les kills et les deaths
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code >= 200) return;

        EventCodes e = (EventCodes) photonEvent.Code;
        object[] o = (object[]) photonEvent.CustomData;

        switch (e)
        {
            case EventCodes.NewPlayer:
                NewPlayer_R(o);
                break;
            case EventCodes.UpdatePlayers:
                UpdatePlayers_R(o);
                break;
            case EventCodes.ChangeStat:
                ChangeStat_R(o);
                break;
        }
    }
    // Les differents events
    public void NewPLayer_S(string nickname)
    {
        object[] package = new object[4];
        package[0] = nickname;
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = (short) 0;
        package[3] = (short) 0;

        PhotonNetwork.RaiseEvent(
            (byte) EventCodes.NewPlayer,
            package,
            new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient},
            new SendOptions {Reliability = true});
    }

    public void NewPlayer_R(object[] data)
    {
        PlayerInfo p = new PlayerInfo(
            (string)data[0],
            (int)data[1],
            (short)data[2],
            (short)data[3]);

        playerInfo.Add(p);

        UpdatePlayers_S(playerInfo);
    }

    public void UpdatePlayers_S(List<PlayerInfo> info)
    {
        object[] package = new object[info.Count];

        for (int i = 0; i < info.Count; i++)
        {
            object[] piece = new object[4];

            piece[0] = info[i].nickname;
            piece[1] = info[i].actor;
            piece[2] = info[i].kills;
            piece[3] = info[i].deaths;

            package[i] = piece;
        }

        PhotonNetwork.RaiseEvent(
            (byte) EventCodes.UpdatePlayers,
            package,
            new RaiseEventOptions {Receivers = ReceiverGroup.All},
            new SendOptions {Reliability = true});
    }

    public void UpdatePlayers_R(object[] data)
    {
        playerInfo = new List<PlayerInfo>();

        for (int i = 0; i < data.Length; i++)
        {
            object[] extract = (object[]) data[i];
            
            PlayerInfo p = new PlayerInfo(
                        (string)extract[0],
                        (int)extract[1],
                    (short)extract[2],
                    (short)extract[3]
                );
            
            playerInfo.Add(p);

            if (PhotonNetwork.LocalPlayer.ActorNumber == p.actor) myind = i;
        }
    }
    
    //Lorsque tu meurs, juste apres le respawn
    //GameManager.ChangeStat_S(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
    //Ensuite il faut destroy le GameObject
    
    //Mettre dans les fonctions des attaques
    //Si j'attaque et je tue, alors
    //GameManager.ChangeStat_S(celui qui a cause les degats, 0, 1);

    public void ChangeStat_S(int actor, byte stat, byte amt)
    {
        object[] package = new object[] {actor, stat, amt};

        PhotonNetwork.RaiseEvent(
            (byte) EventCodes.ChangeStat,
            package,
            new RaiseEventOptions {Receivers = ReceiverGroup.All},
            new SendOptions {Reliability = true}
            );
    }

    public void ChangeStat_R(object[] data)
    {
        int actor = (int) data[0];
        byte stat = (byte) data[1];
        byte amt = (byte) data[2];

        for (int i = 0; i < playerInfo.Count; i++)
        {
            if (playerInfo[i].actor == actor)
            {
                switch (stat)
                {
                    case 0: //kills
                        playerInfo[i].kills += amt;
                        Debug.Log($"Player {playerInfo[i].nickname} : kills = {playerInfo[i].kills}");
                        break;
                    case 1: //deaths
                        playerInfo[i].deaths += amt;
                        Debug.Log($"Player {playerInfo[i].nickname} : deaths = {playerInfo[i].deaths}");
                        break;
                }
                return;
            }
        }
    }

    public override void OnDisconnected (DisconnectCause cause)
    {
        Debug.Log("DECONNECTE DU SERVEUR");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
    }

}
