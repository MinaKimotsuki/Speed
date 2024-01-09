using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool isSubmitted = false;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Card"))
        {
            if (isSubmitted == false && other.gameObject.GetComponent<Card>().isSubmitted == true)
            {
                Debug.Log(isSubmitted);
                Debug.Log("a");
                gameManager.SetSortingOrder(gameObject);
            }
        }
    }*/
}
