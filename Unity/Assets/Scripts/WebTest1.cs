using System;
using System.Net.WebSockets;
using NativeWebSocket;
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
    public Joint right_wrist;
}

public class WebTest1 : MonoBehaviour
{
    NativeWebSocket.WebSocket websocket;
    public Transform rightHand;

    async void Start()
    {
        websocket = new NativeWebSocket.WebSocket("ws://localhost:3000");

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(message);

            PoseData data = JsonUtility.FromJson<PoseData>(message);

            if (data.right_wrist != null)
            {
                MoveHand(data.right_wrist);
            }
        };

        await websocket.Connect();
    }

    void MoveHand(Joint wrist)
    {
        float x = ((640f - wrist.x) / 640f) * 10f - 5f;
        float y = ((480f - wrist.y) / 480f) * 6f - 3f;

        Vector3 newPos = new Vector3(x, y, 0);
        rightHand.position = Vector3.Lerp(rightHand.position, newPos, 0.5f);
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
