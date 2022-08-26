using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>左右移動の力/</summary>
    [SerializeField] int _moveSpeed;
    /// <summary>ジャンプの力/</summary>
    [SerializeField] int _jumpPower;
    /// <summary>ブロックを左に動かす力/</summary>
    [SerializeField] int _blockMoveSpeedLeft;
    /// <summary>ブロックを右に動かす力/</summary>
    [SerializeField] int _blockMoveSpeedRigit;
    /// <summary>Raycastでオブジェクトを判定するレイヤー/</summary>
    [SerializeField] LayerMask _block;
    /// <summary>Rigidbody2Dの格納場所/</summary>
    Rigidbody2D _rb;
    /// <summary>ブロックのポジションをリセットするメソッドを格納するデリゲート</summary>
    public static event Action _positionReset;
    /// <summary>ジャンプ判定の変数/</summary>
    public bool _isGround;
    /// <summary>左右移動の入力を格納する変数/</summary>
    float _x;
    //Vector2 _lineForGround = new Vector2(0f, -1.5f);
    /// <summary>ポジションリセットのデリゲートをプロパティ化</summary>
    public static Action PositionReset { get => _positionReset; set => _positionReset = value; }
    void Start()
    {
        //Rigidbody2Dを格納
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //キャラの移動(左右移動)　　　　　　　　　　　　　　　　　　　　　　　　　　　　
        _rb.velocity = new Vector2(_x * _moveSpeed, _rb.velocity.y);
        //キャラの移動(ジャンプ)
        if(Input.GetButtonDown("Jump") && _isGround)
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }
    void Update()
    {
        //左右の入力を変数に格納
        _x = Input.GetAxisRaw("Horizontal");
        //プレイヤーを入力方向に向ける
        FlipX(_x);
        //Rayを飛ばす為のマウスポジションを取得
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //カメラの位置からマウスの位置までRayを飛ばす
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
                bloackMove.Move(_blockMoveSpeedLeft);
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
                bloackMove.Move(_blockMoveSpeedRigit);
            }
        }
        //エスケープキー(esc)を押したらブロックの位置がリセットされる
        if(Input.GetButtonDown("Cancel"))
        {
            _positionReset();
        }
    }
    //プレイヤーを入力方向に向けるメソッド
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
    //接地判定
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
