using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLogic : NetworkBehaviour
{

    [SerializeField]
    private SpawnLogic spawn;

    [SerializeField]
    private Collector collector;

    [SerializeField]
    private GameLogic gameLogic;

    public int playerID;

    //Values for movement.
    public float moveSpeed = 2f;
    public int direction = 0;
    private float deltaX;
    private float deltaY;

    //Startrotation
    private float rotation = 0;

    [SyncVar]
    public int foodCounter;

    public bool flashmode = false;
    private float upTimeCounter;
    private void Start()
    {
    }

    private void Update()
    {
        if (gameLogic.startedGame == true)
        {
            Move();
            if (flashmode == true)
            {
                UpTimeOfFlashmode();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Top"))
            SetDirectionRight(); 
        if (collider.gameObject.CompareTag("Right"))
            SetDirectionDown();
        if (collider.gameObject.CompareTag("Bottom"))
            SetDirectionLeft();
        if (collider.gameObject.CompareTag("Left"))
            SetDirectionUp();

        if (collider.gameObject.name == "Food")
        {
            Destroy(spawn.food);
            PlayerFoodCounter();
            collector.AddBodyPartToBody();
            spawn.FoodSpawning();
        }
        if (collider.gameObject.name == "Flash")
        {
            EnterFlashmode();
            Destroy(spawn.flash);
        }
    }

    private void Move()
    {
        if (direction == 1)
        {
            deltaY = moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(0f, deltaY, 0f);
        }
        else if (direction == 2)
        {
            deltaY = -moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(0f, deltaY, 0f);
        }
        else if (direction == 3)
        {
            deltaX = -moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(deltaX, 0f, 0f);
        }
        else if (direction == 4)
        {
            deltaX = moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(deltaX, 0f, 0f);
        }

        //collector.positionList.Add(transform);
    }

    /// <summary>
    /// Sets the direction and rotationation.
    /// 1 --> Up
    /// 2 --> Down
    /// 3 --> Left
    /// 4 --> Right
    /// </summary>
    public void SetDirectionUp()
    {
        direction = 1;
        rotation = 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public void SetDirectionDown()
    {
        direction = 2;
        rotation = -90f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public void SetDirectionLeft()
    {
        direction = 3;
        rotation = -180f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public void SetDirectionRight()
    {
        direction = 4;
        rotation = 0f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    private void EnterFlashmode()
    {
        moveSpeed = 3.5f;
        flashmode = true;
        upTimeCounter = 10f;
    }

    private void UpTimeOfFlashmode()
    {
        if (upTimeCounter > 0)
        {
            upTimeCounter -= Time.deltaTime;
            Debug.Log(upTimeCounter);
        }
        else
        {
            moveSpeed = 2f;
            flashmode = false;
            spawn.powerupSpawned = false;
            spawn.cooldown = 25f;
        }
    }

    public void PlayerFoodCounter()
    {
        foodCounter++;
        CmdPlayerFoodCounter(foodCounter);
    }

    [Command]
    private void CmdPlayerFoodCounter(int newFoodCounter)
    {
        RpcSetPlayerFoodCounter(newFoodCounter);
    }
    [ClientRpc]
    private void RpcSetPlayerFoodCounter(int newFoodCounter)
    {
        foodCounter = newFoodCounter;
    }
}
