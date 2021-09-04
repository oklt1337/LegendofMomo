using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStuff
{
    public class Homescreen : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetKey(KeyCode.Return)) return;
            
            SceneManager.LoadScene("StartScene");
        }
    }
}
