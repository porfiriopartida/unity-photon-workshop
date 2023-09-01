using TMPro;
using UnityEngine;

namespace PorfirioPartida.Workshop
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public Transform groundCheck;
        public LayerMask groundLayer;
        
        public TMP_Text uiPlayerName;
        public float rotationSpeed;
        public float accelRate;
        
        private Rigidbody _rigidbody;
        private float _accel;
        public void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            SetPlayerName(PlayerPrefs.GetString(Constants.PlayerName));
        }

        private void SetPlayerName(string playerName)
        {
            uiPlayerName.text = playerName;
        }

        private void Update()
        {
            RaycastHit hit;
            if(!Physics.Raycast(groundCheck.position, Vector3.down, out hit, 3f, groundLayer)){
                //is grounded.
                return;
                //Not grounded let fall.
            }
        
        
            var horizontal = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
            {
                //Accel rigid body
                _accel += accelRate * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.S))
            {
                //Accel rigid body
                _accel -= accelRate * Time.deltaTime;
            }

            if (horizontal < 0)
            {
                //Rotate
                transform.Rotate(Vector3.up * (Time.deltaTime * -rotationSpeed));
            }

            if (horizontal > 0)
            {
                //Rotate
                transform.Rotate(Vector3.up * (Time.deltaTime * rotationSpeed));
            }

            CheckVelocity();
        }

        private void CheckVelocity()
        {
            _rigidbody.velocity = transform.forward * _accel;
        }

        public void Stop()
        {
            _accel = 0;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = false;
        }
        public void Resume()
        {
            _rigidbody.useGravity = true;
        }
    }
}