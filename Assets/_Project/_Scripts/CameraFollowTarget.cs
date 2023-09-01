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
            transform.position = target.position + _initalOffset;
            this.transform.parent = target;
        }
    }
}