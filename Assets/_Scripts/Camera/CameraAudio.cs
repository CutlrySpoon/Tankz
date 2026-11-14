using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraAudio : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = FindAnyObjectByType<Camera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = FindAnyObjectByType<Camera>();
    }

    private void FixedUpdate()
    {
        transform.position = cam.transform.position;
    }
}
