﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AIDirector : NetworkBehaviour
{
    //public bool blep;

    public bool shouldAIDebug = true;           //  Debug flag for all debugging logs
    public bool isDay = true;                   //  Boolean that defines if it is day or night
    //GameObject[] EnemyUnits;
    public GameObject enemyToSpawn;             //  Enemy type to spawn, limited to one for this stage of the game

    List<GameObject> enemyUnits;                //  List of all enemy units within the game
    List<GameObject> players;                   //  List of all players in the game

    //GameObject[] Players;
    public int targetIntensityLevelDay = 20;    //  The intensity level the director aims to keep players at during the day
    public int targetIntensityLevelNight = 200; //  The intensity level the director aims to keep players at during the night
    float waveCooldown;                         //  The cooldown time inbetween waves

    public float playerProximitySize = 50;      //  The area size around the player that detects nearby enemies for intensity checks
    public int intensityPerAI = 10;             //  The amount of intensity each AI unit adds to the player
    float intensityIncreasePercentage = 0.2f;   //  The percentage of the new intensity level added per update
    int intensityDecreaseAmount = 2;            //  The amount of intensity that is decreased when its not increasing
        
    //  Timing Varaibles
    public float intensityUpdateInterval = 3.0f;    //  The time interval between updating the player intensity level
    float intensityLastRunTime = 0.0f;              //  The last time the intensity update was ran

    public float spawnInterval = 5.0f;      //  The time interval between spawning groups of enemies
    public float spawnLast = 0.0f;          //  The last time the AI were spawned
    public int aiSpawnGroupSizeNight = 10;       //  The amount of AI to spawn per group at day
    public int aiSpawnGroupSizeDay = 1;       //  The amount of AI to spawn per group at night

    // Use this for initialization
    void Start () {

        Debug.Log("Director Alive !");

        enemyUnits = new List<GameObject>();    // Init AI list
        players = new List<GameObject>();       // Init player list


        //  Search for AI units and store their gameobjects in the list
        int aiCount = 0;
        foreach (AIStats foundAI in FindObjectsOfType<AIStats>())
        {
            enemyUnits.Add(foundAI.gameObject);
            aiCount++;
            //Debug.Log("Enemy Unit Found by Director !");
        }
        if (shouldAIDebug)
        {
            Debug.Log(aiCount + " AI Unit(s) Found");
        }


        int playerCount = 0;
        foreach (PlayerStats foundAI in FindObjectsOfType<PlayerStats>())
        {
            players.Add(foundAI.gameObject);
            playerCount++;
            //Debug.Log("Player Found by director !");
        }
   
        if (shouldAIDebug)
        {
            Debug.Log(playerCount + " Player(s) Found");
        }

    }
	
	// Update is called once per frame
	void Update () {

        //  Update List with new enemy count
        rescanForAI();

        //  Run intensity update if the update interval has been passed   
        if (Time.time > (intensityLastRunTime + intensityUpdateInterval))
        {
            //Debug.Log("Player Intensity Updating");
            updatePlayerIntensity();
            intensityLastRunTime = Time.time;
        }       
        //  Spawn enemies if the spawn interval has been passed
        if(Time.time > (spawnLast + spawnInterval))
        {
            spawnEnemies();
            spawnLast = Time.time;
        }

    }

    void rescanForAI()
    {
        //  Search for AI units and store their gameobjects in the list
        enemyUnits.Clear();
        foreach (AIStats foundAI in FindObjectsOfType<AIStats>())
        {
            if (foundAI.CompareTag("Enemy"))
            {
                enemyUnits.Add(foundAI.gameObject);
            }
        }

    }

    //  Spawn enemies near the players
    void spawnEnemies()
    {
        foreach (GameObject player in players)
        {            
            if (isDay)
            {
                //  Day time spawning
                if (player.GetComponent<PlayerStats>().intensity < targetIntensityLevelDay)
                {
                    spawnUnits(aiSpawnGroupSizeDay, player.transform.position, player);
                }
            }            
            else
            {
                //  Night time spawning
                if (player.GetComponent<PlayerStats>().intensity < targetIntensityLevelNight)
                {
                    spawnUnits(aiSpawnGroupSizeNight, player.transform.position, player);
                }
            }

        }
    }


    //  Spawn a number of AI units around a position and set them to target the provided player
    void spawnUnits(int number, Vector3 position, GameObject targetPlayer)
    {

        // Spawn the units within random locations near the defined position
        for (int i =0; i < number; i++)
        {
            float xOffset = Random.Range(-10.0f, -10.0f);   //  Random x offset
            float zOffset = Random.Range(-10.0f, -10.0f);   //  Random y offset

            Vector3 spawnPosition = new Vector3(position.x + xOffset, position.y + 0.5f, position.z + zOffset);    //  Generate spawn location

            var spawnedEnemy = (GameObject)Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);   //  Create new AI units
            NetworkServer.Spawn(spawnedEnemy);  //  Add spawned unit to server list
        }

    }


    //  This calculates and updates the intensity level for all players currently within the game
    void updatePlayerIntensity()
    {
        
        int foundAI = 0;        //  The number of AI that are nearby the player
        int trackingAI = 0;     //  The number of AI that are currently targetting the player

        foreach (GameObject player in players)
        {
            PlayerStats statsRef = player.GetComponent<PlayerStats>();

            //  Find all enemies that are near the player
            Collider[] hitCollider = Physics.OverlapSphere(player.transform.position, playerProximitySize);
            for (int i = 0; i < hitCollider.Length; i++)
            {
                if (hitCollider[i].CompareTag("Enemy"))
                {
                    //Debug.Log("Enemy Found Near Player");
                    foundAI++;                    
                }
            }

            //  Find all enemies that are targeting the player
            GameObject[] enemyAI = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemyAI.Length; i++)
            {
                if (enemyAI[i].GetComponent<StateController>().target == player)
                {
                    //Debug.Log("Enemy Tracking Player");
                    trackingAI++;
                }
            }
        


            float intensityLevel = 0.0f;                                //  Overall intensity level

            int nearbyEnemyIntensity = (foundAI * intensityPerAI);      //  0 - infinity based on number of enemies near player
            int healthLost = (100 - statsRef.currentHealth);            //  0 - 100 based on how much health has been lost from 100
            //int ammoIntensity = 
            
            float healthIntensity = ((float)healthLost / 100) + 1;      //  1.0 + value that multiplies the intensity based on how low the players health is


            intensityLevel = nearbyEnemyIntensity * healthIntensity;    //  Finial intensity level based on above atributes


            //  Apply Intensity to player

            if (statsRef.intensity < intensityLevel)
            {
                //  Increase intensity up to intensityLevel per update
                statsRef.intensity += intensityIncreasePercentage * intensityLevel;
            }
            else if (statsRef.intensity > 0)
            {
                //  Decrease instensity gradually back to 0
                statsRef.intensity -= intensityDecreaseAmount;
            }

            //  Ensure that intensity does not fall below 0
            if(statsRef.intensity < 0.0f)
            {
                statsRef.intensity = 0.0f;
            }


        }

    }

    //  Debug functions below ---------------------------------

    void OnGUI()
    {
        if (shouldAIDebug)
        {

            //  Player debug -----------------------------------------------

            string playerText = string.Format("Player Intensity Debug \n\n");
            int playerNumber = 0;

            foreach (GameObject player in players)
            {
                //  Get reference to player stats
                PlayerStats statsRef = player.GetComponent<PlayerStats>();

                //  Add player intensity to the print string
                playerText += "Player " + playerNumber.ToString() + "\n";
                playerText += "-------------------" + "\n";
                playerText += "Overall Intensity = " + string.Format(statsRef.intensity.ToString() + "\n");
                //text += "Health Intensity = " + string.Format(statsRef.intensity.ToString() + "\n");

                playerNumber++;
            }

            float playerHeight = 400;
            float playerWidth = 200;
            GUI.Label(new Rect(Screen.width - playerWidth, 0, playerWidth, playerHeight), playerText);


            //  Enemy Debug -----------------------------------------------

            string enemyText = string.Format("Enemy AI Debug \n\n");
            int enemyNumber = 0;

            foreach (GameObject enemy in enemyUnits)
            {                            
                enemyNumber++;
            }

            enemyText += "Active enemy = " + enemyNumber + "\n";

            float enemyHeight = 400;
            float enemyWidth = 200;

            GUI.Label(new Rect(Screen.width - enemyWidth - playerWidth, 0, enemyWidth, enemyHeight), enemyText);

        }

    }

    //  This does not appear to work with NetworkBehaviour :|
    void OnDrawGizmos()
    {
        /*
        foreach (GameObject player in players)
        {
            //Debug.Log("Drawing Sphere");
            //  Get reference to player stats
            PlayerStats statsRef = player.GetComponent<PlayerStats>();

            //Debug.DrawSphere
            //GameObject.CreatePrimitive(PrimitiveType.Sphere);


            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(player.transform.position, 10);
        }
        */
        

    }

}