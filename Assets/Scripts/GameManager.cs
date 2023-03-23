using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


//GameState to represent the possible states the game can be in.
public enum GameState
{
    Labyrinth_Explore,
    Showdown,
    Lobby,
    Paused,
    GameOver,
    SideEvent,
    EndScreen,
    Startup_New_Game
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

    public List<GameObject> players = new List<GameObject>();

    public GameObject lobbyUI;
    #endregion

    #region Private Fields
    [Tooltip("Internal state the game manager is currently in.")]
    private GameState _state = GameState.Lobby;

    [Tooltip("Simple timer for testing purposes")]
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

    [Tooltip("Event called when the Showdown begins")]
    public UnityEvent ShowdownBegin = new UnityEvent();
    [Tooltip("Event call when the Showdown ends")]
    public UnityEvent ShowdownEnd = new UnityEvent();

    [Tooltip("Event called when the lobby begins")]
    public UnityEvent LobbyBegin = new UnityEvent();
    [Tooltip("Event called when the lobby ends")]
    public UnityEvent LobbyEnd = new UnityEvent();

    [Tooltip("Event called when the game pauses")]
    public UnityEvent PausedBegin = new UnityEvent();
    [Tooltip("Event called when the game unpauses")]
    public UnityEvent PausedEnd = new UnityEvent();

    [Tooltip("Event called when the Game over begins")]
    public UnityEvent GameOverBegin = new UnityEvent();
    [Tooltip("Event called when the Game over ends")]
    public UnityEvent GameOverEnd = new UnityEvent();

    [Tooltip("Event called when the end screen sequence begins")]
    public UnityEvent EndScreenBegin = new UnityEvent();
    [Tooltip("Event called when the end screen sequence ends")]
    public UnityEvent EndScreenEnd = new UnityEvent();

    [Tooltip("Event called when the startup screen begins")]
    public UnityEvent StartupNewGameBegin = new UnityEvent();
    [Tooltip("Event called when the startup screen ends")]
    public UnityEvent StartupNewGameEnd = new UnityEvent();

    [Tooltip("Event called when a Major event begins")]
    public UnityEvent MajorEventBegin = new UnityEvent();
    [Tooltip("Event called when the Major event ends")]
    public UnityEvent MajorEventEnd = new UnityEvent();

    [Tooltip("Event called when a player joined.")]
    public UnityEvent PlayerJoined = new UnityEvent();
    #endregion

    #region Private Methods
    private void Awake()
    {
        Instance = this;

        MicroEventBegin.AddListener(BeginMicroEvent);
        MicroEventEnd.AddListener(EndMicroEvent);

        SideEventBegin.AddListener(BeginSideEvent);
        SideEventEnd.AddListener(EndSideEvent);

        MajorEventBegin.AddListener(TestBeginMajorEvent);
        MajorEventEnd.AddListener(TestEndMajorEvent);

        LabyrinthExploreBegin.AddListener(TestLabyrinthBegin);
        LabyrinthExploreEnd.AddListener(TestLabyrinthEnd);

        ShowdownBegin.AddListener(TestShowdownBegin);
        ShowdownEnd.AddListener(TestShowdownEnd);

        LobbyBegin.AddListener(TestLobbyBegin);
        LobbyEnd.AddListener(TestLobbyEnd);

        PausedBegin.AddListener(TestPausedBegin);
        PausedEnd.AddListener(TestPausedEnd);

        GameOverBegin.AddListener(TestGameOverBegin);
        GameOverEnd.AddListener(TestGameOverEnd);

        EndScreenBegin.AddListener(TestEndScreenBegin);
        EndScreenEnd.AddListener(TestEndScreenEnd);

        StartupNewGameBegin.AddListener(TestStartupNewGameBegin);
        StartupNewGameEnd.AddListener(TestStartupNewGameEnd);

        secondsOfGameTime = 60 * Minutes;


        if (!GameObject.Find("MainCanvas"))
        {
            Debug.LogError("Could not find UI Prefab named MainCanvas");
        }

        timerTextObj = GameObject.Find("Timer");
        timer = timerTextObj?.GetComponent<TMP_Text>();

        if (!timer)
        {
            Debug.LogError("Could not find a TMP_Text component on a GameObject named Timer. Are you missing the UI?");
        }

        if (!GameObject.Find("DungeonGenerator"))
        {
            Debug.LogError("Could not find Dungeon Generator. Are you missing it in the scene?");
        }

        GameObject EvtCtrl = GameObject.Find("EventController");

        if (!EvtCtrl)
        {
            Debug.LogError("Could not find EventController. Are you missing it in the scene?");
        }
        else
        {
            EvtCtrl.transform.position = new Vector3((LabyrinthSize / 2) * DistanceApart, 90, (LabyrinthSize / 2) * DistanceApart);
        }        
    }

    private void Start()
    {
        PlayerInputManager.instance?.playerJoinedEvent.AddListener(InputManagerPlayerJoinedEvent);
    }

