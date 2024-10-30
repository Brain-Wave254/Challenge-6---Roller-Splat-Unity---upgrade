using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton; // Singleton instance

    private GroundPiece[] allGroundPieces; // Ground pieces

    private void Start()
    {
        SetupNewLevel(); // Initialize level
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>(); // Find pieces
    }

    private void Awake()
    {
        if (singleton == null)
            singleton = this; // Set instance
        else if (singleton != this)
            Destroy(gameObject); // Destroy duplicate

        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading; // Scene loaded
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel(); // Reset level
    }

    public void CheckComplete()
    {
        bool isFinished = true; // Completion status

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false; // Not complete
                break; // Exit loop
            }
        }

        if (isFinished)
            NextLevel(); // Load next
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Next scene
    }
}
