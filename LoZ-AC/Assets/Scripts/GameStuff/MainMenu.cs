using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStuff
{
   public class MainMenu : MonoBehaviour
   {
      public GameObject settingsPanel;
      public GameObject creditsPanel;

      public void SettingsToggle()
      {
         if (creditsPanel.activeSelf)
         {
            creditsPanel.SetActive(!creditsPanel.activeSelf);
         }
         settingsPanel.SetActive(!settingsPanel.activeSelf);
      }
      
      public void StartingGame()
      {
         SceneManager.LoadSceneAsync("Overworld");
      }

      public void CreditsToggle()
      {
         if (settingsPanel.activeSelf)
         {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
         }
         creditsPanel.SetActive(!creditsPanel.activeSelf);
      }
      
      public void QuitingGame()
      {
         Application.Quit();
      }
      
      
   }
}
