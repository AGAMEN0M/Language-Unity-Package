using UnityEngine;
using System.Globalization;

public class LanguageIndicator : MonoBehaviour
{
    public string systemLanguage;

    // Start is called before the first frame update
    void Start()
    {
        CultureInfo cultureInfo = CultureInfo.InstalledUICulture;
        systemLanguage = cultureInfo.DisplayName;
        Debug.Log("Idioma do sistema: " + systemLanguage);
    }
}
