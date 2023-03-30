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


    // Start is called before the first frame update
    void Start()
    { 
        GameManager.Instance.LabyrinthExploreBegin.AddListener(ActivateLabyrinthUI);
        GameManager.Instance.LabyrinthExploreEnd.AddListener(DeactivateLabyrinthUI);

        GameManager.Instance.LobbyBegin.AddListener(ActivateLobbyUI);
        GameManager.Instance.LobbyEnd.AddListener(DeactivateLobbyUI);
        DeactivateLabyrinthUI();
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
        lobbyUI.SetActive(true);
        labyrinthUI.SetActive(false);
    }

    void DeactivateLobbyUI()
    { 
        lobbyUI.SetActive(false);
    }
}