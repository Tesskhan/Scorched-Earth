using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string currentPlayerTurn = "Player1"; // Track whose turn it is

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPlayerTurn(bool playerOneTurn)
    {
        if (playerOneTurn)
        {
            return currentPlayerTurn == "Player1";
        }
        else
        {
            return currentPlayerTurn == "Player2";
        }
    }

}
