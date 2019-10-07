using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHelper : NetworkBehaviour
{
    [SyncVar]
    public bool gameHosted = false;
    [SyncVar]
    public bool player1IsReady = false;
    [SyncVar]
    public bool player2IsReady = false;
    public void HostGame()
    {
        CmdGameIsHosted(true);
    }

    [Command]
    private void CmdGameIsHosted(bool newState)
    {
        RpcSetGameIsHosted(newState);
    }

    [ClientRpc]
    private void RpcSetGameIsHosted(bool newState)
    {
        gameHosted = newState;
    }

    public void Player1IsReady()
    {
        CmdPlayer1IsReady(true);
    }

    [Command]
    private void CmdPlayer1IsReady(bool newState)
    {
        RpcSetPlayer1ToReady(newState);
    }
    [ClientRpc]
    private void RpcSetPlayer1ToReady(bool newState)
    {
        player1IsReady = newState;
    }

    public void Player2IsReady()
    {
        CmdPlayer2IsReady(true);
    }

    [Command]
    private void CmdPlayer2IsReady(bool newState)
    {
        RpcSetPlayer2ToReady(newState);
    }
    [ClientRpc]
    private void RpcSetPlayer2ToReady(bool newState)
    {
        player2IsReady = newState;
    }
}
