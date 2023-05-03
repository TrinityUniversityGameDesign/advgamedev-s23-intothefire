using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int position;
    public int player;
    public bool selected = false;
    private WeaponSelectController _wsc;
    private ToggleSelect _ts;
    private TooltipController _tc;

    private void Awake()
    {
        _wsc = transform.parent.GetComponent<WeaponSelectController>();
        _ts = transform.GetComponentInChildren<ToggleSelect>();
        _tc = transform.GetComponentInChildren<TooltipController>();
    }
    public void OnWeaponClicked()
    {
        GameManager.Instance.UpdatePlayerWeapon(player, position);
        _wsc.curr = gameObject;
    }

    public void OnDeselect()
    {
        if (_wsc.curr == gameObject)
        {
            _tc.Hide();
            selected = false;
        }
        else
        {
            _ts.Hide();
            _tc.Hide();
            selected = false;
        }
    }

    public void OnSelect()
    {
        _ts.Show();
        _tc.Show();
        selected = true;
    }

    private void Update()
    {
        if (_wsc.curr != gameObject && !selected)
        {
            OnDeselect();
        }
    }
}
