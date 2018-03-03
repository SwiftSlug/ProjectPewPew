﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerState
{
    public InputController controller;

    public ControllerState(InputController controllerRef)
    {
        //  Set the controller reference for the controller state
        controller = controllerRef;
    }

    public abstract void Update();
}

public class DefaultControllerState : ControllerState
{
    public DefaultControllerState(InputController controllerRef) : base(controllerRef)
    {        
    }

    public override void Update()
    {

        //Debug.Log("Default Controller in Use");

        //  Movement -----------------------------------------------------------

        //  Horizontal Input
        if (Input.GetAxis("Horizontal") != 0)
        {
            controller.playerControllerScript.AddHoritonzalMovement(Input.GetAxis("Horizontal"));
        }

        //  Vertical Input
        if (Input.GetAxis("Vertical") != 0)
        {
            controller.playerControllerScript.AddVerticalMovement(Input.GetAxis("Vertical"));
        }

        //  Aiming -------------------------------------------------------------

        //  Mouse Aiming
        controller.playerControllerScript.MouseAim();

        //  Controller Aiming
        if ((Input.GetAxis("ControllerLookX") > 0) || ((Input.GetAxis("ControllerLookY") > 0)))
        {
            controller.playerControllerScript.ControllerAiming();
        }

        //  Interaction --------------------------------------------------------

        if (Input.GetButton("Interact"))
        {
            controller.playerControllerScript.LootObject();

        }

        //  Weapon handling ----------------------------------------------------

        //  Reloading
        if (Input.GetButton("Reload"))
        {
            controller.weaponShootingScript.StartReload();
        }

        //  Shooting
        if (Input.GetButton("Fire1") || (Input.GetAxis("Fire1") > 0))
        {
            controller.weaponShootingScript.StartShoot();
        }

        //  Weapon swapping

        //  Weapon 1
        if ((Input.GetButton("Weapon1")) || (Input.GetAxis("Weapon1") > 0))
        {
            controller.weaponSwapScript.changeWeapon(0);
        }

        //  Weapon 2
        if ((Input.GetButton("Weapon2")) || (Input.GetAxis("Weapon2") > 0))
        {
            controller.weaponSwapScript.changeWeapon(1);
        }

        //  Weapon 3
        if ((Input.GetButton("Weapon3")) || (Input.GetAxis("Weapon3") > 0))
        {
            controller.weaponSwapScript.changeWeapon(2);
        }

        //  Weapon 4 - Not currently in use
        /*
        if ((Input.GetButton("Weapon3")) || (Input.GetAxis("Weapon3") > 0))
        {
            weaponSwapScript.changeWeapon(3);
        }
        */

        if (Input.GetButton("BaseBuilding"))
        {
            controller.playerBuildingControllerScript.EnterOrExitBuildMode();

            //  Switch Controller Class to Base Building
            //controller.currentControllerState = controller.buidlingControllerState;
            controller.ChangeState(controller.buidlingControllerState);
            //Debug.Log("State Should Change");
        }

    }
}

public class BaseBuildingControllerState : ControllerState
{
    public BaseBuildingControllerState(InputController controllerRef) : base(controllerRef)
    {
    }

    public override void Update()
    {

        //Debug.Log("Base Building Controller in Use");

        //  Movement -----------------------------------------------------------

        //  Horizontal Input
        if (Input.GetAxis("Horizontal") != 0)
        {
            controller.playerControllerScript.AddHoritonzalMovement(Input.GetAxis("Horizontal"));
        }

        //  Vertical Input
        if (Input.GetAxis("Vertical") != 0)
        {
            controller.playerControllerScript.AddVerticalMovement(Input.GetAxis("Vertical"));
        }

        //  Aiming -------------------------------------------------------------

        //  Mouse Aiming
        controller.playerControllerScript.MouseAim();

        //  Controller Aiming
        if ((Input.GetAxis("ControllerLookX") > 0) || ((Input.GetAxis("ControllerLookY") > 0)))
        {
            controller.playerControllerScript.ControllerAiming();
        }

        //  Interaction --------------------------------------------------------

        if (Input.GetButton("Interact"))
        {
            // Place Object Here
        }

        //  Base Building ------------------------------------------------------

        // Building Mode Switch
        if (Input.GetButton("BaseBuilding"))
        {
            controller.playerBuildingControllerScript.EnterOrExitBuildMode();

            //  Swtich back to default controller class
            //controller.currentControllerState = controller.defaultControllerState;
            controller.ChangeState(controller.defaultControllerState);
        }

        //  Place Building
        if (Input.GetButton("BuildingPlace"))
        {
            controller.playerBuildingControllerScript.PlaceBuilding();
        }

        //  Change Rotation
        if (Input.GetButton("BuildingChangeRotation"))
        {
            controller.playerBuildingControllerScript.ChangeRotation();
        }

        //  Change Structure
        if (Input.GetButton("BuildingChangeStructure"))
        {
            controller.playerBuildingControllerScript.ChangeStructure();
        }

        //  Change Material
        if (Input.GetButton("BuildingChangeMaterial"))
        {
            controller.playerBuildingControllerScript.ChangeMaterial();
        }


    }
}

