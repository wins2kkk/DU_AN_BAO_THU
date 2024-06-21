using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Sting : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public void newgame()
    {
        SceneManager.LoadScene("Game");// load scene: truyền vào tên màn
    }
    public void pause()
    {
        Time.timeScale = 0f; // Freeze game time
        GameIsPaused = true;
    }
    public void resume()
    {
        Time.timeScale = 1f; // Resume game time
        GameIsPaused = false;
    }
    public void exitGame()
    {
        Application.Quit(); // thoát game
    }
    // cai dat
    public GameObject panel;
    public GameObject panelrank;
  
    public void htoption()
    {
        panel.SetActive(true);
    //      panelrank.SetActive(false);
        pause();
    }
    // an di option
    public void anoption()
    {
        panel.SetActive(false);    
        resume();
    }
    public void htrank()
    {
        panelrank.SetActive(true);
        panel.SetActive(false);
        pause();

    }
    public void anrank()
    {
        panelrank.SetActive(false );
        resume();
        
    }

    public GameObject setingpanel;
    public GameObject loginpanel;

    public void htseting()
    {
        setingpanel.SetActive(true);
        loginpanel.SetActive(false);
    }
    public void anseting()
    {
        setingpanel.SetActive(false);
    }
    public void htLg()
    {
        loginpanel.SetActive(true);
        setingpanel.SetActive(false);
    }
    public void anLg()
    {
        loginpanel.SetActive(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f; // Resume game time
        GameIsPaused = false;
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(0);
    }
    public void BG()
    {
        SceneManager.LoadScene(1);
    }
    public void Restart()
    {
        
        Time.timeScale = 1f; // Ensure time scale is reset before reloading the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        GameIsPaused = false;
    }
    private SoundManager soundManager;
    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    public string manChoi;
    public void man()
    {
        SceneManager.LoadScene(manChoi);
        //soundManager.PlaySFX(soundManager.next);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            man();
        }
    }
}
