using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject card;
    [SerializeField] GameObject cards;
    [SerializeField] EnemyCardsController enemyCardsController;
    [SerializeField] PlaceController placeController;
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerController playerController;
    public int[] handNumbers = new int[4];
    public bool[] isHandsFull = new bool[4];
    public GameObject[] instantiatedObject = new GameObject[4];
    int handNotFull = 4;
    /*bool isFinishAfterSubmit = true;*/
    /*bool isFinishPlaceSubmit = false;*/
    public bool cannotSubmit = true; //trueÅ®Ç≈Ç´ÇÈ falseÅ®Ç≈Ç´Ç»Ç¢
    public bool isCoroutinePlay = false;
    //public bool[] isHandFull = new bool[4];

    [SerializeField] bool isPlace1andHand1OK;
    [SerializeField] bool isPlace2andHand1OK;
    [SerializeField] bool isPlace1andHand2OK;
    [SerializeField] bool isPlace2andHand2OK;
    [SerializeField] bool isPlace1andHand3OK;
    [SerializeField] bool isPlace2andHand3OK;
    [SerializeField] bool isPlace1andHand4OK;
    [SerializeField] bool isPlace2andHand4OK;

    public bool isCardsFinish = false;
    [SerializeField] Sprite[] cardImage;
    public Coroutine afterSubmitCoroutine;
    public Coroutine placeSubmitCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FirstSubmitCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(PlaceSubmitCoroutine());
        UpdateIsPutPlaceOK();
    }

    IEnumerator FirstSubmitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        FirstSubmit(0);
        yield return new WaitForSeconds(1.5f);
        FirstSubmit(1);
        yield return new WaitForSeconds(1.5f);
        FirstSubmit(2);
        yield return new WaitForSeconds(1.5f);
        FirstSubmit(3);
        cannotSubmit = true;
        JudgeCannotSubmit();
        gameManager.SubmitWhenStuck();
        placeSubmitCoroutine = StartCoroutine(PlaceSubmitCoroutine());
    }

    void FirstSubmit(int i)
    {
        instantiatedObject[i] = Instantiate(card, new Vector3(6, 3, 0), Quaternion.Euler(0, 0, 180));
        instantiatedObject[i].transform.DOLocalMove(new Vector3(3 - (i * 2), 3, 0), 0.1f);
        Debug.Log(enemyCardsController.EnemyCards[0][1] * 13 + enemyCardsController.EnemyCards[0][0]);
        instantiatedObject[i].GetComponent<SpriteRenderer>().sprite = cardImage[enemyCardsController.EnemyCards[0][1] * 13 + enemyCardsController.EnemyCards[0][0] - 1];
        /*instantiatedObject[i].transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
        instantiatedObject[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();*/
        handNumbers[i] = enemyCardsController.EnemyCards[0][0];
        isHandsFull[i] = true;
        enemyCardsController.EnemyCards.RemoveAt(0);
        cannotSubmit = true;
        JudgeCannotSubmit();
        gameManager.SubmitWhenStuck();
    }

    IEnumerator AfterSubmitCoroutine()
    {
        handNotFull = 4;
        for (int i = 0; i < 4; i++)
        {
            if (isHandsFull[i] == false)
            {
                handNotFull = i;
                //isHandFull[i] = false;
            }
        }
        //Debug.Log(handNotFull);
        yield return new WaitForSeconds(1);
        if (handNotFull == 4)
        {
            Debug.Log("c");
            isCoroutinePlay = false;
            playerController.cannotSubmit = true;
            playerController.JudgeCannotSubmit();
            gameManager.SubmitWhenStuck();
            Debug.Log("enemyStuck");
        }
        else
        {
            AfterSubmit();
        }
    }


    void AfterSubmit()
    {
        Debug.Log("AfterSubmit");
        if (afterSubmitCoroutine != null)
        {
            StopCoroutine(afterSubmitCoroutine);
        }
        //afterSubmitCoroutine = StartCoroutine(AfterSubmitCoroutine());
        if (!isCardsFinish)
        {
            instantiatedObject[handNotFull] = Instantiate(card, new Vector3(5, 3, 0), Quaternion.Euler(0, 0, 180));
            Debug.Log(handNotFull);
            instantiatedObject[handNotFull].transform.DOLocalMove(new Vector3(3 - (handNotFull * 2), 3, 0), 0.1f);
            instantiatedObject[handNotFull].GetComponent<SpriteRenderer>().sprite = cardImage[enemyCardsController.EnemyCards[0][1] * 13 + enemyCardsController.EnemyCards[0][0] - 1];
            /*instantiatedObject[handNotFull].transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
            instantiatedObject[handNotFull].transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();*/
            handNumbers[handNotFull] = enemyCardsController.EnemyCards[0][0];
            isHandsFull[handNotFull] = true;
            enemyCardsController.EnemyCards.RemoveAt(0);
        }
        JudgeIfCardsFinish();
        /*isFinishPlaceSubmit = false;*/
        placeSubmitCoroutine = StartCoroutine(PlaceSubmitCoroutine());
        cannotSubmit = true;
        playerController.cannotSubmit = true;
        JudgeCannotSubmit();
        playerController.JudgeCannotSubmit();
        gameManager.SubmitWhenStuck();
        /*Debug.Log("åƒÇŒÇÍÇΩÇP");*/
    }

    public IEnumerator PlaceSubmitCoroutine()
    {
        Debug.Log("PlaceSubmitCoroutine");
        
        /*Debug.Log("åƒÇŒÇÍÇΩÇQ");*/
        /*Debug.Log(isHandsFull[0]);
        Debug.Log(isHandsFull[1]);
        Debug.Log(isHandsFull[2]);
        Debug.Log(isHandsFull[3]);*/
        if (isHandsFull[0] == true && isHandsFull[1] == true && isHandsFull[2] == true && isHandsFull[3] == true)
        {
            isCoroutinePlay = true;
            yield return new WaitForSeconds(3);
            PlaceSubmit();
        }
        else if (isCardsFinish)
        {
            isCoroutinePlay = true;
            yield return new WaitForSeconds(3);
            PlaceSubmit();
        }
        else
        {
            isCoroutinePlay = false;
        }
    }
    void PlaceSubmit()
    {
        if (placeSubmitCoroutine != null)
        {
            StopCoroutine(placeSubmitCoroutine);
        }
        for (int i = 0; i < 4; i++)
        {
            if (isHandsFull[i])
            {
                if (placeController.IsPutPlace1OK(handNumbers[i]))
                {
                    gameManager.SetSortingOrder(instantiatedObject[i]);
                    instantiatedObject[i].transform.DOLocalMove(new Vector3(-1, 0, 0), 0.1f);
                    isHandsFull[i] = false;
                    afterSubmitCoroutine = StartCoroutine(AfterSubmitCoroutine());
                    placeController.SetPlace1Before(handNumbers[i]);
                    playerController.cannotSubmit = true;
                    cannotSubmit = true;
                    playerController.JudgeCannotSubmit();
                    JudgeCannotSubmit();
                    gameManager.SubmitWhenStuck();
                    //Destroy(gameManager.placeSubmittedCard[0]);
                    gameManager.placeSubmittedCard[0] = instantiatedObject[i];
                    Debug.Log(isCardsFinish);
                    gameManager.JudgeIfGameFinish();
                    return;
                }
                else if (placeController.IsPutPlace2OK(handNumbers[i]))
                {
                    gameManager.SetSortingOrder(instantiatedObject[i]);
                    instantiatedObject[i].transform.DOLocalMove(new Vector3(1, 0, 0), 0.1f);
                    isHandsFull[i] = false;
                    afterSubmitCoroutine = StartCoroutine(AfterSubmitCoroutine());
                    placeController.SetPlace2Before(handNumbers[i]);
                    playerController.cannotSubmit = true;
                    cannotSubmit = true;
                    playerController.JudgeCannotSubmit();
                    JudgeCannotSubmit();
                    gameManager.SubmitWhenStuck();
                    //Destroy(gameManager.placeSubmittedCard[1]);
                    gameManager.placeSubmittedCard[1] = instantiatedObject[i];
                    gameManager.JudgeIfGameFinish();
                    return;
                }
            }
            isCoroutinePlay = false;
        }
    }

    public void JudgeCannotSubmit()
    {
        if (!isCardsFinish)
        {
            if (isHandsFull[0] == false || isHandsFull[1] == false || isHandsFull[2] == false || isHandsFull[3] == false) return;
        }
        if (placeController.IsPutPlace1OK(handNumbers[0]) && isHandsFull[0])
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[1]) && isHandsFull[1])
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[2]) && isHandsFull[2])
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[3]) && isHandsFull[3])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[0]) && isHandsFull[0])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[1]) && isHandsFull[1])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[2]) && isHandsFull[2])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[3]) && isHandsFull[3])
        {

        }
        else
        {
            cannotSubmit = false;
            Debug.Log("enemyStuck");
        }
    }

    public void JudgeIfCardsFinish()
    {
        if (enemyCardsController.EnemyCards.Count != 0) return;
        isCardsFinish = true;
        cards.SetActive(false);
    }

    /*public void JudgeIfGameFinish()
    {
        if (!isCardsFinish) return;
        if (isHandsFull[0] || isHandsFull[1] || isHandsFull[2] || isHandsFull[3]) return;
        gameManager.isGameFinish = true;
        StartCoroutine(gameManager.GameOverCoroutine());
    }*/

    private void UpdateIsPutPlaceOK()
    {
        isPlace1andHand1OK = placeController.IsPutPlace1OK(handNumbers[0]);
        isPlace2andHand1OK = placeController.IsPutPlace2OK(handNumbers[0]);
        isPlace1andHand2OK = placeController.IsPutPlace1OK(handNumbers[1]);
        isPlace2andHand2OK = placeController.IsPutPlace2OK(handNumbers[1]);
        isPlace1andHand3OK = placeController.IsPutPlace1OK(handNumbers[2]);
        isPlace2andHand3OK = placeController.IsPutPlace2OK(handNumbers[2]);
        isPlace1andHand4OK = placeController.IsPutPlace1OK(handNumbers[3]);
        isPlace2andHand4OK = placeController.IsPutPlace2OK(handNumbers[3]);
    }

    public void StopEnemyCoroutines()
    {
        StopCoroutine(afterSubmitCoroutine);
        StopCoroutine(placeSubmitCoroutine);
    }
}
