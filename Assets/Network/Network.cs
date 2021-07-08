using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

public class Network : MonoBehaviour
{
    #region Core Variable
    public static SocketIOComponent Socket;
    #endregion

    #region Other Variable
    private bool isStarted = false;
    #endregion

    void Awake()
    {
        Socket = GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Socket.On("open",OnConnected);
        Socket.On("start",onStart);
        Socket.On("close",OnClose);
    }

    #region Base Network Method
    private void OnConnected(SocketIOEvent e)
    {
        if (Socket.sid.Length > 1)
        {
            Socket.Emit("start");
        }
    }
    private void onStart(SocketIOEvent e)
    {
        if (isStarted == false)
        {
            SceneManager.LoadScene("Account", LoadSceneMode.Single);
            isStarted = true;
        }
    }
    private void OnClose(SocketIOEvent e){}

    #endregion
}
