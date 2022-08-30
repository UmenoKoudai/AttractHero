using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>左右移動の力/</summary>
    [SerializeField] int _moveSpeed;
    /// <summary>ジャンプの力/</summary>
    [SerializeField] int _jumpPower;
    /// <summary>Raycastでオブジェクトを判定するレイヤー/</summary>
    [SerializeField] LayerMask _block = 0;
    /// <summary>GameManagerスクリプトを格納する変数</summary>
    [SerializeField] GameManager _gameManager;
    /// <summary>クリアするために必要なカウント</summary>
    [SerializeField] int _clearCount;
    /// <summary>接地判定の時にRayを飛ばす距離</summary>
    [SerializeField] float _isGroundedLength = 1f;
    /// <summary>Rigidbody2Dの格納場所/</summary>
    Rigidbody2D _rb;
    /// <summary>ゲームスタートのポジションを記録するための変数</summary>
    Vector3 _basePosition;
    /// <summary>ブロックのポジションをリセットするメソッドを格納するデリゲート</summary>
    event Action _positionReset;
    /// <summary>ゲームが終了したときに動くメソッドを格納する</summary>
    event Action _gameFinish;
    /// <summary>ジャンプ判定の変数/</summary>
    public bool _isGround;
    /// <summary>ゲーム終了したらtrueになって動作を止める</summary>
    bool _isFinish;
    /// <summary>アイテム取得でカウントが増える</summary>
    int _itemCount;

    /// <summary>ポジションリセットのデリゲートをプロパティ化</summary>
    public Action PositionReset { get => _positionReset; set => _positionReset = value; }
    /// <summary>ゲーム終了のデリゲートをプロパティ化</summary>
    public Action AllGameFinish { get => _gameFinish; set => _gameFinish = value; }

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
            }
            //移動可能のブロックを動かす(プレイヤーと逆方向に移動する)
            if (Input.GetButtonDown("Fire2"))
            {
                //Rayが当たったオブジェクトのtagがBlockだったら処理を実行
                if (hit.collider.gameObject.tag == "Block")
                {
                    //変数BlockにRayが当たったオブジェクトを代入
                    GameObject Block = hit.transform.gameObject;
                    //Blockに入ったオブジェクトに入っているBlockControllerを変数に代入
                    BlockController bloackMove = Block.GetComponent<BlockController>();
                    //BlockControllerにあるMoveメソッドを実行
                    bloackMove.Move(false);
                }
            }
            //エスケープキー(esc)を押したらブロックの位置がリセットされる
            if (Input.GetButtonDown("Cancel"))
            {
                _positionReset();
            }
        }
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
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector3.down, _isGroundedLength, _block);
        Debug.DrawRay(this.transform.position, this.transform.position + Vector3.down * _isGroundedLength);
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

