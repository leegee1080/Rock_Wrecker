using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapSelectCursor_Script : MonoBehaviour
{
    [SerializeField]TMP_Text _sizeText;
    [SerializeField]TMP_Text _lodeText;
    [SerializeField]TMP_Text _dangerText;
    [SerializeField]Color _defaultColor;
    [SerializeField]Color _warnColor;
    [SerializeField]Color _alertColor;
    public void UpdateInfo(MapPOI_ScriptableObject _selectedPOI)
    {
        _sizeText.text = "Size: "+_selectedPOI.poi_size;
        _lodeText.text = "Lode: "+_selectedPOI.poi_difficulty;
        
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
}