    private void FixedUpdate()
    {
        TickState();

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)) //Activate Debug KeyCommands
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnStateEnter(GameState.Startup_New_Game);
            }
        }
    }

    private void OnStateEnter(GameState nextState)
    {
        //Exit the current state, then call the enter on the nextState.
        OnStateExit();
        Instance._state = nextState;
        Debug.Log(Instance._state);
        switch (Instance._state)
        {
            case GameState.GameOver:
                GameOverBegin.Invoke();
                Timer = 0;
                break;
            case GameState.Labyrinth_Explore:

                Debug.Log("Entering Labyrinth_Explore");

                LabyrinthExploreBegin?.Invoke();

                timeUntilNextMicroEvent = Random.Range(MinSecondsBetweenMicroEvents, MaxSecondsBetweenMicroEvents);
                timeUntilNextSideEvent = Random.Range(MinSecondsBetweenSideEvents, MaxSecondsBetweenSideEvents);

                Debug.Log("Time until next Micro Event: " + timeUntilNextMicroEvent + " s");
                Debug.Log("Time until next Side Event: " + timeUntilNextSideEvent + " s");
                break;
            case GameState.Lobby:
                LobbyBegin.Invoke();
                timerTextObj?.SetActive(false);
                break;
            case GameState.Paused:
                PausedBegin.Invoke();
                break;
            case GameState.Showdown:
                ShowdownBegin.Invoke();
                MajorEventEnd.Invoke();
                Debug.Log("Entering Showdown");
                break;
            case GameState.SideEvent:
                SideEventBegin?.Invoke();
                break;
            case GameState.EndScreen:
                EndScreenBegin.Invoke();
                gameInProgress = false;
                break;
            case GameState.Startup_New_Game:
                StartupNewGameBegin.Invoke();
                PlayerInputManager.instance.splitScreen = true;
                MajorEventBegin.Invoke();
                break;
            default:
                break;
        }
    }

    private void TickState()
    {
        switch (Instance._state)
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
            case GameState.Startup_New_Game:
                OnStateEnter(GameState.Labyrinth_Explore);
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (Instance._state)
        {
            case GameState.GameOver:
                GameOverEnd.Invoke();
                break;
            case GameState.Labyrinth_Explore:
                LabyrinthExploreEnd.Invoke();
                break;
            case GameState.Lobby:
                LobbyEnd.Invoke();
                break;
            case GameState.Paused:
                PausedEnd.Invoke();
                break;
            case GameState.Showdown:
                ShowdownEnd.Invoke();
                break;
            case GameState.SideEvent:
                SideEventEnd?.Invoke();
                break;
            case GameState.EndScreen:
                EndScreenEnd.Invoke();
                break;
            case GameState.Startup_New_Game:
                StartupNewGameEnd.Invoke();
                break;
            default:
                break;
        }
    }


    #region Event Calls

    void TestLabyrinthBegin() { Debug.Log("LabyrinthBegin"); }

    void TestLabyrinthEnd() { Debug.Log("Labyrinth Ends"); }

    void TestShowdownBegin() { Debug.Log("Showdown Begins"); }

    void TestShowdownEnd() { Debug.Log("Showdown Ends"); }

    void TestLobbyBegin() { Debug.Log("Lobby Begins"); }
    void TestLobbyEnd() { Debug.Log("Lobby Ends"); }

    void TestPausedBegin() { Debug.Log("Paused Begins"); }
    void TestPausedEnd() { Debug.Log("Paused Ends"); }

    void TestGameOverBegin() { Debug.Log("GameOver Begins"); }
    void TestGameOverEnd() { Debug.Log("GameOver Ends"); }

    void TestEndScreenBegin() { Debug.Log("EndScreen Begins"); }
    void TestEndScreenEnd() { Debug.Log("EndScreen Ends"); }

    void TestStartupNewGameBegin() { Debug.Log("StartupNewGame Begins"); }

    void TestStartupNewGameEnd() { Debug.Log("StartupNewGame Ends"); }

    void TestBeginMajorEvent() { Debug.Log("MajorEvent Begins"); }

    void TestEndMajorEvent() { Debug.Log("MajorEvent Ends"); }

    void BeginSideEvent()
    {
        Debug.Log("Beginning SideEvent as " + Timer + " game seconds.");
        Debug.Log("Side Event Duration: " + SecondsDurationSideEvent + " s");
        timeLeftInSideEvent = SecondsDurationSideEvent;
    }

    void EndSideEvent() { Debug.Log("Ending Side event at " + Timer + " game seconds."); }

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

    private void InputManagerPlayerJoinedEvent(PlayerInput newPlayer)
    {
        Debug.Log("New Player Joined");
        players.Add(newPlayer.gameObject);

        GameObject newUI = Instantiate(lobbyUI, GameObject.Find("Player" + newPlayer.playerIndex).transform);
        newUI.name = "Player" + newPlayer.playerIndex + "Canvas";

        newPlayer.GetComponent<PlayerInput>().uiInputModule = newUI.transform.GetChild(0).GetComponent<InputSystemUIInputModule>();

        PlayerJoined.Invoke();
        //newPlayer.gameObject.SetActive(false);
        
    }

    public void ContinueInput(InputAction.CallbackContext ctx)
    {
        if (Instance._state == GameState.Lobby)
        {
            OnStateEnter(GameState.Startup_New_Game);
        }
    }
    #endregion
    #endregion
}
