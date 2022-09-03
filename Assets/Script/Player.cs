using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("プレイヤーの動き")]
    [SerializeField] GameObject _barral;
    /// <summary>左右移動の力/</summary>
    [SerializeField] int _moveSpeed;
    [SerializeField] int _defaultSpeed;
    [SerializeField] int _dushSpeed;
    /// <summary>ジャンプの力/</summary>
    [SerializeField] int _jumpPower;
    /// <summary>接地判定の時にRayを飛ばす距離</summary>
    [SerializeField] float _isGroundedLength = 1f;

    [Header("ゲームシステム関係")]
    /// <summary>Raycastでオブジェクトを判定するレイヤー/</summary>
    [SerializeField] LayerMask _groundLayer = 0;
    /// <summary>GameManagerスクリプトを格納する変数</summary>
    [SerializeField] GameManager _gameManager;
    /// <summary>クリアするために必要なカウント</summary>
    [SerializeField] int _clearCount;

    [Header("その他")]
    [SerializeField] Transform _muzzlePosition;
    /// <summary>取得した足場の数を表示するテキスト</summary>
    [SerializeField] Text _moveBlockCountText;
    /// <summary>取得した弾の数を表示するテキスト</summary>
    [SerializeField] Text _bulletBlockCountText;
    /// <summary>足場オブジェクトをアタッチ</summary>
    [SerializeField] GameObject _scaffold;
    /// <summary>弾オブジェクトをアタッチ</summary>
    [SerializeField] GameObject _bullet;
    /// <summary>カーソルの位置を取得</summary>
    [SerializeField] Transform _cursor;
    /// <summary>足場ブロックを格納するList</summary>
    [SerializeField] List<GameObject> _scaffoldBlockList = new List<GameObject>();
    /// <summary>弾ブロックを格納するList</summary>
    [SerializeField] List<GameObject> _bulletList = new List<GameObject>();

    /// <summary>Rigidbody2Dの格納場所/</summary>
    Rigidbody2D _rb;
    /// <summary>ゲームスタートのポジションを記録するための変数</summary>
    Vector3 _basePosition;
    /// <summary>ブロックのポジションをリセットするメソッドを格納するデリゲート</summary>
    event Action _positionReset;
    /// <summary>ゲームが終了したときに動くメソッドを格納する</summary>
    event Action _gameFinish;
    /// <summary>ジャンプ判定の変数/</summary>
    bool _isGround;
    /// <summary>ゲーム終了したらtrueになって動作を止める</summary>
    bool _isFinish;
    /// <summary>アイテム取得でカウントが増える</summary>
    int _itemCount;

    public int ItemCount { get => _itemCount; }
    public int ClearCount { get => _clearCount; }
    /// <summary>ポジションリセットのデリゲートをプロパティ化</summary>
    public Action PositionReset { get => _positionReset; set => _positionReset = value; }
    /// <summary>ゲーム終了のデリゲートをプロパティ化</summary>
    public Action AllGameFinish { get => _gameFinish; set => _gameFinish = value; }
    /// <summary>足場ブロックのListをプロパティ</summary>
    public List<GameObject> ScaffoldBlockList { get => _scaffoldBlockList; set => _scaffoldBlockList = value; }
    /// <summary>弾ブロックのListをプロパティ</summary>
    public List<GameObject> BulletList { get => _bulletList; set => _bulletList = value; }


    void Start()
    {
        //Rigidbody2Dを格納
        _rb = GetComponent<Rigidbody2D>();
        //変数にGameManagerスクリプトを代入
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        //ゲーム終了のデリゲートにメソッドを代入
        AllGameFinish += GameFinish;
        //ポジションリセットのデリゲートにメソッドを代入
        PositionReset += PlayerPositionReset;
        //スタートポジションを記録する
        _basePosition = transform.position;
    }
    void Update()
    {
        if (!_isFinish)
        {
            _moveBlockCountText.text = $"足場:{_scaffoldBlockList.Count}";
            _bulletBlockCountText.text = $"弾数:{_bulletList.Count}";
            //キャラの移動と向きを変える
            FlipX(Input.GetAxisRaw("Horizontal"));
            //キャラの移動(ジャンプ)
            if (Input.GetButtonDown("Jump"))
            {
                //IsGround関数を
                if (IsGround())
                {
                    _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                }
            }
            //Rayを飛ばす為のマウスポジションを取得
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //カメラの位置からマウスの位置までRayを飛ばす
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            Debug.DrawLine(ray.origin, ray.direction, Color.red);
            //移動可能のブロックを動かす(プレイヤーの方向に移動する)
            if (Input.GetButtonDown("Fire1"))
            {
                //Rayが当たったオブジェクトのtagがBlockだったら処理を実行
                if (hit.collider.gameObject.tag == "Block")
                {
                    //変数BlockにRayが当たったオブジェクトを代入
                    GameObject Block = hit.transform.gameObject;
                    //Blockに入ったオブジェクトに入っているBlockControllerを変数に代入
                    BlockController bloackMove = Block.GetComponent<BlockController>();
                    //BlockControllerにあるMoveメソッドを実行
                    bloackMove.Move(true);
                }
                else
                {
                    if(_scaffoldBlockList != null)
                    {
                        if (_scaffoldBlockList.Count > 0)
                        {
                            GameObject ScaffoldBlock = Instantiate(_scaffold);
                            ScaffoldBlock.transform.position = _cursor.position;
                            _scaffoldBlockList.RemoveAt(0);
                        }
                    }
                }
            }
            //移動可能のブロックを動かす(プレイヤーと逆方向に移動する)
            if (Input.GetButtonDown("Fire2"))
            {
                if (_bulletList != null)
                {
                    if (_bulletList.Count > 0)
                    {
                        GameObject BulletBlock = Instantiate(_bullet);
                        BulletBlock.transform.position = _muzzlePosition.position;
                        _bulletList.RemoveAt(0);
                    }
                }
            }
            if(Input.GetButtonDown("Fire3"))
            {
                StartCoroutine(Dush());
            }
            //エスケープキー(esc)を押したらブロックの位置がリセットされる
            if (Input.GetButtonDown("Cancel"))
            {
                _positionReset();
            }
        }
    }
    IEnumerator Dush()
    {
        _moveSpeed = _dushSpeed;
        yield return new WaitForSeconds(0.5f);
        _moveSpeed = _defaultSpeed;
    }
    //プレイヤーを入力方向に向けるメソッド
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
    //接地判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Flagに当たった時アイテムカウントがクリアカウントと同じになっていればResult表示
        if (collision.tag == "Flag" && _itemCount == _clearCount)
        {
            _gameFinish();
            _gameManager.GameManagerSetActive();
        }
        //Ballタグのオブジェクトに当たったらアイテムカウントを増やす
        if(collision.tag == "Ball")
        {
            _itemCount++;
            Destroy(collision.gameObject);
        }
    }
    bool IsGround()
    {
        //指定のレイヤーオブジェクトにRayが当たっていなければfalseを返す
        _isGround = false;
        //指定のレイヤーオブジェクトにRayが当たっていればtrueを返す
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, _isGroundedLength, _groundLayer);
        Debug.DrawRay(this.transform.position, Vector2.down * _isGroundedLength);
        if (hit.collider)
        {
            _isGround = true;
        }
        return _isGround;
    }
    //ゲーム終了時にbool型をtrueにする
    void GameFinish()
    {
        _isFinish = true;
    }

    //ポジションリセットの関数
    void PlayerPositionReset()
    {
        transform.position = _basePosition;
    }
}

