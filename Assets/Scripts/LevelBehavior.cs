using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBehavior : MonoBehaviour
{
    public Dictionary<String, int> materialNameAndCountDict = new Dictionary<string, int>();

    public float timeScaleOfLevel = 1f;
    // Start is called before the first frame update
    void Start()
    {
        CreateMaterialNameAndCountDict();
        DisplayTimeScaleText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateMaterialNameAndCountDict()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            String keyMaterialName = transform.GetChild(i).GetComponent<SphereBehavior>().materialName;
            if (!materialNameAndCountDict.ContainsKey(keyMaterialName))
            {
                materialNameAndCountDict.Add(keyMaterialName,1);
            }
            else
            {
                materialNameAndCountDict[keyMaterialName]++;
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisplayLoseText()
    {
        transform.parent.Find("WinLoseText").gameObject.GetComponent<TMP_Text>().text = "You Lose!";
    }

    public void DisplayWinText()
    {
        transform.parent.Find("WinLoseText").gameObject.GetComponent<TMP_Text>().text = "You Win!";
    }

    public void CheckIfLevelWon()
    {
        if (materialNameAndCountDict.Count == 0)
        {
            DisplayWinText();
            transform.parent.Find("NextLevelButton").gameObject.SetActive(true);
        }
    }

    public void GoToNextLevel()
    {
        int currSceneIndex =  int.Parse(SceneManager.GetActiveScene().name.Remove(0, 5)); //Removes the word 'Scene' from the scene name thus getting its index
        SceneManager.LoadScene("Scene" + (currSceneIndex + 1));
    }

    private void DisplayTimeScaleText()
    {
        transform.parent.Find("TimeScaleText").gameObject.GetComponent<TMP_Text>().text =
            "Time Scale: x" + timeScaleOfLevel;
    }
}
