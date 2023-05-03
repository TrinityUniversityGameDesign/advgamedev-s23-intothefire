using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VictoryController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;
    [SerializeField] private VisualTreeAsset _itemTemplate; 
    private List<GameObject> _survivors;

    private VisualElement[] _itemContainers;
    // Start is called before the first frame update
    private void Awake()
    {
        _survivors = GameManager.Instance.players;
    }
    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        _itemContainers = new VisualElement[4]
        {
            _root.Q<VisualElement>("First-Items"),
            _root.Q<VisualElement>("Second-Items"),
            _root.Q<VisualElement>("Third-Items"),
            _root.Q<VisualElement>("Fourth-Items")
        };
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadItems()
    {
        for (int i = 0; i < _survivors.Count; i++)
        {
            var jcm = _survivors[i].GetComponent<JacksonCharacterMovement>();
            FillItemContainer(_itemContainers[i], jcm.inventory);
        }
    }

    private void FillItemContainer(VisualElement itemContainer, List<Item> items)
    {
        foreach (var item in items)
        {
            VisualElement itemEntry = new VisualElement();
            itemEntry.Q<VisualElement>("Item-Icon").style.backgroundImage = new StyleBackground(item.icon);
            itemEntry.Q<Label>("Item-Name").text = item.name;
            itemContainer.Add(itemEntry);
        }
    }
    
    private void LoadIcons()
    {
        var firstIcon = _root.Q<VisualElement>("First-Icon");
        var secondIcon = _root.Q<VisualElement>("Second-Icon");
        var thirdIcon = _root.Q<VisualElement>("Third-Icon");
        var fourthIcon = _root.Q<VisualElement>("Fourth-Icon");
    }
}
