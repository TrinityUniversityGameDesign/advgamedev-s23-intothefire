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
    GameOver,
    SideEvent,
    EndScreen
}

public class GameManager : MonoBehaviour
{

    #region Public Fields
    [Tooltip("Static gamemanager instance.")]
    public static GameManager Instance;

    [Tooltip("Represents the number of fixed frames since the game has started.")]
    public static float Timer = 0;

    [Tooltip("Represents the number of minutes the game should last")]
    public float Minutes = 5;

    [Tooltip("A demo event used to demonstrate how events can be used.")]
    public static UnityEvent DemoEvent;

    [Tooltip("Event used to call when a Side Event is starting")]
    public static UnityEvent SideEventBegin;

    [Tooltip("Event used to indicate when the Labyrinth is beginning to be explored")]
    public static UnityEvent LabyrinthExploreBegin;
    #endregion

    #region Private Fields
    [Tooltip("Internal state the game manager is currently in.")]
    private GameState _state;

    #endregion

    #region Event Fields
    //Timing controls between events
    public float MaxSecondsBetweenSideEvents = 600;
    public float MinSecondsBetweenSideEvents = 300;

    public float MaxSecondsBetweenMicroEvents = 180;
    public float MinSecondsBetweenMicroEvents = 60;

    //Side events are fixed duration
    public float MaxSecondsDurationMicroEvents = 20;
    public float MinSecondsDurationMicroEvents = 10;

    float timeUntilNextMicroEvent;
    float timeLeftInMicroEvent;

    float timeUntilNextSideEvent;
    float timeLeftInSideEvent;

    bool microEventInProgress = false; 
    #endregion 

    #region Private Methods
    private void Start()
    {
        Instance = this;

        if (DemoEvent == null) DemoEvent = new UnityEvent();
        DemoEvent.AddListener(TestDemoEvent);

        if (SideEventBegin == null) SideEventBegin = new UnityEvent();
        SideEventBegin.AddListener(TestSideEventBegin);

        if (LabyrinthExploreBegin == null) LabyrinthExploreBegin = new UnityEvent();
        LabyrinthExploreBegin.AddListener(TestLabyrinthBegin);

        OnStateEnter(GameState.Lobby);
        
    }

    private void FixedUpdate()
    {
        TickState();

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightControl)) //Activate Debug KeyCommands
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnStateEnter(GameState.Labyrinth_Explore);
            }
        }
    }

    private void OnStateEnter(GameState nextState)
    {
        //Exit the current state, then call the enter on the nextState.
        OnStateExit();
        _state = nextState;
        switch (_state)
        {
            case GameState.GameOver:
                Timer = 0;
                break;
            case GameState.Labyrinth_Explore:
                Debug.Log("Entering Labyrinth_Explore");

                LabyrinthExploreBegin?.Invoke();

                timeUntilNextMicroEvent = Random.Range(MinSecondsBetweenMicroEvents, MaxSecondsBetweenMicroEvents);
                timeUntilNextSideEvent = Random.Range(MinSecondsBetweenSideEvents, MaxSecondsBetweenSideEvents);

                Debug.Log("Time until next Micro Event: " + timeUntilNextMicroEvent + " s");
                break;
            case GameState.Lobby:
                break;
            case GameState.Paused:
                break;
            case GameState.Showdown:
                Debug.Log("Entering Showdown");
                break;
            case GameState.SideEvent:
                SideEventBegin?.Invoke();
                break;
            case GameState.EndScreen:
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
                Timer += Time.deltaTime;

                if(!microEventInProgress) timeUntilNextMicroEvent -= Time.deltaTime;
                if (microEventInProgress) timeLeftInMicroEvent -= Time.deltaTime;


                if(timeUntilNextMicroEvent <= 0 && !microEventInProgress)
                {
                    BeginMicroEvent();
                }

                if(microEventInProgress && timeLeftInMicroEvent <= 0)
                {
                    EndMicroEvent();
                }

                if(Timer >= Minutes * 60)
                {
                    OnStateEnter(GameState.Showdown);
                }

                break;
            case GameState.Lobby:
                break;
            case GameState.Paused:
                break;
            case GameState.Showdown:
                break;
            case GameState.SideEvent:
                break;
            case GameState.EndScreen:
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
            case GameState.SideEvent:
                break;
            case GameState.EndScreen:
                break;
            default:
                break;
        }
    }

    void TestDemoEvent()
    {
        Debug.Log("TestDemoEvent");
    }

    void TestSideEventBegin()
    {
        Debug.Log("SideEventBegin");
    }

    void TestLabyrinthBegin()
    {
        Debug.Log("LabyrinthBegin");
    }

    void BeginMicroEvent()
    {
        microEventInProgress = true;
        Debug.Log("Beginning Microevent at " + Timer + " game seconds.");

        timeLeftInMicroEvent = Random.Range(MinSecondsDurationMicroEvents, MaxSecondsDurationMicroEvents);
        Debug.Log("Microevent Duration: " + timeLeftInMicroEvent + " s");
    }

    void EndMicroEvent()
    {
        microEventInProgress = false;
        Debug.Log("Ending Microevent at " + Timer + " game seconds.");

        timeUntilNextMicroEvent = Random.Range(MinSecondsBetweenMicroEvents, MaxSecondsBetweenMicroEvents);
        Debug.Log("Time until next Micro Event: " + timeUntilNextMicroEvent + " s");
    }

    #endregion
}
