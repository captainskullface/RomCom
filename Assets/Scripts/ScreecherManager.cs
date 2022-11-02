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

    private void Awake()
    {
        screecherMan = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (onDeck.Count == 0)
            return;

        if (canPost)
            makeScreech(onDeck[0]);
    }

    IEnumerator CoolDown()
    {
        canPost = false;
        yield return new WaitForSecondsRealtime(Random.Range(postIntervalRange.x, postIntervalRange.y));
        canPost = true;
    }

    public void newScreech(string content)
    {
        onDeck.Add(content);
    }

    void makeScreech(string content)
    {
        StartCoroutine(CoolDown());

        GameObject newScreech = Instantiate(screechPrefab);
        newScreech.transform.SetParent(timeLine);
        newScreech.transform.localScale = Vector3.one;

        screeches.Add(newScreech);

        Screech screechLogic = newScreech.GetComponent<Screech>();
        screechLogic.setUp(content);

        onDeck.Remove(onDeck[0]);
    }
}
