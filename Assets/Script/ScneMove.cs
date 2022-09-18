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
    //シーンを移動させるソッド
    public　static void SceneMove(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void SetActive(GameObject Close, GameObject Open)
    {
        Close.gameObject.SetActive(false);
        Open.gameObject.SetActive(true);
    }
    public void SetActiveButton()
    {
        _close.gameObject.SetActive(false);
        _open.gameObject.SetActive(true);
    }
    public void SetActiveButton4()
    {
        _close.gameObject.SetActive(false);
        _open.gameObject.SetActive(true);
        _close2.gameObject.SetActive(false);
        _open2.gameObject.SetActive(true);
    }
}
