using System.Collections;
using Photon.Pun.Demo.SlotRacer;
using UnityEngine;

namespace PorfirioPartida.Workshop
{
    public class SceneManager : Singleton<SceneManager>
    {

        public float resumeDelay;
        public Transform playersHolder;
        public GameObject carPrefab;
        public Transform[] spawnPoints;
        public Material[] materials;
        public void Start()
        {
            // StartGame();
        }

        public void StartGame()
        {
            var randomPosition = GetRandomPosition();
            var player = GameObject.Instantiate(carPrefab, randomPosition, Quaternion.identity, playersHolder);

            var cameraFollowTarget = Camera.main.GetComponent<CameraFollowTarget>();
            cameraFollowTarget.SetTarget(player.transform);
            cameraFollowTarget.enabled = true;
            
            var selectedColorIndex = PlayerPrefs.GetInt(Constants.SelectedColor, 0);
            player.GetComponent<PlayerController>().SetColor(materials[selectedColorIndex]);
            
            Debug.Log($"Starting game. Spawning at position {randomPosition}");
        }

        private Vector3 GetRandomPosition()
        {
            var idx = Random.Range(0, spawnPoints.Length); 
            
            return spawnPoints[idx].position;
        }

        public void RespawnObject(PlayerController car)
        {
            car.Stop();
            car.transform.position = GetRandomPosition();

            StartCoroutine(DelayResume(car));
        }
        private IEnumerator DelayResume(PlayerController car)
        {
            yield return new WaitForSeconds(resumeDelay);
            car.Resume();
        }
    }
}