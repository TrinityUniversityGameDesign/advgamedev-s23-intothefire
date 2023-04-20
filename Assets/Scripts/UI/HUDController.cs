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

        // Keep the HUD off by default, until it's instantiated
        Debug.Log("Starting the hud controller, disabling the canvas");
        _canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializePlayerHUD(Sprite playerIcon, float health, float maxHealth, List<Item> inventory,
        List<(string, float)> stats)
    {
        // Enable the canvas
        _canvas.enabled = true;
        _healthBar.InitializeHealthBar(health, maxHealth);
        _imageController.SetRawImageToSprite(playerIcon);
        _healthBar.UpdateMaxHealth(150);
        _healthBar.UpdateHealth(80);
        _quickView.InitializeItems(inventory, itemIconPrefab);
        _inventory.InitializeItems(inventory, itemRowPrefab);
        _stats.InitializeStats(stats, statPrefab);
    }
}
