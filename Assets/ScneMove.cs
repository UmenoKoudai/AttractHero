using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScneMove : MonoBehaviour
{
    //シーンをどうするメソッド
    public void SceneMove(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
