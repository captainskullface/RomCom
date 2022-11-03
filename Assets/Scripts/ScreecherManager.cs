using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreecherManager : MonoBehaviour
{
    public static ScreecherManager screecherMan;

    [SerializeField]
    Transform timeLine;

    [SerializeField]
    GameObject screechPrefab;

    [SerializeField]
    List<string> onDeck = new List<string>();

    [SerializeField]
    List<GameObject> screeches = new List<GameObject>();

    [SerializeField]
    Vector2 postIntervalRange;

    bool canPost = true;

    [SerializeField]
    List<Sprite> possibleImages = new List<Sprite>();

    List<Sprite> imageBank = new List<Sprite>();

    [SerializeField]
    float chanceForImage = 25f; //Percent

    private void Awake()
    {
        screecherMan = this;
        imageBank.AddRange(possibleImages);
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (onDeck.Count == 0)
            return;

        if (canPost)
        {
            bool forceImage = false;

            if (onDeck[0].Contains("/"))
            {
                onDeck[0] = onDeck[0].Replace("/", "");
                forceImage = true;
            }

            makeScreech(onDeck[0], forceImage);
        }  
    }

    IEnumerator CoolDown()
    {
        canPost = false;
        yield return new WaitForSecondsRealtime(Random.Range(postIntervalRange.x, postIntervalRange.y));
        canPost = true;
    }

    public void newScreech(string content)
    {
        content = InkHandler.ProcessText(content);
            
        onDeck.Add(content);
    }

    void makeScreech(string content, bool forceImage = false)
    {
        StartCoroutine(CoolDown());

        GameObject newScreech = Instantiate(screechPrefab);
        newScreech.transform.SetParent(timeLine);
        newScreech.transform.localScale = Vector3.one;

        screeches.Add(newScreech);

        Screech screechLogic = newScreech.GetComponent<Screech>();

        Sprite imageSet = null;

        float chance = Random.Range(0, 100);
        if (chance < chanceForImage || forceImage)
        {
            int index = Random.Range(0, possibleImages.Count);
            imageSet = possibleImages[index];
            possibleImages.RemoveAt(index);

            if (possibleImages.Count == 0)
                ResetImages();
        }

        screechLogic.setUp(content, imageSet);

        onDeck.Remove(onDeck[0]);
    }

    void ResetImages()
    {
        possibleImages.AddRange(imageBank);
    }
}
