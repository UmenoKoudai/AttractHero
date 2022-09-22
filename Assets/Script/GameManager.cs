using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    /// <summary>Timer�\���̃e�L�X�g���i�[����ϐ�</summary>
    [SerializeField] Text _timerText;
    [SerializeField] Text _resultTimer;
    /// <summary>SetActive�Ŕ�\���ɂ���I�u�W�F�N�g���i�[����ϐ�</summary>
    [SerializeField] GameObject _close;
    /// <summary>SetActive�ŕ\��������I�u�W�F�N�g���i�[����ϐ�</summary>
    [SerializeField] GameObject _open;
    [SerializeField] string _sceneName;
    [SerializeField] PlayableDirector _endTimeLine;
    /// <summary>Player�X�N���v�g���i�[����ϐ�</summary>
    Player _playerScript;
    BossEnemy _enemyScript;
    ScneMove _scneMove;
    /// <summary>�Q�[���I��������true�ɂȂ��ē�����~�߂�</summary>
    bool _isFinish;
    /// <summary>_close�ϐ����v���p�e�B��</summary>
    public GameObject Close { get => _close; }
    /// <summary>_open�ϐ����v���p�e�B��</summary>
    public GameObject Open { get => _open; }
    /// <summary>timer��\�������邽�߂̕ϐ�</summary>
    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        //Player�X�N���v�g��ϐ��ɑ��
        _playerScript = GameObject.FindObjectOfType<Player>();
        _enemyScript = GameObject.FindObjectOfType<BossEnemy>();
        //Player�X�N���v�g��AllGameFinish�ϐ���GameFinish�ϐ���������
        _playerScript.AllGameFinish += GameFinish;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFinish)
        {
            _timer += Time.deltaTime;
            //timer������ꍇ�̓e�L�X�g���X�V
            if (_timerText)
            {
                _timerText.text = $"Time:{_timer.ToString("f2")}";
            }
            if (_playerScript.HP <= 0)
            {
                _playerScript.AllGameFinish();
                _close.gameObject.SetActive(false);
                _open.gameObject.SetActive(true);
            }
            if (FindObjectOfType<BossEnemy>())
            {
                if (_enemyScript.HP <= 0)
                {
                    _playerScript.AllGameFinish();
                    StartCoroutine(EndTimeLine());
                }
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
    //ScneMove�X�N���v�g��SetActive���\�b�h�����s
    public  void GameManagerSetActive()
    {
        if(Close || Open)
        {
            _scneMove = GameObject.FindObjectOfType<ScneMove>();
            _scneMove.SetActive(Close, Open);
        }
    }
    //�Q�[���I�����Ɏ��s�����
    void GameFinish()
    {
        _isFinish = true;
    }
    IEnumerator EndTimeLine()
    {
        _endTimeLine.Play();
        yield return new WaitForSeconds(12f);
        SceneManager.LoadScene(_sceneName);
    }
}
