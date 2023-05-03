using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR;
using InputDevice = UnityEngine.InputSystem.InputDevice;

public class MenuController : MonoBehaviour
{
    private UIDocument _uiDocument;
    [SerializeField] private VisualTreeAsset settingsButtonsTemplate;
    private VisualElement _settingsButtons;
    private VisualElement _root;
    private VisualElement _defaultMenu;
    private Button _startButton;
    private Button _settingsButton;
    private Button _quitButton;
    private Button _backButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        _defaultMenu = _root.Q<VisualElement>("Default-Menu");
        _startButton = _root.Q<Button>("Start-Button");
        _settingsButton = _root.Q<Button>("Settings-Button");
        _quitButton = _root.Q<Button>("Quit-Button");
        
        _settingsButtons = settingsButtonsTemplate.CloneTree();
        _backButton = _settingsButtons.Q<Button>("Back-Button");

        // Listener for Start Button
        _startButton.clicked += () => SceneManager.LoadScene("Gabriel_Test_Scene 1", LoadSceneMode.Single);
        
        // Listener for Quit Button
        _quitButton.clicked += Application.Quit;
        
        // Listener for Settings Button
        _settingsButton.clicked += SettingsClicked;
        
        // Listener for BackClicked
        _backButton.clicked += BackClicked;
    }

    private void SettingsClicked()
    {
        _defaultMenu.Clear();
        _defaultMenu.Add(_settingsButtons);
    }

    private void BackClicked()
    {
        _defaultMenu.Clear();
        _defaultMenu.Add(_startButton);
        _defaultMenu.Add(_settingsButton);
        _defaultMenu.Add(_quitButton);
    }
}
