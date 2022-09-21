using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using Cinemachine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("�v���C���[�̓���")]
    [SerializeField] GameObject _barral;
    /// <summary>���E�ړ��̗�/</summary>
    [SerializeField] int _moveSpeed;
    [SerializeField] int _defaultSpeed;
    [SerializeField] int _dushSpeed;
    /// <summary>�W�����v�̗�/</summary>
    [SerializeField] int _jumpPower;
    /// <summary>�ڒn����̎���Ray���΂�����</summary>
    [SerializeField] float _isGroundedLength = 1f;
    [SerializeField] AudioSource _magicPlayAudio;
    [SerializeField] AudioSource _itemGetAudio;
    [SerializeField] AudioSource _ballGetAudio;
    [SerializeField] ParticleSystem _magicEffect;
    [SerializeField] Transform _jumpEffect;
    [SerializeField] Transform _groundEffect;
    [SerializeField] GameObject _playerLife;
    [SerializeField] GameObject _breakLife;

    [Header("�Q�[���V�X�e���֌W")]
    /// <summary>Raycast�ŃI�u�W�F�N�g�𔻒肷�郌�C���[/</summary>
    [SerializeField] LayerMask _groundLayer = 0;
    /// <summary>GameManager�X�N���v�g���i�[����ϐ�</summary>
    [SerializeField] GameManager _gameManager;
    /// <summary>�N���A���邽�߂ɕK�v�ȃJ�E���g</summary>
    [SerializeField] int _clearCount;
    [SerializeField] CinemachineVirtualCameraBase _wholeCamera;
    

    [Header("���̑�")]
    [SerializeField] Transform _muzzlePosition;
    /// <summary>�擾��������̐���\������e�L�X�g</summary>
    [SerializeField] Text _moveBlockCountText;
    /// <summary>�擾�����e�̐���\������e�L�X�g</summary>
    [SerializeField] Text _bulletBlockCountText;
    /// <summary>����I�u�W�F�N�g���A�^�b�`</summary>
    [SerializeField] GameObject _scaffold;
    /// <summary>�e�I�u�W�F�N�g���A�^�b�`</summary>
    [SerializeField] GameObject _bullet;
    /// <summary>�J�[�\���̈ʒu���擾</summary>
    [SerializeField] Transform _cursor;
    /// <summary>����u���b�N���i�[����List</summary>
    [SerializeField] List<GameObject> _scaffoldBlockList = new List<GameObject>();
    /// <summary>�e�u���b�N���i�[����List</summary>
    [SerializeField] List<GameObject> _bulletList = new List<GameObject>();
    [SerializeField] List<GameObject> _lifeList = new List<GameObject>();

    /// <summary>Rigidbody2D�̊i�[�ꏊ/</summary>
    Rigidbody2D _rb;
    /// <summary>�Q�[���X�^�[�g�̃|�W�V�������L�^���邽�߂̕ϐ�</summary>
    Vector3 _basePosition;
    Animator _anim;
    /// <summary>�u���b�N�̃|�W�V���������Z�b�g���郁�\�b�h���i�[����f���Q�[�g</summary>
    event Action _positionReset;
    /// <summary>�Q�[�����I�������Ƃ��ɓ������\�b�h���i�[����</summary>
    event Action _gameFinish;
    /// <summary>�W�����v����̕ϐ�/</summary>
    bool _isGround;
    /// <summary>�Q�[���I��������true�ɂȂ��ē�����~�߂�</summary>
    bool _isFinish;
    bool _isMagic;
    /// <summary>�A�C�e���擾�ŃJ�E���g��������</summary>
    int _itemCount;
    bool _isWholeCamera;
    int _hp = 5;

    /// <summary>�|�W�V�������Z�b�g�̃f���Q�[�g���v���p�e�B��</summary>
    public Action PositionReset { get => _positionReset; set => _positionReset = value; }
    /// <summary>�Q�[���I���̃f���Q�[�g���v���p�e�B��</summary>
    public Action AllGameFinish { get => _gameFinish; set => _gameFinish = value; }
    /// <summary>����u���b�N��List���v���p�e�B</summary>
    public List<GameObject> ScaffoldBlockList { get => _scaffoldBlockList; set => _scaffoldBlockList = value; }
    /// <summary>�e�u���b�N��List���v���p�e�B</summary>
    public List<GameObject> BulletList { get => _bulletList; set => _bulletList = value; }
    public int HP { get => _hp; }


    void Start()
    {
        //Rigidbody2D���i�[
        _rb = GetComponent<Rigidbody2D>();
        //�ϐ���GameManager�X�N���v�g����
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        //�Q�[���I���̃f���Q�[�g�Ƀ��\�b�h����
        AllGameFinish += GameFinish;
        //�|�W�V�������Z�b�g�̃f���Q�[�g�Ƀ��\�b�h����
        PositionReset += PlayerPositionReset;
        //�X�^�[�g�|�W�V�������L�^����
        _basePosition = transform.position;
        _anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!_isFinish)
        {
            _isMagic = false;
            if (_moveBlockCountText)
            {
                _moveBlockCountText.text = $"����:{_scaffoldBlockList.Count}";
            }
            if (_bulletBlockCountText)
            {
                _bulletBlockCountText.text = $"�e��:{_bulletList.Count}";
            }
            //�L�����̈ړ��ƌ�����ς���
            FlipX(Input.GetAxisRaw("Horizontal"));
            //�L�����̈ړ�(�W�����v)
            if (Input.GetButtonDown("Jump"))
            {
                //IsGround�֐���
                if (IsGround())
                {
                    _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                }
            }
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
                    bloackMove.Move(true);
                }
                else
                {
                    if(_scaffoldBlockList != null)
                    {
                        if (_scaffoldBlockList.Count > 0)
                        {
                            _isMagic = true;
                            GameObject ScaffoldBlock = Instantiate(_scaffold);
                            ScaffoldBlock.transform.position = _cursor.position;
                            _scaffoldBlockList.RemoveAt(0);
                            _magicPlayAudio.Play();
                            if (IsGround())
                            {
                                Instantiate(_magicEffect, _groundEffect.position, transform.rotation);
                            }
                            else
                            {
                                Instantiate(_magicEffect, _jumpEffect.position, transform.rotation);
                            }
                        }
                    }
                }
            }
            //�ړ��\�̃u���b�N�𓮂���(�v���C���[�Ƌt�����Ɉړ�����)
            if (Input.GetButtonDown("Fire2"))
            {
                if (_bulletList != null)
                {
                    if (_bulletList.Count > 0)
                    {
                        _isMagic = true;
                        GameObject BulletBlock = Instantiate(_bullet);
                        BulletBlock.transform.position = _muzzlePosition.position;
                        _bulletList.RemoveAt(0);
                        _magicPlayAudio.Play();
                        if(IsGround())
                        {
                            Instantiate(_magicEffect, _groundEffect.position, transform.rotation);
                        }
                        else
                        {
                            Instantiate(_magicEffect, _jumpEffect.position, transform.rotation);
                        }
                    }
                }
            }
            if(Input.GetButtonDown("Fire3"))
            {
                StartCoroutine(Dush());
            }
            //�G�X�P�[�v�L�[(esc)����������u���b�N�̈ʒu�����Z�b�g�����
            if (Input.GetButtonDown("Cancel"))
            {
                _positionReset();
            }
            if (_wholeCamera)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    _isWholeCamera = !_isWholeCamera;
                    if (_isWholeCamera)
                    {
                        _wholeCamera.Priority = 11;
                    }
                    else
                    {
                        _wholeCamera.Priority = 9;
                    }
                }
            }
        }
    }
    IEnumerator Dush()
    {
        _moveSpeed = _dushSpeed;
        yield return new WaitForSeconds(0.5f);
        _moveSpeed = _defaultSpeed;
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
        //Flag�ɓ����������A�C�e���J�E���g���N���A�J�E���g�Ɠ����ɂȂ��Ă����Result�\��
        if (collision.tag == "Flag" && _itemCount == _clearCount)
        {
            _gameFinish();
            _gameManager.GameManagerSetActive();
        }
        //Ball�^�O�̃I�u�W�F�N�g�ɓ���������A�C�e���J�E���g�𑝂₷
        if(collision.tag == "Ball")
        {
            _itemCount++;
            _ballGetAudio.Play();
            Destroy(collision.gameObject);
        }
        if(collision.tag == "Block")
        {
            Debug.Log("�A�C�e���Q�b�g");
            _itemGetAudio.Play();
        }
        if(collision.tag == "EnemyAttack")
        {
            if (_lifeList.Count >= 0)
            {
                _hp -= 1;
                Destroy(_lifeList[0]);
                _lifeList.RemoveAt(0);
                GameObject breakLife = Instantiate(_breakLife);
                breakLife.transform.SetParent(_playerLife.transform);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            if (_lifeList.Count >= 0)
            {
                _hp -= 1;
                Destroy(_lifeList[0]);
                _lifeList.RemoveAt(0);
                GameObject breakLife = Instantiate(_breakLife);
                breakLife.transform.SetParent(_playerLife.transform);
            }
        }
    }
    private void LateUpdate()
    {
        _anim.SetFloat("Run", Mathf.Abs(_rb.velocity.x));
        _anim.SetFloat("Jump", _rb.velocity.y);
        _anim.SetBool("Magic", _isMagic);
    }
    bool IsGround()
    {
        //�w��̃��C���[�I�u�W�F�N�g��Ray���������Ă��Ȃ����false��Ԃ�
        _isGround = false;
        //�w��̃��C���[�I�u�W�F�N�g��Ray���������Ă����true��Ԃ�
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, _isGroundedLength, _groundLayer);
        Debug.DrawRay(this.transform.position, Vector2.down * _isGroundedLength);
        if (hit.collider)
        {
            _isGround = true;
        }
        return _isGround;
    }
    //�Q�[���I������bool�^��true�ɂ���
    void GameFinish()
    {
        _isFinish = true;
    }

    //�|�W�V�������Z�b�g�̊֐�
    void PlayerPositionReset()
    {
        transform.position = _basePosition;
    }
}

