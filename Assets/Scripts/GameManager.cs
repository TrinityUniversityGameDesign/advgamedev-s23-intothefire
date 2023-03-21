using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


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

    [Tooltip("Represents the number of minutes the game should last")]
    public float Minutes = 5;

    [Tooltip("Size of the labyrinth across one axis. Default is 5")]
    public int LabyrinthSize = 5;

    [Tooltip("Distance rooms are apart from one another. Default is 120")]
    public int DistanceApart = 120;

    public GameObject[] players;
    #endregion

    #region Private Fields
    [Tooltip("Internal state the game manager is currently in.")]
    private GameState _state;

    [Tooltip("Simple timer for testing purposes")]
    [SerializeField]
    private GameObject timerTextObj;

    [Tooltip("Represents the number of seconds since the game has started.")]
    float Timer = 0;
    float secondsOfGameTime = 0;

    private TMP_Text timer;
    private bool gameInProgress = false;

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

    public float SecondsDurationSideEvent = 60;

    float timeUntilNextMicroEvent;
    float timeLeftInMicroEvent;

    float timeUntilNextSideEvent;
    float timeLeftInSideEvent;

    bool microEventInProgress = false;
    #endregion

    #region Unity Events
    [Tooltip("A demo event used to demonstrate how events can be used.")]
    public static UnityEvent DemoEvent = new UnityEvent();

    [Tooltip("Event uesd to call when a Micro Event is starting")]
    public UnityEvent MicroEventBegin = new UnityEvent();
    [Tooltip("Event used to call when a Micro Event is ending")]
    public UnityEvent MicroEventEnd = new UnityEvent();

    [Tooltip("Event used to call when a Side Event is starting")]
    public UnityEvent SideEventBegin = new UnityEvent();
    [Tooltip("Event used to cal when a Side Event is Starting")]
    public UnityEvent SideEventEnd = new UnityEvent();

    [Tooltip("Event used to indicate when the Labyrinth is beginning to be explored")]
    public UnityEvent LabyrinthExploreBegin = new UnityEvent();
    [Tooltip("Event used to indicate when the Labyrinth explore phase is being exited")]
    public UnityEvent LabyrinthExploreEnd = new UnityEvent();

    public UnityEvent MajorEventBegin = new UnityEvent();
    public UnityEvent MajorEventEnd = new UnityEvent();

    public UnityEvent FirstTimeStartup = new UnityEvent();
    #endregion

    #region Private Methods
    private void Awake()
    {
        Instance = this;

        DemoEvent.AddListener(TestDemoEvent);
        MicroEventBegin.AddListener(BeginMicroEvent);
        MicroEventEnd.AddListener(EndMicroEvent);
        SideEventBegin.AddListener(BeginSideEvent);
        SideEventEnd.AddListener(EndSideEvent);
        LabyrinthExploreBegin.AddListener(TestLabyrinthBegin);
        FirstTimeStartup.AddListener(TestFirstTimeStartup);

        secondsOfGameTime = 60 * Minutes;

        timer = timerTextObj?.GetComponent<TMP_Text>();

        if (!GameObject.Find("DungeonGenerator"))
        {
            Debug.LogError("Could not find Dungeon Generator. Are you missing it in the scene?");
        }

        GameObject EvtCtrl = GameObject.Find("EventController");

        if (!EvtCtrl)
        {
            Debug.LogError("Could not find EventController. Are you missing it in the scene?");
        } else
        {
            EvtCtrl.transform.position = new Vector3((LabyrinthSize / 2) * DistanceApart, 90, (LabyrinthSize / 2) * DistanceApart);
        }

        OnStateEnter(GameState.Lobby);
        
    }

    private void Start()
    {
        
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
                FirstTimeStartup.Invoke();
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
                if (!gameInProgress)
                {
                    MajorEventBegin.Invoke();
                }

                gameInProgress = true;

                Debug.Log("Entering Labyrinth_Explore");

                LabyrinthExploreBegin?.Invoke();

                timeUntilNextMicroEvent = Random.Range(MinSecondsBetweenMicroEvents, MaxSecondsBetweenMicroEvents);
                timeUntilNextSideEvent = Random.Range(MinSecondsBetweenSideEvents, MaxSecondsBetweenSideEvents);

                Debug.Log("Time until next Micro Event: " + timeUntilNextMicroEvent + " s");
                Debug.Log("Time until next Side Event: " + timeUntilNextSideEvent + " s");
                break;
            case GameState.Lobby:
                break;
            case GameState.Paused:
                break;
            case GameState.Showdown:
                MajorEventEnd.Invoke();
                Debug.Log("Entering Showdown");
                break;
            case GameState.SideEvent:
                SideEventBegin?.Invoke();
                break;
            case GameState.EndScreen:
                gameInProgress = false;
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
                //Currently this will reset the timings of Side Events and Micro Events which I think is fine for now....

                Timer += Time.deltaTime;
                if (timer != null) { timer.text = string.Format("{0}:{1}", (int)((secondsOfGameTime - Timer) / 60), (int)((secondsOfGameTime - Timer) % 60)); }

                //Micro events and side events happen differently because micro events are concurrent and side events are disruptive
                if (!microEventInProgress) timeUntilNextMicroEvent -= Time.deltaTime;
                if (microEventInProgress) timeLeftInMicroEvent -= Time.deltaTime;
                timeUntilNextSideEvent -= Time.deltaTime;

                if(timeUntilNextMicroEvent <= 0 && !microEventInProgress)
                {
                    MicroEventBegin?.Invoke();
                }

                if(timeUntilNextSideEvent <= 0)
                {
                    MicroEventEnd?.Invoke();
                    OnStateEnter(GameState.SideEvent);
                    break;
                }

                if(microEventInProgress && timeLeftInMicroEvent <= 0)
                {
                    MicroEventEnd?.Invoke();
                }

                if(Timer >= Minutes * 60)
                {
                    OnStateEnter(GameState.Showdown);
                    break;
                }
                break;
            case GameState.Lobby:
                break;
            case GameState.Paused:
                break;
            case GameState.Showdown:
                break;
            case GameState.SideEvent:
                timeLeftInSideEvent -= Time.deltaTime;
                if(timeLeftInSideEvent <= 0)
                {
                    OnStateEnter(GameState.Labyrinth_Explore);
                }
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
                SideEventEnd?.Invoke();
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

    void TestSideEventEnd()
    {
        Debug.Log("SideEventEnding");
    }

    void TestLabyrinthBegin()
    {
        Debug.Log("LabyrinthBegin");
    }

    void TestBeginMajorEvent()
    {
        Debug.Log("Beginning Major Event Setup");
    }

    void TestEndMajorEvent()
    {
        Debug.Log("Ending Major Event");
    }

    void TestFirstTimeStartup()
    {
        Debug.Log("Received Call that GameManager has Started");
    }

    void BeginSideEvent()
    {
        Debug.Log("Beginning SideEvent as " + Timer + " game seconds.");
        Debug.Log("Side Event Duration: " + SecondsDurationSideEvent + " s");
        timeLeftInSideEvent = SecondsDurationSideEvent;
    }

    void EndSideEvent()
    {
        Debug.Log("Ending Side event at " + Timer + " game seconds.");
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
