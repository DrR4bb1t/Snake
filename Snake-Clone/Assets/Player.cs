using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace SA
{
    public class Player : NetworkBehaviour
    {
        public Node playerNode;
        [SyncVar]
        private int playerID;
        private void Start()
        {
            playerID++;

            if (isLocalPlayer)
            {
                if (playerID == 1)
                {
                    this.gameObject.name = "player1";
                }
                if (playerID == 2)
                {
                    transform.name = "player2";
                }
            }
            else if (!isLocalPlayer)
            {
                if (playerID == 2)
                {
                    this.gameObject.name = "player1";
                }
                if (playerID == 1)
                {
                    transform.name = "player2";
                }
            }
        }
    }
}

