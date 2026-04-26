using UnityEngine;
using UnityEngine.UI;

public class SongProgressBar : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider progressBar;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;
        }
    }

    void Update()
    {
        if (audioSource != null && audioSource.clip != null && audioSource.isPlaying)
        {
            // current time / total song time
            progressBar.value = audioSource.time / audioSource.clip.length;
        }
    }
}