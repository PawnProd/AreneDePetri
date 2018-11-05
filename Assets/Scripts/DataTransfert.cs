using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfert : MonoBehaviour {
    public static DataTransfert Instance;

    public string player1Name = "Joueur 1";
    public string player2Name = "Joueur 2";

    void Awake()
    {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }
}
