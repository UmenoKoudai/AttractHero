using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScneMove : MonoBehaviour
{
    //�V�[�����ǂ����郁�\�b�h
    public void SceneMove(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
