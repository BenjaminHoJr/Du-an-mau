using UnityEngine;

public class PasueGameController : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf == false)
            {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f; // Resume the game
            }
        }
    }
}
