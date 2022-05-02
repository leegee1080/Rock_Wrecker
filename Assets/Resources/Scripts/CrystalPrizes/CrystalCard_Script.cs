using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrystalCard_Script : MonoBehaviour
{   
    [SerializeField]private Image image;
    [SerializeField]private TMP_Text text;
    [SerializeField]private GameObject selectParticles;

    public void ApplyCard(CrystalPrize_ScriptableObject so)
    {
        image.sprite = so.art;
        text.text = so.desc;
    }

    public void PopCard()
    {
        //selectParticles.Play();
    }
}
