using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    public void PlaySound(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}