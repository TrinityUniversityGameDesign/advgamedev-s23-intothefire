using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class VictoryController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;
    private Button _startButton;
    private Button _quitButton;
    [SerializeField] private VisualTreeAsset _itemTemplate; 
    private List<GameObject> _survivors;

    private VisualElement[] _itemContainers;
    private VisualElement[] _iconContainers;
    // Start is called before the first frame update
    private void Awake()
    {
        _survivors = GameObject.Find("VictorWatcher").GetComponent<ShowdownVictorScript>().playersInOrderOfLoss;
    }
    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        _startButton = _root.Q<Button>("Start-Button");
        _quitButton = _root.Q<Button>("Quit-Button");
        _itemContainers = new VisualElement[4]
        {
            _root.Q<VisualElement>("First-Items"),
            _root.Q<VisualElement>("Second-Items"),
            _root.Q<VisualElement>("Third-Items"),
            _root.Q<VisualElement>("Fourth-Items")
        };
        _iconContainers = new VisualElement[4]
        {
            _root.Q<VisualElement>("First-Icon"),
            _root.Q<VisualElement>("Second-Icon"),
            _root.Q<VisualElement>("Third-Icon"),
            _root.Q<VisualElement>("Fourth-Icon")
        };

        _startButton.clicked += StartNewGame;
        _quitButton.clicked += ReturnToMenu;

        LoadItems();
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene("Gabriel_Test_Scene 1", LoadSceneMode.Single);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    private void LoadItems()
    {
        for (int i = 0; i < _survivors.Count; i++)
        {
            var jcm = _survivors[i].GetComponent<JacksonCharacterMovement>();
            var itemContainer = _itemContainers[i];
            var iconContainer = _iconContainers[i];
            FillItemContainer(_itemContainers[i], jcm.inventory);
            FillIcon(_iconContainers[i], jcm.Icon);
        }
    }

    private void FillItemContainer(VisualElement itemContainer, List<Item> items)
    {
        foreach (var item in items)
        {
            VisualElement itemEntry = _itemTemplate.Instantiate();
            itemEntry.Q<VisualElement>("Item-Icon").style.backgroundImage = new StyleBackground(item.icon);
            itemEntry.Q<Label>("Item-Name").text = item.name;
            itemContainer.Add(itemEntry);
        }
    }
    
    private void FillIcon(VisualElement iconContainer, Sprite icon)
    {
        iconContainer.style.backgroundImage = new StyleBackground(icon);
    }
}
