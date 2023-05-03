using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class PlayerSelectController : MonoBehaviour
{
    private Slider _slider;
    private List<CharacterData> _characters;
    private TMP_Text _name;
    private TMP_Text _desc;
    private RawImage _texture;
    
    private int _selected;
    private float _currentAlpha = 1;
    private int _player;
    private bool _init;
    private void Start()
    {
        _characters = GameManager.Instance.characters;
        _slider = GetComponentInChildren<Slider>();
        //_slider.maxValue = _characters.Count - 1;
        _selected = 0;
        _slider.value = 0;
        _slider.maxValue = _characters.Count - 1;

        _texture = GetComponentInChildren<RawImage>();
        
        var textComponents = GetComponentsInChildren<TMP_Text>();
        _name = textComponents[0];
        //_desc = textComponents[1];
        
        _player = GameManager.Instance.LastJoinedPlayer;
        _init = true;
        UpdateCharacter();
    }

    public void OnSliderChanged()
    {
        if (!_init) return;
        StopAllCoroutines();
        _selected = (int)_slider.value;
        GameManager.Instance.UpdatePlayerCharacter(_player, _selected);
        StartCoroutine(SwapCharacter());
    }

    public void SelectSlider()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_slider.gameObject);
    }

    private void UpdateAlphas(float alpha)
    {
        _currentAlpha = alpha;
        _name.color = new Color(_name.color.r, _name.color.g, _name.color.b, alpha);
        //_desc.color = new Color(_desc.color.r, _desc.color.g, _desc.color.b, alpha);
        _texture.color = new Color(_texture.color.r, _texture.color.g, _texture.color.b, alpha);
    }

    private IEnumerator FadeInText(float timeSpeed)
    {
        UpdateCharacter();
        _name.transform.parent.gameObject.SetActive(true);
        UpdateAlphas(0);
        while (_currentAlpha < 1.0f)
        {
            UpdateAlphas(_currentAlpha + Time.deltaTime * timeSpeed);
            yield return null;
        }
    }
    private IEnumerator FadeOutText(float timeSpeed)
    {
        UpdateAlphas(1);
        while (_currentAlpha > 0.0f)
        {
            UpdateAlphas(_currentAlpha - Time.deltaTime * timeSpeed);
            yield return null;
        }
        _name.transform.parent.gameObject.SetActive(false);
    }

    private IEnumerator SwapCharacter (float duration = 10f)
    {
        yield return StartCoroutine(FadeOutText(duration));
        yield return new WaitForSeconds(0.02f);
        yield return StartCoroutine(FadeInText(duration));
    }
    private void UpdateCharacter()
    {
        var curr = _characters[_selected];
        _name.text = curr.name;
        //_desc.text = curr.Description;
        _texture.texture = curr.renderTexture;
    }
}
