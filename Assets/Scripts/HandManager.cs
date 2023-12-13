using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    Hand[] playerHands = new Hand[4];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckAndPutCurrentHand(Hand rayedObject)
    {
        Hand currentHand = null;
        //hoge
        for(int i=0;i < playerHands.Length; i++)
        {
            if(rayedObject.GetInstanceID() == playerHands[i].GetInstanceID())
            {
                currentHand = playerHands[i];
            }
        }
        
        return !currentHand.isFull;
    }
}
