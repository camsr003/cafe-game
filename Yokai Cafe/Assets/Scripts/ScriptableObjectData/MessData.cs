using UnityEngine;

[CreateAssetMenu(menuName = "Cleaning/Mess")]
public class MessData : ScriptableObject
{
    public string messName;
    public GameObject messPrefab;
    public float cleanTime = 1f;
    //public int scoreValue = 10;
    //public ParticleSystem cleanEffect;
    //public AudioClip cleanSound;
}
