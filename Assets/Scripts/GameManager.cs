using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

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
    int sortingOrder = 1;
    public GameObject[] placeSubmittedCard = new GameObject[2];
    public bool isGameFinish = false;
    [SerializeField] Sprite[] cardImage;
    [SerializeField] Text resultText;
    List<string> pausableCoroutines = new List<string>();
    public static string result;

    // Start is called before the first frame update
    void Awake()
    {
        SplitCards();
        SubmitFirstCard();
        resultText.enabled = false;
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
        if (isGameFinish) return;
        /*Debug.Log(playerController.cannotSubmit);
        Debug.Log(enemyController.cannotSubmit);*/
        if (playerController.cannotSubmit == true || enemyController.cannotSubmit == true) return;
        if (!playerController.isCardsFinish && !enemyController.isCardsFinish)
        {
            Submit();
        }
        else if (playerController.isCardsFinish && !enemyController.isCardsFinish)
        {
            PlayerLastSubmit();
            EnemySubmit();
        }
        else if (!playerController.isCardsFinish && enemyController.isCardsFinish)
        {
            PlayerSubmit();
            EnemyLastSubmit();
        }
        else if (playerController.isCardsFinish && enemyController.isCardsFinish)
        {
            PlayerLastSubmit();
            EnemyLastSubmit();
        }
        Debug.Log("bothStuck");
        playerController.JudgeIfPlayerCardsFinish();
        enemyController.JudgeIfCardsFinish();
        playerController.cannotSubmit = true;
        enemyController.cannotSubmit = true;
        playerController.JudgeCannotSubmit();
        enemyController.JudgeCannotSubmit();
        SubmitWhenStuck();
        if (!enemyController.isCoroutinePlay)
        {
            StartCoroutine(enemyController.PlaceSubmitCoroutine());
        }
    }

    void Submit()
    {
        GameObject firstSubmitedCard1 = Instantiate(card, playerCardsController.transform.position, Quaternion.identity);
        firstSubmitedCard1.transform.DOLocalMove(place1.transform.position, 0.1f);
        GameObject firstSubmitedCard2 = Instantiate(card, enemyCardsController.transform.position, Quaternion.Euler(0, 0, 180));
        firstSubmitedCard2.transform.DOLocalMove(place2.transform.position, 0.1f);
        /*Debug.Log(playerCardsController.PlayerCards.Count);
        Debug.Log(enemyCardsController.EnemyCards.Count);*/
        Debug.Log(playerCardsController.PlayerCards[0][1] * 13 + playerCardsController.PlayerCards[0][0]);
        firstSubmitedCard1.GetComponent<SpriteRenderer>().sprite = cardImage[playerCardsController.PlayerCards[0][1] * 13 + playerCardsController.PlayerCards[0][0] - 1];
        /*firstSubmitedCard1.transform.GetChild(0).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][1].ToString();
        firstSubmitedCard1.transform.GetChild(1).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][0].ToString();*/
        firstSubmitedCard2.GetComponent<SpriteRenderer>().sprite = cardImage[enemyCardsController.EnemyCards[0][1] * 13 + enemyCardsController.EnemyCards[0][0] - 1];
        /*firstSubmitedCard2.transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
        firstSubmitedCard2.transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();*/
        firstSubmitedCard1.GetComponent<Card>().isSubmitted = true;
        firstSubmitedCard2.GetComponent<Card>().isSubmitted = true;
        SetSortingOrder(firstSubmitedCard1);
        SetSortingOrder(firstSubmitedCard2);
        placeController.SetPlace1Before(playerCardsController.PlayerCards[0][0]);
        placeController.SetPlace2Before(enemyCardsController.EnemyCards[0][0]);
        playerCardsController.PlayerCards.RemoveAt(0);
        enemyCardsController.EnemyCards.RemoveAt(0);
    }

    void PlayerSubmit()
    {
        GameObject firstSubmitedCard1 = Instantiate(card, playerCardsController.transform.position, Quaternion.identity);
        firstSubmitedCard1.transform.DOLocalMove(place1.transform.position, 0.1f);
        /*Debug.Log(playerCardsController.PlayerCards.Count);
        Debug.Log(enemyCardsController.EnemyCards.Count);*/
        firstSubmitedCard1.GetComponent<SpriteRenderer>().sprite = cardImage[playerCardsController.PlayerCards[0][1] * 13 + playerCardsController.PlayerCards[0][0] - 1];
        /*firstSubmitedCard1.transform.GetChild(0).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][1].ToString();
        firstSubmitedCard1.transform.GetChild(1).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][0].ToString();*/
        firstSubmitedCard1.GetComponent<Card>().isSubmitted = true;
        SetSortingOrder(firstSubmitedCard1);
        playerController.cardInHand1.GetComponent<Card>().isSubmitted = true;
        placeController.SetPlace1Before(playerController.playerCardsNumber1);
        //playerController.isHand1Full = false;
        JudgeIfGameFinish();
        //placeController.SetPlace1Before(playerCardsController.PlayerCards[0][0]);
        playerCardsController.PlayerCards.RemoveAt(0);
    }

    void EnemySubmit()
    {
        GameObject firstSubmitedCard2 = Instantiate(card, enemyCardsController.transform.position, Quaternion.Euler(0, 0, 180));
        firstSubmitedCard2.transform.DOLocalMove(place2.transform.position, 0.1f);
        /*Debug.Log(playerCardsController.PlayerCards.Count);
        Debug.Log(enemyCardsController.EnemyCards.Count);*/
        firstSubmitedCard2.GetComponent<SpriteRenderer>().sprite = cardImage[enemyCardsController.EnemyCards[0][1] * 13 + enemyCardsController.EnemyCards[0][0] - 1];
        /*firstSubmitedCard2.transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
        firstSubmitedCard2.transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();*/
        firstSubmitedCard2.GetComponent<Card>().isSubmitted = true;
        SetSortingOrder(firstSubmitedCard2);
        JudgeIfGameFinish();
        placeController.SetPlace2Before(enemyCardsController.EnemyCards[0][0]);
        enemyCardsController.EnemyCards.RemoveAt(0);
    }

    void PlayerLastSubmit()
    {
        if (playerController.isHand1Full)
        {
            playerController.cardInHand1.transform.DOLocalMove(place1.transform.position, 0.1f);
            SetSortingOrder(playerController.cardInHand1);
            playerController.cardInHand1.GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace1Before(playerController.playerCardsNumber1);
            playerController.isHand1Full = false;
            JudgeIfGameFinish();
        }
        else if (playerController.isHand2Full)
        {
            playerController.cardInHand2.transform.DOLocalMove(place1.transform.position, 0.1f);
            SetSortingOrder(playerController.cardInHand2);
            playerController.cardInHand2.GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace1Before(playerController.playerCardsNumber2);
            playerController.isHand2Full = false;
            JudgeIfGameFinish();
        }
        else if (playerController.isHand3Full)
        {
            playerController.cardInHand3.transform.DOLocalMove(place1.transform.position, 0.1f);
            SetSortingOrder(playerController.cardInHand3);
            playerController.cardInHand3.GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace1Before(playerController.playerCardsNumber3);
            playerController.isHand3Full = false;
            JudgeIfGameFinish();
        }
        else if (playerController.isHand4Full)
        {
            playerController.cardInHand4.transform.DOLocalMove(place1.transform.position, 0.1f);
            SetSortingOrder(playerController.cardInHand4);
            playerController.cardInHand4.GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace1Before(playerController.playerCardsNumber4);
            playerController.isHand4Full = false;
            JudgeIfGameFinish();
        }
    }

    void EnemyLastSubmit()
    {
        if (enemyController.isHandsFull[0])
        {
            enemyController.instantiatedObject[0].transform.DOLocalMove(place2.transform.position, 0.1f);
            SetSortingOrder(enemyController.instantiatedObject[0]);
            enemyController.instantiatedObject[0].GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace2Before(enemyController.handNumbers[0]);
            enemyController.isHandsFull[0] = false;
            StopEnemyCoroutines();
            StartCoroutine(enemyController.PlaceSubmitCoroutine());
            Debug.Log("g");
            JudgeIfGameFinish();
        }
        else if (playerController.isHand2Full)
        {
            enemyController.instantiatedObject[1].transform.DOLocalMove(place2.transform.position, 0.1f);
            SetSortingOrder(enemyController.instantiatedObject[1]);
            enemyController.instantiatedObject[1].GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace2Before(enemyController.handNumbers[1]);
            enemyController.isHandsFull[1] = false;
            StopEnemyCoroutines();
            StartCoroutine(enemyController.PlaceSubmitCoroutine());
            Debug.Log("g");
            JudgeIfGameFinish();

        }
        else if (playerController.isHand3Full)
        {
            enemyController.instantiatedObject[2].transform.DOLocalMove(place2.transform.position, 0.1f);
            SetSortingOrder(enemyController.instantiatedObject[2]);
            enemyController.instantiatedObject[2].GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace2Before(enemyController.handNumbers[2]);
            enemyController.isHandsFull[2] = false;
            StopEnemyCoroutines();
            StartCoroutine(enemyController.PlaceSubmitCoroutine());
            Debug.Log("g");
            JudgeIfGameFinish();
        }
        else if (playerController.isHand4Full)
        {
            enemyController.instantiatedObject[3].transform.DOLocalMove(place2.transform.position, 0.1f);
            SetSortingOrder(enemyController.instantiatedObject[3]);
            enemyController.instantiatedObject[3].GetComponent<Card>().isSubmitted = true;
            placeController.SetPlace2Before(enemyController.handNumbers[3]);
            enemyController.isHandsFull[3] = false;
            StopEnemyCoroutines();
            StartCoroutine(enemyController.PlaceSubmitCoroutine());
            Debug.Log("g");
            JudgeIfGameFinish();
        }
    }

    public void JudgeIfGameFinish()
    {
        bool playerFinish = false;
        bool enemyFinish = false;
        if (playerController.isCardsFinish && !playerController.isHand1Full && !playerController.isHand2Full && !playerController.isHand3Full && !playerController.isHand4Full)
        {
            playerFinish = true;
        }
        if (enemyController.isCardsFinish && !enemyController.isHandsFull[0] && !enemyController.isHandsFull[1] && !enemyController.isHandsFull[2] && !enemyController.isHandsFull[3])
        {
            enemyFinish = true;
        }

        if (playerFinish && enemyFinish)
        {
            resultText.enabled = true;
            resultText.text = "DRAW";
            result = "DRAW";
        }
        else if (playerFinish)
        {
            resultText.enabled = true;
            resultText.text = "WIN";
            result = "WIN";
        }
        else if (enemyFinish)
        {
            resultText.enabled = true;
            resultText.text = "LOSE";
            result = "LOSE";
        }
        else
        {
            return;
        }
        isGameFinish = true;
        StartCoroutine(GameOverCoroutine());
    }

    public void SetSortingOrder(GameObject setObject)
    {
        Debug.Log(setObject);
        if (setObject == null) return;
        setObject.GetComponent<SortingGroup>().sortingOrder = sortingOrder;
        sortingOrder++;
    }

    public IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        SceneManager.LoadScene("Result");
    }

    void StopEnemyCoroutines()
    {
        enemyController.StopEnemyCoroutines();
    }
}
