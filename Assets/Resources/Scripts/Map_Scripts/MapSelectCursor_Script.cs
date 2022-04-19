using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapSelectCursor_Script : MonoBehaviour
{
    [SerializeField]TMP_Text _sizeText;
    [SerializeField]TMP_Text _lodeText;
    [SerializeField]TMP_Text _lodeDiaText;
    [SerializeField]TMP_Text _lodeRubText;
    [SerializeField]TMP_Text _lodeTopText;
    [SerializeField]TMP_Text _dangerText;
    [SerializeField]TMP_Text _fuelText;
    public bool FuelCostTooHigh;
    public int FuelCost;
    [SerializeField]Color _defaultColor;
    [SerializeField]Color _warnColor;
    [SerializeField]Color _alertColor;

    private IEnumerator _runningBlinkRountine;
    public void UpdateInfo(MapPOI_ScriptableObject _selectedPOI)
    {
        if(_runningBlinkRountine != null){StopCoroutine(_runningBlinkRountine); _fuelText.gameObject.transform.localScale = Vector3.one;}
        
        _sizeText.text = "Size: "+_selectedPOI.poi_size;
        // _lodeText.text = "Lode: <color=blue>" +_selectedPOI.lode_dia.ToString() + "</color> <color=red>" +_selectedPOI.lode_rub.ToString() + "</color> <color=yellow>" + _selectedPOI.lode_top.ToString() + "</color>";
        _lodeDiaText.text = _selectedPOI.lode_dia.ToString();
        _lodeRubText.text = _selectedPOI.lode_rub.ToString();
        _lodeTopText.text = _selectedPOI.lode_top.ToString();



        _fuelText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _defaultColor);
        _fuelText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _defaultColor);
        int fuelCost = ((int)(Vector2.Distance(Levelselect_Controller_Script.levelselect_controller_singletion.OccupiedPOI.poi_info_so.map_pos, _selectedPOI.map_pos) / Levelselect_Controller_Script.levelselect_controller_singletion.DistPerFuelCost));
        _fuelText.text = "Fuel: "+ fuelCost;
        if(fuelCost > Overallgame_Controller_Script.overallgame_controller_singleton.PlayerFuel)
        {
           _fuelText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _alertColor);
           _fuelText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _alertColor);
           FuelCostTooHigh = true;
           FuelCost =fuelCost;
        }
        else
        {
           _fuelText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _defaultColor);
           _fuelText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _defaultColor);
           FuelCostTooHigh = false;
           FuelCost =fuelCost;
        }


        switch (_selectedPOI.poi_difficulty)
        {
            case 0:
                _dangerText.text = "Danger: LOW";
                _dangerText.color = _defaultColor;
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _defaultColor);
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _defaultColor);
                break;
            case 1:
                _dangerText.text = "Danger: MED";
                _dangerText.color = _warnColor;
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _warnColor);
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _warnColor);
                break;
            case 2:
                _dangerText.text = "Danger: MED";
                _dangerText.color = _warnColor;
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _warnColor);
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _warnColor);
                break;
            case 3:
                _dangerText.text = "Danger: HIGH";
                _dangerText.color = _alertColor;
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _alertColor);
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _alertColor);
                break;
            default:
                _dangerText.text = "Danger: LOW";
                _dangerText.color = _defaultColor;
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _defaultColor);
                _dangerText.fontSharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, _defaultColor);
                break;
        }

    }

    public void StartBlinkCoroutine()
    {
        if(_runningBlinkRountine != null){StopCoroutine(_runningBlinkRountine);}
        _runningBlinkRountine = BlinkFuelCost();
        StartCoroutine(_runningBlinkRountine);
    }

    private IEnumerator BlinkFuelCost()
    {  
        for (int i = 0; i < 5; i++)
        {
            _fuelText.gameObject.transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(0.25f);
            _fuelText.gameObject.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
