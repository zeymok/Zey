using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;
    public AudioSource musiqueSource;
    public AudioSource effetSource;
    public AudioClip musiqueClassique;
    public AudioClip victoire;
    public AudioClip defaite;

    void Awake()
    {
        // un constructeur
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void JouerMusiqueClassique()
    {
        // une instance de classe
        musiqueSource.clip = musiqueClassique;
        musiqueSource.Play();
        Debug.Log("La musique est utilise");
    }

    public void JouerVictoire()
    {
        // une instance de classe
        effetSource.clip = victoire;
        effetSource.Play();
        Debug.Log("La musique de Victoire est utilise");
    }

    public void JouerDefaite()
    {
        // une instance de classe
        effetSource.clip = defaite;
        effetSource.Play();
        Debug.Log("La musique de Defaite est utilise");
    }

    public void ChangerVolumeMusique(float volume)
    {
        // une instance de classe
        audioMixer.SetFloat("MusiqueVolume", volume);
    }

    public void ChangerVolumeEffets(float volume)
    {
        // une instance de classe
        audioMixer.SetFloat("EffetsVolume", volume);
    }
}
