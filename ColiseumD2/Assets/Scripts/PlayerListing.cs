using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _text;
    public Player Player { get; private set; }
    
    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _text.text = player.NickName;
    }
}