public class DownedControllerState : ControllerState
{
    public DownedControllerState(InputController controllerRef) : base(controllerRef)
    {
    }

    public override void Update()
    {
    }
}

public class InputController : MonoBehaviour {

    //  This class handles all of the player input for the players within the game
    //  It detects the input from the player and then calls the corresponding functionality
    //  from the desired scripts attached to the player
    //  The actions that can be called are dependants on the current state of the controller

    
    //public bool blep = false;    

    public WeaponShooting weaponShootingScript;
    public WeaponSwap weaponSwapScript;
    public PlayerSkills playerSkillScript;
    public PlayerController playerControllerScript;
    public PlayerBuildingController playerBuildingControllerScript;

    
    //  Controller States -------------------------------------------------------

    //  Main controller State
    public ControllerState currentControllerState;

    public DefaultControllerState defaultControllerState;
    public BaseBuildingControllerState buidlingControllerState;
    public ControllerState downedControllerState;

    // Use this for initialization
    void Start () {
        weaponShootingScript = GetComponent<WeaponShooting>();
        weaponSwapScript = GetComponent<WeaponSwap>();
        playerSkillScript = GetComponent<PlayerSkills>();
        playerControllerScript = GetComponent<PlayerController>();
        playerBuildingControllerScript = GetComponent<PlayerBuildingController>();

        defaultControllerState = new DefaultControllerState(this);
        buidlingControllerState = new BaseBuildingControllerState(this);

        currentControllerState = defaultControllerState;
    }

    // Update is called once per frame
    void Update()
    {
        currentControllerState.Update();
        //Debug.Log(defaultControllerState.GetType().Name);
    }

    public void ChangeState(ControllerState newState)
    {
        currentControllerState = newState;
    }

    void AllInputMethods() { 

        //  Movement -----------------------------------------------------------

        //  Horizontal Input
        if (Input.GetAxis("Horizontal") != 0)
        {
            playerControllerScript.AddHoritonzalMovement(Input.GetAxis("Horizontal"));
        }

        //  Vertical Input
        if (Input.GetAxis("Vertical") != 0)
        {
            playerControllerScript.AddVerticalMovement(Input.GetAxis("Vertical"));
        }


        //  Aiming -------------------------------------------------------------

        //  Mouse Aiming
        playerControllerScript.MouseAim();

        //  Controller Aiming
        if( (Input.GetAxis("ControllerLookX") > 0) || ((Input.GetAxis("ControllerLookY") > 0)) )
        {
            playerControllerScript.ControllerAiming();
        }



        //  Interaction --------------------------------------------------------

        if (Input.GetButton("Interact"))
        {
            playerControllerScript.LootObject();

        }


        //  Skill abilities ---------------------------------------------------

        //  Skill 1
        if (Input.GetButton("Skill1"))
        {
            playerSkillScript.SkillButtonDown();
        }
        else
        {
            playerSkillScript.SkillButtonUp();
        }


        //  Weapon handling ----------------------------------------------------

        //  Reloading
        if (Input.GetButton("Reload"))
        {
            weaponShootingScript.StartReload();
        }

        //  Shooting
        if (Input.GetButton("Fire1") || (Input.GetAxis("Fire1") > 0))
        {
            weaponShootingScript.StartShoot();
        }

        //  Weapon swapping

        //  Weapon 1
        if ((Input.GetButton("Weapon1")) || (Input.GetAxis("Weapon1") > 0))
        {
            weaponSwapScript.changeWeapon(0);
        }

        //  Weapon 2
        if ((Input.GetButton("Weapon2")) || (Input.GetAxis("Weapon2") > 0))
        {
            weaponSwapScript.changeWeapon(1);
        }

        //  Weapon 3
        if ((Input.GetButton("Weapon3")) || (Input.GetAxis("Weapon3") > 0))
        {
            weaponSwapScript.changeWeapon(2);
        }

        //  Weapon 4 - Not currently in use
        /*
        if ((Input.GetButton("Weapon3")) || (Input.GetAxis("Weapon3") > 0))
        {
            weaponSwapScript.changeWeapon(3);
        }
        */


        //  Base Building ------------------------------------------------------

        // Building Mode Switch
        if (Input.GetButton("BaseBuilding"))
        {
            playerBuildingControllerScript.EnterOrExitBuildMode();
        }

        //  Place Building
        if (Input.GetButton("BuildingPlace"))
        {
            playerBuildingControllerScript.PlaceBuilding();
        }

        //  Change Rotation
        if (Input.GetButton("BuildingChangeRotation"))
        {
            playerBuildingControllerScript.ChangeRotation();
        }

        //  Change Structure
        if (Input.GetButton("BuildingChangeStructure"))
        {
            playerBuildingControllerScript.ChangeStructure();
        }

        //  Change Material
        if (Input.GetButton("BuildingChangeMaterial"))
        {
            playerBuildingControllerScript.ChangeMaterial();
        }

    }
}
