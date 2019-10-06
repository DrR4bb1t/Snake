using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnLogic : NetworkBehaviour
{
    [SerializeField]
    private PlayerLogic player;

    [SerializeField]
    private GameLogic gameLogic;

    private Sprite sprite;
    private Vector2 foodPosition;
    private Vector2 flashPosition;
    private Vector2 foodSize = new Vector2(64, 64);
    private float worldWidth;
    private float worldHeight;

    //[SyncVar]
    public GameObject food;
    //[SyncVar]
    public GameObject flash;
    [SyncVar]
    public float cooldown = 25f;
    [SyncVar]
    public bool foodSpawned = false;
    [SyncVar]
    public bool powerupSpawned = false;
    private void Start()
    {
        World(4.5f, 3.3f);
    }

    private void Update()
    {
        if (gameLogic.startedGame == true)
        {
            if (foodSpawned == false)
                FoodSpawning();
            if (player.flashmode == false && powerupSpawned == false)
                Countdown();
        }
    }


    public void World(float worldWidth, float worldHeight)
    {
        this.worldWidth = worldWidth;
        this.worldHeight = worldHeight;
    }

    private void SpawnFood()
    {
        //Creates the Box with its components.
        food = new GameObject("Food");
        SpriteRenderer renderer = food.AddComponent<SpriteRenderer>();
        Rigidbody2D rigidbody2D = food.AddComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = food.AddComponent<BoxCollider2D>();
        NetworkIdentity networkIdentity = food.AddComponent<NetworkIdentity>();

        //Sets values of components.
        rigidbody2D.gravityScale = 0;
        boxCollider.size = new Vector2(0.64f, 0.64f);
        sprite = Resources.Load<Sprite>("Sprites/snakeRobotics_box");
        renderer.sprite = sprite;
        foodPosition = new Vector2(Random.Range(-4.6f, worldWidth), Random.Range(-0.9f, worldHeight));
        food.transform.position = new Vector3(foodPosition.x, foodPosition.y, -0.1f);
        //FoodSpawned();
        foodSpawned = true;
    }

    public void FoodSpawning()
    {
        SpawnFood();
        //CmdFoodSpawning(food);
    }

    //[Command]
    //private void CmdFoodSpawning(GameObject spawnedFood)
    //{
    //    RpcFoodSpawning(spawnedFood);
    //}

    //[ClientRpc]
    //private void RpcFoodSpawning(GameObject spawnedFood)
    //{
    //    food = spawnedFood;
    //}

    private void FoodSpawned()
    {
        CmdFoodIsSpawned(true);
    }

    [Command]
    private void CmdFoodIsSpawned(bool newState)
    {
        RpcSetFoodIsSpawned(newState);
    }

    [ClientRpc]
    private void RpcSetFoodIsSpawned(bool newState)
    {
        foodSpawned = newState;
    }

    public void SpawnPowerUp()
    {
        //Creates the Box with its components.
        flash = new GameObject("Flash");
        SpriteRenderer renderer = flash.AddComponent<SpriteRenderer>();
        Rigidbody2D rigidbody2D = flash.AddComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = flash.AddComponent<BoxCollider2D>();

        //Sets values of components.
        rigidbody2D.gravityScale = 0;
        boxCollider.size = new Vector2(0.64f, 0.64f);
        sprite = Resources.Load<Sprite>("Sprites/snakeRobotics_flash");
        renderer.sprite = sprite;
        flashPosition = new Vector2(Random.Range(-4.6f, worldWidth), Random.Range(-0.9f, worldHeight));
        flash.transform.position = new Vector3(flashPosition.x, flashPosition.y, -0.1f);
    }

    private void Countdown()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            SpawnPowerUp();
            powerupSpawned = true;
        }
    }
}
