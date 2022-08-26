using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
    /// <summary>Rigidbody2D�̊i�[�ꏊ/</summary>
    Rigidbody2D _rb;
    /// <summary>�u���b�N�̃|�W�V���������Z�b�g���郁�\�b�h���i�[����f���Q�[�g</summary>
    public static event Action _positionReset;
    /// <summary>�W�����v����̕ϐ�/</summary>
    public bool _isGround;
    /// <summary>���E�ړ��̓��͂��i�[����ϐ�/</summary>
    float _x;
    //Vector2 _lineForGround = new Vector2(0f, -1.5f);
    /// <summary>�|�W�V�������Z�b�g�̃f���Q�[�g���v���p�e�B��</summary>
    public static Action PositionReset { get => _positionReset; set => _positionReset = value; }
    void Start()
    {
        //Rigidbody2D���i�[
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //�L�����̈ړ�(���E�ړ�)�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@
        _rb.velocity = new Vector2(_x * _moveSpeed, _rb.velocity.y);
        //�L�����̈ړ�(�W�����v)
        if(Input.GetButtonDown("Jump") && _isGround)
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }
    void Update()
    {
        //���E�̓��͂�ϐ��Ɋi�[
        _x = Input.GetAxisRaw("Horizontal");
        //�v���C���[����͕����Ɍ�����
        FlipX(_x);
        //Ray���΂��ׂ̃}�E�X�|�W�V�������擾
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //�J�����̈ʒu����}�E�X�̈ʒu�܂�Ray���΂�
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        Debug.DrawLine(ray.origin, ray.direction, Color.red);
        //Vector2 start = transform.position;
        //RaycastHit2D groundHit = Physics2D.Raycast(start, start + _lineForGround);
        //Debug.DrawLine(start, start + _lineForGround);
        //if (groundHit && groundHit.collider.tag != "GameArea")
        //{
        //    _isGround = true;
        //}
        //else
        //{
        //    _isGround = false;
        //}
        ////if (groundHit.collider.tag == "Ground" || groundHit.collider.tag == "Block" && groundHit.collider.tag != "Player")
        ////{
        ////    _isGround = true;
        ////}
        ////else
        ////{
        ////    _isGround = false;
        ////}
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
        if(Input.GetButtonDown("Cancel"))
        {
            _positionReset();
        }
    }
    //�v���C���[����͕����Ɍ����郁�\�b�h
    void FlipX(float X)
    {
        if(X > 0)
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
        else if(collision.tag =="Flag")
        {
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Block")
        {
            _isGround = false;
        }
    }
}
