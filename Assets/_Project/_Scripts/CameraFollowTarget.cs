using UnityEngine;

namespace PorfirioPartida.Workshop
{
    public class CameraFollowTarget : MonoBehaviour
    {
        public Transform target;
        private Vector3 _initalOffset;
        public bool started = false;

        public void SetTarget(Transform target)
        {
            this.target = target;
            started = true;
            _initalOffset = new Vector3(-.75f, 14.94f, -13.06f);
        }

        // void Start()
        // {
        //     _initalOffset = transform.position - target.position;
        //     Debug.Log($"Initial Offset: {_initalOffset}");
        // }

        void FixedUpdate()
        {
            if (!started)
            {
                //Can't follow because no target is set, just wait for external enablement.
                this.enabled = false;
                return;
            }

            transform.position = target.position + _initalOffset;
        }
    
        //TODO: Attach to local car for multiplayer.
    }
}