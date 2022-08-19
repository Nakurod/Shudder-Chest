using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class WristMenu : MonoBehaviour
{

    [SerializeField] private GameObject[] enemySpawners;

    [SerializeField] private GameObject wristUI;
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool isPaused = false;
    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        DisplayWristUI();
        ToggleGame();
    }

    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayWristUI();
        }
    }

    public void DisplayWristUI()
    {
        if (isActive)
        {
            wristUI.SetActive(false);
            isActive = false;
        }
        else if (!isActive)
        {
            wristUI.SetActive(true);
            isActive = true;
        }
    }

    public void ToggleGame()
    {
        if (!isPaused)
        {
            foreach (var spawner in enemySpawners)
            {
                spawner.SetActive(false);
                foreach (var enemy in spawner.GetComponent<EnemySpawner>().spawnedEnemies)
                {
                    enemy.Die();
                }
                spawner.GetComponent<EnemySpawner>().spawnedEnemies.Clear();
            }
            text.text = "Paused!";
            isPaused = true;
        }
        else if (isPaused)
        {
            foreach (var spawner in enemySpawners)
            {
                spawner.SetActive(true);
            }
            text.text = "Started!";
            isPaused = false;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
