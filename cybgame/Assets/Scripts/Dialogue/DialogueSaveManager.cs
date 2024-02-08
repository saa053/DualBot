using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueSaveManager : MonoBehaviour
{
    public static DialogueSaveManager instance;
    void Awake()
    {
        instance = this;
    }

    public bool SaveStoryState(Story story, string saveString)
    {
        if (story != null)
        {
            string storyStateJson = story.state.ToJson();

            PlayerPrefs.SetString(saveString, storyStateJson);
            PlayerPrefs.Save();

            return true;
        }

        return false;
 
    }

    public Story LoadStoryState(Story story, string saveString)
    {
        string storyStateJson = PlayerPrefs.GetString(saveString);
        if (!string.IsNullOrEmpty(storyStateJson))
        {
            story.state.LoadJson(storyStateJson);
            return story;
        }
   
        return null;
  
    }

    /* private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    } */
}
