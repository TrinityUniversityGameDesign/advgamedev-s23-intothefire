using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float barWidthMultiplier = 3f;
    [SerializeField] private float scaleTime = 10f;

    private float currentMaxHealth;
    
    private Slider _slider;
    private RectTransform _rectTransform;
    private RectTransform _parentRectTransform;
    private TMP_Text _text;

    // Start is called before the first frame update
    void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
        _parentRectTransform = GetComponentInParent<RectTransform>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        if (!Mathf.Approximately(currentMaxHealth, maxHealth))
        {
            StartCoroutine(ScaleMaxHealthWidth(health, maxHealth));
        }
        else
        {
            _slider.value = health/maxHealth;
            _text.text = $"{health}/{maxHealth}";
        }
    }

    private void SetHealthWidth(float value)
    {
        _rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, value * barWidthMultiplier);
    }
    
    public void InitializeHealthBar(float health, float maxHealth)
    {
        currentMaxHealth = maxHealth;
        Debug.Log("Initializing health at " + health + "/" + maxHealth);
        UpdateHealth(health, maxHealth);
        SetHealthWidth(currentMaxHealth);
    }
    
    IEnumerator ScaleMaxHealthWidth(float health, float maxHealth)
    {
        float elapsedTime = 0f;
        while (elapsedTime < scaleTime)
        {
            // Set the elapsed tmie and grow ratio
            elapsedTime += Time.deltaTime;
            var growRatio = elapsedTime / scaleTime;
            
            // Set the healthbar width
            var finalBarWidth = maxHealth * barWidthMultiplier;
            _rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 200, 
                Mathf.MoveTowards(_rectTransform.rect.width, finalBarWidth, growRatio));
            //_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.MoveTowards(_rectTransform.rect.width, finalBarWidth, growRatio));
            
            // Update currentMaxHealth
            var finalMaxHealth = growRatio / barWidthMultiplier;
            currentMaxHealth = Mathf.MoveTowards(currentMaxHealth, maxHealth, finalMaxHealth);
            _slider.value = health/currentMaxHealth;
            _text.text = $"{health}/{Mathf.Round(currentMaxHealth)}";
            yield return null;
        }
    }
}
