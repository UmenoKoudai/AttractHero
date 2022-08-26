using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScneMove : MonoBehaviour
{
    //シーンを移動させるソッド
    public　static void SceneMove(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public static void SetActive(GameObject Close, GameObject Open)
    {
        Close.gameObject.SetActive(false);
        Open.gameObject.SetActive(true);
    }
}
