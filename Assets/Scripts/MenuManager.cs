using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour {

    

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Debug.Log("Fermeture de l'application...");
        Application.Quit();
    }

    public void LoadCredits ()
    {
        transform.Find("[Panel] StartMenu").gameObject.SetActive(false);
        transform.Find("[Panel] Credits").gameObject.SetActive(true);
    }

    public void UnloadCredits ()
    {
        transform.Find("[Panel] Credits").gameObject.SetActive(false);
        transform.Find("[Panel] StartMenu").gameObject.SetActive(true);
    }

    public void Player1NameChanged (string name) {
        DataTransfert.Instance.player1Name = name;
        transform.Find("[Panel] StartMenu").Find("[InputField] Player1Name").Find("Text").GetComponent<Text>().text = name;
    }

    public void Player2NameChanged(string name) {
        DataTransfert.Instance.player2Name = name;
        transform.Find("[Panel] StartMenu").Find("[InputField] Player2Name").Find("Text").GetComponent<Text>().text = name;
    }
}
