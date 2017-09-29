using UnityEngine;
using System.Collections;

public class AudioAnimation : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private AudioSource source;

    private void Start()
    {
        this.source = this.GetComponent<AudioSource>();
    }

    public void Play(int value)
    {
        if (value < this.clips.Length)
        {
            if (this.source.clip != this.clips[value] || !this.source.isPlaying)
            {
                this.source.clip = this.clips[value];
                this.source.Play();
            }
        }
        else
            Debug.LogError("O valor está fora do limite do array no objeto " + this.name);
    }
}
