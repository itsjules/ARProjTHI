//Created by JulP
using UnityEngine;
using UnityEngine.UI;

public class WebsiteLinkHandler : MonoBehaviour
{
    [SerializeField]
    private Button QRButton;
    [SerializeField]
    private Button PINButton;

    [SerializeField]
    private string urlQR = "https://www3.primuss.de/cgi-bin/bew_anmeldung_v3/index.pl?Session=&FH=fhin&Email=&Portal=1&Language=en"; //PRIMUSS
    [SerializeField]
    private string urlPIN = "https://outlook.office.com/"; //Stud-Mail

    private void Start()
    {
        if (QRButton != null)
        {
            QRButton.onClick.AddListener(() => OpenWebsite(urlQR));
        }

        if (PINButton != null)
        {
            PINButton.onClick.AddListener(() => OpenWebsite(urlPIN));
        }
    }

    private void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }
}