using UnityEngine;

namespace PorfirioPartida.Workshop
{
    public class Shredder : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            var playerController = other.GetComponent<PlayerController>();
            SceneManager.Instance.RespawnObject(playerController);
        }
    }
}