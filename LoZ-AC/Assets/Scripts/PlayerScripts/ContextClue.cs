using UnityEngine;

namespace PlayerScripts
{
    public class ContextClue : MonoBehaviour
    {
        public GameObject contextClue;

        private bool _contextActive;
        

        public void SetContextStatus()
        {
            _contextActive = !_contextActive;
            contextClue.SetActive(_contextActive);
        }
    }
}
