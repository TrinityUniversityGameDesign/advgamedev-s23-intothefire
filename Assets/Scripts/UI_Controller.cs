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
        GameManager.Instance?.LabyrinthExploreBegin.AddListener(ActivateLabyrinthUI);
        GameManager.Instance?.LabyrinthExploreEnd.AddListener(DeactivateLabyrinthUI);

        GameManager.Instance?.LobbyBegin.AddListener(ActivateLobbyUI);
        GameManager.Instance?.LobbyEnd.AddListener(DeactivateLobbyUI);
    }

    void ActivateLabyrinthUI()
    {
        Debug.Log("Acitivating labyrinth");
        labyrinthUI.SetActive(true);
    }

    void DeactivateLabyrinthUI()
    {
        Debug.Log("Deactiviating labyrinth");
        labyrinthUI.SetActive(false);
    }

    void ActivateLobbyUI()
    {
        Debug.Log("Activating Lobby");
        lobbyUI.SetActive(true);
    }

    void DeactivateLobbyUI()
    {
        Debug.Log("Deactivating Lobby");
        lobbyUI.SetActive(false);
    }
}