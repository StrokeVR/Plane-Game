using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class ClientController : MonoBehaviour {
    [SerializeField]
    public SocketIOComponent socket;
	// Use this for initialization
	void Start () {
        StartCoroutine(ConnectToServer());
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("forUnity", getData);
	}
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("USER_CONNECT");
        Debug.Log("User Connected");
        yield return new WaitForSeconds(0.5f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = "User" + Random.Range(0, 9999);
        data["type"] = "Unity";

        socket.Emit("GETDATA", new JSONObject(data));
    }
    private void OnUserConnected (SocketIOEvent evt)
    {
        Debug.Log("Client Connected: " + evt.data);
    }
    public void getData(SocketIOEvent msg)
    {
        Debug.Log("Got data from server" + msg.data); 
    }
	
}
