using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject card;
    [SerializeField] GameObject cards;
    [SerializeField] EnemyCardsController enemyCardsController;
    [SerializeField] PlaceController placeController;
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerController playerController;
    public int[] handNumbers = new int[4];
    bool[] isHandsFull = new bool[4];
    public GameObject[] instantiatedObject = new GameObject[4];
    int handNotFull = 4;
    /*bool isFinishAfterSubmit = true;*/
    /*bool isFinishPlaceSubmit = false;*/
    public bool cannotSubmit = true; //trueÅ®Ç≈Ç´ÇÈ falseÅ®Ç≈Ç´Ç»Ç¢
    public bool isCoroutinePlay = false;
    public bool[] isHandFull = new bool[4];

    [SerializeField] bool isPlace1andHand1OK;
    [SerializeField] bool isPlace2andHand1OK;
    [SerializeField] bool isPlace1andHand2OK;
    [SerializeField] bool isPlace2andHand2OK;
    [SerializeField] bool isPlace1andHand3OK;
    [SerializeField] bool isPlace2andHand3OK;
    [SerializeField] bool isPlace1andHand4OK;
    [SerializeField] bool isPlace2andHand4OK;

    public bool isCardsFinish = false;

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
        yield return new WaitForSeconds(1f);
        FirstSubmit(0);
        yield return new WaitForSeconds(1f);
        FirstSubmit(1);
        yield return new WaitForSeconds(1f);
        FirstSubmit(2);
        yield return new WaitForSeconds(1f);
        FirstSubmit(3);
        cannotSubmit = true;
        JudgeCannotSubmit();
        StartCoroutine(PlaceSubmitCoroutine());
    }

    void FirstSubmit(int i)
    {
        instantiatedObject[i] = Instantiate(card, new Vector3(3 - (i * 2), 3, 0), Quaternion.Euler(0, 0, 180));
        instantiatedObject[i].transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
        instantiatedObject[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();
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
                isHandFull[i] = false;
            }
        }
        yield return new WaitForSeconds(3);
        /*if (handNotFull == 4)
        {
            Debug.Log("c");
            isCoroutinePlay = false;
            cannotSubmit = false;
            playerController.JudgeCannotSubmit();
            gameManager.SubmitWhenStuck();
            Debug.Log("enemyStuck");
        }
        else
        {*/
            AfterSubmit();
        /*}*/
    }


    void AfterSubmit()
    {
        if (!isCardsFinish)
        {
            instantiatedObject[handNotFull] = Instantiate(card, new Vector3(3 - (handNotFull * 2), 3, 0), Quaternion.Euler(0, 0, 180));
            instantiatedObject[handNotFull].transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
            instantiatedObject[handNotFull].transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();
            handNumbers[handNotFull] = enemyCardsController.EnemyCards[0][0];
            isHandsFull[handNotFull] = true;
            enemyCardsController.EnemyCards.RemoveAt(0);
        }
        JudgeIfCardsFinish();
        /*isFinishPlaceSubmit = false;*/
        StartCoroutine(PlaceSubmitCoroutine());
        cannotSubmit = true;
        playerController.cannotSubmit = true;
        JudgeCannotSubmit();
        playerController.JudgeCannotSubmit();
        gameManager.SubmitWhenStuck();
        /*Debug.Log("åƒÇŒÇÍÇΩÇP");*/
    }

    public IEnumerator PlaceSubmitCoroutine()
    {
        /*Debug.Log("åƒÇŒÇÍÇΩÇQ");*/
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
        for (int i = 0; i < 4; i++)
        {
            if (placeController.IsPutPlace1OK(handNumbers[i]))
            {
                gameManager.SetSortingOrder(instantiatedObject[i]);
                instantiatedObject[i].transform.position = new Vector3(-1, 0, 0);
                isHandsFull[i] = false;
                StartCoroutine(AfterSubmitCoroutine());
                placeController.SetPlace1Before(handNumbers[i]);
                playerController.cannotSubmit = true;
                cannotSubmit = true;
                playerController.JudgeCannotSubmit();
                JudgeCannotSubmit();
                Destroy(gameManager.placeSubmittedCard[0]);
                gameManager.placeSubmittedCard[0] = instantiatedObject[i];
                Debug.Log(isCardsFinish);
                JudgeIfGameFinish();
                return;
            }
            else if (placeController.IsPutPlace2OK(handNumbers[i]))
            {
                gameManager.SetSortingOrder(instantiatedObject[i]);
                instantiatedObject[i].transform.position = new Vector3(1, 0, 0);
                isHandsFull[i] = false;
                StartCoroutine(AfterSubmitCoroutine());
                placeController.SetPlace2Before(handNumbers[i]);
                playerController.cannotSubmit = true;
                cannotSubmit = true;
                playerController.JudgeCannotSubmit();
                JudgeCannotSubmit();
                Destroy(gameManager.placeSubmittedCard[1]);
                gameManager.placeSubmittedCard[1] = instantiatedObject[i];
                JudgeIfGameFinish();
                return;
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
        if (placeController.IsPutPlace1OK(handNumbers[0]) && isHandFull[0])
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[1]) && isHandFull[1])
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[2]) && isHandFull[2])
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[3]) && isHandFull[3])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[0]) && isHandFull[0])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[1]) && isHandFull[1])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[2]) && isHandFull[2])
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[3]) && isHandFull[3])
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

    void JudgeIfGameFinish()
    {
        if (!isCardsFinish) return;
        if (isHandFull[0] || isHandFull[1] || isHandFull[2] || isHandFull[3]) return;
        gameManager.isGameFinish = true;
        StartCoroutine(gameManager.GameOverCoroutine());
    }

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
}
