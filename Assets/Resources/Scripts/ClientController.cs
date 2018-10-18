using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Assets.Resources.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientController : MonoBehaviour {
    [SerializeField]
    public SocketIOComponent socket;

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
        Dictionary<string, string> data = msg.data.ToDictionary();
        
        string type = data["type"];
        string value = data["data"];

       
        switch(type)
        {
            case "startGame":
                GameObject.Find("Start").GetComponent<ClickButton>().StartGame();
                break;

            case "restartGame":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;

            case "toggleHoop":
                Data.willOscillate = (value.Equals("true")) ? true : false;
                returnToClinician("toggleHoop", "" + Data.willOscillate);
                break;

            case "difficulty":
                Debug.Log("Diff: " + float.Parse(value));
                Data.difficulty = float.Parse(value);
                Data.oscillateSpeed = Data.difficulty / 30;
                GameObject.Find("Difficulty").GetComponent<Text>().text = "Difficulty: " + (Data.difficulty * 10).ToString("0.0") + "%";
                returnToClinician("difficulty", "" + Data.difficulty);
                break;
        }
    }

    public void returnToClinician(string type, string value)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["type"] = type;
        data["data"] = value;
        socket.Emit("forClinician", new JSONObject(data));
    }
	
}
