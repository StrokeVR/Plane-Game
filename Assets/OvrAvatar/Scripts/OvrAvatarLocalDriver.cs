using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Oculus.Avatar;

public class OvrAvatarLocalDriver : OvrAvatarDriver {

    public bool OculusUsage;
    public KinectManager kinectManager;
    private const float mobileBaseHeadHeight = 1.7f;

    float voiceAmplitude = 0.0f;
    ControllerPose GetControllerPose(OVRInput.Controller controller)
    {
        ovrAvatarButton buttons = 0;
        ovrAvatarTouch touches = 0;
        Vector2 joystickPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
        float indexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        float handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        bool isActive = (OVRInput.GetActiveController() & controller) != 0;
        // get data from Oculus Touch Controllers
        if (OculusUsage)
        {
            if (OVRInput.Get(OVRInput.Button.One, controller)) buttons |= ovrAvatarButton.One;
            if (OVRInput.Get(OVRInput.Button.Two, controller)) buttons |= ovrAvatarButton.Two;
            if (OVRInput.Get(OVRInput.Button.Start, controller)) buttons |= ovrAvatarButton.Three;
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, controller)) buttons |= ovrAvatarButton.Joystick;
            
            if (OVRInput.Get(OVRInput.Touch.One, controller)) touches |= ovrAvatarTouch.One;
            if (OVRInput.Get(OVRInput.Touch.Two, controller)) touches |= ovrAvatarTouch.Two;
            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, controller)) touches |= ovrAvatarTouch.Joystick;
            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbRest, controller)) touches |= ovrAvatarTouch.ThumbRest;
            if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, controller)) touches |= ovrAvatarTouch.Index;
            if (!OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, controller)) touches |= ovrAvatarTouch.Pointing;
            if (!OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, controller)) touches |= ovrAvatarTouch.ThumbUp;
        }
        // else get data from Kinect tracking
        else
        {
            isActive = true;
        }
        return new ControllerPose
        {
            buttons = buttons,
            touches = touches,
            joystickPosition = joystickPosition,
            indexTrigger = indexTrigger,
            handTrigger = handTrigger,
            isActive = isActive,
        };
    }

    private PoseFrame GetCurrentPose()
    {
        Vector3 headPos = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);
#if UNITY_ANDROID && !UNITY_EDITOR
        headPos.y += mobileBaseHeadHeight;
#endif
        if (OculusUsage)
        {
            return new PoseFrame
            {
                voiceAmplitude = voiceAmplitude,
                headPosition = headPos,
                headRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye),
                handLeftPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch),
                handLeftRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch),
                handRightPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
                handRightRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch),
                controllerLeftPose = GetControllerPose(OVRInput.Controller.LTouch),
                controllerRightPose = GetControllerPose(OVRInput.Controller.RTouch),
            };
        }
        else
        {
            return new PoseFrame
            {
                voiceAmplitude = voiceAmplitude,
                headPosition = headPos,
                headRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye),
                handLeftPosition = kinectManager.GetLeftKinectHandPosition(),
                handLeftRotation = kinectManager.GetLeftKinectHandRotation(),
                handRightPosition = kinectManager.GetRightKinectHandPosition(),
                handRightRotation = kinectManager.GetRightKinectHandRotation(),
                controllerLeftPose = GetControllerPose(OVRInput.Controller.LTouch),
                controllerRightPose = GetControllerPose(OVRInput.Controller.RTouch),
            };
        }
    }

    public override void UpdateTransforms(IntPtr sdkAvatar)
    {
        if (sdkAvatar != IntPtr.Zero)
        {
            PoseFrame pose = GetCurrentPose();

            ovrAvatarTransform bodyTransform = OvrAvatar.CreateOvrAvatarTransform(pose.headPosition, pose.headRotation);
            ovrAvatarHandInputState inputStateLeft = OvrAvatar.CreateInputState(OvrAvatar.CreateOvrAvatarTransform(pose.handLeftPosition, pose.handLeftRotation), pose.controllerLeftPose);
            ovrAvatarHandInputState inputStateRight = OvrAvatar.CreateInputState(OvrAvatar.CreateOvrAvatarTransform(pose.handRightPosition, pose.handRightRotation), pose.controllerRightPose);

            CAPI.ovrAvatarPose_UpdateBody(sdkAvatar, bodyTransform);
            CAPI.ovrAvatarPose_UpdateHands(sdkAvatar, inputStateLeft, inputStateRight);
        }
    }
}
