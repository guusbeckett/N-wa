using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickToPlay : MonoBehaviour {
    public GameObject MusicController;  //Fade this one out
    public GameObject optionsText;      //Move this out of view
    public GameObject exitText;         //This too
    public GameObject player_still;     //And blast this one off to space
    //Local bool to stop repeatedly setting off the animations
    bool isClicked = false;

    public void LoadScene()
    {
        if (isClicked)
            return;
        isClicked = true;
        Animator optionsAnimator = optionsText.GetComponent<Animator>();
        Animator exitAnimator = exitText.GetComponent<Animator>();
        Animator playerAnimator = player_still.GetComponent<Animator>();
        StartCoroutine("FadeOutCoroutine");
        optionsAnimator.SetTrigger("Remove_self");
        playerAnimator.SetTrigger("Blastoff");
        exitAnimator.SetTrigger("Remove_self");
        StartCoroutine("WaitToStart");
    }

    public IEnumerator FadeOutCoroutine()
    {
        AudioSource MusicControl = MusicController.GetComponent<AudioSource>();
        while (MusicControl.volume > 0)
        {
            MusicControl.volume -= 0.015f;
            yield return new WaitForSeconds(0.015f);
        }
    }

    public IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(1);
    }
}
