using UnityEngine;

namespace GameStuff.ResetRoom
{
    public class OverworldRooms : Rooms
    {
        public GameObject audiClip;
        public AudioSource audioSource;

        public override void OnTriggerEnter2D(Collider2D other)
        {
            ActivateAudio(other);
                
            base.OnTriggerEnter2D(other);
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            DeactivateAudio(other);
            
            base.OnTriggerExit2D(other);
        }

        private void ActivateAudio(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            audiClip.SetActive(true);
            audioSource.Play();
        }

        private void DeactivateAudio(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            audiClip.SetActive(false);
            audioSource.Stop();
        }
    }
}
