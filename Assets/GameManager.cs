using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text _timerText;
    [SerializeField] GameObject _close;
    [SerializeField] GameObject _open;
    [SerializeField] GameStat _stat = GameStat.Playing;
    public  GameObject Close { get => _close; }
    public  GameObject Open { get => _open; }
    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_stat == GameStat.Finish)
        {
            _timer += Time.deltaTime;
            if (_timerText)
            {
                _timerText.text = $"Time:{_timer.ToString("f2")}";
            }
        }
    }
    public  void GameManagerSetActive()
    {
        if(Close || Open)
        {
            ScneMove.SetActive(Close, Open);
        }
    }
}
enum GameStat
{
    Playing,
    Finish,
}
