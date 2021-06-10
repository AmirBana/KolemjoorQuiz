using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Animator transition;
    public float Transitiontime = 2f;
    public void LoadScene(string scene) {
        StartCoroutine("LoadCross", scene);
    }
    IEnumerator LoadCross(string NewScene)
    {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(Transitiontime);
        SceneManager.LoadScene(NewScene);

    }

}