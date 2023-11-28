using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    GameObject clickedGameObject;
    GameObject clickedGameObjectInHand;
    bool isCardMovingToHand = false;
    bool isCardMovingToPlace = false;
    Vector3 screenPoint;
    int orderInRayer = 0;
    GameObject nextObjectToHand;
    [SerializeField] PlayerCardsController playerCardsController;
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
            if (hit2d.collider.gameObject.CompareTag("Cards"))
            {
                if (isHand1Full == false || isHand2Full == false || isHand3Full == false || isHand4Full == false)
                {
                    clickedGameObject = Instantiate(card, cards.transform.position, Quaternion.identity);
                    clickedGameObject.GetComponent<SpriteRenderer>().sortingOrder = orderInRayer;
                    clickedGameObject.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = orderInRayer;
                    clickedGameObject.transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = orderInRayer;
                    orderInRayer++;
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
            }
            if (hit2d.collider.gameObject.name == "Hand2")
            {
                if (cardInHand2 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand2;
            }
            if (hit2d.collider.gameObject.name == "Hand3")
            {
                if (cardInHand3 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand3;
            }
            if (hit2d.collider.gameObject.name == "Hand4")
            {
                if (cardInHand4 == null) return;
                clickedGameObjectInHand = hit2d.collider.gameObject;
                isCardMovingToPlace = true;
                nextObjectToHand = cardInHand4;
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
                if (clickedGameObject.CompareTag("Hand"))
                {
                    Destroy(pastClickedGameObject);
                    isCardMovingToHand = false;
                }
            }
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                if (clickedGameObject.name == "Hand1" && isHand1Full == false)
                {
                    PlaceCardToHand(pastClickedGameObject);
                    isHand1Full = true;
                    cardInHand1 = pastClickedGameObject;
                }
                else if (clickedGameObject.name == "Hand2" && isHand2Full == false)
                {
                    PlaceCardToHand(pastClickedGameObject);
                    isHand2Full = true;
                    cardInHand2 = pastClickedGameObject;
                }
                else if (clickedGameObject.name == "Hand3" && isHand3Full == false)
                {
                    PlaceCardToHand(pastClickedGameObject);
                    isHand3Full = true;
                    cardInHand3 = pastClickedGameObject;
                }
                else if (clickedGameObject.name == "Hand4" && isHand4Full == false)
                {
                    PlaceCardToHand(pastClickedGameObject);
                    isHand4Full = true;
                    cardInHand4 = pastClickedGameObject;
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
                if (pastClickedGameObjectInHand.name == "Hand1")
                {
                    cardInHand1.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand1Full = false;
                }
                if (pastClickedGameObjectInHand.name == "Hand2")
                {
                    cardInHand2.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand2Full = false;
                }
                if (pastClickedGameObjectInHand.name == "Hand3")
                {
                    cardInHand3.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand3Full = false;
                }
                if (pastClickedGameObjectInHand.name == "Hand4")
                {
                    cardInHand4.transform.position = clickedGameObjectInHand.transform.position;
                    isCardMovingToPlace = false;
                    isHand4Full = false;
                }
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

    //if (HandManager.CheckAndPutCurrentHand())
    //{

    //}
    //else
    //{

    //}

}
