using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Controller : MonoBehaviour
{

    #region Private Fields
    private GameObject lobbyUI;
    private GameObject labyrinthUI;
    #endregion


    #region Public Fields

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance?.LabyrinthExploreBegin.AddListener(ActivateLabyrinthUI);
        GameManager.Instance?.LabyrinthExploreEnd.AddListener(DeactivateLabyrinthUI);

        GameManager.Instance?.LobbyBegin.AddListener(ActivateLobbyUI);
        GameManager.Instance?.LobbyEnd.AddListener(DeactivateLabyrinthUI);
    }

    void ActivateLabyrinthUI()
    {
        labyrinthUI.SetActive(true);
    }

    void DeactivateLabyrinthUI()
    {
        labyrinthUI.SetActive(false);
    }

    void ActivateLobbyUI()
    {
        lobbyUI.SetActive(true);
    }

    void DeactivateLobbyUI()
    {
        lobbyUI.SetActive(false);
    }
}