using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private UIDocument _uiDocument;

    private VisualElement _root;
    // Start is called before the first frame update
    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        _root.Q<Button>("Start-Button").clicked += () =>
        {
            SceneManager.LoadScene("Gabriel_Test_Scene 1", LoadSceneMode.Single);
        };
        _root.Q<Button>("Quit-Button").clicked += () =>
        {
            Application.Quit();
        };
        //_root.Q<Button>("Start-Button").Focus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
