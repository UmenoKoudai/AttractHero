using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlockController : MonoBehaviour
{
    /// <summary>プレイヤーの場所を取得</summary>
    [SerializeField] GameObject _player;
    [SerializeField] int _moveSpeed;
    [SerializeField] bool _scaffoldBlock = false;
    [SerializeField] bool _bulletBlock = false;
    /// <summary>Rigidbody2Dを格納する場所</summary>
    Rigidbody2D _rb;
    /// <summary>プレイヤースクリプトを代入する型</summary>
    Player _playerScript;
    /// <summary>ブロックの静止と移動の判定</summary>
    bool _isBlockMove;
    /// <summary>ゲーム終了したらtrueになって動作を止める</summary>
    bool _isFinish;
    /// <summary>ブロックの元の位置を記録する型</summary>
    Vector3 _basePositon;
    void Start()
    {
        //プレイヤースクリプトを代入
        _playerScript = GameObject.FindObjectOfType<Player>();
        //最初のポジションを変数に代入
        _basePositon = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        //PlayerスクリプトのPositionReset変数にBlockPositionResetメソッドを代入
        _playerScript.PositionReset += BlockPositionReset;
        //PlayerスクリプトのAllGameFinish変数にGameFinishメソッドを代入
        _playerScript.AllGameFinish += GameFinish;
    }
    //ブロックを移動させるためのメソッド
    public void Move(bool MoveSwich)
    {
        if (!_isFinish)
        {
            //もう1度実行されると静止と移動の判定が逆になる
            _isBlockMove = !_isBlockMove;
            
            //_isBlockMoveがtrueの場合はプレイヤーに向かって移動する
            if (_isBlockMove)
            {
                //bodyTypeがDynamicになる
                _rb.bodyType = RigidbodyType2D.Dynamic;
                //プレイヤーに向かって移動する
                _rb.velocity = (_player.transform.position - transform.position).normalized * _moveSpeed;
            }
            //_isBlockMoveがfalseの場合は移動が静止する
            else
            {
                //bodyTypeがKinematicになる(プレイヤーが当たっても動かなくなる)
                _rb.bodyType = RigidbodyType2D.Kinematic;
                //移動の力(ベクトル)がゼロになる
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
    //ブロックのポジションをReset
    void BlockPositionReset()
    {
        transform.position = _basePositon;
    }
    //ゲーム終了時に実行
    void GameFinish()
    {
        _isFinish = true;
    }
}
