using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class BossEnemy : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Transform _muzzle;
    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _bladeAttack1;
    [SerializeField] GameObject _bladeAttack2;
    [SerializeField] GameObject _bladeAttack3;
    [SerializeField] Slider _enemyLife;
    [SerializeField] int _moveSpeed;
    [SerializeField] float _distance;
    [SerializeField] float _intarval;
    Rigidbody2D _rb;
    Animator _anima;
    Player _playerScript;
    Vector2 _velocity;
    float _timer;
    int _randomN = 1;
    int _hp = 10;
    bool _isFinish;

    public int HP { get => _hp; }
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
        _playerScript = GameObject.FindObjectOfType<Player>();
        _playerScript.AllGameFinish += GameFinish;
        _velocity = _rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFinish)
        {
            _timer += Time.deltaTime;
            float dir = Vector2.Distance(transform.position, _player.position);
            if (dir >= _distance)
            {
                if (transform.position.x >= _player.position.x)
                {
                    _velocity = Vector2.left * _moveSpeed;
                    FlipX(Vector2.left.x);
                    _anima.SetFloat("Run", Mathf.Abs(_velocity.x));
                }
                if (transform.position.x <= _player.position.x)
                {
                    _velocity = Vector2.right * _moveSpeed;
                    FlipX(Vector2.right.x);
                    _anima.SetFloat("Run", Mathf.Abs(_velocity.x));
                }
                if (_timer >= _intarval)
                {
                    _velocity.x = 0;
                    _anima.SetFloat("Run", Mathf.Abs(_velocity.x));
                    StartCoroutine(BulletAttack());
                    _timer = 0;
                }
            }
            else
            {
                _velocity.x = 0;
                _anima.SetFloat("Run", Mathf.Abs(_velocity.x));
                switch (_randomN)
                {
                    case 1:
                        if (_timer >= _intarval)
                        {
                            StartCoroutine(BladeAttack1());
                            _timer = 0;
                        }
                        _randomN = Random.Range(1, 4);
                        break;
                    case 2:
                        if (_timer >= _intarval)
                        {
                            StartCoroutine(BladeAttack2());
                            _timer = 0;
                        }
                        _randomN = Random.Range(1, 4);
                        break;
                    case 3:
                        if (_timer >= _intarval)
                        {
                            StartCoroutine(BladeAttack3());
                            _timer = 0;
                        }
                        _randomN = Random.Range(1, 4);
                        break;
                }
            }
        }
    }
    //プレイヤーを入力方向に向けるメソッド
    void FlipX(float X)
    {
        _rb.velocity = new Vector2(X * _moveSpeed, _rb.velocity.y);
        if (X < 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (X > 0)
        {
            transform.localScale = new Vector2(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
    void GameFinish()
    {
        _isFinish = true;
    }
    
    IEnumerator BulletAttack()
    {
        _anima.SetBool("BulletAttack", true);
        yield return new WaitForSeconds(0.1f);
        Instantiate(_bullet, _muzzle.position, transform.rotation);
        _anima.SetBool("BulletAttack", false);
    }
    IEnumerator BladeAttack1()
    {
        _bladeAttack1.gameObject.SetActive(true);
        _anima.SetBool("BladeAttack1", true);
        yield return new WaitForSeconds(0.5f);
        _bladeAttack1.gameObject.SetActive(false);
        _anima.SetBool("BladeAttack1", false);
    }
    IEnumerator BladeAttack2()
    {
        _bladeAttack2.gameObject.SetActive(true);
        _anima.SetBool("BladeAttack2", true);
        yield return new WaitForSeconds(0.5f);
        _bladeAttack2.gameObject.SetActive(false);
        _anima.SetBool("BladeAttack2", false);
    }
    IEnumerator BladeAttack3()
    {
        _bladeAttack3.gameObject.SetActive(true);
        _anima.SetBool("BladeAttack3", true);
        yield return new WaitForSeconds(0.5f);
        _bladeAttack3.gameObject.SetActive(false);
        _anima.SetBool("BladeAttack3", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            _hp -= 1;
            _enemyLife.value -= 1;
        }
    }
}
