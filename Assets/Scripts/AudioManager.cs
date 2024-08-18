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
        musiqueSource.clip = musiqueClassique;
        musiqueSource.Play();
        Debug.Log("La musique est utilise");
    }

    public void JouerVictoire()
    {
        musiqueSource.clip = victoire;
        musiqueSource.Play();
        Debug.Log("La musique de Victoire est utilise");
    }

    public void JouerDefaite()
    {
        musiqueSource.clip = defaite;
        musiqueSource.Play();
        Debug.Log("La musique de Defaite est utilise");
    }

    public void ChangerVolumeMusique(float volume)
    {
        audioMixer.SetFloat("MusiqueVolume", volume);
    }

    public void ChangerVolumeEffets(float volume)
    {
        audioMixer.SetFloat("EffetsVolume", volume);
    }
}
