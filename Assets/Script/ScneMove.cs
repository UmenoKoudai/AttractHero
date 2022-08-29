using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScneMove : MonoBehaviour
{
    [SerializeField] GameObject _close;
    [SerializeField] GameObject _open;
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
}
