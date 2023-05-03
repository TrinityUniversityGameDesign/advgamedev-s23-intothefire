using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    private WeaponData _weapon;
    private TMP_Text _desc;
    // Start is called before the first frame update
    void Awake()
    {
        _desc = GetComponentInChildren<TMP_Text>();
    }

    public void Show()
    {
        _desc.text = _weapon.Description;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetWeapon(WeaponData weapon)
    {
        _weapon = weapon;
    }
}
