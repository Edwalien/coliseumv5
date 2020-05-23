using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Coliseum
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public TMPro.TMP_Dropdown resolutionDropdown;

        private Resolution[] resolutions;

        void Start()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            for (int i = 0; i < resolutions.Length; i++) //Convertir le tableau en liste
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);
            }

            resolutionDropdown.AddOptions(options);
        }

        public void SetVolume(float volume) //Volume principal
        {
            Debug.Log("Le volume est " + volume);

            audioMixer.SetFloat("volume", volume);
        }

        public void SetQuality(int qualityIndex) // 0 pour TRES BAS, 1 pour BAS etc..
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen) //Plein ecran
        {
            Screen.fullScreen = isFullscreen;
        }
    }
}
