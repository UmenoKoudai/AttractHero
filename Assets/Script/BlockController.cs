using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlockController : MonoBehaviour
{
    /// <summary>�v���C���[�̏ꏊ���擾</summary>
    [SerializeField] GameObject _player;
    [SerializeField] int _moveSpeed;
    [SerializeField] bool _scaffoldBlock = false;
    [SerializeField] bool _bulletBlock = false;
    /// <summary>Rigidbody2D���i�[����ꏊ</summary>
    Rigidbody2D _rb;
    /// <summary>�v���C���[�X�N���v�g��������^</summary>
    Player _playerScript;
    /// <summary>�u���b�N�̐Î~�ƈړ��̔���</summary>
    bool _isBlockMove;
    /// <summary>�Q�[���I��������true�ɂȂ��ē�����~�߂�</summary>
    bool _isFinish;
    /// <summary>�u���b�N�̌��̈ʒu���L�^����^</summary>
    Vector3 _basePositon;
    void Start()
    {
        //�v���C���[�X�N���v�g����
        _playerScript = GameObject.FindObjectOfType<Player>();
        //�ŏ��̃|�W�V������ϐ��ɑ��
        _basePositon = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        //Player�X�N���v�g��PositionReset�ϐ���BlockPositionReset���\�b�h����
        _playerScript.PositionReset += BlockPositionReset;
        //Player�X�N���v�g��AllGameFinish�ϐ���GameFinish���\�b�h����
        _playerScript.AllGameFinish += GameFinish;
    }
    //�u���b�N���ړ������邽�߂̃��\�b�h
    public void Move(bool MoveSwich)
    {
        if (!_isFinish)
        {
            //����1�x���s�����ƐÎ~�ƈړ��̔��肪�t�ɂȂ�
            _isBlockMove = !_isBlockMove;
            
            //_isBlockMove��true�̏ꍇ�̓v���C���[�Ɍ������Ĉړ�����
            if (_isBlockMove)
            {
                //bodyType��Dynamic�ɂȂ�
                _rb.bodyType = RigidbodyType2D.Dynamic;
                //�v���C���[�Ɍ������Ĉړ�����
                _rb.velocity = (_player.transform.position - transform.position).normalized * _moveSpeed;
            }
            //_isBlockMove��false�̏ꍇ�͈ړ����Î~����
            else
            {
                //bodyType��Kinematic�ɂȂ�(�v���C���[���������Ă������Ȃ��Ȃ�)
                _rb.bodyType = RigidbodyType2D.Kinematic;
                //�ړ��̗�(�x�N�g��)���[���ɂȂ�
                _rb.velocity = Vector2.zero;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (_bulletBlock)
            {
                _playerScript.BulletList.Add(gameObject);
            }
            else if (_scaffoldBlock)
            {
                _playerScript.ScaffoldBlockList.Add(gameObject);
            }
            Destroy(gameObject);
        }
    }
    //�u���b�N�̃|�W�V������Reset
    void BlockPositionReset()
    {
        transform.position = _basePositon;
    }
    //�Q�[���I�����Ɏ��s
    void GameFinish()
    {
        _isFinish = true;
    }
}
