using UnityEngine;
using static GameManager;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip hoverSound;
    [SerializeField]
    private AudioClip clickSound;

    public void OnHover()
    {
        gm.audioSource.PlayOneShot(hoverSound, 0.1f);
    }

    public void OnClick()
    {
        gm.audioSource.PlayOneShot(clickSound, 0.1f);
    }
}
