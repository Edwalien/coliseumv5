using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
//using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject EndMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        EndMenu.SetActive(false);
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        Debug.Log("Chargement du menu...");
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    { 
        PhotonNetwork.Disconnect();
       while (PhotonNetwork.IsConnected)
           yield return null;
       SceneManager.LoadScene("menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        PhotonNetwork.LeaveRoom();
        Application.Quit();
    }
}
