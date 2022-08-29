using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>���E�ړ��̗�/</summary>
    [SerializeField] int _moveSpeed;
    /// <summary>�W�����v�̗�/</summary>
    [SerializeField] int _jumpPower;
    /// <summary>�u���b�N�����ɓ�������/</summary>
    [SerializeField] int _blockMoveSpeedLeft;
    /// <summary>�u���b�N���E�ɓ�������/</summary>
    [SerializeField] int _blockMoveSpeedRigit;
    /// <summary>Raycast�ŃI�u�W�F�N�g�𔻒肷�郌�C���[/</summary>
    [SerializeField] LayerMask _block;
    /// <summary>GameManager�X�N���v�g���i�[����ϐ�</summary>
    [SerializeField] GameManager _gameManager;
    /// <summary>Rigidbody2D�̊i�[�ꏊ/</summary>
    Rigidbody2D _rb;
    /// <summary>�u���b�N�̃|�W�V���������Z�b�g���郁�\�b�h���i�[����f���Q�[�g</summary>
    event Action _positionReset;
    /// <summary>�Q�[�����I�������Ƃ��ɓ������\�b�h���i�[����</summary>
    event Action _gameFinish;
    /// <summary>�W�����v����̕ϐ�/</summary>
    public bool _isGround;
    /// <summary>�Q�[���I��������true�ɂȂ��ē�����~�߂�</summary>
    bool _isFinish;
    /// <summary>���E�ړ��̓��͂��i�[����ϐ�/</summary>
    float _x;
    /// <summary>�|�W�V�������Z�b�g�̃f���Q�[�g���v���p�e�B��</summary>
    public Action PositionReset { get => _positionReset; set => _positionReset = value; }
    /// <summary>�Q�[���I���̃f���Q�[�g���v���p�e�B��</summary>
    public Action AllGameFinish { get => _gameFinish; set => _gameFinish = value; }
    void Start()
    {
        //Rigidbody2D���i�[
        _rb = GetComponent<Rigidbody2D>();
        //�ϐ���GameManager�X�N���v�g����
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        //�Q�[���I���̃f���Q�[�g�Ƀ��\�b�h����
        AllGameFinish += GameFinish;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_isFinish)
        {
            //�L�����̈ړ��ƌ�����ς���
            FlipX(Input.GetAxisRaw("Horizontal"));
            //�L�����̈ړ�(�W�����v)
            if (Input.GetButtonDown("Jump") && _isGround)
            {
                _isGround = false;
                _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            }
        }
    }
    void Update()
    {
        if (!_isFinish)
        {
            //Ray���΂��ׂ̃}�E�X�|�W�V�������擾
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //�J�����̈ʒu����}�E�X�̈ʒu�܂�Ray���΂�
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            Debug.DrawLine(ray.origin, ray.direction, Color.red);
            //�ړ��\�̃u���b�N�𓮂���(�v���C���[�̕����Ɉړ�����)
            if (Input.GetButtonDown("Fire1"))
            {
                //Ray�����������I�u�W�F�N�g��tag��Block�������珈�������s
                if (hit.collider.gameObject.tag == "Block")
                {
                    //�ϐ�Block��Ray�����������I�u�W�F�N�g����
                    GameObject Block = hit.transform.gameObject;
                    //Block�ɓ������I�u�W�F�N�g�ɓ����Ă���BlockController��ϐ��ɑ��
                    BlockController bloackMove = Block.GetComponent<BlockController>();
                    //BlockController�ɂ���Move���\�b�h�����s
                    bloackMove.Move(_blockMoveSpeedLeft);
                }
            }
            //�ړ��\�̃u���b�N�𓮂���(�v���C���[�Ƌt�����Ɉړ�����)
            if (Input.GetButtonDown("Fire2"))
            {
                //Ray�����������I�u�W�F�N�g��tag��Block�������珈�������s
                if (hit.collider.gameObject.tag == "Block")
                {
                    //�ϐ�Block��Ray�����������I�u�W�F�N�g����
                    GameObject Block = hit.transform.gameObject;
                    //Block�ɓ������I�u�W�F�N�g�ɓ����Ă���BlockController��ϐ��ɑ��
                    BlockController bloackMove = Block.GetComponent<BlockController>();
                    //BlockController�ɂ���Move���\�b�h�����s
                    bloackMove.Move(_blockMoveSpeedRigit);
                }
            }
            //�G�X�P�[�v�L�[(esc)����������u���b�N�̈ʒu�����Z�b�g�����
            if (Input.GetButtonDown("Cancel"))
            {
                _positionReset();
            }
        }
    }
    //�v���C���[����͕����Ɍ����郁�\�b�h
    void FlipX(float X)
    {
        _rb.velocity = new Vector2(X * _moveSpeed, _rb.velocity.y);
        if (X > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if(X < 0)
        {
            transform.localScale = new Vector2(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
    //�ڒn����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Block")
        {
            _isGround = true;
        }
        else if (collision.tag == "Flag")
        {
            _gameFinish();
            _gameManager.GameManagerSetActive();
        }
    }
    //�Q�[���I������bool�^��true�ɂ���
    void GameFinish()
    {
        _isFinish = true;
    }
}

