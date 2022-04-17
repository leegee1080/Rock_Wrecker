using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI_Script : MonoBehaviour
{
    public static MapUI_Script singleton;
    [SerializeField]private Animator _animator;
    [SerializeField]private GameObject _ShipButtonsGroup;
    [SerializeField]private GameObject _MapButtonsGroup;

    private void Awake()
    {
        singleton = this;
    }

    public void OpenSelectionMiniMenu()
    {
        _MapButtonsGroup.SetActive(true);
        _ShipButtonsGroup.SetActive(false);
        _animator.Play("Base Layer.ShowMapbuttonsMini",layer: 0,normalizedTime: 0);
    }
    public void CloseSelectionMiniMenu()
    {
        _animator.Play("Base Layer.HideMapbuttonsMini",layer: 0,normalizedTime: 0);
    }
    public void OpenShipMiniMenu()
    {
        _MapButtonsGroup.SetActive(false);
        _ShipButtonsGroup.SetActive(true);
        _animator.Play("Base Layer.OpenShipMenu",layer: 0,normalizedTime: 0);
    }
    public void CloseShipMiniMenu()
    {
        _animator.Play("Base Layer.CloseShipMenu",layer: 0,normalizedTime: 0);
    }
}
