using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject card;
    [SerializeField] EnemyCardsController enemyCardsController;
    [SerializeField] PlaceController placeController;
    int[] handNumbers = new int[4];
    bool[] isHandsFull = new bool[4];
    GameObject[] instantiatedObject = new GameObject[4];
    int handNotFull = 4;
    /*bool isFinishAfterSubmit = true;
    bool isFinishPlaceSubmit = false;*/

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FirstSubmitCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        /*StartCoroutine(AfterSubmitCoroutine());
        StartCoroutine(PlaceSubmitCoroutine());*/
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
    }

    IEnumerator AfterSubmitCoroutine()
    {
        for (int i = 0; i< 4; i++)
        {
            if (isHandsFull[i] == false)
            {
                handNotFull = i;
            }
        }
        /*if (!isFinishAfterSubmit)
        {*/
            if (handNotFull != 4)
            {
                /*isFinishAfterSubmit = true;*/
                yield return new WaitForSeconds(10f);
                AfterSubmit();
            }
        /*}*/
    }


    void AfterSubmit()
    {
        instantiatedObject[handNotFull] = Instantiate(card, new Vector3(3 - (handNotFull * 2), 3, 0), Quaternion.Euler(0, 0, 180));
        instantiatedObject[handNotFull].transform.GetChild(0).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][1].ToString();
        instantiatedObject[handNotFull].transform.GetChild(1).GetComponent<TextMeshPro>().text = enemyCardsController.EnemyCards[0][0].ToString();
        handNumbers[handNotFull] = enemyCardsController.EnemyCards[0][0];
        isHandsFull[handNotFull] = true;
        enemyCardsController.EnemyCards.RemoveAt(0);
        handNotFull = 4;
        /*isFinishPlaceSubmit = false;*/
        StartCoroutine(PlaceSubmitCoroutine());
    }

    public IEnumerator PlaceSubmitCoroutine()
    {
        if (isHandsFull[0] == true && isHandsFull[1] == true && isHandsFull[2] == true && isHandsFull[3] == true)
        {
            /*if (!isFinishPlaceSubmit)
            {
                isFinishPlaceSubmit = true;*/
                yield return new WaitForSeconds(10f);
                PlaceSubmit();
            /*}*/
        }
    }
    void PlaceSubmit()
    {
        for (int i = 0; i < 4; i++)
        {
            if (placeController.IsPutPlace1OK(handNumbers[i]))
            {
                instantiatedObject[i].transform.position = new Vector3(-1, 0, 0);
                isHandsFull[i] = false;
                /*isFinishAfterSubmit = false;*/
                StartCoroutine(AfterSubmitCoroutine());
                placeController.SetPlace1Before(handNumbers[i]);
                return;
            }
            else if (placeController.IsPutPlace2OK(handNumbers[i]))
            {
                instantiatedObject[i].transform.position = new Vector3(1, 0, 0);
                isHandsFull[i] = false;
                /*isFinishAfterSubmit = false;*/
                StartCoroutine(AfterSubmitCoroutine());
                placeController.SetPlace2Before(handNumbers[i]);
                return;
            }

        }
    }
}
