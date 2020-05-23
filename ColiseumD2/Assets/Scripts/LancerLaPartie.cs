using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LancerLaPartie : MonoBehaviour
{
    public void Lancer_La_Partie()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (DropDownArena.arenaID == 0)
                PhotonNetwork.LoadLevel("Arene 1");
            else
            {
                PhotonNetwork.LoadLevel("Arene 2");
            }
        }
    }
}
