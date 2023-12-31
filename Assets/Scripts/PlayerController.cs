using TMPro;
using UnityEngine;

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
    bool isHand1Full = false;
    bool isHand2Full = false;
    bool isHand3Full = false;
    bool isHand4Full = false;
    GameObject cardInHand1;
    GameObject cardInHand2;
    GameObject cardInHand3;
    GameObject cardInHand4;
    int hand1Number;
    int hand2Number;
    int hand3Number;
    int hand4Number;
    int playerCardsNumber1;
    int playerCardsNumber2;
    int playerCardsNumber3;
    int playerCardsNumber4;
    public bool cannotSubmit = true; //trueÅ®Ç≈Ç´ÇÈ falseÅ®Ç≈Ç´Ç»Ç¢
    public bool isCoroutinePlay = true;
    GameObject[] handObjects = new GameObject[4];


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
                    /*playerCardsController.CardObjects.Add(clickedGameObject);
                    playerCardsController.PutCardsNumber();*/
                    /*clickedGameObject.GetComponent<SpriteRenderer>().sortingOrder = orderInRayer;
                    clickedGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = orderInRayer;
                    clickedGameObject.transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = orderInRayer;
                    orderInRayer++;*/
                    clickedGameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][1].ToString();
                    clickedGameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = playerCardsController.PlayerCards[0][0].ToString();
                    isCardMovingToHand = true;
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
            }
            if (hit2d.collider.gameObject.name == "Hand2")
            {
                if (cardInHand2 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand2;
                movingCard = cardInHand2;
                hand2Number = playerCardsNumber2;
            }
            if (hit2d.collider.gameObject.name == "Hand3")
            {
                if (cardInHand3 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand3;
                movingCard = cardInHand3;
                hand3Number = playerCardsNumber3;
            }
            if (hit2d.collider.gameObject.name == "Hand4")
            {
                if (cardInHand4 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand4;
                movingCard = cardInHand4;
                hand4Number = playerCardsNumber4;
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
                Destroy(pastClickedGameObject);
                isCardMovingToHand = false;
            }
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                Debug.Log(clickedGameObject.name);
                if (clickedGameObject.name == "Hand1" && isHand1Full == false)
                {
                    playerCardsNumber1 = playerCardsController.PlayerCards[0][0];
                    handObjects[0] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand1Full = true;
                    cardInHand1 = pastClickedGameObject;
                    JudgeCannotSubmit();
                }
                else if (clickedGameObject.name == "Hand2" && isHand2Full == false)
                {
                    playerCardsNumber2 = playerCardsController.PlayerCards[0][0];
                    handObjects[1] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand2Full = true;
                    cardInHand2 = pastClickedGameObject;
                    JudgeCannotSubmit();
                }
                else if (clickedGameObject.name == "Hand3" && isHand3Full == false)
                {
                    playerCardsNumber3 = playerCardsController.PlayerCards[0][0];
                    handObjects[2] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand3Full = true;
                    cardInHand3 = pastClickedGameObject;
                    JudgeCannotSubmit();
                }
                else if (clickedGameObject.name == "Hand4" && isHand4Full == false)
                {
                    playerCardsNumber4 = playerCardsController.PlayerCards[0][0];
                    handObjects[3] = pastClickedGameObject;
                    PlaceCardToHand(pastClickedGameObject);
                    isHand4Full = true;
                    cardInHand4 = pastClickedGameObject;
                    JudgeCannotSubmit();
                }
                else
                {
                    Destroy(pastClickedGameObject);
                    isCardMovingToHand = false;
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
                    movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    return;
                }
                if (pastClickedGameObjectInHand.name == "Hand1")
                {
                    if (clickedGameObjectInHand.name == "Place1")
                    {
                        if (!placeController.IsPutPlace1OK(hand1Number))
                        {
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            return;
                        }
                        placeController.SetPlace1Before(hand1Number);
                        Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand1Number))    
                        {
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            return;
                        }
                        placeController.SetPlace2Before(hand1Number);
                        Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
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
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            return;
                        }
                        placeController.SetPlace1Before(hand2Number);
                        Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand2Number))
                        {
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            return;
                        }
                        placeController.SetPlace2Before(hand2Number);
                        Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
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
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            return;
                        }
                        placeController.SetPlace1Before(hand3Number);
                        Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand3Number))
                        {
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            return;
                        }
                        placeController.SetPlace2Before(hand3Number);
                        Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
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
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            return;
                        }
                        placeController.SetPlace1Before(hand4Number);
                        Destroy(gameManager.placeSubmittedCard[0]);
                        gameManager.placeSubmittedCard[0] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
                        }
                    }
                    if (clickedGameObjectInHand.name == "Place2")
                    {
                        if (!placeController.IsPutPlace2OK(hand4Number))
                        {
                            movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                            movingCard.GetComponent<Card>().isSubmitted = true;
                            isCardMovingToPlace = false;
                            enemyController.cannotSubmit = true;
                            enemyController.JudgeCannotSubmit();
                            return;
                        }
                        placeController.SetPlace2Before(hand4Number);
                        Destroy(gameManager.placeSubmittedCard[1]);
                        gameManager.placeSubmittedCard[1] = movingCard;
                        movingCard.GetComponent<Card>().isSubmitted = true;
                        if (!isCoroutinePlay)
                        {
                            StartCoroutine(enemyController.PlaceSubmitCoroutine());
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
                movingCard.transform.position = pastClickedGameObjectInHand.transform.position;
                isCardMovingToPlace = false;
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

    public void JudgeCannotSubmit()
    {
        if (isHand1Full == false || isHand2Full == false || isHand3Full == false || isHand4Full == false) return;
        if (placeController.IsPutPlace1OK(hand1Number))
        {

        }
        else if (placeController.IsPutPlace1OK(hand2Number))
        {

        }
        else if (placeController.IsPutPlace1OK(hand3Number))
        {

        }
        else if (placeController.IsPutPlace1OK(hand4Number))
        {

        }
        else if (placeController.IsPutPlace2OK(hand1Number))
        {

        }
        else if (placeController.IsPutPlace2OK(hand2Number))
        {

        }
        else if (placeController.IsPutPlace2OK(hand3Number))
        {

        }
        else if (placeController.IsPutPlace2OK(hand4Number))
        {

        }
        else
        {
            cannotSubmit = false;
            gameManager.SubmitWhenStuck();
            Debug.Log("playerStuck");
        }
    }

    //if (HandManager.CheckAndPutCurrentHand())
    //{

    //}
    //else
    //{

    //}

}
