using TMPro;
using UnityEngine;

namespace PorfirioPartida.Workshop
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour //IPunInstantiateMagicCallback
    {
        public Transform groundCheck;
        public LayerMask groundLayer;
        
        public TMP_Text uiPlayerName;
        public float rotationSpeed;
        public float accelRate;
        
        private Rigidbody _rigidbody;
        private float _accel;

        public MeshRenderer carModel;
        
        public void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            //Online move to event.
            SetPlayerName(PlayerPrefs.GetString(Constants.PlayerName));
            SetPlayerColor(PlayerPrefs.GetInt(Constants.SelectedColor, 0));
        }
        private void SetPlayerName(string playerName)
        {
            uiPlayerName.text = playerName;
        }
        private void SetColor(Material material)
        {
            //Only this has influence over the car color, the rest is windows or wheels.
            Material[] materials = carModel.materials;
            materials[0] = material;

            carModel.GetComponent<MeshRenderer>().materials = materials;
        }
        private void SetPlayerColor(int materialIdx)
        {
            SetColor(SceneManager.Instance.materials[materialIdx]);
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
        // public void OnPhotonInstantiate(PhotonMessageInfo info)
        // {
        //     var instantiationData = info.photonView.InstantiationData;
        //     var player = info.photonView.GetComponent<PlayerController>();
        //     var playerName = (string)instantiationData[0];
        //     var selectedColorIndex = (int)instantiationData[1];
        //     var playerController = player.GetComponent<PlayerController>(); 
        //     playerController.SetColor(SceneManager.Instance.materials[selectedColorIndex]);
        //     playerController.SetPlayerName(playerName);
        //     Debug.Log(instantiationData);
        // }
    }
}