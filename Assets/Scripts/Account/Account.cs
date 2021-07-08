using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;

public class Account : MonoBehaviour
{
    #region UI Variable
    public GameObject UI_Panel_FadeScreen;
    public GameObject UI_Panel_Login;
    public InputField UI_Input_Username;
    public InputField UI_Input_Password;
    // Error Box
    public GameObject UI_ErrorBox;
    public Text UI_ErrorText;
    #endregion

    void Start()
    {
        Network.Socket.Emit("account:has", NetworkPacketStruct.UniqueIdentifier(ObscuredPrefs.GetString("uid")));
        
        Network.Socket.On("account:fadescreen", FadeScreen);
        Network.Socket.On("account:enterGame", EnterGame);
        Network.Socket.On("account:saveUniqID", SaveUniqID);
        Network.Socket.On("account:error", Error);
    }

    #region Network Method
    private void FadeScreen(SocketIOEvent e)
    {
        UI_Panel_FadeScreen.SetActive(false);
    }
    private void EnterGame(SocketIOEvent e)
    {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }
    private void SaveUniqID(SocketIOEvent e)
    {
        ObscuredPrefs.SetString("uid", NetworkUtil.FixString(e.data["uid"].ToString()));
    }
    private void Error(SocketIOEvent e) {
        UI_ErrorBox.SetActive(true);
        UI_ErrorText.text = NetworkUtil.FixString(e.data["msg"].ToString());
        Invoke("HideErrorBox", 3f);
    }
    #endregion

    #region UI Method
    public void OnLoginMode()
    {
        UI_Panel_Login.SetActive(true);
    }
    public void onGuestMode()
    {
        Network.Socket.Emit("account:CreateGuest");
    }
    public void onClickLogin()
    {
        if (UI_Input_Username.text.Length > 3 && UI_Input_Password.text.Length > 3)
        {
            Network.Socket.Emit("account:Login",NetworkPacketStruct.Account_Login(UI_Input_Username.text,UI_Input_Password.text));
        }
        else
        {
            UI_ErrorBox.SetActive(true);
            UI_ErrorText.text = "·ÿ›« ›Ì·œ Â«Ì ‰«„ ò«—»—Ì Ê Å”Ê—œ —« ò«„· ò‰Ìœ.";
            Invoke("HideErrorBox",3f);
        }
    }
    private void HideErrorBox()
    {
        UI_ErrorBox.SetActive(false);
    }
    #endregion
}
