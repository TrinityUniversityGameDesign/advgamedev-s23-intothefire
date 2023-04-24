using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UI_Controller : MonoBehaviour
{

    #region Private Fields
    public GameObject lobbyUI;
    public GameObject labyrinthUI;
    public GameObject sideEventUI;
    #endregion


    #region Public Fields

    #endregion

    private void Awake()
    {
        GameManager.Instance.LabyrinthExploreBegin.AddListener(ActivateLabyrinthUI);
        GameManager.Instance.LabyrinthExploreEnd.AddListener(DeactivateLabyrinthUI);

        GameManager.Instance.LobbyBegin.AddListener(ActivateLobbyUI);
        GameManager.Instance.LobbyEnd.AddListener(DeactivateLobbyUI);

        GameManager.Instance.SideEventBegin.AddListener(ActivateSideEventUI);
        GameManager.Instance.SideEventEnd.AddListener(DeactivateSideEventUI);
    }
    // Start is called before the first frame update
    void Start()
    { 

    }

    private void Update()
    {
        if (sideEventUI.activeInHierarchy)
        {
            sideEventUI.GetComponentInChildren<TMP_Text>().text = ((int)GameManager.Instance.timeLeftInSideEvent).ToString();
        }

        if (labyrinthUI.activeInHierarchy)
        {
            int duration = (int)(GameManager.Instance.secondsOfGameTime - GameManager.Instance.Timer);
            int seconds = duration % 60;
            labyrinthUI.GetComponentInChildren<TMP_Text>().text = string.Format("{0}:{1}", duration / 60, seconds < 10 ? "0" + seconds : seconds); 
        }
    }

    void ActivateLabyrinthUI()
    {
        labyrinthUI.SetActive(true);
        gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }

    void DeactivateLabyrinthUI()
    {
        labyrinthUI.SetActive(false);
    }

    void ActivateLobbyUI()
    {
        lobbyUI.SetActive(true);
        gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
    }

    void DeactivateLobbyUI()
    { 
        lobbyUI.SetActive(false);
    }

    void ActivateSideEventUI()
    {
        sideEventUI.SetActive(true);
    }

    void DeactivateSideEventUI()
    {
        sideEventUI.SetActive(false);
    }
}