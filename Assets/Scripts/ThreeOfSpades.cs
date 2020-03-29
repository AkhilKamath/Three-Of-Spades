using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThreeOfSpades : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject dummyCardPrefab;

    public GameObject topHand;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject bottomHand;

    public static string[] suites = {"S", "D", "C", "H"};
    public static string[] values = {"A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2"};
    public static string trump = "H";
    public static string current = "H";
    //public static bool nextChance = false;

    //multiplayer attr
    private static List<string> handpl1 = new List<string>();
    private static List<string> handpl2 = new List<string>();
    private static List<string> handpl3 = new List<string>();
    private static List<string> handpl4 = new List<string>();

    public static List<string> deck;

    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<100; i++)
        {
            if(i==99)
                print(current);
        }
    }

    public void PlayCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);
        foreach (string d in deck)
        {
            //print(d);
        }
        DistriubteCards();
    }

    void Shuffle<T>(List<T> list) 
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suites)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }

    void DistriubteCards()
    {
        int n = deck.Count/4;
        for(int i=0; i<n; i++)
        {
            handpl1.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            handpl2.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            handpl3.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
            handpl4.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
        }
        SortHand(handpl1);
        SortHand(handpl2);
        SortHand(handpl3);
        SortHand(handpl4);
        StartCoroutine(DealHands());
    }

    void SortHand(List<string> hand)
    {
        List<string> refDeck = GenerateDeck();
        for(int i=0; i<hand.Count; i++)
        {
            for(int j=0; j<hand.Count-i-1; j++)
            {
                if (refDeck.IndexOf(hand[j]) > refDeck.IndexOf(hand[j + 1]))
                {
                    string temp = hand[j];
                    hand[j] = hand[j + 1];
                    hand[j + 1] = temp;
                }
            }
        }
    }

    IEnumerator DealHands()
    {
        //dealing current player hand
        float xOffset = 0;
        float yOffset = 0;
        float zOffset = 0;
        foreach (string card in handpl1)
        {
            yield return new WaitForSeconds(0.01f);
            GameObject newCard = Instantiate(
                cardPrefab,
                new Vector3(
                    bottomHand.transform.position.x + xOffset,
                    bottomHand.transform.position.y,
                    bottomHand.transform.position.z - zOffset),
                Quaternion.identity,
                bottomHand.transform);
            newCard.name = card;
            xOffset = xOffset + 0.5f;
            zOffset = zOffset + 0.03f;
        }

        //dealing other players hands - only UI
        //top player
        xOffset = 0;
        zOffset = 0;
        for (int i = 0; i < handpl2.Count; i++)
        {
            yield return new WaitForSeconds(0.01f);
            GameObject newCard = Instantiate(
                dummyCardPrefab,
                new Vector3(
                    topHand.transform.position.x + xOffset,
                    topHand.transform.position.y,
                    topHand.transform.position.z - zOffset),
                Quaternion.identity,
                topHand.transform);
            newCard.transform.rotation = topHand.transform.rotation;
            xOffset = xOffset + 0.5f;
            zOffset = zOffset + 0.03f;
        }
        //left player
        yOffset = 0;
        zOffset = 0;
        for (int i = 0; i < handpl2.Count; i++)
        {
            yield return new WaitForSeconds(0.01f);
            GameObject newCard = Instantiate(
                dummyCardPrefab,
                new Vector3(
                    leftHand.transform.position.x,
                    leftHand.transform.position.y - yOffset,
                    leftHand.transform.position.z - zOffset),
                Quaternion.identity,
                leftHand.transform);
            newCard.transform.rotation = leftHand.transform.rotation;
            yOffset = yOffset + 0.5f;
            zOffset = zOffset + 0.03f;
        }
        //right player
        yOffset = 0;
        zOffset = 0;
        for (int i = 0; i < handpl2.Count; i++)
        {
            yield return new WaitForSeconds(0.01f);
            GameObject newCard = Instantiate(
                dummyCardPrefab,
                new Vector3(
                    rightHand.transform.position.x,
                    rightHand.transform.position.y - yOffset,
                    rightHand.transform.position.z - zOffset),
                Quaternion.identity,
                rightHand.transform);
            newCard.transform.rotation = rightHand.transform.rotation;
            yOffset = yOffset + 0.5f;
            zOffset = zOffset + 0.03f;
        }
    }
}
