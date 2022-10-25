using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PublishingManager : MonoBehaviour
{
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

    [SerializeField]
    AnimationCurve transferCurve;

    float maxTransferSpeed = 75000;

    [SerializeField]
    float transferSpeed = 0;

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

    private void Start()
    {
        PublishBook(2, 3, 1, 2, 2, 4);
    }

    public void PublishBook(int genre, int subGenre, int sequel, int bestDemo, int marketingDemo, int bookValue)
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
    }

    void ChangeMoney(int change)
    {
        if (change == 0)
            return;

        if(change < 0)
        {
            money += change;
        }
        else
        {
            pendingMoney += change;
        }
    }

    private void Update()
    {
        displayMoney = Mathf.MoveTowards(displayMoney, money, Time.deltaTime * 1000);

        
        if(pendingMoney > 0)
        {
            float transfer = transferSpeed * Time.deltaTime;
            if (pendingMoney >= transfer)
            {
                pendingMoney -= transfer;
                money += transfer;
            }
            else
            {
                pendingMoney = 0;
                money += pendingMoney;
            }

            transferSpeed = transferCurve.Evaluate(pendingMoney / maxBookRevenue) * maxTransferSpeed;
        }
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
