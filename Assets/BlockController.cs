using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlockController : MonoBehaviour
{
    /// <summary>�v���C���[�̏ꏊ���擾</summary>
    [SerializeField] Transform _player;
    /// <summary>Rigidbody2D���i�[����ꏊ</summary>
    Rigidbody2D _rb;
    Player _playerScript;
    /// <summary>�u���b�N�̐Î~�ƈړ��̔���</summary>
    bool _isBlockMove;
    Vector3 _basePositon;
    // Start is called before the first frame update
    void Start()
    {
        _playerScript = GameObject.FindObjectOfType<Player>();
        //�ŏ��̃|�W�V������ϐ��ɑ��
        _basePositon = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        //Player�X�N���v�g��PositionReset�ϐ���BlockPositionReset���\�b�h����
        _playerScript.PositionReset += BlockPositionReset;
    }
    //�u���b�N���ړ������邽�߂̃��\�b�h
    public void Move(int MoveSpeed)
    {
        //����1�x���s�����ƐÎ~�ƈړ��̔��肪�t�ɂȂ�
        _isBlockMove = !_isBlockMove;
        //_isBlockMove��true�̏ꍇ�̓v���C���[�Ɍ������Ĉړ�����
        if (_isBlockMove)
        {
            //bodyType��Dynamic�ɂȂ�
            _rb.bodyType = RigidbodyType2D.Dynamic;
            //�v���C���[�Ɍ������Ĉړ�����
            _rb.velocity = (_player.position - transform.position).normalized * MoveSpeed;
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
    //�u���b�N�̃|�W�V������Reset
    void BlockPositionReset()
    {
        transform.position = _basePositon;
    }
}
