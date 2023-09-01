using System.Collections;
using Photon.Pun;
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
            var selectedColorIndex = PlayerPrefs.GetInt(Constants.SelectedColor, 0);
            var playerName = PlayerPrefs.GetString(Constants.PlayerName, "");

            var randomPosition = GetRandomPosition();
            // var player = GameObject.Instantiate(carPrefab, randomPosition, Quaternion.identity, playersHolder);
            var player = PhotonNetwork.Instantiate("Car Prefab", GetRandomPosition(), Quaternion.identity, 0, new object[]{playerName, selectedColorIndex});
            
            var cameraFollowTarget = Camera.main.GetComponent<CameraFollowTarget>();
            cameraFollowTarget.SetTarget(player.transform);
            cameraFollowTarget.enabled = true;
            
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