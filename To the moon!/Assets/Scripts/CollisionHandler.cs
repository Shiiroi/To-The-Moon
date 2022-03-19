using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadSceneDelay = 1f;
    [SerializeField] AudioClip Crash;
    [SerializeField] AudioClip Success;
    [SerializeField] ParticleSystem CrashParticles;
    [SerializeField] ParticleSystem SuccessParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisable = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisable) { return;}
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
               StartNextLevelSequence();
                break;
            default:
                StartCrashingSequence();
                break;
        }
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; //toggle on and off
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void StartCrashingSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        CrashParticles.Play();
        audioSource.PlayOneShot(Crash);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadScene",loadSceneDelay);
    }
    void StartNextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        SuccessParticles.Play();
        audioSource.PlayOneShot(Success);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadSceneDelay);
    }
}
