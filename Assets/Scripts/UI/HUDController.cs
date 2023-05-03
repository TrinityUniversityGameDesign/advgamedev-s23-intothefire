using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public bool showdown = false;
    [SerializeField] private GameObject itemIconPrefab;
    [SerializeField] private GameObject itemRowPrefab;
    [SerializeField] private GameObject statPrefab;
    [SerializeField] private float notifDuration = 2f;
    private float currentAlpha = 0f;
    private ImageController _imageController;
    private HealthBar _healthBar;
    private QuickView _quickView;
    private Stats _stats;
    private Inventory _inventory;
    private Minimap _minimap;
    private InventoryContainer _inventoryContainer;
    private TextController _timer;
    

    private Image _eventNotification;
    private TMP_Text _eventTitle;
    private TMP_Text _eventInstructions;
    
    private Canvas _canvas;
    
    private void Awake()
    {
        GameManager.Instance?.StartupNewGameBegin.AddListener(NotifyNewGame);
        GameManager.Instance?.MicroEventBegin.AddListener(NotifyMicroEvent);
        GameManager.Instance?.SideEventBegin.AddListener(NotifySideEvent);
        GameManager.Instance?.ShowdownBegin.AddListener(NotifyShowdown);
    }

    // Start is called before the first frame update
    void Start()
    {
        _imageController = GetComponentInChildren<ImageController>();
        _healthBar = GetComponentInChildren<HealthBar>();
        _canvas = GetComponent<Canvas>();
        _quickView = GetComponentInChildren<QuickView>();
        _stats = GetComponentInChildren<Stats>();
        _inventory = GetComponentInChildren<Inventory>();
        _inventoryContainer = GetComponentInChildren<InventoryContainer>();
        _minimap = GetComponentInChildren<Minimap>();
        _timer = GetComponentInChildren<TextController>();

        _eventNotification = transform.Find("Notification").GetComponent<Image>();
        _eventTitle = _eventNotification.transform.Find("Event Title").GetComponent<TMP_Text>();
        _eventInstructions = _eventNotification.transform.Find("Event Instructions").GetComponent<TMP_Text>();

        // Keep the HUD off by default, until it's instantiated
        Debug.Log("Starting the hud controller, disabling the canvas");
        _canvas.enabled = false;
    }

    /// <summary>
    ///  Initialize the HUD at the start of the game
    /// </summary>
    /// <param name="playerIcon">sprite from player controller</param>
    /// /// <param name="color">color for player</param>
    /// <param name="health">heatlh status of player</param>
    /// <param name="maxHealth">max health status of player</param>
    /// <param name="inventory">list of inventory items</param>
    /// <param name="stats">list of player stats</param>
    public void InitializePlayerHUD(int index, Sprite playerIcon, Color color, float health, float maxHealth, List<Item> inventory, List<(string, float)> stats)
    {
        // Enable the canvas
        _canvas.enabled = true;
        _healthBar.InitializeHealthBar(health, maxHealth);
        _imageController.SetRawImageToSprite(playerIcon);
        _inventory.Initialize(inventory, itemRowPrefab);
        _quickView.Initialize(inventory, itemIconPrefab);
        _stats.InitializeStats(stats, statPrefab);
        _minimap.SetIndexTexture(index);
        foreach (var componentsInChild in transform.parent.GetComponentsInChildren<ColorController>())
        {
            componentsInChild.SetImageColor(color);
        }
        InitializeNotifications();
        ToggleInventory();
    }
    
    //public void InitializePlayerHUD(Sprite playerIcon, float health, float maxHealth, List<ItemData> inventory,
    //    PlayerStats stats)
    //{
    //    // Enable the canvas
    //    _canvas.enabled = true;
    //    _healthBar.InitializeHealthBar(health, maxHealth);
    //    _imageController.SetRawImageToSprite(playerIcon);
    //    _quickView.InitializeItems(inventory, itemIconPrefab);
    //    _inventory.InitializeItems(inventory, itemRowPrefab);
    //    _stats.InitializeStats(stats, statPrefab);
    //    ToggleInventory();
    //    InitializeNotifications();
    //}

    private void InitializeNotifications()
    {
        _eventNotification.gameObject.SetActive(false);
    }

    public void UpdateHealth(float health)
    {
        _healthBar.UpdateHealth(health);
    }

    public void UpdateMaxHealth(float health, float maxHealth)
    {
        _healthBar.UpdateMaxHealth(health, maxHealth);
    }

    public void AddItem(Item item)
    {
        _inventory.AddItem(item);
        _quickView.AddItem(item);
    }

    public void UpdateTimer()
    {
        int duration = (int)(GameManager.Instance.secondsOfGameTime - GameManager.Instance.Timer);
        int seconds = duration % 60;
        _timer.UpdateText($"{duration / 60}:{(seconds < 10 ? "0" + seconds : seconds)}"); 
    }

    /// <summary>
    ///  Toggle visibility of HUD; will also turn off the inventory view
    /// </summary>
    public void ToggleHud()
    {
        _canvas.enabled = !_canvas.enabled;
    }

    /// <summary>
    /// Toggle visibility of minimap
    /// </summary>
    public void ToggleMinimap()
    {
        _minimap.ToggleMinimap();
    }

    /// <summary>
    /// Toggle visibility of inventory window
    /// </summary>
    public void ToggleInventory()
    {
        _inventoryContainer.ToggleInventory();
    }

    private void NotifyNewGame()
    {
        GameManagerGlobalStatics.GameEvent gameEvent = GameManagerGlobalStatics.Events[GameEvents.Explore];
        _eventTitle.text = gameEvent.Title;
        _eventInstructions.text = gameEvent.Text;
        StartCoroutine(DeliverNotification(notifDuration));
    }
    
    private void NotifyShowdown()
    {
        GameManagerGlobalStatics.GameEvent gameEvent = GameManagerGlobalStatics.Events[GameEvents.Showdown];
        _eventTitle.text = gameEvent.Title;
        _eventInstructions.text = gameEvent.Text;
        StartCoroutine(DeliverNotification(notifDuration));
    }

    private void NotifySideEvent()
    {
        GameManagerGlobalStatics.GameEvent gameEvent = GameManagerGlobalStatics.Events[GameManager.Instance.CurrentEvent];
        _eventTitle.text = gameEvent.Title;
        _eventInstructions.text = gameEvent.Text;
        StartCoroutine(DeliverNotification(notifDuration));
    }

    private void NotifyMicroEvent()
    {
        GameManagerGlobalStatics.GameEvent gameEvent = GameManagerGlobalStatics.Events[GameManager.Instance.CurrentEvent];
        _eventTitle.text = gameEvent.Title;
        _eventInstructions.text = gameEvent.Text;
        StartCoroutine(DeliverNotification(notifDuration/2));
    }

    private void UpdateNotificationAlpha(float alpha)
    {
        currentAlpha = alpha;
        _eventNotification.color = new Color(_eventNotification.color.r, _eventNotification.color.g, _eventNotification.color.b, currentAlpha);
        _eventTitle.color = new Color(_eventTitle.color.r, _eventTitle.color.g, _eventTitle.color.b, currentAlpha);
        _eventInstructions.color = new Color(_eventInstructions.color.r, _eventInstructions.color.g, _eventInstructions.color.b, currentAlpha);
    }

    private IEnumerator FadeInText(float timeSpeed)
    {
        _eventNotification.gameObject.SetActive(true);
        UpdateNotificationAlpha(0);
        while (currentAlpha < 1.0f)
        {
            UpdateNotificationAlpha(currentAlpha + Time.deltaTime * timeSpeed);
            yield return null;
        }
    }
    private IEnumerator FadeOutText(float timeSpeed)
    {
        UpdateNotificationAlpha(1);
        while (currentAlpha > 0.0f)
        {
            UpdateNotificationAlpha(currentAlpha - Time.deltaTime * timeSpeed);
            yield return null;
        }
        _eventNotification.gameObject.SetActive(false);
    }

    private IEnumerator DeliverNotification (float duration = 2f) {
        yield return StartCoroutine(FadeInText(duration * 4));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeOutText(duration * 2));
    }
}
