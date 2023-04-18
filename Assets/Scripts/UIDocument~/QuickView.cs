using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.UIElements;

public class QuickView : MonoBehaviour
{
    [SerializeField] private Gradient _healthBarGradient;
    [SerializeField] private float _healthDrainTime = 0.25f;
    private JacksonCharacterMovement _player;
    private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _inventoryContainer;
    private ProgressBar _progressBar;
    private Coroutine _drainHealthBarCoroutine;
    private Color _newHealthColor;
    
    // List of items from inventory
    private List<Item> _items;

    // Open by default. Will eventually open/toggle on key press
    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        _inventoryContainer = _root.Q<VisualElement>("Quick-View");
        _progressBar = _root.Q<ProgressBar>("Health-Bar");

        // Retrieve the player from the parent
        _player = transform.parent.GetComponentInChildren<JacksonCharacterMovement>();
        
        // Create the gradient since script setting doesn't work
        GradientColorKey[] gck = new GradientColorKey[3];
        GradientAlphaKey[] gak = new GradientAlphaKey[3];
        gck[0].color = new Color(0.4811f, 0f, 0f);
        gck[0].time = 0f;
        gak[0].alpha = 1f;
        gak[0].time = 0f;
        gck[1].color = new Color(1f, 0.7180f, 0f);
        gck[1].time = 0.5f;
        gak[1].alpha = 1f;
        gak[1].time = 0.5f;
        gck[2].color = new Color(0.0248f, 0.5049f, 0.0556f);
        gck[2].time = 1.0f;
        gak[2].alpha = 1f;
        gak[2].time = 1f;
        _healthBarGradient.SetKeys(gck, gak);
        ToggleUI();
    }
    // Called in Player to load the elements in after the player loads because multiplayer
    public void LoadUI()
    {
        // Set the player icon
        _root.Q<VisualElement>("Player-Icon").style.backgroundImage = new StyleBackground(_player.Icon);
        // Set the minimap
        // _root.Q<VisualElement>("Minimap").style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>("Textures/Minimap Render Texture"));
        // Set the player healthbar
        SetHealthColor(_healthBarGradient.Evaluate(_player.GetHealth() / _player.GetMaxHealth()));
        _progressBar.value = _player.GetHealth();
        _progressBar.highValue = _player.GetMaxHealth();
        _progressBar.title = $"{_player.GetHealth()}/{_player.GetMaxHealth()}";
    }
    // Call when inventory items or stats change
    public void UpdateUI()
    {
        _items = _player.GetInventory();
        FillInventoryList();
    }

    public void ToggleUI()
    {
        _uiDocument.rootVisualElement.visible = !_uiDocument.rootVisualElement.visible;
    }

    // Fill the inventory with items
    void FillInventoryList()
    {
        _inventoryContainer.Clear();
        for (int i = 0; i < _items.Count; i++)
        {
            Sprite icon = _items[i].icon;
            VisualElement itemCell = new VisualElement();
            itemCell.AddToClassList("item-icon");
            itemCell.style.backgroundImage = new StyleBackground(icon);
            _inventoryContainer.Add(itemCell);
        }
    }

    public void UpdateHealth()
    {
        // Set player health
        
        StartCoroutine(DrainHealthBar());
        //CheckHealthBarGradientAmount();
    }

    public void UpdateMaxHealth()
    {
        float maxHealthValue = _player.GetMaxHealth();
        _progressBar.highValue = maxHealthValue;
    }

    private IEnumerator DrainHealthBar()
    {
        float newHealth = _player.GetHealth();
        float currentHealth = _progressBar.value;
        Color currentColor = _root.Q(className: "unity-progress-bar__progress").style.backgroundColor.value;
        
        float elapsedTime = 0f;
        while (elapsedTime < _healthDrainTime)
        {
            elapsedTime += Time.deltaTime;

            float drainRatio = elapsedTime / _healthDrainTime;
            float updatedHealth = Mathf.Lerp(currentHealth, newHealth, drainRatio);
            _progressBar.value = updatedHealth;
            Debug.Log(_progressBar.value);
            _progressBar.title = $"{Mathf.Round(updatedHealth)}/{_player.GetMaxHealth()}";
            SetHealthColor(_healthBarGradient.Evaluate(_progressBar.value/_progressBar.highValue));
            
            yield return null;
        }
        // _progressBar.title = $"{_player.GetHealth()}/{_player.GetMaxHealth()}";
    }

    private void CheckHealthBarGradientAmount()
    {
        _newHealthColor = _healthBarGradient.Evaluate(_player.GetHealth() / _player.GetMaxHealth());
    }

    private void SetHealthColor(Color color)
    {
        _root.Q(className: "unity-progress-bar__progress").style.backgroundColor = new StyleColor(color);
    }
}
