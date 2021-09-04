using UnityEngine;
using UnityEngine.Analytics;

namespace GameStuff
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject settingsPanel;
        
        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetButtonDown("Cancel")) return;
            
            Continue();
        }

        public void QuitingGame()
        {
            Application.Quit();
        }

        public void SettingToggle()
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }

        public void Continue()
        {
            if (settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(!settingsPanel.activeSelf);
            }
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
        }
    }
}
