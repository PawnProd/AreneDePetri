using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryManager : MonoBehaviour {

    public Player player1;
    public Player player2;

    public TextMeshProUGUI bravo;

    public Transform player1Stats;
    public Transform player2Stats;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        majP1Stats();
        majP2Stats();
        majBravo();
    }

    private void majP1Stats ()
    {
        player1Stats.Find("Player").GetComponent<TextMeshProUGUI>().text = player1.playerName != "" ? player1.playerName : "Joueur 1";
        player1Stats.Find("DestroyedCells").GetComponent<TextMeshProUGUI>().text = ""+player1.totalCellsKilled;
        player1Stats.Find("DamagesGiven").GetComponent<TextMeshProUGUI>().text = ""+player1.totalGivenDamages;
        player1Stats.Find("DamagesTaken").GetComponent<TextMeshProUGUI>().text = ""+player1.totalTakenDamages;
        player1Stats.Find("Healing").GetComponent<TextMeshProUGUI>().text = ""+player1.totalHeal;
        player1Stats.Find("BonusPoints").GetComponent<TextMeshProUGUI>().text = ""+player1.totalBonusPoints;
    }

    private void majP2Stats ()
    {
        player2Stats.Find("Player").GetComponent<TextMeshProUGUI>().text = player2.playerName != "" ? player2.playerName : "Joueur 2";
        player2Stats.Find("DestroyedCells").GetComponent<TextMeshProUGUI>().text = "" + player2.totalCellsKilled;
        player2Stats.Find("DamagesGiven").GetComponent<TextMeshProUGUI>().text = "" + player2.totalGivenDamages;
        player2Stats.Find("DamagesTaken").GetComponent<TextMeshProUGUI>().text = "" + player2.totalTakenDamages;
        player2Stats.Find("Healing").GetComponent<TextMeshProUGUI>().text = "" + player2.totalHeal;
        player2Stats.Find("BonusPoints").GetComponent<TextMeshProUGUI>().text = "" + player2.totalBonusPoints;
    }

    private void majBravo()
    {
        string name;
        if (GameController.Instance.winner != null)
        {
            name = GameController.Instance.winner.playerName;
        } else {
            name = "Joueur";
        }
        bravo.text = "Bravo " + name +" !!!";
    }
}
