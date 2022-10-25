using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
public class ScreechContent : MonoBehaviour
{
    [Header("Ink Story")] 
    [SerializeField] TextAsset inkText;
    Story inkStory;
    [Header("Displaying Text")]
    [SerializeField] TextMeshProUGUI screeches;
    // Start is called before the first frame update
    void Start()
    {

        inkStory = new Story(inkText.text);
    }

    // Update is called once per frame
    void Update()
    {

        ScreechStuff();
        
        
    }
    public void ScreechStuff()
    {
        screeches.text = inkStory.Continue();
    }
}
