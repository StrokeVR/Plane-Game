using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class MoveWithCameraOnAxis : MonoBehaviour
    {

        public new Camera Camera;

        public bool X;
        public bool Y;
        public bool Z;
        public float Distance;

        // Update is called once per frame
        void Update()
        {
            if (X)
                transform.position = new Vector3(Camera.transform.position.x + Distance, transform.position.y, transform.position.z);
            else if (Y)
                transform.position = new Vector3(transform.position.x, Camera.transform.position.y + Distance, transform.position.z);
            else if (Z)
                transform.position = new Vector3(transform.position.x, transform.position.y, Camera.transform.position.z + Distance);
        }
    }
}
