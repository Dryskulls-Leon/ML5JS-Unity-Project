using System;
using System.Net.WebSockets;
using NativeWebSocket;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Joint
{
    public float x;
    public float y;
}

[Serializable]
public class PoseData
{
    public Joint nose;
    public Joint left_eye;
    public Joint right_eye;
    public Joint left_ear;
    public Joint right_ear;
    public Joint left_shoulder;
    public Joint right_shoulder;
    public Joint left_elbow;
    public Joint right_elbow;
    public Joint left_wrist;
    public Joint right_wrist;
    public Joint left_hip;
    public Joint right_hip;
    public Joint left_knee;
    public Joint right_knee;
    public Joint left_ankle;
    public Joint right_ankle;
}

public class WebTest1 : MonoBehaviour
{
    NativeWebSocket.WebSocket websocket;
    [Header("KeyPoints")]
    public Transform rightHand;
    public Transform leftHand;
    public Transform rightElbow;
    public Transform leftElbow;
    public Transform rightShoulder;
    public Transform leftShoulder;
    public Transform leftEar;
    public Transform rightEar;
    public Transform leftEye;
    public Transform rightEye;
    public Transform nose;
    public Transform leftHip;
    public Transform rightHip;
    public Transform leftKnee;
    public Transform rightKnee;
    public Transform leftAnkle;
    public Transform rightAnkle;
    [Header("Bones")]
    public Transform leftUpperArm;
    public Transform rightUpperArm;
    public Transform leftLowerArm;
    public Transform rightLowerArm;
    public Transform leftUpperLeg;
    public Transform rightUpperLeg;
    public Transform leftLowerLeg;
    public Transform rightLowerLeg;
    public Transform shoulders;
    public Transform hips;

    async void Start()
    {
        websocket = new NativeWebSocket.WebSocket("ws://localhost:3000");

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(message);

            PoseData data = JsonUtility.FromJson<PoseData>(message);

            if (data != null)
            {
                MoveJoint(nose, data.nose);
                MoveJoint(leftEye, data.left_eye);
                MoveJoint(rightEye, data.right_eye);
                MoveJoint(leftEar, data.left_ear);
                MoveJoint(rightEar, data.right_ear);
                MoveJoint(leftShoulder, data.left_shoulder);
                MoveJoint(rightShoulder, data.right_shoulder);
                MoveJoint(leftElbow, data.left_elbow);
                MoveJoint(rightElbow, data.right_elbow);
                MoveJoint(leftHand, data.left_wrist);
                MoveJoint(rightHand, data.right_wrist);
                MoveJoint(leftHip, data.left_hip);
                MoveJoint(rightHip, data.right_hip);
                MoveJoint(leftKnee, data.left_knee);
                MoveJoint(rightKnee, data.right_knee);
                MoveJoint(leftAnkle, data.left_ankle);
                MoveJoint(rightAnkle, data.right_ankle);
            }
        };

        await websocket.Connect();
    }

    void MoveJoint(Transform joint, Joint p)
    {
        if (joint == null || p == null) return;

        float x = ((640f - p.x) / 640f) * 10f - 5f;
        float y = ((480f - p.y) / 480f) * 6f - 3f;

        Vector3 newPos = new Vector3(x, y, 0);

        joint.position = Vector3.Lerp(joint.position, newPos, 0.5f);
    }

    void ConnectJoints(Transform a, Transform b, Transform bone)
    {
        if (a == null || b == null || bone == null) return;

        bone.position = (a.position + b.position) / 2f;

        Vector3 dir = b.position - a.position;

        bone.up = dir;

        float distance = dir.magnitude;
        bone.localScale = new Vector3(0.2f , distance / 2f, 0.2f);
    }
    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
        ConnectJoints(leftShoulder, leftElbow, leftUpperArm);
        ConnectJoints(leftElbow, leftHand, leftLowerArm);

        ConnectJoints(rightShoulder, rightElbow, rightUpperArm);
        ConnectJoints(rightElbow, rightHand, rightLowerArm);

        ConnectJoints(leftHip, leftKnee, leftUpperLeg);
        ConnectJoints(leftKnee, leftAnkle, leftLowerLeg);

        ConnectJoints(rightHip, rightKnee, rightUpperLeg);
        ConnectJoints(rightKnee, rightAnkle, rightLowerLeg);

        ConnectJoints(leftShoulder, rightShoulder, shoulders);
        ConnectJoints(leftHip, rightHip, hips);
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
