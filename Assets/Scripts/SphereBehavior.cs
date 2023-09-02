using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SphereBehavior : MonoBehaviour
{
    public String colorName;
    private bool inMotion = false;
    private bool isTarget = false;
    private bool isGameLost = false;
    private float movementSpeed = 4f;
    private Vector3 destination;
    
    void Start()
    {
        movementSpeed = GetComponentInParent<LevelBehavior>().timeScaleOfLevel * 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMotion)
        {
            transform.position += transform.forward * (movementSpeed * Time.deltaTime);
        }
    }

    private void OnSphereClicked(SphereBehavior clickedSphere)
    {
        if (!isTarget && clickedSphere.colorName.Equals(colorName) )
        {
            inMotion = true;
            destination = clickedSphere.transform.position;
            transform.LookAt(destination);
            
            if(GetComponent<MovingSphereBehavior>()) //if the sphere is a moving sphere, it no longer is
                Destroy(GetComponent<MovingSphereBehavior>());
        }
    }

    private void StopMoving()
    {
        inMotion = false;
        isGameLost = true;
    }
    
    void OnMouseDown() {
        if (!inMotion && !isGameLost)
        {
            isTarget = true;
            transform.parent.gameObject.BroadcastMessage("OnSphereClicked", this);
            
            if(GetComponent<MovingSphereBehavior>()) //if the sphere is a moving sphere, it no longer is
                Destroy(GetComponent<MovingSphereBehavior>());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isTarget)
        {
            if (collision.gameObject.GetComponent<SphereBehavior>().colorName.Equals(colorName)) //The colliding sphere is the same color as 'this'
            {
                GetComponentInParent<LevelBehavior>().colorAndCountDict[colorName]--;
                
                if (GetComponentInParent<LevelBehavior>().colorAndCountDict[colorName] == 1) //if the clicked sphere is the only one left of the clicked color
                {
                    GetComponentInParent<LevelBehavior>().colorAndCountDict.Remove(colorName);
                    Destroy(collision.gameObject);
                    GetComponentInParent<LevelBehavior>().CheckIfLevelWon();
                }
                Destroy(gameObject);
            }
            else
            {
                GetComponentInParent<LevelBehavior>().DisplayLoseText();
                transform.parent.gameObject.BroadcastMessage("StopMoving");
                
                transform.parent.transform.parent.Find("RestartButton").gameObject.GetComponent<Image>().color = Color.yellow; //just to make the restartButton to emphasize the need to restart after losing
            }
        }
    }
}
