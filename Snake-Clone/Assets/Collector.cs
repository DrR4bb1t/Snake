using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Collector : MonoBehaviour
{
    [SerializeField]
    private PlayerLogic player;

    [SerializeField]
    private Text playerCounter;


    private List<GameObject> collection = new List<GameObject>();
    //public List<Transform> positionList = new List<Transform>();

    private Sprite sprite;
    private GameObject bodyPart;
    private Vector3 bodyPartPosition;
    private string bodyPartName;
    private float moveSpeed;
    private int direction;
    private float deltaY;
    private float deltaX;
    private float bodySize;
    private float bodyPartSize = 0.64f;

    private Vector3 storedPlayerPosition;
    private string bodyPartSprite;

    private void Update()
    {
        if (player.foodCounter > 0)
        {
            bodyPartMoving();
        }
    }

    public void AddBodyPartToBody()
    {
        collection.Add(bodyPart);
        CreateBodyPart();
        UpdateCounters();
    }

    private void CreateBodyPart()
    {
        

        bodyPartName = "bodyPart";
        //bodyPartName = "bodyPart" + player.foodCounter;
        bodyPart = new GameObject(bodyPartName);
        SpriteRenderer renderer = bodyPart.AddComponent<SpriteRenderer>();
        Rigidbody2D rigidbody2D = bodyPart.AddComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = bodyPart.AddComponent<BoxCollider2D>();
        //bodyPartstats bodyPartstat = bodyPart.AddComponent<bodyPartstats>();
        //Sets values of components.

        if (player.playerID == 1)
            bodyPartSprite = "Sprites/snakeRobot_link_noWheel_purple";
        else
            bodyPartSprite = "Sprites/snakeRobot_link_noWheel_red";

        //bodyPartstat.bodyPartID = bodyPartstat.bodyPartID + player.foodCounter;
        rigidbody2D.gravityScale = 0;
        //rigidbody2D.isKinematic = true; //TOBETESTED
        boxCollider.size = new Vector2(0.64f, 0.64f);
        sprite = Resources.Load<Sprite>(bodyPartSprite);
        renderer.sprite = sprite;
        bodyPartPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        bodyPart.transform.position = new Vector3(bodyPartPosition.x, bodyPartPosition.y, -0.1f);
    }

    public void UpdateCounters()
    {
        string output;
        output = player.foodCounter.ToString();
        playerCounter.text = output;
    }

    public void bodyPartMoving()
    {
        //Debug.Log("move");
        //Transform lastPosition = positionList[0];
        //bodyPart.transform.position = lastPosition.position;

        //positionList.Clear();

        //storedPlayerPosition = player.transform.position;

        bodySize = bodyPartSize * player.foodCounter;
        moveSpeed = player.moveSpeed;
        direction = player.direction;



        if (direction == 1)
        {
            deltaY = moveSpeed * Time.deltaTime;
            bodyPart.transform.position = player.transform.position + new Vector3(0, deltaY - bodySize, 0f);
        }
        else if (direction == 2)
        {
            deltaY = -moveSpeed * Time.deltaTime;
            bodyPart.transform.position = player.transform.position + new Vector3(0f, deltaY + bodySize, 0f);
        }
        else if (direction == 3)
        {
            deltaX = -moveSpeed * Time.deltaTime;
            bodyPart.transform.position = player.transform.position + new Vector3(deltaX + bodySize, 0f, 0f);
        }
        else if (direction == 4)
        {
            deltaX = moveSpeed * Time.deltaTime;
            bodyPart.transform.position = player.transform.position + new Vector3(deltaX - bodySize, 0f, 0f);
        }
    }
}



