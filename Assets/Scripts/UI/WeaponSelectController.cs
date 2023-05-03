using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelectController : MonoBehaviour
{
    [SerializeField] private GameObject weaponObject;
    private List<GameObject> _weapons;
    public int player;
    public GameObject curr;
    
    private GameObject _firstButton;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.weapons.Count; i++)
        {
            var weapon = GameManager.Instance.weapons[i];
            var newObject = Instantiate(weaponObject, transform);
            var tooltip = newObject.GetComponentInChildren<TooltipController>();
            var toggle = newObject.GetComponentInChildren<ToggleSelect>();
            
            toggle.Hide();
            tooltip.SetWeapon(weapon);
            tooltip.Hide();
            
            newObject.GetComponentInChildren<Image>().sprite = weapon.Icon;
            newObject.GetComponentInChildren<TMP_Text>().text = weapon.name;
            newObject.GetComponent<WeaponController>().position = i;
        }
        _firstButton = transform.GetChild(0).gameObject;
        Debug.Log("First button is: " + _firstButton);
    }
}
