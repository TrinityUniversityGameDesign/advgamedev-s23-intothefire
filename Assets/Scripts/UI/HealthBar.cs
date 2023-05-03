using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float barWidthMultiplier = 3f;
    private const float AnimationSpeed = 200f;

    private float _currentMaxHealth;
    private float _currentHealth;
    private float _targetHealth;
    private float _targetMaxHealth;
    private bool _initialized = false;
    private QueueManager _queueManager;
    
    private Slider _slider;
    private RectTransform _rectTransform;
    private TMP_Text _text;

    // Start is called before the first frame update
    void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TMP_Text>();
        _queueManager = gameObject.AddComponent<QueueManager>();
    }

    private void Update()
    {
        if (!_initialized) return;
        if (!Mathf.Approximately(_currentHealth, _targetHealth))
        {
            _currentHealth = Mathf.MoveTowards(_currentHealth, _targetHealth, 1 - Mathf.Pow(1 - AnimationSpeed * Time.deltaTime, 3));
            _slider.value = _currentHealth/_currentMaxHealth;
            _text.text = $"{Mathf.Round(_currentHealth)} / {Mathf.Round(_currentMaxHealth)}";
        }
        if (!Mathf.Approximately(_currentMaxHealth, _targetMaxHealth))
        {
            _currentMaxHealth = Mathf.MoveTowards(_currentMaxHealth, _targetMaxHealth, 1 - Mathf.Pow(1 - AnimationSpeed * Time.deltaTime, 3));
            _slider.value = _currentHealth/_currentMaxHealth;
            _text.text = $"{Mathf.Round(_currentHealth)} / {Mathf.Round(_currentMaxHealth)}";
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
                Mathf.MoveTowards(_rectTransform.rect.width, _targetMaxHealth * barWidthMultiplier, 1 - Mathf.Pow(1 - AnimationSpeed * Time.deltaTime, 3)));
        }
    }

    /// <summary>
    ///  Initialize healthbar with starting values from the HUD
    /// </summary>
    /// <param name="health">health of player</param>
    /// <param name="maxHealth">max health of player</param>
    public void InitializeHealthBar(float health, float maxHealth)
    {
        //Debug.Log("Init: " + health + ", " + maxHealth);
        // Set health values
        _currentHealth = health;
        _targetHealth = health;
        _currentMaxHealth = maxHealth;
        _targetMaxHealth = maxHealth;
        _slider.value = health / maxHealth;

        // Set health ratio displayed
        _text.text = $"{Mathf.Round(health)} / {Mathf.Round(maxHealth)}";
        //Debug.Log("0 INIT Health / Max health: " + health + ", " + maxHealth);
        //Debug.Log("0 INIT current Health / current Max health: " + _currentHealth + ", " + _currentMaxHealth);

        // _rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, _currentMaxHealth * barWidthMultiplier);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,_currentMaxHealth * barWidthMultiplier);
        
        _initialized = true;
    }
    
    /// <summary>
    ///  Update the player's health value
    /// </summary>
    /// <param name="health">player health</param>
    public void UpdateHealth(float health)
    {
        _targetHealth = Mathf.Max(health, 0);
        Debug.Log("Passed-in player health:" + health);
        //if (_initialized)
        //{
        //    if (Mathf.Approximately(health, _currentMaxHealth))
        //    {
        //        _currentHealth = health;
        //        _slider.value = _currentHealth/_currentMaxHealth;
        //        _text.text = $"{Mathf.Round(_currentHealth)} / {Mathf.Round(_currentMaxHealth)}";
        //    } else _queueManager.AddToQueue(ScaleHealthBar(health));
        //}
    }
    
    /// <summary>
    ///  Update the player's max health
    /// </summary>
    /// /// <param name="health">player health</param>
    /// <param name="maxHealth">player max health</param>
    public void UpdateMaxHealth(float health, float maxHealth)
    {
        _targetHealth = Mathf.Max(health, 0);
        _targetMaxHealth = maxHealth;
        //if (_initialized) _queueManager.AddToQueue(ScaleMaxHealthWidth(health, maxHealth));
    }

    private IEnumerator ScaleHealthBar(float health)
    {
        float timeRatio = 0f;
        Debug.Log("Pre Health: curr: " + _currentHealth + ", Health: " + health);
        float elapsedTime = 0f;
        while (elapsedTime < AnimationSpeed)
        {
            // Set elapsed time for animation
            elapsedTime += Time.deltaTime;
            var changeRatio = elapsedTime / AnimationSpeed;
            timeRatio = changeRatio;
            //Debug.Log("Percent complete: " + changeRatio);

            // Set health value
            _currentHealth = Mathf.MoveTowards(_currentHealth, health, changeRatio);
            //Debug.Log(_currentHealth + ", " + _currentMaxHealth);
            _slider.value = _currentHealth/_currentMaxHealth;
            _text.text = $"{Mathf.Round(_currentHealth)} / {Mathf.Round(_currentMaxHealth)}";
            
            yield return null;
        }
        Debug.Log("Final updated player health:" + _currentHealth + " an max health:" + _currentMaxHealth);
        Debug.Log("Original intended value: " + health);
        _currentHealth = health;
        //Debug.Log("Post Health: curr: " + _currentHealth);
    }
    
    private IEnumerator ScaleMaxHealthWidth(float health, float maxHealth)
    {
        //Debug.Log("Pre Max: curr: " + _currentHealth + ", health: " + health);
        var newHealth = Mathf.Round(health);
        var elapsedTime = 0f;
        var finalBarWidth = maxHealth * barWidthMultiplier;
        while (elapsedTime < AnimationSpeed)
        {
            // Set the elapsed time and grow ratio
            elapsedTime += Time.deltaTime;
            // Scale elapsed time to duration
            var changeRatio = elapsedTime / AnimationSpeed;
            // Scale health slider by bar width
            var healthRatio = changeRatio / barWidthMultiplier;
            
            // Set the healthbar width
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
                Mathf.MoveTowards(_rectTransform.rect.width, finalBarWidth, changeRatio));
            
            // Update currentMaxHealth
            
            _currentMaxHealth = Mathf.MoveTowards(_currentMaxHealth, maxHealth, healthRatio);
            
            // Update health
            _currentHealth = Mathf.MoveTowards(_currentHealth, newHealth, healthRatio);
            
            // Update slider
            _slider.value = _currentHealth/_currentMaxHealth;
            _text.text = $"{Mathf.Round(_currentHealth)} / {Mathf.Round(_currentMaxHealth)}";
            
            // Return the coroutine
            yield return null;
        }

        _currentHealth = health;
        _currentMaxHealth = maxHealth;
        //Debug.Log("Post Max: curr: " + _currentMaxHealth);
    }
}