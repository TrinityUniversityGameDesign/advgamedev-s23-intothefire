using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum GameState
{
    Labyrinth_Explore,
    Showdown,
    Lobby,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{

    #region Public Fields
    public static GameManager instance;
    public static UnityEvent DemoEvent;
    #endregion

    #region Private Fields
    private GameState _state;
    #endregion

    #region Private Methods
    private void Start()
    {
        if (DemoEvent == null) DemoEvent = new UnityEvent();
        DemoEvent.AddListener(Ping);
    }

    private void Update()
    {
        TickState();

        if (Input.GetKeyDown(KeyCode.P))
        {
            DemoEvent?.Invoke();
        }
    }

    private void OnStateEnter(GameState nextState)
    {
        OnStateExit();
        _state = nextState;
        switch (_state)
        {
            case GameState.GameOver:
                break;
            case GameState.Labyrinth_Explore:
                break;
            case GameState.Lobby:
                break;
            case GameState.Paused:
                break;
            case GameState.Showdown:
                break;
            default:
                break;
        }
    }

    private void TickState()
    {
        switch (_state)
        {
            case GameState.GameOver:
                break;
            case GameState.Labyrinth_Explore:
                break;
            case GameState.Lobby:
                break;
            case GameState.Paused:
                break;
            case GameState.Showdown:
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (_state)
        {
            case GameState.GameOver:
                break;
            case GameState.Labyrinth_Explore:
                break;
            case GameState.Lobby:
                break;
            case GameState.Paused:
                break;
            case GameState.Showdown:
                break;
            default:
                break;
        }
    }

    void Ping()
    {
        Debug.Log("Ping");
    }

    #endregion
}
