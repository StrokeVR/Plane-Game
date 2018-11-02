
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Threading; 
using System;


public class Client : MonoBehaviour
{
    Thread thread;
    static TcpClient client;
    static StreamReader sw;

    void Start()
    {
        thread = new Thread(new ParameterizedThreadStart(ThreadMethod));
        thread.Start(UnityEngine.Random.Range(0, 99999));
    }
    void OnApplicationQuit()
    {
        try
        {
            sw.Close();
            client.Close();
            thread.Abort();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    static void ThreadMethod(object obj)
    {
        client = new TcpClient("localhost", 1234);

        sw = new StreamReader(client.GetStream());

        if (client.Connected)
        {
            Debug.Log("Connected to Server!");

            while (!sw.EndOfStream)
            {

                string line = sw.ReadLine();

                Debug.Log(line);
                if (!line.Equals("{") && !line.Equals("}")) 
                {
                   // Debug.Log("NOT EQUAL");
                   SaveToData(line);
                }               
            }
        }

        sw.Close();
        client.Close();
    }

    static void SaveToData(string line)
    {
        string type = line.Split(':')[0];
        string data = line.Split(':')[1];
        string vect = data.Substring(2, data.Length - 3);
        switch (type)
        {
            case "\"Left Wrist\"":
                Data.leftWrist = StringToVector3(vect);
                break;
            case "\"Right Wrist\"":
                Data.rightWrist = StringToVector3(vect);
                break;
            case "\"Center\"":
                Data.center = StringToVector3(vect);
                break;
        }
    }
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the brackets
        if (sVector.StartsWith("[") && sVector.EndsWith("]"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));
        Debug.Log(result);
        return result;
    }
}

