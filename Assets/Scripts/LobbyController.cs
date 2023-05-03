using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyController : MonoBehaviour
{
    [Header("Player Configuration")]
    [Tooltip("Player selection screen template")]
    [SerializeField] private VisualTreeAsset playerSelectTemplate;
    [Tooltip("Set colors for each screen")]
    [SerializeField] private Color[] playerColors = new Color[4]
    {
        new Color(0.4f, 0.8f, 0.9f, 1), 
        new Color(0.13f, 0.53f, 0.2f, 1), 
        new Color(0.8f, 0.73f, 0.26f, 1), 
        new Color(0.6f,  0.2f,  0.46f, 1)
    };

    [Header("Weapon Configuration")]
    [Tooltip("Include all weapons you want to be playable")]
    [SerializeField] private List<WeaponData> playableWeapons;
    
    [Header("Character Configuration")]
    [Tooltip("Include character data here for players tha can be selected")]
    [SerializeField] private List<CharacterData> playableCharacters;

    private UIDocument _uiDocument;
    private VisualElement _root;
    
    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        
        // This will be inside of a listener that registers when a given player joins
        InitializePlayerSelect(0);
        InitializePlayerSelect(1);
        InitializePlayerSelect(2);
        InitializePlayerSelect(3);
    }

    void InitializePlayerSelect(int playerPosition)
    {
        
    }
}
