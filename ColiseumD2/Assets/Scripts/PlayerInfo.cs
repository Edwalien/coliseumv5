using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class PlayerInfo
{
    public string nickname;
    public int actor;
    public short kills;
    public short deaths;

    public PlayerInfo(string n, int a, short k, short d)
    {
        this.nickname = n;
        this.actor = a;
        this.kills = k;
        this.deaths = d;
    }

}
