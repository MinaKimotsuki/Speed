using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    GameObject clickedGameObject;
    GameObject clickedGameObjectInHand;
    bool isCardMovingToHand = false;
    bool isCardMovingToPlace = false;
    Vector3 screenPoint;
    int orderInRayer = 1;
    GameObject movingCard;
    GameObject nextObjectToHand;
    [SerializeField] PlayerCardsController playerCardsController;
    [SerializeField] PlaceController placeController;
    [SerializeField] EnemyController enemyController;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject card;
    [SerializeField] GameObject cards;
    [SerializeField] GameObject hand1;
    [SerializeField] GameObject hand2;
    [SerializeField] GameObject hand3;
    [SerializeField] GameObject hand4;
    public bool isHand1Full = false;
    public bool isHand2Full = false;
    public bool isHand3Full = false;
    public bool isHand4Full = false;
    public GameObject cardInHand1;
    public GameObject cardInHand2;
    public GameObject cardInHand3;
    public GameObject cardInHand4;
    int hand1Number;
    int hand2Number;
    int hand3Number;
    int hand4Number;
    public int playerCardsNumber1;
    public int playerCardsNumber2;
    public int playerCardsNumber3;
    public int playerCardsNumber4;
    public bool cannotSubmit = true; //trueÅ®Ç≈Ç´ÇÈ falseÅ®Ç≈Ç´Ç»Ç¢
    public bool isCoroutinePlay = true;
    GameObject[] handObjects = new GameObject[4];

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetMouseButtonDown();
        GetMouseButtonUp();
        GetMouseButtonUpInPlace();
        CardTransformWhileMovingToHand();
        CardTransformWhileMovingToPlace();
        UpdateIsPutPlaceOK();
        //gameManager.JudgeIfGameFinish();
    }

    void GetMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedGameObject = null;
            clickedGameObjectInHand = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (!hit2d) return;
            if (hit2d.collider.gameObject.name == "PlayerCards")
            {
                if (isHand1Full == false || isHand2Full == false || isHand3Full == false || isHand4Full == false)
                {
                    clickedGameObject = Instantiate(card, cards.transform.position, Quaternion.identity);
                    gameManager.SetSortingOrder(clickedGameObject);
                    Debug.Log(playerCardsController.PlayerCards[0][1] * 13 + playerCardsController.PlayerCards[0][0]);
                    clickedGameObject.GetComponent<SpriteRenderer>().sprite = cardImage[playerCardsController.PlayerCards[0][1] * 13 + playerCardsController.PlayerCards[0][0] - 1];
                    /*clickedGameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][1].ToString();
                    clickedGameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][0].ToString();*/
                    isCardMovingToHand = true;
                    JudgeIfCardsFinish();
                }
            }
            if (hit2d.collider.gameObject.name == "Hand1")
            {
                if (cardInHand1 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand1;
                movingCard = cardInHand1;
                hand1Number = playerCardsNumber1;
                gameManager.SetSortingOrder(cardInHand1);
            }
            if (hit2d.collider.gameObject.name == "Hand2")
            {
                if (cardInHand2 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand2;
                movingCard = cardInHand2;
                hand2Number = playerCardsNumber2;
                gameManager.SetSortingOrder(cardInHand2);
            }
            if (hit2d.collider.gameObject.name == "Hand3")
            {
                if (cardInHand3 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand3;
                movingCard = cardInHand3;
                hand3Number = playerCardsNumber3;
                gameManager.SetSortingOrder(cardInHand3);
            }
            if (hit2d.collider.gameObject.name == "Hand4")
            {
                if (cardInHand4 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand4;
                movingCard = cardInHand4;
                hand4Number = playerCardsNumber4;
                gameManager.SetSortingOrder(cardInHand4);
            }
        }
    }

    void GetMouseButtonUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (clickedGameObject == null) return;
            GameObject pastClickedGameObject = clickedGameObject;
            clickedGameObject = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (!hit2d)
            {
                StartCoroutine(GetMouseButtonUpWithNothing(pastClickedGameObject));
            }
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                if (clickedGameObject.name == "Hand1" && isHand1Full == false)
                {
                    playerCardsNumber1 = playerCardsController.PlayerCards[0][0];
                    handObjects[0] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand1Full = true;
                    cardInHand1 = pastClickedGameObject;
                    cannotSubmit = true;
                    enemyController.cannotSubmit = true; 
                    JudgeCannotSubmit();
                    enemyController.JudgeCannotSubmit();
                    gameManager.SubmitWhenStuck();
                }
                else if (clickedGameObject.name == "Hand2" && isHand2Full == false)
                {
                    playerCardsNumber2 = playerCardsController.PlayerCards[0][0];
                    handObjects[1] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand2Full = true;
                    cardInHand2 = pastClickedGameObject;
                    cannotSubmit = true;
                    enemyController.cannotSubmit = true;
                    JudgeCannotSubmit();
                    enemyController.JudgeCannotSubmit();
                    gameManager.SubmitWhenStuck();
                }
                else if (clickedGameObject.name == "Hand3" && isHand3Full == false)
                {
                    playerCardsNumber3 = playerCardsController.PlayerCards[0][0];
                    handObjects[2] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand3Full = true;
                    cardInHand3 = pastClickedGameObject;
                    cannotSubmit = true;
                    enemyController.cannotSubmit = true;
                    JudgeCannotSubmit();
                    enemyController.JudgeCannotSubmit();
                    gameManager.SubmitWhenStuck();
                }
                else if (clickedGameObject.name == "Hand4" && isHand4Full == false)
                {
                    playerCardsNumber4 = playerCardsController.PlayerCards[0][0];
                    handObjects[3] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand4Full = true;
                    cardInHand4 = pastClickedGameObject;
                    cannotSubmit = true;
                    enemyController.cannotSubmit = true;
                    JudgeCannotSubmit();
                    enemyController.JudgeCannotSubmit();
                    gameManager.SubmitWhenStuck();
                }
                else
                {
                    StartCoroutine(GetMouseButtonUpWithNothing(pastClickedGameObject));
                }
            }
        }
    }

    void GetMouseButtonUpInPlace()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (clickedGameObjectInHand == null) return;
            GameObject pastClickedGameObjectInHand = clickedGameObjectInHand;
            clickedGameObjectInHand = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit2d)
            {
                clickedGameObjectInHand = hit2d.collider.gameObject;
                if (clickedGameObjectInHand.CompareTag("Hand") || clickedGameObjectInHand.CompareTag("Cards"))
                {
                    GetMouseButtonUpInPlaceWithNothing(pastClickedGameObjectInHand);
                    return;
                }
                if (pastClickedGameObjectInHand.name == "Hand1")
                {
                    if (clickedGameObjectInHand.name == "Place1")
                    {
                        if (!placeController.IsPutPlace1OK(hand1Number))
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            return;
                        }
                        placeController.SetPlace1Before(hand1Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand1Number))    
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            return;
                        }
                        placeController.SetPlace2Before(hand1Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    cardInHand1.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand1Full = false;
                    cardInHand1 = null;
                }
                if (pastClickedGameObjectInHand.name == "Hand2")
                {
                    if (clickedGameObjectInHand.name == "Place1")
                    {
                        if (!placeController.IsPutPlace1OK(hand2Number))
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            return;
                        }
                        placeController.SetPlace1Before(hand2Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand2Number))
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            return;
                        }
                        placeController.SetPlace2Before(hand2Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    cardInHand2.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand2Full = false;
                    cardInHand2 = null;
                }
                if (pastClickedGameObjectInHand.name == "Hand3")
                {
                    if (clickedGameObjectInHand.name == "Place1")
                    {
                        if (!placeController.IsPutPlace1OK(hand3Number))
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            return;
                        }
                        placeController.SetPlace1Before(hand3Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand3Number))
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            return;
                        }
                        placeController.SetPlace2Before(hand3Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    cardInHand3.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand3Full = false;
                    cardInHand3 = null;
                }
                if (pastClickedGameObjectInHand.name == "Hand4")
                {
                    if (clickedGameObjectInHand.name == "Place1")
                    {
                        if (!placeController.IsPutPlace1OK(hand4Number))
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            return;
                        }
                        placeController.SetPlace1Before(hand4Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand4Number))
                        {
                            movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            return;
                        }
                        placeController.SetPlace2Before(hand4Number);
                        gameManager.SetSortingOrder(movingCard);
                        //Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        cannotSubmit = true;
                        enemyController.cannotSubmit = true;
                        JudgeCannotSubmit();
                        enemyController.JudgeCannotSubmit();
                        if (isCardsFinish)
                        {
                            gameManager.SubmitWhenStuck();
                        }
                        gameManager.JudgeIfGameFinish();
                        Debug.Log(enemyController.isCoroutinePlay);
                        if (!enemyController.isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                            Debug.Log("c");
                        }
                    }
                    cardInHand4.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand4Full = false;
                    cardInHand4 = null;
                }
            }
            if (!hit2d)
            {
                GetMouseButtonUpInPlaceWithNothing(pastClickedGameObjectInHand);
            }
        }
    }

    void CardTransformWhileMovingToHand()
    {
        if (!isCardMovingToHand) return;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        float screenX = Input.mousePosition.x;
        float screenY = Input.mousePosition.y;
        float screenZ = screenPoint.z;

        Vector3 currentScreenPoint = new Vector3(screenX, screenY, screenZ);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);
        clickedGameObject.transform.position = currentPosition;
    }

    void CardTransformWhileMovingToPlace()
    {
        if (!isCardMovingToPlace) return;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        float screenX = Input.mousePosition.x;
        float screenY = Input.mousePosition.y;
        float screenZ = screenPoint.z;

        Vector3 currentScreenPoint = new Vector3(screenX, screenY, screenZ);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);
        nextObjectToHand.transform.position = currentPosition;
    }

    void PlaceCardToHand(GameObject pastClickedGameObject)
    {
        playerCardsController.PlayerCards.RemoveAt(0);
        isCardMovingToHand = false;
        pastClickedGameObject.transform.position = clickedGameObject.transform.position;
    }

    public void JudgeIfCardsFinish()
    {
        Debug.Log(playerCardsController.PlayerCards.Count);
        if (playerCardsController.PlayerCards.Count == 1)
        {
            Debug.Log("f");
            cards.SetActive(false);
            isCardsFinish = true;
        }
    }

    public void JudgeIfPlayerCardsFinish()
    {
        Debug.Log(playerCardsController.PlayerCards.Count);
        if (playerCardsController.PlayerCards.Count != 0) return;
        Debug.Log("f");
        cards.SetActive(false);
        isCardsFinish = true;
    }

    /*public void JudgeIfGameFinish()
    {
        if (!isCardsFinish) return;
        if (isHand1Full || isHand2Full || isHand3Full || isHand4Full) return;
        gameManager.isGameFinish = true;
        StartCoroutine(gameManager.GameOverCoroutine());
    }*/

    IEnumerator GetMouseButtonUpWithNothing(GameObject pastClickedGameObject)
    {
        pastClickedGameObject.transform.DOLocalMove(cards.transform.position, 0.1f);
        isCardMovingToHand = false;

        yield return new WaitForSeconds(0.1f);

        Destroy(pastClickedGameObject);
        if (isCardsFinish)
        {
            cards.SetActive(true);
            isCardsFinish = false;
        }
    }

    void GetMouseButtonUpInPlaceWithNothing(GameObject pastClickedGameObjectInHand)
    {
        movingCard.transform.DOLocalMove(pastClickedGameObjectInHand.transform.position, 0.1f);
        isCardMovingToPlace = false;

    }

    public void JudgeCannotSubmit()
    {
        if (!isCardsFinish)
        {
            if (isHand1Full == false || isHand2Full == false || isHand3Full == false || isHand4Full == false) return;
        }
        if (placeController.IsPutPlace1OK(playerCardsNumber1) && isHand1Full)
        {
            Debug.Log("1");
        }
        else if (placeController.IsPutPlace1OK(playerCardsNumber2) && isHand2Full)
        {
            Debug.Log("1");
        }
        else if (placeController.IsPutPlace1OK(playerCardsNumber3) && isHand3Full)
        {
            Debug.Log("1");
        }
        else if (placeController.IsPutPlace1OK(playerCardsNumber4) && isHand4Full)
        {
            Debug.Log("1");
        }
        else if (placeController.IsPutPlace2OK(playerCardsNumber1) && isHand1Full)
        {
            Debug.Log("1");
        }
        else if (placeController.IsPutPlace2OK(playerCardsNumber2) && isHand2Full)
        {
            Debug.Log("1");
        }
        else if (placeController.IsPutPlace2OK(playerCardsNumber3) && isHand3Full)
        {
            Debug.Log("1");
        }
        else if (placeController.IsPutPlace2OK(playerCardsNumber4) && isHand4Full)
        {
            Debug.Log("1");
        }
        else
        {
            cannotSubmit = false;
            Debug.Log("playerStuck");
        }
    }

    private void UpdateIsPutPlaceOK()
    {
        isPlace1andHand1OK = placeController.IsPutPlace1OK(playerCardsNumber1);
        isPlace2andHand1OK = placeController.IsPutPlace2OK(playerCardsNumber1);
        isPlace1andHand2OK = placeController.IsPutPlace1OK(playerCardsNumber2);
        isPlace2andHand2OK = placeController.IsPutPlace2OK(playerCardsNumber2);
        isPlace1andHand3OK = placeController.IsPutPlace1OK(playerCardsNumber3);
        isPlace2andHand3OK = placeController.IsPutPlace2OK(playerCardsNumber3);
        isPlace1andHand4OK = placeController.IsPutPlace1OK(playerCardsNumber4);
        isPlace2andHand4OK = placeController.IsPutPlace2OK(playerCardsNumber4);
    }

    //if (HandManager.CheckAndPutCurrentHand())
    //{

    //}
    //else
    //{

    //}

}
