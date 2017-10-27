﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseBuildingPlayerScript : NetworkBehaviour
{

    public Transform Object;
    public GameObject StructurePrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown ("b"))
        {
            StructurePrefab = (GameObject)Instantiate(StructurePrefab, Object.position, Object.rotation);
            NetworkServer.Spawn(StructurePrefab);

            Debug.Log("build mode engaged!!!");
        }
	}

    private void OnTriggerEnter(Collider CollidedAsset)
    {
       if (CollidedAsset.CompareTag("GridCollider"))
       {
            if (StructurePrefab.GetComponent<WallController>().placeStatus != true)
            {
                StructurePrefab.GetComponent<Transform>().position = new Vector3(CollidedAsset.transform.position.x, CollidedAsset.transform.position.y, CollidedAsset.transform.position.z);
            }
       }
    }
}
