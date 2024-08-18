using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class MenuPrincipal : MonoBehaviour
{
    public TMP_InputField surnomInput;
    public Button jouerButton;
    public Button parametreButton;
    public Button quitterButton;
    private string surnom;

    void Start()
    {
        jouerButton.onClick.AddListener(Jouer);
        parametreButton.onClick.AddListener(Parametre);
        quitterButton.onClick.AddListener(Quitter);
    }

    void Jouer()
    {
        surnom = surnomInput.text;
        if (!string.IsNullOrEmpty(surnom))
        {
            PlayerPrefs.SetString("Surnom", surnom);
            SceneManager.LoadScene("MenuJeu");
        }
    }

    void Parametre()
    {
        SceneManager.LoadScene("MenuParametre");
    }

    void Quitter()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
