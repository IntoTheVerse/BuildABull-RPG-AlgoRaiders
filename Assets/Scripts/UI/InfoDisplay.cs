using TMPro;
using UnityEngine;

public class InfoDisplay : SingletonMonobehaviour<InfoDisplay>
{
    public Transform infoDisplay;
    public TextMeshProUGUI infoTitle;
    public TextMeshProUGUI infoBody;

    public void ShowInfo(string title, string body)
    {
        infoTitle.text = title;
        infoBody.text = body;
        infoDisplay.gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        infoDisplay.gameObject.SetActive(false);
    }
}
