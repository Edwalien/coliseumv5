using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Coliseum
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject InputField;

        public void QuitGame() // Quitter le jeu
        {
            Debug.Log("Vous avez quitte le jeu !");
            Application.Quit();
        }

        public void PlayGame()
        {
            PhotonNetwork.NickName = InputField.GetComponent<TMPro.TMP_Text>().text;
        }

    }
}
