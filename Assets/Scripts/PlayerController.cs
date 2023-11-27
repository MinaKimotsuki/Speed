using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    GameObject clickedGameObject;
    bool isCardMovingToHand = false;
    Vector3 screenPoint;
    int orderInRayer = 0;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseButtonDown();
        GetMouseButtonUp();
        CardTransformWhileMoving();
    }

    void GetMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedGameObject = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit2d)
            {
                if (isHand1Full == false || isHand2Full == false || isHand3Full == false || isHand4Full == false)
                {
                    if (hit2d.collider.gameObject.CompareTag("Cards"))
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
                if (clickedGameObject.CompareTag("Hand"))
                {
                    if (clickedGameObject.name == "Hand1")
                    {
                        if (isHand1Full == false)
                        {
                            playerCardsController.PlayerCards.RemoveAt(0);
                            isCardMovingToHand = false;
                            pastClickedGameObject.transform.position = clickedGameObject.transform.position;
                            isHand1Full = true;
                        }
                        else
                        {
                            Destroy(pastClickedGameObject);
                            isCardMovingToHand = false;
                        }
                    }
                    if (clickedGameObject.name == "Hand2")
                    {
                        if (isHand2Full == false)
                        {
                            playerCardsController.PlayerCards.RemoveAt(0);
                            isCardMovingToHand = false;
                            pastClickedGameObject.transform.position = clickedGameObject.transform.position;
                            isHand2Full = true;
                        }
                        else
                        {
                            Destroy(pastClickedGameObject);
                            isCardMovingToHand = false;
                        }
                    }
                    if (clickedGameObject.name == "Hand3")
                    {
                        if (isHand3Full == false)
                        {
                            playerCardsController.PlayerCards.RemoveAt(0);
                            isCardMovingToHand = false;
                            pastClickedGameObject.transform.position = clickedGameObject.transform.position;
                            isHand3Full = true;
                        }
                        else
                        {
                            Destroy(pastClickedGameObject);
                            isCardMovingToHand = false;
                        }
                    }
                    if (clickedGameObject.name == "Hand4")
                    {
                        if (isHand4Full == false)
                        {
                            playerCardsController.PlayerCards.RemoveAt(0);
                            isCardMovingToHand = false;
                            pastClickedGameObject.transform.position = clickedGameObject.transform.position;
                            isHand4Full = true;
                        }
                        else
                        {
                            Destroy(pastClickedGameObject);
                            isCardMovingToHand = false;
                        }
                    }
                }
                else
                {
                    Destroy(pastClickedGameObject);
                    isCardMovingToHand = false;
                }
            }
        }
    }

    void CardTransformWhileMoving()
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
}
