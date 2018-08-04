﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PublicCode;

public class UserInput : MonoBehaviour
{
    public InputManager inputManager;
    public GameObject WindowMenu;
    public GameObject WindowUI;
    bool IsOutOfFocus = false;
    public bool GamePaused = false;
    bool StopCameraControls = false;

    private void Start()                                                                //Triggered on start
    {
    }
    private void Update()                                                               //Triggered before frame update
    {
        
    }
    private void LateUpdate()                                                           //Triggered after frame update
    {
        if (!IsOutOfFocus)                                                                          //If the game isn't paused
        {
            AlwaysControls();                                                                   //Controls that always need to be executed (like the ESC button)
            if (!StopCameraControls)                                                            //If we are somewhere where we dont want to control the camera
            {
                ExecuteInputs();                                                                //Check if we need to move the camera                                       
            }
        }
    }
    public void CameraControls(bool SetTo)                                              //With this buttons can change the camera mode
    {
        StopCameraControls = !SetTo;                                                            //Set the camera mode to what ever there is given (CameraControls FALSE = Stop camera)
    }
    void OnApplicationFocus(bool hasFocus)                                              //Triggered when the game is in focus
    {
        IsOutOfFocus = !hasFocus;                                                                   //Set game to be in focus
    }
    void OnApplicationPause(bool pauseStatus)                                           //Triggered when the game is out focus
    {
        IsOutOfFocus = pauseStatus;                                                                 //Set game to be out of focus
    }
    private void AlwaysControls()                                                       //Triggered in LateUpdate (unless the game is out of focus)
    {
        if (inputManager.GetButtonDownOnce("Menu"))                                             //If the Open/Close menu button is pressed
        {
            StopCameraControls = !WindowMenu.activeSelf;                                        //Flag that the camera controls should be active or not
            WindowMenu.SetActive(StopCameraControls);                                           //Set the menu's visibility
        }
        if (inputManager.GetButtonDownOnce("Pause"))                                            //If the Open/Close menu button is pressed
        {
            GamePaused = true;
        }
    }
    private void ExecuteInputs()                                                        //Triggered in LateUpdate (unless the game is out of focus, or camera controls are disabled) this controlls the camera movement
    {
        if (inputManager.GetButtonDownOnce("Toggle UI"))                                        //If the Toggle UI button is pressed
        {
            WindowUI.SetActive(!WindowUI.activeSelf);                                           //Goggle the UI
        }
        float X = Camera.main.transform.position.x;                                             //Get main camera location
        float Y = Camera.main.transform.position.y;                                             //^
        float Z = Camera.main.transform.position.z;                                             //^
        float Xr = Camera.main.transform.eulerAngles.x;                                         //Get main camera rotation
        float Yr = Camera.main.transform.eulerAngles.y;                                         //^
        float Speed = Y * JelleWho.HeighSpeedIncrease;                                          //The height has X of speed increase per block
        //Edge scroll
        float xpos = Input.mousePosition.x;                                                     //Save mouse position
        float ypos = Input.mousePosition.y;                                                     //^
        if (xpos >= 0 && xpos < JelleWho.MoveIfThisCloseToTheSides)                             //horizontal camera movement
        {
            float R = -JelleWho.MoveEdgeScrollSpeed * Speed;                                    //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(R, 0, 0);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        else if (xpos <= Screen.width && xpos > Screen.width - JelleWho.MoveIfThisCloseToTheSides)
        {
            float R = +JelleWho.MoveEdgeScrollSpeed * Speed;                                    //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(R, 0, 0);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        if (ypos >= 0 && ypos < JelleWho.MoveIfThisCloseToTheSides)                             //vertical camera movement
        {
            float R = -JelleWho.MoveEdgeScrollSpeed * Speed;                                    //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(0, 0, R);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        else if (ypos <= Screen.height && ypos > Screen.height - JelleWho.MoveIfThisCloseToTheSides)
        {
            float R = +JelleWho.MoveEdgeScrollSpeed * Speed;                                    //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(0, 0, R);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        if (inputManager.GetButtonDown("Left"))
        {
            float R = -JelleWho.MoveSpeedKeyboard * Speed;                                      //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(R, 0, 0);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        if (inputManager.GetButtonDown("Right"))
        {
            float R = JelleWho.MoveSpeedKeyboard * Speed;                                       //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(R, 0, 0);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        if (inputManager.GetButtonDown("Down"))
        {
            float R = -JelleWho.MoveSpeedKeyboard * Speed;                                      //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(0, 0, R);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        if (inputManager.GetButtonDown("Up"))
        {
            float R = JelleWho.MoveSpeedKeyboard * Speed;                                       //Set the distance we need to move
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            Vector3 Range = new Vector3(0, 0, R);                                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        if (inputManager.GetButtonDown("Drag"))                                                 //If the Drag button is presse
        {
            Vector2 LookingAt = Camera.main.transform.eulerAngles;                              //The angles we are looking at
            float MovementX = -Input.GetAxis("Mouse X") * JelleWho.MoveSpeedMouse * Speed;      //Calculate howmuch we need to move in the axes
            float MovementY = -Input.GetAxis("Mouse Y") * JelleWho.MoveSpeedMouse * Speed;      //^
            Vector3 Range = new Vector3(MovementX, 0, MovementY);                               //The range where we need to go relative to the angles
            Vector3 XYZ = PolarToCartesian(LookingAt, Range);                                   //Calculate the XYZ values
            X += XYZ.x;                                                                         //Move bit
            Z += XYZ.z;                                                                         //^
        }
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");                           //Get the scrollwheel location
        if (ScrollWheelChange != 0)                                                             //If the scrollwheel has changed
        {                          
            float R = ScrollWheelChange * JelleWho.ZoomScrollWheelSpeed * Speed;                //The radius from current camera
            Vector3 XYZ = PolarToCartesian(Camera.main.transform.eulerAngles, new Vector3(0, 0, R)); //Calculate the Polar coord of the camera to the XYZ Cartesian coords
            X += XYZ.x;                                                                         //Scroll a bit
            Y += XYZ.y;                                                                         //^
            Z += XYZ.z;                                                                         //^
        }
        if (inputManager.GetButtonDown("Rotate left"))
        {
            Yr -= JelleWho.RotateSpeedKeyboard;                                                 //Get the mouse movement
        }
        if (inputManager.GetButtonDown("Rotate right"))
        {
            Yr += JelleWho.RotateSpeedKeyboard;                                                 //Get the mouse movement
        }
        if (inputManager.GetButtonDown("Rotate"))                                               //If the rotate button is pressed
        {
            Xr -= Input.GetAxis("Mouse Y") * JelleWho.RotateSpeedMouse;                         //Get the mouse movement
            Yr += Input.GetAxis("Mouse X") * JelleWho.RotateSpeedMouse;                         //^
        }
        //Limit movement
        if (X > JelleWho.MaxMoveHorizontalOnMap)                                                //Limit range North South(?)
        {
            X = JelleWho.MaxMoveHorizontalOnMap;                                                //^
        }
        else if (X < -1 * JelleWho.MaxMoveHorizontalOnMap)                                      //^
        {
            X = -1 * JelleWho.MaxMoveHorizontalOnMap;                                           //^
        }
        if (Y > JelleWho.MaxCameraHeight)                                                       //Limit height
        {   
            Y = JelleWho.MaxCameraHeight;                                                       //^
        }
        else if (Y < JelleWho.MinCameraHeight)                                                  //^
        {
            Y = JelleWho.MinCameraHeight;                                                       //^
        }
        if (Z > JelleWho.MaxMoveHorizontalOnMap)                                                //Limit range South West(?)
        {
            Z = JelleWho.MaxMoveHorizontalOnMap;                                                //
        }
        else if (Z < -1 * JelleWho.MaxMoveHorizontalOnMap)                                      //
        {
            Z = -1 * JelleWho.MaxMoveHorizontalOnMap;                                           //
        }
        if (Xr < 0)                                                                             //If the camera rotation is to HIGH
        {
            Xr = 0;                                                                             //Set camera to max HIGH level
        }
        if (Xr > 90)                                                                            //If the camera rotation is to LOW
        {
            Xr = 90;                                                                            //Set camera to max LOW level
        }
        Camera.main.transform.position = new Vector3(X, Y, Z);                                  //Move the main camera
        Camera.main.transform.eulerAngles = new Vector2(Xr, Yr);
    }
    Vector3 PolarToCartesian(Vector2 polar, Vector3 Offset)                                     //Offset=(Left, Up, Forward)
    {
        var rotation = Quaternion.Euler(polar.x, polar.y, 0);                                   //Convert it
        return rotation * Offset;                                                               //Return the Vector 3 of the target point
    }
}

/*
    public GameObject Tempblock;
    void Temp()
    {
        Vector3 OffsetXYZ = Camera.main.transform.position;                                     //The Starting position
        Vector2 LookingAt = Camera.main.transform.eulerAngles;                                  //The angles we are looking at
        Vector3 Range = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));     //The range where we need to go relative to the angles
        Tempblock.transform.position = OffsetXYZ + PolarToCartesian(LookingAt, Range);          //Calculate and move
    }
    */
