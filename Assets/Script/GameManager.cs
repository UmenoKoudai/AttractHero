using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>Timer表示のテキストを格納する変数</summary>
    [SerializeField] Text _timerText;
    [SerializeField] Text _resultTimer;
    /// <summary>SetActiveで非表示にするオブジェクトを格納する変数</summary>
    [SerializeField] GameObject _close;
    /// <summary>SetActiveで表示させるオブジェクトを格納する変数</summary>
    [SerializeField] GameObject _open;
    /// <summary>Playerスクリプトを格納する変数</summary>
    Player _playerScript;
    ScneMove _scneMove;
    /// <summary>ゲーム終了したらtrueになって動作を止める</summary>
    bool _isFinish;
    /// <summary>_close変数をプロパティ化</summary>
    public GameObject Close { get => _close; }
    /// <summary>_open変数をプロパティ化</summary>
    public GameObject Open { get => _open; }
    /// <summary>timerを表示させるための変数</summary>
    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        //Playerスクリプトを変数に代入
        _playerScript = GameObject.FindObjectOfType<Player>();
        //PlayerスクリプトのAllGameFinish変数にGameFinish変数を代入する
        _playerScript.AllGameFinish += GameFinish;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFinish)
        {
            _timer += Time.deltaTime;
            //timerがある場合はテキストを更新
            if (_timerText)
            {
                _timerText.text = $"Time:{_timer.ToString("f2")}";
            }
        }
        else
        {
            if(_resultTimer)
            {
                _resultTimer.text = $"Time{_timer.ToString("f2")}";
            }
        }
    }
    //ScneMoveスクリプトのSetActiveメソッドを実行
    public  void GameManagerSetActive()
    {
        if(Close || Open)
        {
            _scneMove = GameObject.FindObjectOfType<ScneMove>();
            _scneMove.SetActive(Close, Open);
        }
    }
    //ゲーム終了時に実行される
    void GameFinish()
    {
        _isFinish = true;
    }
}
