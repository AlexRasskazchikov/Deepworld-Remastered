using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhotonConnect : MonoBehaviour
{   
    public GameObject SecView2, SecView3, LoadingScreen;

    public Animator anim;
    public string BuildVersion = "0.1v";
    public TMPro.TextMeshProUGUI ConnectionTryText;
    public int ConnectionTryID = 0;

    void Start(){
        connectToPhoton();
    }

    public void connectToPhoton(){
        PhotonNetwork.ConnectUsingSettings(BuildVersion);
        LoadingScreen.SetActive(true);
        Debug.Log("Connecting to PUN");
    }

    private void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        Debug.Log("Connected to Master.");
    }

    private void OnJoinedLobby(){
        anim.SetBool("Connected", true);
        Debug.Log("Joined Lobby."); 
    }

    private void OnDisconnectedFromPhoton(){
        ConnectionTryID ++;
        anim.SetInteger("ConnectionTryID", ConnectionTryID);
        ConnectionTryText.text = "Ошибка подключения, попытка номер " + ConnectionTryID.ToString();

        connectToPhoton();
        Debug.Log("(1) No connection.");
    }

    private void OnFailedToConnectToPhoton(){

        ConnectionTryID ++;
        anim.SetInteger("ConnectionTryID", ConnectionTryID);
        ConnectionTryText.text = "Ошибка подключения, попытка номер " + ConnectionTryID.ToString();
        
        connectToPhoton();
        Debug.Log("(2) No connection.");
    }

    private void DisconnectedUI(){
        SecView2.SetActive(false);
        LoadingScreen.SetActive(false);
        SecView3.SetActive(true);
    }
}
