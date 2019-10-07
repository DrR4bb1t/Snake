using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameLogic : NetworkBehaviour
{
    [SerializeField]
    private GameObject startScreen;

    [SerializeField]
    private Text gamestartCountdownText;

    [SerializeField]
    private CustomNetworkManager networkManager;

    [SerializeField]
    private GameObject player1;

    [SerializeField]
    private GameObject player2;

    [SerializeField]
    private GameObject player1Counter;

    [SerializeField]
    private GameObject player2Counter;

    private string text = "Match will start in ";
    private string countdownTillGameStarts;

    private float timeoutCountdown = 30f;
    private float gamestartCountdown = 10f;

    [SyncVar]
    public bool startedGame = false;

    private void Update()
    {
        //if (startedGame == false)
        //{
        //    CheckForTimeout();
        //    StartGame();
        //}

        startedGame = true;
    }

    //private void CheckForTimeout()
    //{
    //    if (networkManager.networkHelper.player1IsReady == true)
    //    {
    //        if (timeoutCountdown >= 0)
    //        {
    //            timeoutCountdown -= Time.deltaTime;
    //        }
    //        else
    //        {
    //            networkManager.StopHost();
    //            networkManager.networkMenu.SetActive(true);
    //            networkManager.waitingScreen.SetActive(false);
    //            networkManager.networkHelper.player1IsReady = false;
    //            networkManager.player1HUD.SetActive(false);
    //            networkManager.networkHelper.gameHosted = false;
    //            timeoutCountdown = 30f;
    //        }
    //    }
    //}

    private void StartGame()
    {
        if (networkManager.networkHelper.player1IsReady == true && networkManager.networkHelper.player2IsReady == true)
        {
            networkManager.waitingScreen.SetActive(false);
            startScreen.SetActive(true);
            if (gamestartCountdown >= 0)
            {
                gamestartCountdown -= Time.deltaTime;
                //gamestartCountdown = Mathf.Round(gamestartCountdown);
                UpdateCounters();
                player1.SetActive(true);
                player2.SetActive(true);
                player1Counter.SetActive(true);
                player2Counter.SetActive(true);
            }
            else
            {
                //GameStarted();
                startScreen.SetActive(false);
            }
        }
    }

    private void UpdateCounters()
    {
        string output;
        countdownTillGameStarts = gamestartCountdown.ToString();
        output = text + countdownTillGameStarts;
        gamestartCountdownText.text = output;
    }

    //private void GameStarted()
    //{
    //    CmdGameIsStarted(true);
    //}

    //[Command]
    //private void CmdGameIsStarted(bool newState)
    //{
    //    RpcSetGameToStarted(newState);
    //}
    //[ClientRpc]
    //private void RpcSetGameToStarted(bool newState)
    //{
    //    startedGame = newState;
    //}
}
