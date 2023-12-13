using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceController : MonoBehaviour
{
    int place1Before;
    int place2Before;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPutPlace1OK(int place1now)
    {
        if (place1now + 1 == place1Before || place1now - 1 == place1Before)
        {
            return true;
        }
        else if ((place1now == 1 && place1Before == 13) || (place1now == 13 && place1Before == 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsPutPlace2OK(int place2now)
    {
        if (place2now + 1 == place2Before || place2now - 1 == place2Before)
        {
            return true;
        }
        else if ((place2now == 1 && place2Before == 13) || (place2now == 13 && place2Before == 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetPlace1Before(int place1before)
    {
        place1Before = place1before;
    }

    public void SetPlace2Before(int place2before)
    {
        place2Before = place2before;
    }
}
