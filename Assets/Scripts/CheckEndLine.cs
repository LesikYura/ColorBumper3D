using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEndLine : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (GameController.Instance == null || GameController.Instance.gameState != GameState.Started) return;
        
        if (other.tag == "Player")
        {
            GameController.Instance.WinGame();
        }
    }
}
