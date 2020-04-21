using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    public MenuController MenuController;
    public void SetRestart()
    {
        SceneManager.LoadScene(1);
    }
    public void SetMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void SetResume()
    {
        MenuController.Unpause();
    }
    public void SetExit()
    {
        Application.Quit();
    }


}
