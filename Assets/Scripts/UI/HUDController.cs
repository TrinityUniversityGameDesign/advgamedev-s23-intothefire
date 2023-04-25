using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
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

    private Image _eventNotification;
    private TMP_Text _eventTitle;
    private TMP_Text _eventInstructions;
    
    private Canvas _canvas;
    
    private void Awake()
    {
        GameManager.Instance?.StartupNewGameBegin.AddListener(NotifyNewGame);
        GameManager.Instance?.MicroEventBegin.AddListener(NotifyMicroEvent);
        GameManager.Instance?.SideEventBegin.AddListener(NotifySideEvent);
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
    /// <param name="health">heatlh status of player</param>
    /// <param name="maxHealth">max health status of player</param>
    /// <param name="inventory">list of inventory items</param>
    /// <param name="stats">list of player stats</param>
    public void InitializePlayerHUD(Sprite playerIcon, float health, float maxHealth, List<Item> inventory,
        List<(string, float)> stats)
    {
        // Enable the canvas
        Debug.Log("Loading HUD");
        _canvas.enabled = true;
        Debug.Log("Canvas done");
        _healthBar.InitializeHealthBar(health, maxHealth);
        Debug.Log("Healthbar done");
        _imageController.SetRawImageToSprite(playerIcon);
        Debug.Log("Icon done");
        _healthBar.UpdateMaxHealth(150);
        Debug.Log("Max health update done");
        _healthBar.UpdateHealth(80);
        Debug.Log("Health update done");
        _quickView.InitializeItems(inventory, itemIconPrefab);
        Debug.Log("Quickview items done");
        _inventory.InitializeItems(inventory, itemRowPrefab);
        Debug.Log("Inventory items done");
        _stats.InitializeStats(stats, statPrefab);
        Debug.Log("Stats done");
        ToggleInventory();
        Debug.Log("Closing inventory done");
    }
    
    public void InitializePlayerHUD(Sprite playerIcon, float health, float maxHealth, List<ItemData> inventory,
        PlayerStats stats)
    {
        // Enable the canvas
        Debug.Log("Loading HUD");
        _canvas.enabled = true;
        Debug.Log("Canvas done");
        _healthBar.InitializeHealthBar(health, maxHealth);
        Debug.Log("Healthbar done");
        _imageController.SetRawImageToSprite(playerIcon);
        Debug.Log("Icon done");
        _healthBar.UpdateMaxHealth(150);
        Debug.Log("Max health update done");
        _healthBar.UpdateHealth(80);
        Debug.Log("Health update done");
        _quickView.InitializeItems(inventory, itemIconPrefab);
        Debug.Log("Quickview items done");
        _inventory.InitializeItems(inventory, itemRowPrefab);
        Debug.Log("Inventory items done");
        _stats.InitializeStats(stats, statPrefab);
        Debug.Log("Stats done");
        ToggleInventory();
        Debug.Log("Closing inventory done");
        InitializeNotifications();
    }

    private void InitializeNotifications()
    {
        _eventNotification.gameObject.SetActive(false);
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
        _eventTitle.text = "A new fire burns...";
        _eventInstructions.text = "Navigate the labyrinth, conquer rooms, and earn items";
        StartCoroutine(DeliverNotification(notifDuration));
    }

    private void NotifySideEvent()
    {
        _eventTitle.text = "Spleef";
        _eventInstructions.text = "Shovel your way to victory but don't fall through";
        StartCoroutine(DeliverNotification(notifDuration));
    }

    private void NotifyMicroEvent()
    {
        _eventTitle.text = "Meteor shower";
        _eventInstructions.text = "Dodge meteors";
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
        yield return StartCoroutine(FadeInText(duration / 2));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeOutText(duration / 2));
    }
}
