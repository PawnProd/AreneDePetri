using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveawayCell : MonoBehaviour {

    public int pointsToGive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void givePointsToKiller ()
    {
        Debug.Log("Coucou");
        Player playerToGrantPoints = GameController.Instance.activPlayer;

        playerToGrantPoints.bonusPoints += pointsToGive;
        playerToGrantPoints.totalBonusPoints += pointsToGive;
       
    }
}
