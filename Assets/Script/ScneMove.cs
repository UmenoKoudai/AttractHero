using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScneMove : MonoBehaviour
{
    [SerializeField] GameObject _close;
    [SerializeField] GameObject _close2;
    [SerializeField] GameObject _open;
    [SerializeField] GameObject _open2;
    [SerializeField] AudioSource _clickButton;
    //シーンを移動させるソッド
    public void SceneMove(string SceneName)
    {
        StartCoroutine(AudioPlay(SceneName));
    }
    public void SetActive(GameObject Close, GameObject Open)
    {
        Close.gameObject.SetActive(false);
        Open.gameObject.SetActive(true);
    }
    public void SetActiveButton()
    {
        _clickButton.Play();
        _close.gameObject.SetActive(false);
        _open.gameObject.SetActive(true);
    }
    public void SetActiveButton4()
    {
        _clickButton.Play();
        _close.gameObject.SetActive(false);
        _open.gameObject.SetActive(true);
        _close2.gameObject.SetActive(false);
        _open2.gameObject.SetActive(true);
    }
    IEnumerator AudioPlay(string SceneName)
    {
        _clickButton.Play();
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneName);
    }
}
