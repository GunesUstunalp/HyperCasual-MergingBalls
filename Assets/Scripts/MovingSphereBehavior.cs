using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphereBehavior : MonoBehaviour
{
    public Vector3[] pointsToGo; //The sphere moves between points until moved by player action

    private int currDestinationIndex = 0;
    private float movementSpeed = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pointsToGo.Length; i++)
        {
            pointsToGo[i] += transform.position; //pointsToGo initially accepts offsets, this translates them to real destination coordinates
        }
        transform.LookAt(pointsToGo[currDestinationIndex]);
        movementSpeed = GetComponentInParent<LevelBehavior>().timeScaleOfLevel * 4f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (movementSpeed * Time.deltaTime);
        
        if (Vector3.Distance(pointsToGo[currDestinationIndex], transform.position) < 0.1f)
        {
            currDestinationIndex = (currDestinationIndex + 1) % pointsToGo.Length;    
            transform.LookAt(pointsToGo[currDestinationIndex]);
        }
    }
}
