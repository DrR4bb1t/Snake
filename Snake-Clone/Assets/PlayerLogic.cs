using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLogic : NetworkBehaviour
{

    [SerializeField]
    private SpawnLogic spawn;

    [SerializeField]
    private GameLogic gameLogic;

    private List<GameObject> completeSnake = new List<GameObject>();

    public int playerID;

    //Values for movement.
    public float moveSpeed = 2f;
    public int m_Direction = 0;
    private float deltaX;
    private float deltaY;

    //Startrotation
    private float rotation = 0;

    [SyncVar]
    public int foodCounter;

    public bool flashmode = false;
    private float upTimeCounter;

    [SerializeField]
    private GameObject m_BodyPartPrefab;

    private bool m_BodyDestinationSet = false;
    private Vector3[] m_BodyPartDestinations = new Vector3[100];

    private void Start()
    {
        if (m_BodyPartPrefab == null)
            Debug.LogError(nameof(m_BodyPartPrefab) + " is Missing on: " + nameof(this.GetType));

        completeSnake.Add(this.gameObject);
        m_BodyPartDestinations[0] = Vector3.zero;
    }

    private void Update()
    {
        if (gameLogic.startedGame == true)
        {
            Move();
            if (flashmode == true)
                UpTimeOfFlashmode();

            UpdateBodyParts();
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
            PlayerFoodCounter();
            Destroy(spawn.food);
            spawn.SpawnFood();

            AddBodyPart(m_BodyPartPrefab);
            //collector.AddBodyPartToBody();
        }
        if (collider.gameObject.name == "Flash")
        {
            EnterFlashmode();
            Destroy(spawn.flash);
        }
    }

    private void AddBodyPart(GameObject prefab)
    {
        GameObject bodyPart = GameObject.Instantiate(m_BodyPartPrefab);
        SetBodyPartSprite(bodyPart, playerID);
        completeSnake.Add(bodyPart);
        //TODO:
        //AddBodyPartPositionOnSnakeTail();
    }

    private void UpdateBodyParts()
    {
        if(completeSnake.Count <= 1)
        {
            Debug.Log("No bodypart attached");
            return;
        }

        if (!m_BodyDestinationSet)
            SetDestination();

        //update
        for (int i = 1; i < completeSnake.Count; i++)
        {
            if ((m_BodyPartDestinations[i] - completeSnake[i].transform.position).magnitude >= 0.1 && (m_BodyPartDestinations[i] - completeSnake[i].transform.position).magnitude <= 0.2)
                m_BodyDestinationSet = false;

            Vector3 dir = (m_BodyPartDestinations[i] - completeSnake[i].transform.position).normalized;
            completeSnake[i].transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void SetDestination()
    {
        if (completeSnake.Count <= 1)
        {
            Debug.Log("No bodypart attached");
            return;
        }

        for (int i = 1; i < completeSnake.Count; i++)
            m_BodyPartDestinations[i] = completeSnake[i - 1].transform.position;

        m_BodyDestinationSet = true;
    }

    private void SetBodyPartSprite(GameObject obj, int plaID)
    {
        string path;

        if (plaID == 1)
            path = "Sprites/snakeRobot_link_noWheel_purple";
        else
            path = "Sprites/snakeRobot_link_noWheel_red";

        obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }

    private void Move()
    {
        if (m_Direction == 1)
        {
            deltaY = moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(0f, deltaY, 0f);
        }
        else if (m_Direction == 2)
        {
            deltaY = -moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(0f, deltaY, 0f);
        }
        else if (m_Direction == 3)
        {
            deltaX = -moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(deltaX, 0f, 0f);
        }
        else if (m_Direction == 4)
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
        m_Direction = 1;
        rotation = 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public void SetDirectionDown()
    {
        m_Direction = 2;
        rotation = -90f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public void SetDirectionLeft()
    {
        m_Direction = 3;
        rotation = -180f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public void SetDirectionRight()
    {
        m_Direction = 4;
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
