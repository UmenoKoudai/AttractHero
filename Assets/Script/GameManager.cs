using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>Timer�\���̃e�L�X�g���i�[����ϐ�</summary>
    [SerializeField] Text _timerText;
    [SerializeField] Text _resultTimer;
    /// <summary>SetActive�Ŕ�\���ɂ���I�u�W�F�N�g���i�[����ϐ�</summary>
    [SerializeField] GameObject _close;
    /// <summary>SetActive�ŕ\��������I�u�W�F�N�g���i�[����ϐ�</summary>
    [SerializeField] GameObject _open;
    [SerializeField] Text _clearCountText;
    /// <summary>Player�X�N���v�g���i�[����ϐ�</summary>
    Player _playerScript;
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
            if(_clearCountText)
            {
                _clearCountText.text = $"�c��A�C�e��:{_playerScript.ItemCount}/{_playerScript.ClearCount}";
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
}
