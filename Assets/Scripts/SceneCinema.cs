using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCinema : MonoBehaviour
{
    public GameObject imageChat; // GameObject du chat
    public GameObject logo; // GameObject du logo
    public float vitesseMarche = 180f; 
    public float tempsAttenteAvantRetour = 4f; 

    private Vector3 positionDepart;
    private Vector3 positionMilieu;
    private Vector3 positionFin;

    private bool cinematiqueFinie = false;

    private void Start()
    {
        positionDepart = new Vector3(1260, -310, 0);
        positionMilieu = new Vector3(0, -310, 0);
        positionFin = new Vector3(-1260, -310, 0);

        // Met Chat au debut d'action
        imageChat.transform.localPosition = positionDepart;

        // Désactiver logo au début du cinéma
        logo.SetActive(false);

        // Lancer la cinéma
        StartCoroutine(JouerCinematique());
    }

    private void Update()
    {
        // si un clic est appuyé donc le cinema skip
        if (Input.GetMouseButtonDown(0) && !cinematiqueFinie)
        {
            PasserCinematique();
        }
    }

    private IEnumerator JouerCinematique()
    {
        // Chat va jusqu au milieu
        yield return StartCoroutine(DeplacerChatVersPosition(positionMilieu));

        // Logo se montre
        logo.SetActive(true);

        // Attendre le temps défini avant que le chat reparte
        yield return new WaitForSeconds(tempsAttenteAvantRetour);

        // Logo se cache
        logo.SetActive(false);

        // Chat continue jusqu au sortie
        yield return StartCoroutine(DeplacerChatVersPosition(positionFin));

        // quand il sort d'ecran, il est effacé
        imageChat.SetActive(false);

        // Cinema fini
        cinematiqueFinie = true;

        // Charger menu principal
        ChargerMenuPrincipal();
    }

    private IEnumerator DeplacerChatVersPosition(Vector3 positionCible)
    {
        while (Vector3.Distance(imageChat.transform.localPosition, positionCible) > 0.1f)
        {
            imageChat.transform.localPosition = Vector3.MoveTowards(imageChat.transform.localPosition, positionCible, vitesseMarche * Time.deltaTime);
            yield return null;
        }
    }

    private void PasserCinematique()
    {
        // Arrêter toutes les coroutines
        StopAllCoroutines();

        // Finir la cinéma
        cinematiqueFinie = true;

        // Charger menu principal
        ChargerMenuPrincipal();
    }

    private void ChargerMenuPrincipal()
    {
        // Charger la scène du menu principal 
        SceneManager.LoadScene("MenuPrincipal");
    }
}
