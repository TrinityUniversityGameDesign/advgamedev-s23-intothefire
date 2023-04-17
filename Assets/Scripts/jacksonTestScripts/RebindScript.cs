using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private InputActionReference reInput = null;
    GameObject controller = null;

    [SerializeField] private TMP_Text bindingDisplayNameText = null;

    private InputActionRebindingExtensions.RebindingOperation rebind = null;
    [SerializeField] private string bindName = "";

    // Update is called once per frame
    void Start()
    {
        int bindingIndex = controller.GetComponent<PlayerInput>().actions[bindName].GetBindingIndexForControl(controller.GetComponent<PlayerInput>().actions[bindName].controls[0]);
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(controller.GetComponent<PlayerInput>().actions[bindName].bindings[bindingIndex].effectivePath,
           InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void StartRebinding()
    {
        controller.GetComponent<PlayerInput>().actions[bindName].Disable();
        bindingDisplayNameText.text = "press a button to rebind";
        rebind = controller.GetComponent<PlayerInput>().actions[bindName].PerformInteractiveRebinding()
            //.WithControlsExcluding()
            .OnMatchWaitForAnother(0.01f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        int bindingIndex = controller.GetComponent<PlayerInput>().actions[bindName].GetBindingIndexForControl(controller.GetComponent<PlayerInput>().actions[bindName].controls[0]);
        controller.GetComponent<PlayerInput>().actions[bindName].Enable();
        rebind.Dispose();
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(controller.GetComponent<PlayerInput>().actions[bindName].bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void AssignPlayer(GameObject j)
    {
        controller = j;
    }
}
