using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPausaManager : MonoBehaviour
{
   public static bool JuegoPausa = false;

    public GameObject MenuPausaUI;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausa)
            {
                Resume();
            }
            else
            {
                Pause();
            
            }   
        }
    }
    public void Resume()
    {
        MenuPausaUI.SetActive(false);
        Time.timeScale = 1f;
        JuegoPausa = false;
    }

    public void Pause()
    {
        MenuPausaUI.SetActive(true);
        Time.timeScale = 0.0f;
        JuegoPausa = true;
    }
}
