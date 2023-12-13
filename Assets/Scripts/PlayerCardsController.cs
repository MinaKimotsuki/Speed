using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsController : MonoBehaviour
{
    public List<List<int>> PlayerCards { get; set; } = new List<List<int>>();
    /*public List<GameObject> CardObjects { get; set; } = new List<GameObject>();
    Card[] cards = new Card[26];*/

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayerCards.Count; i++)
        {
            Debug.Log("player" + i + ":" + PlayerCards[i][0] + "," + PlayerCards[i][1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void PutCardsNumber()
    {
        for (int i = 0; i < CardObjects.Count; i++)
        {
            cards[i] = CardObjects[i].GetComponent<Card>();
            cards[i].number = PlayerCards[i][0];
        }
    }*/
}
