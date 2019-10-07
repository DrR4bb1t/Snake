using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField]
    public GameObject networkMenu;

    [SerializeField]
    public GameObject waitingScreen;

    [SerializeField]
    public GameObject player1HUD;

    [SerializeField]
    public GameObject player2HUD;

    [SerializeField]
    public NetworkHelper networkHelper;




    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
        networkMenu.SetActive(false);
        waitingScreen.SetActive(true);
        //networkHelper.Player1IsReady();
        player1HUD.SetActive(true);
        networkHelper.HostGame();
    }

    public void JoinGame()
    {
        bool test = networkHelper.gameHosted;
        if (test == true)
        {
            SetIPAddress();
            SetPort();
            NetworkManager.singleton.StartClient();
            networkMenu.SetActive(false);
            //networkHelper.Player2IsReady();
            player2HUD.SetActive(true);
        }
    }

    private void SetIPAddress()
    {
        string IPAdress = GameObject.Find("InputFieldIPAdress").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = IPAdress;
    }

    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    private void SetupNetworkButtons()
    {
        GameObject.Find("Host Game").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Host Game").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("Join Game").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Join Game").GetComponent<Button>().onClick.AddListener(JoinGame);
    }


}
