using System.Collections;
using UnityEngine;

namespace PorfirioPartida.Workshop
{
    public class SceneManager : Singleton<SceneManager>
    {

        public float resumeDelay;
        public Transform playersHolder;
        public GameObject carPrefab;
        public Transform[] spawnPoints;
        public void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            var randomPosition = GetRandomPosition();
            var player = GameObject.Instantiate(carPrefab, randomPosition, Quaternion.identity, playersHolder);

            var cameraFollowTarget = Camera.main.GetComponent<CameraFollowTarget>();
            cameraFollowTarget.SetTarget(player.transform);
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