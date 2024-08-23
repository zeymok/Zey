using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuResultat : MonoBehaviour
{
    public TMP_Text resultatText; 
    public Button rejouerButton;
    public Button parametreButton;
    public Button retourButton;
    public Button quitterButton;
    //public AudioSource audioSource; 
    public AudioClip victoireClip;   
    public AudioClip defaiteClip;    
    public float typingSpeed = 0.1f;  // Vitesse d'affichage des caractères

    void Start()
    {
        string surnom = PlayerPrefs.GetString("Surnom");
        bool victoire = PlayerPrefs.GetInt("Victoire") == 1;
        string motADeviner = PlayerPrefs.GetString("MotADeviner");

        string resultat = surnom + " a " + (victoire ? "gagne" : "perdu") + ", la reponse est " + motADeviner;
        
        StartCoroutine(TypeText(resultat));

        if (victoire)
        {
            //audioSource.clip = victoireClip;
            AudioManager.Instance.JouerVictoire();
        }
        else
        {
            //audioSource.clip = defaiteClip;
            AudioManager.Instance.JouerDefaite();
        }
        //audioSource.Play();

        rejouerButton.onClick.AddListener(Rejouer);
        parametreButton.onClick.AddListener(Parametre);
        retourButton.onClick.AddListener(RetourMenu);
        quitterButton.onClick.AddListener(Quitter);
    }

    IEnumerator TypeText(string text)
    {
        resultatText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            resultatText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void Rejouer()
    {
        SceneManager.LoadScene("MenuJeu");
    }

    public void Parametre()
    {
        SceneManager.LoadScene("MenuParametre");
    }

    public void RetourMenu()
    {
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
}
