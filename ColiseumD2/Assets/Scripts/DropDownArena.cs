using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDownArena : MonoBehaviour
{
    // Start is called before the first frame update
    public static int arenaID;

    public void HandleInputData(int val)
    {
        if (val == 1)
        {
            arenaID = 1;
        }
        else
        {
            arenaID = 0;
        }
    }
}
