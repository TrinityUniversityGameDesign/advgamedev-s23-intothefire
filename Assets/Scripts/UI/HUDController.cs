using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject itemIconPrefab;
    [SerializeField] private GameObject itemRowPrefab;
    [SerializeField] private GameObject statPrefab;
    private ImageController _imageController;
    private HealthBar _healthBar;
    private QuickView _quickView;
    private Stats _stats;
    private Inventory _inventory;
    private Minimap _minimap;
    private InventoryContainer _inventoryContainer;
    
    private Canvas _canvas;

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
}
