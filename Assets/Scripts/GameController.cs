using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance { private set; get; }

    public AthController ath;
    // Player Controllers
    public Player player1;
    public Player player2;

    public bool awaitingPlayerAction;

    public bool isGameOver;

    public Player activPlayer;
    public int currentTurn;
    public int turnPhase;

    public List<Cell> playerCells;
    public List<Cell> playerHealerCells;
    public List<Cell> playerAttackingCells;
    public List<Cell> playerDyingCells;

    public Player winner;

    public List<Cell> giveAwayCells;

    void Awake()
    {
        Instance = this;
        activPlayer = player1;
    }

    // Use this for initialization
    void Start () {
        ath = AthController.Instance;
        
        turnPhase = 1;
        awaitingPlayerAction = false;
        currentTurn = 1;
        isGameOver = false;

        player1.playerName = DataTransfert.Instance.player1Name != "" ? DataTransfert.Instance.player1Name : "Joueur 1";
        player2.playerName = DataTransfert.Instance.player2Name != "" ? DataTransfert.Instance.player2Name : "Joueur 2";
    }
	
	// Update is called once per frame
	void Update () {
        // On effectue les actions du tour
        if (!isGameOver)
        {
            PlayerTurn();
        }
        
	}

    private void PlayerTurn ()
    {
        if (winner == null)
        {
            switch (turnPhase)
            {
                case 1:
                    Debug.Log("Phase 1");
                    TurnPhase1();
                    break;
                case 2:
                    //Debug.Log("Phase 2");
                    TurnPhase2();
                    break;
                case 3:
                    Debug.Log("Phase 3");
                    TurnPhase3();
                    break;
                default:
                    Debug.Log("On est pas sensé passer là.");
                    break;
            }
        } else { // Cas fin de partie
            EndOfGame();
        }
    }

    // Phase auto de regen de l'armure des cells du joueur actif et des soins de ces cellules.
    private void TurnPhase1 ()
    {
        ath.ShowPanelTurn(currentTurn, activPlayer.playerName, activPlayer.playerColor);

        // On récupère la liste des cellules du joueur
        playerCells = activPlayer.cells;
        playerAttackingCells = new List<Cell>();
        playerHealerCells = new List<Cell>();
        playerDyingCells = new List<Cell>();

        // On remet l'armure des cells à leur valeur max
        foreach (Cell cell in playerCells)
        {
            cell.ReconstructArmor();
            // On récupère les cells soigneuses
            if (cell.isHealer && cell.isAlive)
            {
                playerHealerCells.Add(cell);
            }
            // On récupère la liste des cellules attaquantes
            if (!cell.isHealer && cell.isAlive)
            {
                playerAttackingCells.Add(cell);
            }
            // On récupère la liste des cellules mourantes
            if (!cell.isAlive)
            {
                playerDyingCells.Add(cell);
            }
        }

        // On effectue les soins
        foreach (Cell cell in playerHealerCells)
        {
            cell.Heal();
        }

        // Passage à la phase suivante
        turnPhase = 2;
        awaitingPlayerAction = true;
    }

    // Phase où le joueur intéragis avec l'éditeur, il clone et mute une cell
    private void TurnPhase2 ()
    {
       

        
        // Attente des actions du joueur
        if (!awaitingPlayerAction)
        {
            Debug.Log("Le joueur a effectué son action");
            // Passage à la phase suivante
            turnPhase = 3;
        }
    }

    // Phase auto où les cells du joueur actif attaquent celles de l'autre joueur.
    private void TurnPhase3 ()
    {
        activPlayer.isPlaying = false;

        foreach (Cell cell in giveAwayCells)
        {
            if (cell != null && !cell.isAlive)
            {
                Destroy(cell.transform.parent.gameObject);
            }
        }

        foreach (Cell cell in playerDyingCells)
        {
            activPlayer.cells.Remove(cell);
            Destroy(cell.transform.parent.gameObject);
        }

        // Effectuer les attaques des cells
        foreach (Cell cell in playerCells)
        {
            if (!cell.isHealer)
            {
                cell.Attack();
            }
        }

            if (IsOpponentStemCellAlive())
        {
            // Incrémentation du compteur de tour
            if (activPlayer == player2) { currentTurn++; }

            // Changer de joueur actif
            if (activPlayer == player1) {
                activPlayer = player2;
            } else if (activPlayer == player2) {
                activPlayer = player1;
            }

            // Passage à la phase suivante
            turnPhase = 1;
        } else {
            winner = activPlayer;
            EndOfGame();
        }
        
    }

    // Permet au joueur de notifier qu'il a fini les actions de son tour de jeu
    public void PlayerActionDone()
    {
        awaitingPlayerAction = false;
    }

    // Vérification des conditions de victoire
    bool IsOpponentStemCellAlive()
    {
        Debug.Log("Verification des conditions de victoire");
        bool isStemCellStillAlive = true;
        if (activPlayer == player1)
        {
            isStemCellStillAlive = player2.stemCell != null && player2.stemCell.isAlive;
            Debug.Log("Joueur 1 : "+isStemCellStillAlive);
        } else {
            isStemCellStillAlive = player1.stemCell != null && player1.stemCell.isAlive;
            Debug.Log("Joueur 2 : " + isStemCellStillAlive);
        }
        return isStemCellStillAlive;
    }

    // Gestion de la fin de partie
    public void EndOfGame ()
    {
        Debug.Log("Le joueur : "+winner.name+" a remporté la partie!!!");
        isGameOver = true;
        ath.transform.Find("[Panel] Victory").gameObject.SetActive(true);
    }
}
