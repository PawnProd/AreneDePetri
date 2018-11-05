using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnInfos : MonoBehaviour {

    GameObject currentTurn;
    GameObject currentPlayer;

    GameController gm;

	// Use this for initialization
	void Start () {
        gm = GameController.Instance;
        currentTurn = transform.Find("CurrentTurn").gameObject;
        currentPlayer = transform.Find("CurrentPlayer").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        currentTurn.GetComponent<Text>().text = ""+gm.currentTurn;
        currentPlayer.GetComponent<Text>().text = gm.activPlayer.name;
	}
}
