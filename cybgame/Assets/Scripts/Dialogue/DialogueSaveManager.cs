using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

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

    public void ResetStoryState(TextAsset inkTextAsset)
    {
        if (inkTextAsset != null)
        {
            Story story = new Story(inkTextAsset.text);

            string saveString = GetSaveString(inkTextAsset);

            SaveStoryState(story, saveString);
        }
    }

    public bool GetBool(TextAsset text, string boolName)
    {
        Story story = new Story(text.text);
        story = LoadStoryState(story, GetSaveString(text));
        if (story == null)
            return false;
        
        return (bool)story.variablesState[boolName];
    }

    public string GetSaveString(TextAsset text)
    {
        Story story = new Story(text.text);
        return (string)story.variablesState["saveString"];
    }

    public bool GetShouldSave(TextAsset text)
    {
        Story story = new Story(text.text);
        return (bool)story.variablesState["shouldSave"];
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
