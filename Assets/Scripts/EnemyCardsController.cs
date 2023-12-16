using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardsController : MonoBehaviour
{
    public List<List<int>> EnemyCards { get; set; } = new List<List<int>>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < EnemyCards.Count; i++)
        {
            /*Debug.Log("enemy" + i + ":" + EnemyCards[i][0] + "," + EnemyCards[i][1]);*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
