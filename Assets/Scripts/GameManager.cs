using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    List<List<int>> firstCards = new List<List<int>>();
    List<List<int>> shuffleCards = new List<List<int>>();
    [SerializeField] PlayerCardsController playerCardsController;
    [SerializeField] EnemyCardsController enemyCardsController;
    [SerializeField] PlayerController playerController;
    [SerializeField] EnemyController enemyController;
    [SerializeField] GameObject card;
    [SerializeField] GameObject place1;
    [SerializeField] GameObject place2;
    [SerializeField] PlaceController placeController;

    // Start is called before the first frame update
    void Awake()
    {
        SplitCards();
        SubmitFirstCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SplitCards()
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

    void SubmitFirstCard()
    {
        Submit();
        /*firstSubmitedCard1.GetComponent<MeshRenderer>().sortingOrder = -1;
        firstSubmitedCard2.GetComponent<MeshRenderer>().sortingOrder = -1;*/
        /*firstSubmitedCard1.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 0;
        firstSubmitedCard1.transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = 0;
        firstSubmitedCard2.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 0;
        firstSubmitedCard2.transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = 0;*/
    }

    public void SubmitWhenStuck()
    {
        Debug.Log(playerController.cannotSubmit);
        Debug.Log(enemyController.cannotSubmit);
        if (playerController.cannotSubmit == true || enemyController.cannotSubmit == true) return;
        Submit();
        Debug.Log("bothStuck");
        playerController.cannotSubmit = true;
        enemyController.cannotSubmit = true;
    }

    void Submit()
    {
        GameObject firstSubmitedCard1 = Instantiate(card, place1.transform.position, Quaternion.identity);
        GameObject firstSubmitedCard2 = Instantiate(card, place2.transform.position, Quaternion.Euler(0, 0, 180));
        firstSubmitedCard1.transform.GetChild(0).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][1].ToString();
        firstSubmitedCard1.transform.GetChild(1).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][0].ToString();
        firstSubmitedCard2.transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
        firstSubmitedCard2.transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();
        /*Debug.Log(playerCardsController.PlayerCards[0][0]);*/
        placeController.SetPlace1Before(playerCardsController.PlayerCards[0][0]);
        placeController.SetPlace2Before(enemyCardsController.EnemyCards[0][0]);
        playerCardsController.PlayerCards.RemoveAt(0);
        enemyCardsController.EnemyCards.RemoveAt(0);
    }
}
