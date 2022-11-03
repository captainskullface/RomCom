using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class PublishingManager : MonoBehaviour
{
    public static PublishingManager publishMan;

    public float money = 20000f;

    [SerializeField]
    float displayMoney = 0;

    [SerializeField]
    float pendingMoney = 0f;

    int bookPublishCost = 5000;

    int maxBookRevenue = 1000000;
    float marketingWeight = 0.75f;
    float valueWeight = 0.25f;
    float sequelWeight = 0.1f;

    Vector2 moneyBurstCooldown = new Vector2(1f, 5);
    Vector2 moneyBurstSize = new Vector2(0.1f, 0.3f);
    bool canSendMoney = true;

    [SerializeField]
    AnimationCurve amountSmoothCurve;

    public enum Genre
    {
        Eighties,
        Erotica,
        Crime,
        Gay,
        Sports,
        Horror,
        Scifi,
        Office,
        Fanfiction,
    }

    [System.Serializable]
    public class GenreWeight
    {
        public Genre genre;
        public float weight;
    }

    [SerializeField]
    List<GenreWeight> weightStats = new List<GenreWeight>();

    [SerializeField]
    Genre faveGenre;

    float profits;
    float losses;
    int booksPublished;

    public string lastPublished;

    [Header("UI")]
    [SerializeField]
    TMP_Text currentMoneyDisplay;

    [SerializeField]
    TMP_Text profitsDisplay;

    [SerializeField]
    TMP_Text lossesDisplay;

    [SerializeField]
    TMP_Text lastPublishedDisplay;

    [SerializeField]
    TMP_Text amountPublishedDisplay;

    private void Awake()
    {
        publishMan = this;
    }

    private void Start()
    {
        displayMoney = money;
    }

    public void PublishBook(int genre, int subGenre, int sequel, int bestDemo, int marketingDemo, int bookValue, string title, int index)
    {
        ChangeMoney(-bookPublishCost);

        addWeight((Genre)genre, 1);
        addWeight((Genre)subGenre, 0.5f);

        float marketingScore = (1 - (Mathf.Abs(bestDemo - marketingDemo) / 2)) * marketingWeight;

        float valueScore = ((bookValue + 1) / 5) * valueWeight;

        float finalScore = marketingScore + valueScore + (sequel * sequelWeight);
        finalScore = Mathf.Clamp(finalScore, 0.001f, 2);

        float outPutRevenue = maxBookRevenue * finalScore;

        ChangeMoney(Mathf.RoundToInt(outPutRevenue));

        booksPublished++;

        lastPublished = title;
        lastPublishedDisplay.text = title;
        amountPublishedDisplay.text = booksPublished.ToString();

        InkHandler.inkMan.NewBookPublished(index);
    }

    void ChangeMoney(int change)
    {
        if (change == 0)
            return;

        if(change < 0)
        {
            money += change;
            losses += change;
            lossesDisplay.text = "$" + Mathf.Abs(losses).ToString("F0");
        }
        else
        {
            pendingMoney += change;
        }
    }

    private void Update()
    {
        float ratio = displayMoney / money;
        displayMoney = Mathf.MoveTowards(displayMoney, money, Time.deltaTime * (1000000 * amountSmoothCurve.Evaluate(ratio)));

        if(pendingMoney > 0)
        {
            if(canSendMoney)
            {
                float transferAmount = pendingMoney * Random.Range(moneyBurstSize.x, moneyBurstSize.y);
                if (pendingMoney < transferAmount || pendingMoney <= bookPublishCost)
                {
                    transferAmount = pendingMoney;
                    pendingMoney = 0;
                }

                if(pendingMoney > 0)
                    pendingMoney -= transferAmount;

                money += transferAmount;
                profits += transferAmount;
                    
                StartCoroutine(BurstCooldown());
            }
        }

        profitsDisplay.text = "$" + profits.ToString("F0");
        currentMoneyDisplay.text = "$" + displayMoney.ToString("F0");
    }

    IEnumerator BurstCooldown()
    {
        canSendMoney = false;
        yield return new WaitForSecondsRealtime(Random.Range(moneyBurstCooldown.x, moneyBurstCooldown.y));
        canSendMoney = true;
    }

    void addWeight(Genre type, float amount)
    {
        for(int i = 0; i < weightStats.Count; i++)
        {
            if (type == weightStats[i].genre)
            {
                weightStats[i].weight += amount;
            }
        }

        CalculateStats();
    }

    void CalculateStats()
    {
        weightStats = weightStats.OrderByDescending(go => go.weight).ToList();

        faveGenre = weightStats[0].genre;
    }
}
