using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class OvrAvatarLeftHand : MonoBehaviour
    {
        private bool _grabbing = false;                      //hand is grabbing bool
        private GameObject _grabbedObject;                   //the object currently being grasped  
        private RaycastHit[] _hits;                          //array of grabbable objects hit by raycast
        private bool _released = false;                      //grabbedobject has been released bool
        private Vector3 _initialToss;                        //the velocity at release (initial flight path)                         
        private float levelOfAssistance;                  //level of assistance from settings, determines assistance on the initial flight path
        private GameObject _hoop;                            //hoop gameobject
        private Vector3 _initialPos;
        private OvrAvatarRightHand _righthand;
        private bool _extended = false;

        public Transform GripTransform;         //hand grabbing pose
        public float GrabRadius;                //radius around hand for grabbing objects
        public LayerMask GrabMask;              //grabbable layer
        public OVRInput.Controller Controller;  //right hand controller
        public OVRPlayerController Player;
        public Camera Centereyecamera;
        public GameObject Spawnpoint;

        public void Start()
        {
            _righthand = GameObject.FindObjectOfType<OvrAvatarRightHand>();
            Player = GameObject.FindObjectOfType<OVRPlayerController>();
            levelOfAssistance = Data.getLevelOfAssistance();
        }

        public void SetDefaultHandPose()
        {
            this.GetComponentInParent<OvrAvatar>().LeftHandCustomPose = null;
        }

        void Update()
        {
            //detection of grabbable objects nearby within grabRadius
            _hits = Physics.SphereCastAll(transform.position, GrabRadius, transform.forward, 0f, GrabMask);

            //initiate grab
            if (!_released && _hits.Length > 0 && !_righthand.IsGrabbing())
            {
                GrabObject();
                if (Spawnpoint.GetComponent<Spawn>().CurrentInteractables == 0)
                    Spawnpoint.GetComponent<Spawn>().SpawnPrefab();
            }
            //rotate grabbedObject with hand
            if (_grabbedObject != null && !_released && _grabbing && _extended)
            {
                ReleaseObject();
                _released = true;    //cannot regrab released object
                _extended = false;
                _grabbing = false;
            }
        }

        void GrabObject()
        {
            _grabbing = true;
            int closestHit = 0;
            //determine closest grabbable object
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].distance < _hits[closestHit].distance) closestHit = i;
            }
            this.GetComponentInParent<OvrAvatar>().LeftHandCustomPose = GripTransform;     //change pose of hand to grabbing
            _grabbedObject = _hits[closestHit].transform.gameObject;                          //set grabbedObject to closest grabbable object
            _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            _grabbedObject.transform.position = transform.position;                          //rotate and move grabbedObject with the hand            
            _grabbedObject.transform.localRotation = transform.localRotation;
            _grabbedObject.GetComponent<LineRenderer>().enabled = true;

            //start up engine sound effect for jet
            if (_grabbedObject.GetComponent<AudioSource>())
                _grabbedObject.GetComponent<AudioSource>().mute = false;
        }
        void ReleaseObject()
        {
            _grabbing = false;
            SetDefaultHandPose();          //set hand pose back to default
            if (_grabbedObject != null)
            {
                _grabbedObject.transform.parent = null;
                _grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                _grabbedObject.GetComponent<LineRenderer>().enabled = false;
                _hoop = GameObject.Find("hoop(Clone)");
                //vector to determine ideal flight path towards the hoop
                Vector3 towardsHoop = _hoop.transform.transform.position - _grabbedObject.transform.position;
                // flight on release, flight path changed by level of difficulty

                Vector3 velocity = OVRInput.GetLocalControllerVelocity(Controller);
                if (Mathf.Abs(velocity.x) < towardsHoop.normalized.x + .15)
                    velocity = towardsHoop.normalized * levelOfAssistance;
                else
                {
                    velocity += towardsHoop.normalized * levelOfAssistance;
                }
                velocity = velocity / levelOfAssistance * 3;                    //velocity always around 3
                _grabbedObject.GetComponent<Rigidbody>().velocity = velocity;    //set thrown object's velocity to calculated velocity
                //changing the rotation of the thrown object to look natural based upon throw
                /*
                                                                             grabbedObject.transform.up = Vector3.up; ;
                                                                             grabbedObject.transform.forward = OVRInput.GetLocalControllerVelocity(Controller).normalized;
                                                                             grabbedObject.transform.forward = Vector3.Cross(grabbedObject.transform.up, grabbedObject.transform.right);
                                                                             grabbedObject.transform.right = Vector3.Cross(grabbedObject.transform.forward, gameObject.transform.up);
                                                                             */
                _grabbedObject.transform.LookAt(_hoop.transform);
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
