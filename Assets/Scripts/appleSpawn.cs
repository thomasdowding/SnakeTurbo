using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appleSpawn : MonoBehaviour {

    public int appleID;
    //This variable is used to determine if a red, golden or poisonous apple is spawned.

    public int currentApple;
    //Used as an index to identify which apple in the list of apples is the one currently onscreen

    public System.Random generator = new System.Random();
    //Used to generate the value stored by appleID

    public GameObject redApple;
    public GameObject goldApple;
    public GameObject poisonApple;
    //References the appropriate prefabs so that the apples can be instantiated.

    public List<GameObject> apples = new List<GameObject>();
    //Stores all the apples as they are instantiated.

    public Collider2D playerCollision;
    public Collider2D appleCollision;
    //References the collision of the player and the apples so that they can be eaten by the player

    public Vector3 spawnPOS;
    //Used to tell the game where to instantiate the apples

    void Start()
    {
        currentApple = 0;
        //Sets apple index to 0

        StartCoroutine(Spawner());
        //Starts the apple spawner
    }

    void Update()
    {
        if (Player.Player.health <= 0)
        {
            Destroy(apples[currentApple]);
            //If the player dies, the apple that is onscreen will be destroyed.
        }

        spawnPOS = new Vector3(4000, generator.Next(-1400, 1400), 40);
        //Creates the value used by spawnPOS and assigns it to spawnPOS. x and z always remain the same whilst the y variable changes.

        apples[currentApple].transform.Translate(new Vector3(-1, 0, 0) * Player.Player.speed * Time.deltaTime);
        //Moves the current apple to the left, at the speed stored by the player.

        if (apples[currentApple].transform.position.x < -4500) {
            Destroy(apples[currentApple]);
            currentApple = currentApple + 1;
            StartCoroutine(Spawner());
            //If the apple goes offscreen, it is destroyed and a new one is spawned. The index of apples is also increased so that the new apple can be refered to correctly.
        }

        else if (appleCollision.IsTouching(playerCollision))
        {
            Destroy(apples[currentApple]);
            //If the player touches the apple, the apple is destroyed...

            if (appleID == 0)
            {
                Player.Player.RedApple();
            }

            else if(appleID == 1)
            {
                Player.Player.GoldenApple();
            }

            else if(appleID == 2)
            {
                Player.Player.PoisonApple();
            }

            //... and the appleID value is used to call the correct Player function for the type of apple eaten, which will perform the code necessary to have the relevant effect on the snake.

            currentApple = currentApple + 1;
            //The apple index is then increased by 1

            StartCoroutine(Spawner());
            //And another apple is spawned
        }

    }

    IEnumerator Spawner() {
        yield return new WaitForSeconds(Player.Player.waitSeconds);

        appleID = generator.Next(0, 3);
        //Stores a random value in the appleID variable, which will be used to determine which type of apple to spawn

        yield return new WaitForSeconds(Player.Player.waitSeconds);
        //Waits for the specified period of time before continuing with the code. This is to prevent the game from instaniating apples without enough time in-between (the value used gets lower as the game goes on to make it harder) 

        if (appleID == 0)
        {
            apples.Add(Instantiate(redApple, spawnPOS, Quaternion.identity));
        }

        if (appleID == 1)
        {
            apples.Add(Instantiate(goldApple, spawnPOS, Quaternion.identity));
        }

        if (appleID == 2)
        {
            apples.Add(Instantiate(poisonApple, spawnPOS, Quaternion.identity));
            
        }

        //Instantiates the correct apple depending on the value of appleID and at the position previously set in Update()

        appleCollision = apples[currentApple].AddComponent<BoxCollider2D>();
        //Sets the reference created earlier to the collision of the current apple
    }
}
