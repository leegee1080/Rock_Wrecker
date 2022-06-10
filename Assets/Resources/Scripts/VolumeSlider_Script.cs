using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider_Script : MonoBehaviour
{
    [SerializeField]private Sound_Type_Tags _volumeType;
    [SerializeField]private int _volume;
    float _volumeFactor;
    IEnumerator _tickUpdaterRoutine;
    [SerializeField]private Transform _tickContainer;
    [SerializeField]private GameObject _tickMuteImage;
    [SerializeField]private float _tickUpdateSpeed;

    void Start()
    {
        _volumeFactor = 100/_tickContainer.childCount;
        //UpdateVolumeTicks();
    }

    public void UpdateVolumeTicks()
    {
        if(_tickUpdaterRoutine != null){StopCoroutine(_tickUpdaterRoutine);}

        _volume = PlayerPrefs.GetInt(_volumeType.ToString());
        if (_volume <= 0){ _tickMuteImage.SetActive(true);}else{_tickMuteImage.SetActive(false);}


        foreach (Transform child in _tickContainer)
        {
            child.gameObject.SetActive(false);
        }

        IEnumerator updateTick()
        {
            int max = _volume/(int)_volumeFactor;
            if(max >= _tickContainer.childCount){yield return null;}
            if(max <= 0){yield return null;}
            Sound_Events.Change_Volume((float)_volume/300f, _volumeType);
            for (int i = 0; i < max; i++)
            {
                yield return new WaitForSeconds(_tickUpdateSpeed);
                _tickContainer.GetChild(i).gameObject.SetActive(true);
            }
        }

        _tickUpdaterRoutine = updateTick();
        StartCoroutine(_tickUpdaterRoutine);
    }

    public void VolumeUp()
    {   
        if(_volume > 100){return;}
        _volume += (int)_volumeFactor;
        if(_volume > 100){_volume = 100;}
        PlayerPrefs.SetInt(_volumeType.ToString(), _volume);
        UpdateVolumeTicks();
    }
    public void VolumeDown()
    {
        if(_volume <= 0){return;}
        _volume -= (int)_volumeFactor;
        if(_volume <= 0){_volume = 0;}
        PlayerPrefs.SetInt(_volumeType.ToString(), _volume);
        UpdateVolumeTicks();
    }
}
