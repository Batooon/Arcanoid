using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    public void PlaySound()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.Play();
    }
}