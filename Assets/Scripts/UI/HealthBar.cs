using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float barWidthMultiplier = 3f;
    [SerializeField] private float animationSpeed = 10f;

    private float _currentMaxHealth;
    private float _currentHealth;
    
    private Slider _slider;
    private RectTransform _rectTransform;
    private TMP_Text _text;

    // Start is called before the first frame update
    void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    ///  Update the player's health value
    /// </summary>
    /// <param name="health">player health</param>
    public void UpdateHealth(float health)
    {
        StartCoroutine(ScaleHealthBar(health));
    }
    
    /// <summary>
    ///  Update the player's max health
    /// </summary>
    /// <param name="maxHealth">player max health</param>
    public void UpdateMaxHealth(float health, float maxHealth)
    {
        StartCoroutine(ScaleMaxHealthWidth(health, maxHealth));
    }
    
    /// <summary>
    ///  Initialize healthbar with starting values from the HUD
    /// </summary>
    /// <param name="health">health of player</param>
    /// <param name="maxHealth">max health of player</param>
    public void InitializeHealthBar(float health, float maxHealth)
    {
        // Set health values
        _currentHealth = health;
        _slider.value = health / maxHealth;
        
        // Set health ratio displayed
        _text.text = $"{health}/{maxHealth}";
        
        // Set the current max health at initial load
        _currentMaxHealth = maxHealth;
        // _rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, _currentMaxHealth * barWidthMultiplier);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,_currentMaxHealth * barWidthMultiplier);
    }

    IEnumerator ScaleHealthBar(float health)
    {
        float elapsedTime = 0f;
        float healthAnimationSpeed = 1f;
        while (elapsedTime < healthAnimationSpeed)
        {
            // Set elapsed time for animation
            elapsedTime += Time.deltaTime;
            var changeRatio = elapsedTime / healthAnimationSpeed;

            // Set health value
            var newHealth = Mathf.MoveTowards(_currentHealth, health, changeRatio);
            _slider.value = newHealth/_currentMaxHealth;
            _text.text = $"{Mathf.Round(newHealth)}/{_currentMaxHealth}";
            
            yield return null;
        }
    }
    
    IEnumerator ScaleMaxHealthWidth(float health, float maxHealth)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationSpeed)
        {
            // Set the elapsed tmie and grow ratio
            elapsedTime += Time.deltaTime;
            var changeRatio = elapsedTime / animationSpeed;
            
            // Set the healthbar width
            var finalBarWidth = maxHealth * barWidthMultiplier;
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.MoveTowards(_rectTransform.rect.width, finalBarWidth, changeRatio));
            //_rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 200, 
            //    Mathf.MoveTowards(_rectTransform.rect.width, finalBarWidth, changeRatio));
            
            // Update currentMaxHealth
            var healthRatio = changeRatio / barWidthMultiplier;
            _currentMaxHealth = Mathf.MoveTowards(_currentMaxHealth, maxHealth, healthRatio);
            _slider.value = _currentHealth/_currentMaxHealth;
            _text.text = $"{_currentHealth}/{Mathf.Round(_currentMaxHealth)}";
            yield return null;
        }
        UpdateHealth(health);
    }
}
