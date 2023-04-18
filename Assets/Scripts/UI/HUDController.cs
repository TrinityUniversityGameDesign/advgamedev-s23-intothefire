using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private ImageController _imageController;
    private HealthBar _healthBar;

    private Canvas _canvas;
    // Start is called before the first frame update
    void Start()
    {
        _imageController = GetComponentInChildren<ImageController>();
        _healthBar = GetComponentInChildren<HealthBar>();
        _canvas = GetComponent<Canvas>();
        
        // Keep the HUD off by default, until it's instantiated
        Debug.Log("Starting the hud controller, disabling the canvas");
        _canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiatePlayerHUD(Sprite playerIcon, float health, float maxHealth)
    {
        // Enable the canvas
        _canvas.enabled = true;
        _healthBar.InitializeHealthBar(health, maxHealth);
        _imageController.SetRawImageToSprite(playerIcon);
        _healthBar.UpdateMaxHealth(150);
        _healthBar.UpdateHealth(80);
    }
}
