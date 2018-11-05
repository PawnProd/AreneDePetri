using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AthController : MonoBehaviour {

    public static AthController Instance { private set; get; }

    public GameObject mutationPanel;
    public GameObject panelTurn;

    public TextMeshProUGUI turnText;
    public TextMeshProUGUI playerText;

    public RectTransform attackTransform;
    public RectTransform healTransform;

    public Button validateMutationButton;

    private IEnumerator co_ShowPanel;
    private bool panelTurnOn = false;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetMouseButtonDown(0) && panelTurnOn)
        {
            panelTurnOn = false;
            StopCoroutine(co_ShowPanel);
            GameController.Instance.activPlayer.isPlaying = true;
            panelTurn.SetActive(false);
        }*/
	}

    public void ShowMutationPanel()
    {
        mutationPanel.SetActive(true);
    }

    public void EnableOrDisableValidateMutationButton(bool enable)
    {
        validateMutationButton.interactable = enable;
    }

    public void ShowPanelTurn(int nbTurn, string playerName, Color playerColor)
    {
        turnText.text = "Tour n°" + nbTurn;
        playerText.text = playerName;
        playerText.color = playerColor;
        co_ShowPanel = CO_ShowPanel();
        StartCoroutine(co_ShowPanel);
        panelTurnOn = true;
    }


    IEnumerator CO_ShowPanel()
    {
        yield return new WaitForSeconds(2);
        panelTurn.SetActive(true);

        yield return new WaitForSeconds(2);

        panelTurn.SetActive(false);
        panelTurnOn = false;
        GameController.Instance.activPlayer.isPlaying = true;
        yield return null;
    }

}
