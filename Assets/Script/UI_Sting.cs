using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Sting : MonoBehaviour
{
    public void newgame()
    {
        SceneManager.LoadScene("Game");// load scene: truyền vào tên màn
    }
    public void pause()
    {
        Time.timeScale = 0;
    }
    public void resume()
    {
        Time.timeScale = 1.0f;
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

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        panel.SetActive(false);
        SceneManager.LoadScene(2);
        resume();
    }


    public string manChoi;
    public void man()
    {
        SceneManager.LoadScene(manChoi);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            man();
        }
    }
}
