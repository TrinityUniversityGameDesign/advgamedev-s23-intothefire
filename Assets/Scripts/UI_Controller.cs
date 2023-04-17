using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Controller : MonoBehaviour
{

    #region Private Fields
    public GameObject lobbyUI;
    public GameObject labyrinthUI;
    #endregion


    #region Public Fields

    #endregion

    private void Awake()
    {
        GameManager.Instance.LabyrinthExploreBegin.AddListener(ActivateLabyrinthUI);
        GameManager.Instance.LabyrinthExploreEnd.AddListener(DeactivateLabyrinthUI);

        GameManager.Instance.LobbyBegin.AddListener(ActivateLobbyUI);
        GameManager.Instance.LobbyEnd.AddListener(DeactivateLobbyUI);
        //Debug.Log("Awake UI Controller");
    }
    // Start is called before the first frame update
    void Start()
    { 
        //Debug.Log("Start UI Controller");
        //DeactivateLabyrinthUI();
    }

    void ActivateLabyrinthUI()
    {
        labyrinthUI.SetActive(true);
        lobbyUI.SetActive(false);
    }

    void DeactivateLabyrinthUI()
    {
        labyrinthUI.SetActive(false);
    }

    void ActivateLobbyUI()
    {
        //Debug.LogError("Beginning Lobby from UI");
        lobbyUI.SetActive(true);
        labyrinthUI.SetActive(false);
    }

    void DeactivateLobbyUI()
    { 
        lobbyUI.SetActive(false);
    }
}