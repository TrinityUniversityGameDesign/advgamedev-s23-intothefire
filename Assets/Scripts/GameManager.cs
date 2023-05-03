using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public enum GameEvents
{
    Meteor,
    Spleef,
    Miniboss,
    Explore,
    Showdown
}

// Maybe use this if you want to pass messages to players. You can also use the notif system for rooms,
// though figuring out how to send that through from the rooms to the players may be tricky
public static class GameManagerGlobalStatics
{
    public struct GameEvent
    {
        public readonly string Title;
        public readonly string Text;

        public GameEvent(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
    public static readonly Dictionary<GameEvents, GameEvent> Events = new Dictionary<GameEvents, GameEvent>
    {
        { GameEvents.Meteor, new GameEvent("Meteors!", "Avoid the rocks falling from the sky.") },
        { GameEvents.Miniboss, new GameEvent("Rampant Mummy", "Damage the mummy!") },
        { GameEvents.Spleef, new GameEvent("Spleef", "Keep moving! Avoid falling through the holes in the floor! Knock your enemies through them!") },
        { GameEvents.Explore, new GameEvent("Out of the Frying Pan", "Explore the labyrinth, collect rewards. Beware the Minotaur") },
        { GameEvents.Showdown, new GameEvent("Into the Fire", "Destroy your opponents, whatever means necessary.") }
    };
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

    public List<GameObject> playableCharacters;
    public List<GameObject> playableWeapons;

    Dictionary<int, int> playerCharacters = new Dictionary<int, int>() { {0, 0}, { 1, 0 }, { 2, 0 }, { 3, 0 } };
    public List<CharacterData> characters;
    Dictionary<int, int> playerWeapons = new Dictionary<int, int>() { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } };
    public List<WeaponData> weapons;
    public int LastJoinedPlayer = 0;

    [SerializeField]
    private int minPlayerCount = 1;

    public GameObject lobbyUI;
    
    public GameEvents CurrentEvent;
    #endregion

    #region Private Fields
    [Tooltip("Internal state the game manager is currently in.")]
    private GameState _state = GameState.Lobby;

    [HideInInspector]
    [Tooltip("Represents the number of seconds since the game has started.")]
    public float Timer = 0;
    [HideInInspector]
    public float secondsOfGameTime = 0;

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
    [HideInInspector]
    public float timeLeftInSideEvent;

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

    [Tooltip("Event to toggle player invincibility status on")]
    public UnityEvent EnablePlayerInvincibility;
    [Tooltip("Event to toggle player invincibility status off")]
    public UnityEvent DisablePlayerInvincibility;

    [Tooltip("Event called when a player joined.")]
    public UnityEvent PlayerJoined;

    [Tooltip("Event called when the dungeon generation is complete")]
    public UnityEvent DungeonGenerationComplete;
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

        Instance.DungeonGenerationComplete.AddListener(TeleportPlayersToSpawnPoints);

        secondsOfGameTime = 60 * Minutes;


        if (!GameObject.Find("MainCanvas"))
        {
            //Debug.LogError("Could not find UI Prefab named MainCanvas");
        }

        if (!GameObject.Find("DungeonGenerator"))
        {
            //Debug.LogError("Could not find Dungeon Generator. Are you missing it in the scene?");
        }

        GameObject EvtCtrl = GameObject.Find("EventController");

        if (!EvtCtrl)
        {
            //Debug.LogError("Could not find EventController. Are you missing it in the scene?");
        }
        else
        {
            EvtCtrl.transform.position = new Vector3((LabyrinthSize / 2) * DistanceApart, 90, (LabyrinthSize / 2) * DistanceApart);
        }
        //characters = Resources.LoadAll<CharacterData>("Characters").ToList();
        //weapons = Resources.LoadAll<WeaponData>("Weapons").ToList();
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

