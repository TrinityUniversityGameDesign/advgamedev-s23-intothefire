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
    static Color player1color = new Color(0.4f, 0.8f, 0.9f, 1); //Cyan
    static Color player2color = new Color(0.13f, 0.53f, 0.2f, 1); //Green
    static Color player3color = new Color(0.8f, 0.73f, 0.26f, 1); //Yellow
    static Color player4color = new Color(0.6f,  0.2f,  0.46f, 1); //Purple
    public static Color[] colors = new Color[4] { player1color, player2color, player3color, player4color };

    List<GameObject> playableMeshes = new List<GameObject>();
    List<GameObject> pickableWeapons = new List<GameObject>();

    Dictionary<int, int> playerMeshes = new Dictionary<int, int>() { {0, 0}, { 1, 0 }, { 2, 0 }, { 3, 0 } };

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
    public UnityEvent MicroEventBegin;
    [Tooltip("Event used to call when a Micro Event is ending")]
    public UnityEvent MicroEventEnd;

    [Tooltip("Event used to call when a Side Event is starting")]
    public UnityEvent SideEventBegin;
    [Tooltip("Event used to cal when a Side Event is Starting")]
    public UnityEvent SideEventEnd;

    [Tooltip("Event used to indicate when the Labyrinth is beginning to be explored")]
    public UnityEvent LabyrinthExploreBegin;
    [Tooltip("Event used to indicate when the Labyrinth explore phase is being exited")]
    public UnityEvent LabyrinthExploreEnd;

    [Tooltip("Event called when the Showdown begins")]
    public UnityEvent ShowdownBegin;
    [Tooltip("Event call when the Showdown ends")]
    public UnityEvent ShowdownEnd;

    [Tooltip("Event called when the lobby begins")]
    public UnityEvent LobbyBegin;
    [Tooltip("Event called when the lobby ends")]
    public UnityEvent LobbyEnd;

    [Tooltip("Event called when the game pauses")]
    public UnityEvent PausedBegin;
    [Tooltip("Event called when the game unpauses")]
    public UnityEvent PausedEnd;

    [Tooltip("Event called when the Game over begins")]
    public UnityEvent GameOverBegin;
    [Tooltip("Event called when the Game over ends")]
    public UnityEvent GameOverEnd;

    [Tooltip("Event called when the end screen sequence begins")]
    public UnityEvent EndScreenBegin;
    [Tooltip("Event called when the end screen sequence ends")]
    public UnityEvent EndScreenEnd;

    [Tooltip("Event called when the startup screen begins")]
    public UnityEvent StartupNewGameBegin;
    [Tooltip("Event called when the startup screen ends")]
    public UnityEvent StartupNewGameEnd;

    [Tooltip("Event called when a Major event begins")]
    public UnityEvent MajorEventBegin;
    [Tooltip("Event called when the Major event ends")]
    public UnityEvent MajorEventEnd;

    [Tooltip("Event called when a player joined.")]
    public UnityEvent PlayerJoined;
    #endregion

    #region Private Methods
    private void Awake()
    {
        Instance = this;

        Instance.MicroEventBegin.AddListener(BeginMicroEvent);
        Instance.MicroEventEnd.AddListener(EndMicroEvent);

        Instance.SideEventBegin.AddListener(BeginSideEvent);
        Instance.SideEventEnd.AddListener(EndSideEvent);

        Instance.MajorEventBegin.AddListener(TestBeginMajorEvent);
        Instance.MajorEventEnd.AddListener(TestEndMajorEvent);

        Instance.LabyrinthExploreBegin.AddListener(TestLabyrinthBegin);
        Instance.LabyrinthExploreEnd.AddListener(TestLabyrinthEnd);

        Instance.ShowdownBegin.AddListener(TestShowdownBegin);
        Instance.ShowdownEnd.AddListener(TestShowdownEnd);

        Instance.LobbyBegin.AddListener(TestLobbyBegin);
        Instance.LobbyEnd.AddListener(TestLobbyEnd);

        Instance.PausedBegin.AddListener(TestPausedBegin);
        Instance.PausedEnd.AddListener(TestPausedEnd);

        Instance.GameOverBegin.AddListener(TestGameOverBegin);
        Instance.GameOverEnd.AddListener(TestGameOverEnd);

        Instance.EndScreenBegin.AddListener(TestEndScreenBegin);
        Instance.EndScreenEnd.AddListener(TestEndScreenEnd);

        Instance.StartupNewGameBegin.AddListener(TestStartupNewGameBegin);
        Instance.StartupNewGameEnd.AddListener(TestStartupNewGameEnd);

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
        Instance.OnStateEnter(GameState.Lobby);
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
        Instance.OnStateExit();
        Instance._state = nextState;
        switch (Instance._state)
        {
            case GameState.GameOver:
                Instance.GameOverBegin.Invoke();
                Timer = 0;
                break;
            case GameState.Labyrinth_Explore:
                Instance.LabyrinthExploreBegin.Invoke();
                timeUntilNextMicroEvent = Random.Range(MinSecondsBetweenMicroEvents, MaxSecondsBetweenMicroEvents);
                timeUntilNextSideEvent = Random.Range(MinSecondsBetweenSideEvents, MaxSecondsBetweenSideEvents);

                //Debug.Log("Time until next Micro Event: " + timeUntilNextMicroEvent + " s");
                //Debug.Log("Time until next Side Event: " + timeUntilNextSideEvent + " s");
                break;
            case GameState.Lobby:
                Instance.LobbyBegin.Invoke();
                break;
            case GameState.Paused:
                Instance.PausedBegin.Invoke();
                break;
            case GameState.Showdown:
                Instance.ShowdownBegin.Invoke();
                Instance.MajorEventEnd.Invoke();
                break;
            case GameState.SideEvent:
                Instance.SideEventBegin.Invoke();
                break;
            case GameState.EndScreen:
                Instance.EndScreenBegin.Invoke();
                gameInProgress = false;
                break;
            case GameState.Startup_New_Game:
                Instance.StartupNewGameBegin.Invoke();
                PlayerInputManager.instance.splitScreen = true;
                Instance.MajorEventBegin.Invoke();
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
                    Instance.MicroEventBegin?.Invoke();
                }

                if(timeUntilNextSideEvent <= 0)
                {
                    Instance.MicroEventEnd?.Invoke();
                    OnStateEnter(GameState.SideEvent);
                    break;
                }

                if(microEventInProgress && timeLeftInMicroEvent <= 0)
                {
                    Instance.MicroEventEnd?.Invoke();
                }

                if(Timer >= Minutes * 60)
                {
                    Instance.OnStateEnter(GameState.Showdown);
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
                    Instance.OnStateEnter(GameState.Labyrinth_Explore);
                }
                break;
            case GameState.EndScreen:
                break;
            case GameState.Startup_New_Game:
                Instance.OnStateEnter(GameState.Labyrinth_Explore);
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
                Instance.GameOverEnd.Invoke();
                break;
            case GameState.Labyrinth_Explore:
                Instance.LabyrinthExploreEnd.Invoke();
                break;
            case GameState.Lobby:
                Instance.LobbyEnd.Invoke();
                break;
            case GameState.Paused:
                Instance.PausedEnd.Invoke();
                break;
            case GameState.Showdown:
                Instance.ShowdownEnd.Invoke();
                break;
            case GameState.SideEvent:
                Instance.SideEventEnd?.Invoke();
                break;
            case GameState.EndScreen:
                Instance.EndScreenEnd.Invoke();
                break;
            case GameState.Startup_New_Game:
                Instance.StartupNewGameEnd.Invoke();
                break;
            default:
                break;
        }
    }

    #region Event Calls

    void TestLabyrinthBegin() { Debug.Log("Labyrinth Begin"); }

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
        //Debug.Log("New Player Joined");
        newPlayer.gameObject.name = ("Player" + newPlayer.playerIndex);
        players.Add(newPlayer.gameObject);

        GameObject newUI = Instantiate(lobbyUI, GameObject.Find("Player" + newPlayer.playerIndex + "UI").transform);
        newUI.name = "Player" + newPlayer.playerIndex + "Canvas";
        newUI.transform.GetChild(1).GetComponent<Outline>().effectColor = colors[newPlayer.playerIndex];
        newUI.GetComponent<RotatingSelectScript>().playerIndex = newPlayer.playerIndex;

        newPlayer.GetComponent<PlayerInput>().uiInputModule = newUI.transform.GetChild(0).GetComponent<InputSystemUIInputModule>();

        Instance.PlayerJoined.Invoke();
        //newPlayer.gameObject.SetActive(false);
    }

    public int ChangePlayerMesh(int playerIndex)
    {
        int currentPlayerMesh = playerMeshes[playerIndex];
        if(currentPlayerMesh < playerMeshes.Count - 1)
        {
            playerMeshes[playerIndex]++;
        } else if(currentPlayerMesh == playerMeshes.Count - 1)
        {
            playerMeshes[playerIndex] = 0;
        }

        Debug.Log(playerMeshes[playerIndex]);
        return 1;
    }

    public int ChangeWeaponMesh(int playerIndex, int currentPosition)
    {
        return 1;
    }

    public void ContinueInput(InputAction.CallbackContext ctx)
    {
        if (Instance._state == GameState.Lobby)
        {
            Instance.OnStateEnter(GameState.Startup_New_Game);
        }
    }
    #endregion
    #endregion
}
