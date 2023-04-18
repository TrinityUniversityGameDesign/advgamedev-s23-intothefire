using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSelectController
{
    private VisualElement _playerSelectTemplate;
    private VisualElement _container;
    private VisualElement _characterContainer;
    private SliderInt _characterSlider;
    private VisualElement _weaponsContainer;
    private List<WeaponData> _playableWeapons;
    private List<CharacterData> _playableCharacters;
    
    /// <summary>
    /// Build the player selection screen based on the given paramaters
    /// </summary>
    /// <param name="playerSelectTemplate">Template for player select screen</param>
    /// <param name="playableWeapons">list of weapons available to this player</param>
    /// <param name="playableCharacters">list of characters available to this player</param>
    public void CreatePlayerSelectScreen(VisualElement playerSelectTemplate, List<WeaponData> playableWeapons, List<CharacterData> playableCharacters, Color playerColor)
    {
        _playerSelectTemplate = playerSelectTemplate;
        _playableCharacters = playableCharacters;
        _playableWeapons = playableWeapons;
        
        _container = _playerSelectTemplate.Q<VisualElement>("Container");
        _weaponsContainer = _playerSelectTemplate.Q<VisualElement>("Weapons");
        _characterContainer = _playerSelectTemplate.Q<VisualElement>("Character-Container");
        _characterSlider = _characterContainer.Q<SliderInt>("Character-Slider");
        
        // Set border color
        _container.style.borderTopColor = playerColor;

        // Instantiate the weapons objects
        _playableWeapons.ForEach(weapon =>
        {
            Sprite icon = weapon.Icon;
            Button weaponButton = new Button();
            weaponButton.AddToClassList("weapon-button");
            weaponButton.text = weapon.WeaponType.ToString();
            weaponButton.tooltip = weapon.Description;
            VisualElement weaponIcon = new VisualElement();
            weaponIcon.AddToClassList("weapon-icon");
            weaponIcon.style.backgroundImage = new StyleBackground(icon);
            weaponButton.Add(weaponIcon);
            _weaponsContainer.Add(weaponButton);
        });

        // Instantiate the characters
        _characterSlider.highValue = playableCharacters.Count - 1;
        _characterSlider.RegisterValueChangedCallback(value => UpdateCharacterSlider(value.newValue));
        // Initial update to load it in properly
        _characterSlider.value = Random.Range(0, playableCharacters.Count);
        UpdateCharacterSlider(_characterSlider.value);
    }
    
    /// <summary>
    /// Update the Character selection slider to show the description and image of the selected character
    /// </summary>
    /// <param name="value">position in slider: 0 to number of characters</param>
    private void UpdateCharacterSlider(int value)
    {
        CharacterData characterChosen = _playableCharacters[value];
        _characterSlider.style.backgroundImage = Background.FromRenderTexture(characterChosen.renderTexture);
        _characterContainer.Q<Label>("Character-Name").text = characterChosen.name;
        _characterContainer.Q<Label>("Character-Description").text = characterChosen.Description;
    }
}
