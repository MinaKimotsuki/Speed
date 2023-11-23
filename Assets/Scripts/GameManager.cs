using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<List<int>> firstCards = new List<List<int>>();
    List<List<int>> shuffleCards = new List<List<int>>();
    [SerializeField] PlayerCardsController playerCardsController;
    [SerializeField] EnemyCardsController enemyCardsController;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 1; i < 14; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                List<int> items = new List<int>();
                items.Add(i);
                items.Add(j);
                firstCards.Add(items);
            }
        }
        for (int i = 0; i < 52; i++)
        {
            int r = Random.Range(0, firstCards.Count);
            shuffleCards.Add(firstCards[r]);
            firstCards.RemoveAt(r);
        }
        for (int i = 0; i < 26; i++)
        {
            playerCardsController.PlayerCards.Add(shuffleCards[0]);
            shuffleCards.RemoveAt(0);
        }
        for (int i = 0; i < 26; i++)
        {
            enemyCardsController.EnemyCards.Add(shuffleCards[0]);
            shuffleCards.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
