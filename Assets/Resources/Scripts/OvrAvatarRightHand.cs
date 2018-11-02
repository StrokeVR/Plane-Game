using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class OvrAvatarRightHand : MonoBehaviour
    {
        private bool _grabbing = false;                      // hand is grabbing bool
        private GameObject _grabbedObject;                   // the object currently being grasped  
        private RaycastHit[] _hits;                          // array of grabbable objects hit by raycast
        private bool _released = false;                      // grabbedobject has been released bool
        private Vector3 _initialToss;                        // the velocity at release (initial flight path)                         
        private int levelOfAssistance = 3;                   // level of assistance from settings, determines assistance on the initial flight path
        private GameObject _hoop;                            // hoop gameobject
        private Vector3 _initialPos;
        private OvrAvatarLeftHand _lefthand;
        private bool _extended = false;
        public Material defaultMat;
        public Material hoverMat;
        public Transform GripTransform;         // hand grabbing pose
        public float GrabRadius;                // radius around hand for grabbing objects
        public LayerMask GrabMask;              // grabbable layer
        public OVRInput.Controller Controller;  // right hand controller
        public OVRPlayerController Player;
        public Camera Centereyecamera;
        public GameObject Spawnpoint;
        bool move = false;
        Level currLevel;
        public void Start()
        {
            currLevel = GameObject.Find("PlaneLevel").GetComponent<Level>();
            _lefthand = GameObject.FindObjectOfType<OvrAvatarLeftHand>();
            Player = GameObject.FindObjectOfType<OVRPlayerController>();
        }

        public void setDefaultHandPose()
        {
            this.GetComponentInParent<OvrAvatar>().RightHandCustomPose = null;
        }

        public void setGrabHandPose()
        {
            this.GetComponentInParent<OvrAvatar>().RightHandCustomPose = GripTransform;     
        }

        void Update()
        {
            // detection of grabbable objects nearby within grabRadius
            _hits = Physics.SphereCastAll(transform.position, GrabRadius, transform.forward, 0f, GrabMask);
        
            // initiate grab
            if (!_released && _hits.Length > 0 && !_lefthand.IsGrabbing())
            {
                GrabObject();
                if (Spawnpoint.GetComponent<Spawn>().CurrentInteractables == 0)
                    Spawnpoint.GetComponent<Spawn>().SpawnPrefab();
            }
            // release plane
            if (_grabbedObject != null && !_released && _grabbing && _extended)
            {
                ReleaseObject();
                _released = true;    // cannot regrab released object
                _extended = false;
                _grabbing = false;
            }
            if (_grabbedObject != null)
            {
                // RaycastHit hit;
                RaycastHit hit;
                int layer = LayerMask.GetMask("RayLayer");

                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(_grabbedObject.transform.position, _grabbedObject.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
                {
                    Debug.DrawRay(_grabbedObject.transform.position, _grabbedObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    _hoop = GameObject.Find("hoop(Clone)");
                    // Debug.Log(Vector3.Distance(hit.point, _hoop.transform.position));
                    if (_hoop != null && Vector3.Distance(hit.point, _hoop.transform.position) < 1.3)
                    {
                        _hoop.gameObject.GetComponent<MeshRenderer>().material = hoverMat;
                    }
                    else if (_hoop != null)
                    {
                        _hoop.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
                    }
                    if (move)
                    {
                        _grabbedObject.transform.LookAt(hit.point);
                        _grabbedObject.transform.position = Vector3.MoveTowards(_grabbedObject.transform.position, hit.point, 0.02f);
                        if (Vector3.Distance(_grabbedObject.transform.position, hit.point) <= 0)
                        {
                            _grabbedObject.GetComponent<Rigidbody>().useGravity = true;
                            currLevel.DecrementInteractables(_grabbedObject);
                            move = false;
                            // Data.planesMissed++;
                        }
                    }
                    // Debug.Log("Did Hit; " + hit.point.ToString());
                }
                else
                {
                    Debug.DrawRay(_grabbedObject.transform.position, _grabbedObject.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    // Debug.Log("Did not Hit");
                    // Data.planesMissed++;
                }
            }
        }

        void GrabObject()
        {
            _grabbing = true;
            int closestHit = 0;
            // determine closest grabbable object
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].distance < _hits[closestHit].distance) closestHit = i;
            }
            setGrabHandPose(); // change pose of hand to grabbing
            _grabbedObject = _hits[closestHit].transform.gameObject;                          // set grabbedObject to closest grabbable object
            _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;                     
            _grabbedObject.transform.position = transform.position;                          // rotate and move grabbedObject with the hand            
            _grabbedObject.transform.localRotation = transform.localRotation;
            _grabbedObject.GetComponent<LineRenderer>().enabled = true;
            // start up engine sound effect
            _grabbedObject.GetComponent<AudioSource>().mute = false;
            // start up propellor animation, only applicable for right side planes
            _grabbedObject.GetComponentInChildren<Animation>().Play();
        }

        void ReleaseObject()
        {
            Data.totalPlanes++;
            Debug.Log(Data.totalPlanes);
            setDefaultHandPose();          // set hand pose back to default
            if (_grabbedObject != null)
            {
                _grabbedObject.transform.parent = null;
                _grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                _grabbedObject.GetComponent<LineRenderer>().enabled = false;
                move = true;
                // _hoop = GameObject.Find("hoop(Clone)");
                // vector to determine ideal flight path towards the hoop
                // Vector3 towardsHoop = _hoop.transform.transform.position - _grabbedObject.transform.position;
                // flight on release, flight path changed by level of difficulty

                // Vector3 velocity = OVRInput.GetLocalControllerVelocity(Controller);
                // Debug.Log(towardsHoop.normalized);
                // Debug.Log(velocity);
                /*if (Mathf.Abs(velocity.x) < towardsHoop.normalized.x + .15)
                    velocity = towardsHoop.normalized * levelOfAssistance;
                else
                {
                    velocity += towardsHoop.normalized * levelOfAssistance;
                }
                velocity = velocity / levelOfAssistance * 3;                    // velocity always around 3
                _grabbedObject.GetComponent<Rigidbody>().velocity = velocity;   // set thrown object's velocity to calculated velocity
                _grabbedObject.transform.LookAt(_hoop.transform);*/
            }
        }
        public void SetReleasedToFalse()
        {
            _released = false;
        }
        public void SetExtendedToTrue()
        {
            if (_grabbing)
            {
                _extended = true;
            }
        }
        public bool IsGrabbing()
        {
            return _grabbing;
        }
    }
}
