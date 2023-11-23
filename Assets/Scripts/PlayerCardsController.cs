using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsController : MonoBehaviour
{
    public List<List<int>> PlayerCards { get; set; } = new List<List<int>>();
    

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayerCards.Count; i++)
        {
            Debug.Log("player" + i + ":" + PlayerCards[i][0] + "," + PlayerCards[i][1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
