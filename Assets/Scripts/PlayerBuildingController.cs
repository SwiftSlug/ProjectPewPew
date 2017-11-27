﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerBuildingController : NetworkBehaviour
{
    public bool InbuildMode = false;
    public Transform PointToSpawnStructure;
    public GameObject PlayerReference;
    public GameObject StructureSpawnerRef;
    public GameObject TempStructureGuide;
    public GameObject NetworkSpawnedStructure;

    public int StructureNeeded = 0;
    public int MaterialNeeded = 0;

    void EnterOrExitBuildMode()
    {
        if (InbuildMode == true)
        {
            InbuildMode = false;
            DestroyObject(TempStructureGuide);
            TempStructureGuide = null;
        }
        else
        {
            InbuildMode = true;
            TempStructureGuide = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, PointToSpawnStructure.rotation);
        }
    }

    [Command]
    void CmdPlaceStructure()
    {
        NetworkSpawnedStructure = (GameObject)Instantiate(TempStructureGuide, PointToSpawnStructure.position, TempStructureGuide.transform.rotation); //GetComponent<Transform>().rotation);
        NetworkServer.Spawn(NetworkSpawnedStructure);
        NetworkSpawnedStructure.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        NetworkSpawnedStructure.GetComponent<BuildingController>().StructurePlaced = true;
    }

    [Command]
    void CmdRotateStructure()
    {
        TempStructureGuide.transform.Rotate(0, 10, 0);
    }
    [Command]
    void CmdSelectStructure()
    {
        if (StructureNeeded == 2)
        {
            StructureNeeded = 0;
            TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);


        }
        else
        {
            StructureNeeded = StructureNeeded + 1;
            TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);

        }
    }
    [Command]
    void CmdSelectMaterial()
    {
        if (MaterialNeeded == 2)
            {
                MaterialNeeded = 0;
                TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
            }
            else
            {
                MaterialNeeded = MaterialNeeded + 1;
                TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
            }
    }

    // Use this for initialization
    void Awake()
    {
        PlayerReference = gameObject;
        StructureSpawnerRef.GetComponent<BuildingController>().LinkedPlayer = PlayerReference;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (TempStructureGuide != null)
        {
            TempStructureGuide.transform.position = PointToSpawnStructure.position;
            
               // Debug.Log("Information for player: " + PlayerReference);
               // Debug.Log("Black blob location Before Placement: " + PointToSpawnStructure.position);
               // Debug.Log("Placement Ghost Location Before Placement: " + TempStructureGuide.transform.position);
        }

        if (Input.GetKeyDown("b"))
        {
            EnterOrExitBuildMode();
            Debug.Log("build mode on/off");
        }
        if (Input.GetKeyDown(("space")) && (InbuildMode == true))
        {
            CmdPlaceStructure();
           
            
            /* Debug.Log(" ");
            Debug.Log(" ");
            Debug.Log(" ");
            Debug.Log(" ");
            Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Information for player: " + PlayerReference);
            Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Black blob location After Placement: " + PointToSpawnStructure.position);
            Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Placement Ghost Location After Placement: " + TempStructureGuide.transform.position);
            Debug.Log(" ");
            Debug.Log(" ");
            Debug.Log(" ");
            Debug.Log(" ");*/
        }

        if (Input.GetKeyDown(("r")) && (InbuildMode == true))
        {
            CmdRotateStructure();
        }

        if (Input.GetKeyDown("1") && (InbuildMode == true))
        {
            CmdSelectStructure();
        }

        if (Input.GetKeyDown("2") && (InbuildMode == true))
        {
            CmdSelectMaterial();
        }
    }
}


























































//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Networking;
//using UnityEngine;

//public class PlayerBuildingController : NetworkBehaviour
//{
//    public bool InbuildMode = false;
//    public Transform PointToSpawnStructure;
//    public GameObject PlayerReference;
//    public GameObject StructureSpawnerRef;
//    public GameObject TempStructureGuide;
//    public GameObject NetworkStructureToSpawn;

//    public GameObject NetworkSpawnedStructure;

//    public int StructureNeeded = 0;
//    public int MaterialNeeded = 0;

//    [Client]
//    void EnterOrExitBuildMode()
//    {
//        if (InbuildMode == true)
//        {
//            InbuildMode = false;
//            DestroyObject(TempStructureGuide);
//            TempStructureGuide = null;
//        }
//        else
//        {
//            InbuildMode = true;
//            TempStructureGuide = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, PointToSpawnStructure.rotation);
//        }
//    }

//    /*[Client]
//    void ClientPlaceStructure()
//    {
//        Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, TempStructureGuide.GetComponent<Transform>().rotation);
//    }
//    [Server]
//    void ServerPlaceStructure()
//    {
//    NetworkSpawnedStructure = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, TempStructureGuide.GetComponent<Transform>().rotation);
//    NetworkServer.Spawn(NetworkSpawnedStructure);
//    }*/

//    void RotateStructure()
//    {
//        TempStructureGuide.transform.Rotate(0, 10, 0);
//    }

//    // Use this for initialization
//    void Awake()
//    {
//        PlayerReference = gameObject;
//        StructureSpawnerRef.GetComponent<BuildingController>().LinkedPlayer = PlayerReference;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!isLocalPlayer)
//        {
//            return;
//        }

//        if (Input.GetKeyDown("b"))
//        {
//            /*if (InbuildMode == true)
//            {
//                InbuildMode = false;
//                DestroyObject(TempStructureGuide);
//                TempStructureGuide = null;
//            }
//            else
//            {
//                InbuildMode = true;
//                TempStructureGuide = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, PointToSpawnStructure.rotation);
//            }*/
//            EnterOrExitBuildMode();
//            Debug.Log("build mode on/off");
//        }
//        if (Input.GetKeyDown(("space")) && (InbuildMode == true))
//        {
//            //Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, TempStructureGuide.GetComponent<Transform>().rotation);
//            //CmdPlaceStructure();
//            /*if (!isServer)
//            {
//                ClientPlaceStructure();
//            }
//            else
//            {
//                ServerPlaceStructure();
//            }*/


//        }

//        if (Input.GetKeyDown(("r")) && (InbuildMode == true))
//        {
//            //TempStructureGuide.transform.Rotate(0, 10, 0);
//            RotateStructure();
//        }

//        if (Input.GetKeyDown("1") && (InbuildMode == true))
//        {
//            if (StructureNeeded == 2)
//            {
//                StructureNeeded = 0;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Structure Needed: " + StructureNeeded);
//            }
//            else
//            {
//                StructureNeeded = StructureNeeded + 1;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Structure Needed: " + StructureNeeded);
//            }
//        }

//        if (Input.GetKeyDown("2") && (InbuildMode == true))
//        {
//            if (MaterialNeeded == 2)
//            {
//                MaterialNeeded = 0;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Material Needed: " + MaterialNeeded);
//            }
//            else
//            {
//                MaterialNeeded = MaterialNeeded + 1;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Material Needed: " + MaterialNeeded);
//            }
//        }

//        if (TempStructureGuide != null)
//        {
//            TempStructureGuide.transform.position = PointToSpawnStructure.position;
//        }
//    }
//}

