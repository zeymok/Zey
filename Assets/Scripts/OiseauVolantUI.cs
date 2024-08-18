using UnityEngine;

public class OiseauVolantUI : MonoBehaviour
{
    public Vector2 pointGauche = new Vector2(-810, 490);  // Position à gauche
    public Vector2 pointDroit = new Vector2(810, 490);    // Position à droite
    public float vitesse = 200.0f; // Vitesse de l'oiseau

    private RectTransform rectTransform; 
    private Vector2 destination;
    private bool versGauche = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // go la position de départ de l'oiseau
        rectTransform.anchoredPosition = pointGauche;
        // go au destination au point droit
        destination = pointDroit;
    }

    void Update()
    {
        // Déplacer l'oiseau vers la destination
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, destination, vitesse * Time.deltaTime);

        // Vérifier si l'oiseau est arrivé à la destination
        if (Vector2.Distance(rectTransform.anchoredPosition, destination) < 0.1f)
        {
            // Si l'oiseau est en route vers gauche, mtt il go vers droite
            if (versGauche)
            {
                destination = pointDroit;
                versGauche = false;
                // Modifier Pos X d'oiseau pour il regarde vers la direction ( droite)
                rectTransform.localScale = new Vector3(Mathf.Abs(rectTransform.localScale.x), rectTransform.localScale.y, rectTransform.localScale.z);
            }
            // Si l'oiseau est en route vers droite, mtt il go vers gauche
            else
            {
                destination = pointGauche;
                versGauche = true;
                // Modifier Pos X d'oiseau pour il regarde vers la direction (gauche)
                rectTransform.localScale = new Vector3(-Mathf.Abs(rectTransform.localScale.x), rectTransform.localScale.y, rectTransform.localScale.z);
            }
        }
    }
}
