using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//GameState to represent the possible states the game can be in.
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
    [Tooltip("Static gamemanager instance.")]
    public static GameManager Instance;

    [Tooltip("Represents the number of fixed frames since the game has started.")]
    public static float Timer; 

    [Tooltip("A demo event used to demonstrate how events can be used.")]
    public static UnityEvent DemoEvent;
    #endregion

    #region Private Fields
    [Tooltip("Internal state the game manager is currently in.")]
    private GameState _state;
    #endregion

    #region Private Methods
    private void Start()
    {
        Instance = this;

        if (DemoEvent == null) DemoEvent = new UnityEvent();
        DemoEvent.AddListener(Ping);
    }

    private void FixedUpdate()
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
                Timer = 0;
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
                Timer++;
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