            if (Input.GetKeyDown(KeyCode.L))
            {
                AwardRandomItem(0);
                //Instance.SideEventBegin.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.L))
            {
                //Instance.SideEventEnd.Invoke();
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

    void TestLabyrinthBegin() { }//Debug.Log("Labyrinth Begin"); }

    void TestLabyrinthEnd() { }//Debug.Log("Labyrinth Ends"); }

    void TestShowdownBegin() { }//Debug.Log("Showdown Begins"); }

    void TestShowdownEnd() { }//Debug.Log("Showdown Ends"); }

    void TestLobbyBegin() { PlayerInputManager.instance.EnableJoining(); }//Debug.Log("Lobby Begins"); }
    void TestLobbyEnd() {
        //Debug.Log("Lobby Ends");

        PlayerInputManager.instance.DisableJoining();

    }

    void TestPausedBegin() { } //Debug.Log("Paused Begins"); }
    void TestPausedEnd() { } //Debug.Log("Paused Ends"); }

    void TestGameOverBegin() { } //Debug.Log("GameOver Begins"); }
    void TestGameOverEnd() { } //Debug.Log("GameOver Ends"); }

    void TestEndScreenBegin() { } // Debug.Log("EndScreen Begins"); }
    void TestEndScreenEnd() { } //Debug.Log("EndScreen Ends"); }

    void TestStartupNewGameBegin() { 
        foreach(KeyValuePair<int, int> pair in Instance.playerCharacters)
        {
            if (pair.Key < Instance.players.Count)
            {
                Instantiate(Instance.characters[pair.Value].gameObject, Instance.players[pair.Key].transform);
                Outline temp = Instance.players[pair.Key].gameObject.AddComponent<Outline>();
                temp.OutlineColor = colors[pair.Key];
                temp.OutlineWidth = 8;
            }
        }

        foreach(KeyValuePair<int, int> pair in Instance.playerWeapons)
        {
            if(pair.Key < Instance.players.Count)
            {
                //Debug.Log("Generating a: " + weapons[pair.Value]);
                Instance.players[pair.Key].gameObject.GetComponent<JacksonCharacterMovement>().AssignWeapon(weapons[pair.Value].Create());
                return;
                //player.gameObject.GetComponent<JacksonCharacterMovement>().WeaponAssignFunction;
                //Call the weapon function in the player @Jackson TODO
                if(pair.Value == 0)
                {
                    Instance.players[pair.Key].gameObject.GetComponent<JacksonCharacterMovement>().AssignWeapon(new Hammer());
                }
                else if(pair.Value == 1)
                {
                    Instance.players[pair.Key].gameObject.GetComponent<JacksonCharacterMovement>().AssignWeapon(new Swords());
                }
                else if (pair.Value == 2)
                {
                    Instance.players[pair.Key].gameObject.GetComponent<JacksonCharacterMovement>().AssignWeapon(new FryingPan());
                }
                else if(pair.Value == 3)
                {
                    Instance.players[pair.Key].gameObject.GetComponent<JacksonCharacterMovement>().AssignWeapon(new Scythe());
                }
                
            }
        }
    }
    void TestStartupNewGameEnd() { } //Debug.Log("StartupNewGame Ends"); }

    void TestBeginMajorEvent() { } //Debug.Log("MajorEvent Begins"); }

    void TestEndMajorEvent() { } //Debug.Log("MajorEvent Ends"); }

    void BeginSideEvent()
    {
        //Debug.Log("Beginning SideEvent as " + Timer + " game seconds.");
        //Debug.Log("Side Event Duration: " + SecondsDurationSideEvent + " s");
        timeLeftInSideEvent = SecondsDurationSideEvent;
    }

    void EndSideEvent() { } //Debug.Log("Ending Side event at " + Timer + " game seconds."); }

    void BeginMicroEvent()
    {
        microEventInProgress = true;
        //Debug.Log("Beginning Microevent at " + Timer + " game seconds.");

        timeLeftInMicroEvent = Random.Range(MinSecondsDurationMicroEvents, MaxSecondsDurationMicroEvents);
        //Debug.Log("Microevent Duration: " + timeLeftInMicroEvent + " s");
    }

    void EndMicroEvent()
    {
        microEventInProgress = false;
        //Debug.Log("Ending Microevent at " + Timer + " game seconds.");

        timeUntilNextMicroEvent = Random.Range(MinSecondsBetweenMicroEvents, MaxSecondsBetweenMicroEvents);
        //Debug.Log("Time until next Micro Event: " + timeUntilNextMicroEvent + " s");
    }

    private void InputManagerPlayerJoinedEvent(PlayerInput newPlayer)
    {
        //Debug.Log("New Player Joined");
        Instance.LastJoinedPlayer = newPlayer.playerIndex;
        newPlayer.gameObject.name = ("Player" + newPlayer.playerIndex);
        newPlayer.gameObject.GetComponent<PlayerData>().PlayerIndex = newPlayer.playerIndex;
        newPlayer.gameObject.GetComponent<PlayerData>().playerColor = colors[newPlayer.playerIndex];
        Instance.players.Add(newPlayer.gameObject);

        GameObject newUI = Instantiate(lobbyUI, GameObject.Find("PlayerLobby" + newPlayer.playerIndex + "UI").transform);
        newUI.name = "Player" + newPlayer.playerIndex + "Canvas";
        newUI.transform.GetChild(1).GetComponent<UnityEngine.UI.Outline>().effectColor = colors[newPlayer.playerIndex];

        newPlayer.GetComponent<PlayerInput>().uiInputModule = newUI.transform.GetComponentInChildren<InputSystemUIInputModule>();

        Instance.PlayerJoined.Invoke();
    }

    public void UpdatePlayerWeapon(int player, int index)
    {
        Instance.playerWeapons[player] = index;
    }

    public void UpdatePlayerCharacter(int player, int index)
    {
        Instance.playerCharacters[player] = index;
    }

    public void ContinueInput(InputAction.CallbackContext ctx)
    {
        if (Instance._state == GameState.Lobby && Instance.players.Count >= Instance.minPlayerCount)
        {
            Instance.OnStateEnter(GameState.Startup_New_Game);
        }
    }

    public void AwardRandomItem(int victor)
    {
        Item newItem = Item.GrantNewRandomItem();
        if(newItem != null && !(victor < 0 || victor > Instance.players.Count-1)) players[victor].GetComponent<JacksonCharacterMovement>().AddItem(newItem);
    }

    void TeleportPlayersToSpawnPoints()
    {
        for(int i = 0; i < Instance.players.Count; i++)
        {
            players[i].transform.position = GameObject.Find("Spawn" + i).transform.position;
        }
    }
    
    public void TeleportPlayerToSpawn(GameObject playerToTeleport)
    {
        for(int i = 0; i < Instance.players.Count; i++) { 
            if(Instance.players[i] == playerToTeleport)
            {
                playerToTeleport.transform.position = GameObject.Find("Spawn" + i).transform.position;
            }
        }
    }

    #endregion
    #endregion
}
