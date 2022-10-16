using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentajeCompleteText;
    [SerializeField] private TextMeshProUGUI currentHealthText;

    public void SetData(GameData data)
    {
        //No hay datos para este profileId
        if(data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        //Hay datos para este profileId
        else 
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            percentajeCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";
            currentHealthText.text = "CURRENT HEALTH: " + data.currentHealth;
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }
}
