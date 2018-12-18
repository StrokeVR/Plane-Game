using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using System.Linq;

public class KinectRightHand : MonoBehaviour {

    private KinectSensor _sensor;
    private BodyFrameReader _bodyFramereader;
    private Body[] _bodies = null;
    
    public GameObject rightHand;
    public Transform GripTransform;

    void Start()
    {
        _sensor = KinectSensor.GetDefault();
        if (_sensor != null)
        {
            _bodyFramereader = _sensor.BodyFrameSource.OpenReader();
            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }
            _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
        }
    }
    
    void Update () {
        if (_bodyFramereader != null)
        {
            var frame = _bodyFramereader.AcquireLatestFrame();
            if (frame != null)
            {
                frame.GetAndRefreshBodyData(_bodies);
                foreach (var body in _bodies.Where(b => b.IsTracked))
                {
                    // change oculus hand position if kinect hand changes
                    if (body.HandRightState == HandState.Open)
                    {
                        //this.GetComponentInParent<OvrAvatar>().RightHandCustomPose = null;
                    }
                    else if (body.HandRightState == HandState.Closed)
                    {
                        //this.GetComponentInParent<OvrAvatar>().RightHandCustomPose = GripTransform;
                    }
                }
            }
        }
    }
}
