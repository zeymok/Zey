using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections; // Nécessaire pour UnityWebRequest
using UnityEngine.EventSystems; 

[System.Serializable]
public class MotData
{
    public string status;
    public string motChoisi;
    public int nombreDeMots;
    public int emplacementDuMotDansLeDictionnaire;
    public string regles;
}

public class MenuJeu : MonoBehaviour
{
    public TMP_Text surnomText;
    public TMP_Text wordDisplay;
    public TMP_Text incorrectWordsText;
    public TMP_Text erreursRestantesText;
    public GameObject panelPause;
    public Slider musiqueSlider;
    public Slider effetsSonoresSlider;
    public Image[] imagesPendu;
    public TMP_InputField lettreInput;
    public Button validerButton;

    private int erreurs;
    private int erreursMax = 6;
    private string motADeviner;
    private string lettresDevinees = "";
    private string motsIncorrects = "";
    private bool jeuEnPause = false;

    private readonly string urlJson = "https://makeyourgame.fun/api/pendu/avoir-un-mot";

    void Start()
    {
        surnomText.text = PlayerPrefs.GetString("Surnom");
        Debug.Log($"Je m'appelle: {surnomText.text}"); 

        panelPause.SetActive(false);

        StartCoroutine(ChargerMotDepuisURL()); // Charger les mots depuis l'URL

        musiqueSlider.onValueChanged.AddListener(ChangerVolumeMusique);
        effetsSonoresSlider.onValueChanged.AddListener(ChangerVolumeEffets);
        validerButton.onClick.AddListener(ValiderLettre);

        // Assurer que le champ de saisie reçoit le focus initialement
        FocusInputField();
    }

    void Update()
    {
        // ENTREE pour valider
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ValiderLettre();
        }

        // la pause par ECHAP
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jeuEnPause)
            {
                Reprendre();
            }
            else
            {
                Pause();
            }
        }
    }

    private IEnumerator ChargerMotDepuisURL()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(urlJson))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                MotData motData = JsonUtility.FromJson<MotData>(json);
                motADeviner = motData.motChoisi.ToUpper(); 
                Debug.Log($"Mot à deviner: {motADeviner}");

                UpdateWordDisplay();
                UpdateErreursRestantes();
            }
            else
            {
                Debug.LogError("Impossible de charger le fichier JSON depuis l'URL !");
                motADeviner = "DEFAULT";
            }
        }
    }

    public void Pause()
    {
        panelPause.SetActive(true);
        jeuEnPause = true;
        Time.timeScale = 0;
    }

    public void Reprendre()
    {
        panelPause.SetActive(false);
        jeuEnPause = false;
        Time.timeScale = 1;
    }
    public void Redemarrer()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuJeu");
    }

    public void RetourMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuPrincipal");
    }
    
    public void Quitter()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ValiderLettre()
    {
        string lettre = lettreInput.text.ToUpper();
        Debug.Log($"Lettre entrée: {lettre}");

        if (!string.IsNullOrEmpty(lettre) && lettre.Length == 1)
        {
            if (motADeviner.Contains(lettre))
            {
                lettresDevinees += lettre;
                UpdateWordDisplay();
                // Vérifiez si le mot est complètement deviné
                if (IsMotDevine())
                {
                    Victoire();
                }
            }
            else
            {
                erreurs++;
                motsIncorrects += lettre + " "; 
                UpdatePendu();
                UpdateErreursRestantes();
                if (erreurs >= erreursMax)
                {
                    Defaite();
                }
            }
            // Réinitialiser le champ de saisie et mettre le focus dessus
            lettreInput.text = "";
            FocusInputField();
        }
    }

    void UpdateWordDisplay()
    {
        string displayText = "";
        bool motComplet = true;

        foreach (char lettre in motADeviner)
        {
            if (lettresDevinees.Contains(lettre.ToString()))
            {
                displayText += lettre + " ";
            }
            else
            {
                displayText += "_ ";
                motComplet = false;
            }
        }

        wordDisplay.text = displayText.Trim();

        // Si le mot est complètement deviné, Go au menu des résultats
        if (motComplet)
        {
            Victoire();
        }
    }

    void UpdatePendu()
    {
        for (int i = 0; i < imagesPendu.Length; i++)
        {
            if (i < erreurs)
            {
                imagesPendu[i].gameObject.SetActive(true);
            }
            else
            {
                imagesPendu[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateErreursRestantes()
    {
        erreursRestantesText.text = "Erreurs restantes: " + (erreursMax - erreurs);
        incorrectWordsText.text = "Mots incorrects: " + motsIncorrects;
    }

    void ChangerVolumeMusique(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ChangerVolumeMusique(volume);
        }
    }

    void ChangerVolumeEffets(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ChangerVolumeEffets(volume);
        }
    }

    void FocusInputField()
    {
        // Mettre le focus sur le champ de saisie
        if (EventSystem.current != null && lettreInput != null)
        {
            EventSystem.current.SetSelectedGameObject(lettreInput.gameObject);
            lettreInput.ActivateInputField();
        }
    }

    bool IsMotDevine()
    {
        foreach (char lettre in motADeviner)
        {
            if (!lettresDevinees.Contains(lettre.ToString()))
            {
                return false;
            }
        }
        return true;
    }

    void Victoire()
    {
        PlayerPrefs.SetInt("Victoire", 1);
        PlayerPrefs.SetInt("ErreursRestantes", erreurs);
        PlayerPrefs.SetString("MotADeviner", motADeviner);
        SceneManager.LoadScene("MenuResultat");
    }

    void Defaite()
    {
        PlayerPrefs.SetInt("Victoire", 0);
        PlayerPrefs.SetInt("ErreursRestantes", erreurs);
        PlayerPrefs.SetString("MotADeviner", motADeviner);
        SceneManager.LoadScene("MenuResultat");
    }
}
