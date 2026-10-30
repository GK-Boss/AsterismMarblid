using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip audioClip;
    public bool off;
    public float volume;
    public float pitch;

}
