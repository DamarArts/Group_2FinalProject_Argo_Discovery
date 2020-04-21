using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private int i;
    [SerializeField] private bool isPaused;

    public GameObject deathScreen, winScreen, pauseScreen, player;
    private PlayerMovement playerscript;
    void Start()
    {
        playerscript = player.GetComponent<PlayerMovement>();
        deathScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // player = GameObject.FindGameObjectWithTag("Player");
        if ((Input.GetKeyDown(KeyCode.P)) || (Input.GetKeyDown(KeyCode.Escape)))
        {
            if (!isPaused)
                Pause();
            else
                Unpause();
        }

        if (playerscript.isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            deathScreen.SetActive(true);
        }
        if (playerscript.gameWon == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winScreen.SetActive(true);
        }

    }
   public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        isPaused = true;
    }

    public void Unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        isPaused = false;
    }
}
