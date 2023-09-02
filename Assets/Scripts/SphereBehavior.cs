using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SphereBehavior : MonoBehaviour
{
    public String materialName;
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

    void OnSphereClicked(SphereBehavior clickedSphere)
    {
        if (!isTarget && clickedSphere.materialName.Equals(materialName) )
        {
            inMotion = true;
            destination = clickedSphere.transform.position;
            transform.LookAt(destination);
            
            if(GetComponent<MovingSphereBehavior>()) //if the sphere is a moving sphere, it no longer is
                Destroy(GetComponent<MovingSphereBehavior>());
        }
    }

    void StopMoving()
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
            if (collision.gameObject.GetComponent<SphereBehavior>().materialName.Equals(materialName)) //The colliding sphere is the same color as 'this'
            {
                GetComponentInParent<LevelBehavior>().materialNameAndCountDict[materialName]--;
                
                if (GetComponentInParent<LevelBehavior>().materialNameAndCountDict[materialName] == 1) //if the clicked sphere is the only one left of the clicked color
                {
                    GetComponentInParent<LevelBehavior>().materialNameAndCountDict.Remove(materialName);
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
