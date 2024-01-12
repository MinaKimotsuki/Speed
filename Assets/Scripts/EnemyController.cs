using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject card;
    [SerializeField] EnemyCardsController enemyCardsController;
    [SerializeField] PlaceController placeController;
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerController playerController;
    int[] handNumbers = new int[4];
    bool[] isHandsFull = new bool[4];
    GameObject[] instantiatedObject = new GameObject[4];
    int handNotFull = 4;
    /*bool isFinishAfterSubmit = true;*/
    /*bool isFinishPlaceSubmit = false;*/
    public bool cannotSubmit = true; //trueÅ®Ç≈Ç´ÇÈ falseÅ®Ç≈Ç´Ç»Ç¢
    public bool isCoroutinePlay = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FirstSubmitCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(PlaceSubmitCoroutine());
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
        JudgeCannotSubmit();
        gameManager.SubmitWhenStuck();
    }

    IEnumerator AfterSubmitCoroutine()
    {
        handNotFull = 4;
        for (int i = 0; i< 4; i++)
        {
            if (isHandsFull[i] == false)
            {
                handNotFull = i;
            }
        }
        yield return new WaitForSeconds(10f);
        if (handNotFull == 4)
        {
            Debug.Log("c");
            isCoroutinePlay = false;
            cannotSubmit = false;
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
        instantiatedObject[handNotFull] = Instantiate(card, new Vector3(3 - (handNotFull * 2), 3, 0), Quaternion.Euler(0, 0, 180));
        instantiatedObject[handNotFull].transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
        instantiatedObject[handNotFull].transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();
        handNumbers[handNotFull] = enemyCardsController.EnemyCards[0][0];
        isHandsFull[handNotFull] = true;
        enemyCardsController.EnemyCards.RemoveAt(0);
        /*isFinishPlaceSubmit = false;*/
        StartCoroutine(PlaceSubmitCoroutine());
        JudgeCannotSubmit();
        playerController.JudgeCannotSubmit();
        gameManager.SubmitWhenStuck();
        Debug.Log("åƒÇŒÇÍÇΩÇP");
    }

    public IEnumerator PlaceSubmitCoroutine()
    {
        Debug.Log("åƒÇŒÇÍÇΩÇQ");
        if (isHandsFull[0] == true && isHandsFull[1] == true && isHandsFull[2] == true && isHandsFull[3] == true)
        {
            isCoroutinePlay = true;
            yield return new WaitForSeconds(10f);
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
                playerController.JudgeCannotSubmit();
                Destroy(gameManager.placeSubmittedCard[0]);
                gameManager.placeSubmittedCard[0] = instantiatedObject[i];
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
                playerController.JudgeCannotSubmit();
                Destroy(gameManager.placeSubmittedCard[1]);
                gameManager.placeSubmittedCard[1] = instantiatedObject[i];
                return;
            }

        }
    }

    public void JudgeCannotSubmit()
    {
        if (isHandsFull[0] == false || isHandsFull[1] == false || isHandsFull[2] == false || isHandsFull[3] == false) return;
        //Debug.Log("a");
        if (placeController.IsPutPlace1OK(handNumbers[0]))
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[1]))
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[2]))
        {

        }
        else if (placeController.IsPutPlace1OK(handNumbers[3]))
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[0]))
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[1]))
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[2]))
        {

        }
        else if (placeController.IsPutPlace2OK(handNumbers[3]))
        {

        }
        else
        {
            cannotSubmit = false;
            Debug.Log("enemyStuck");
        }
    }
}
