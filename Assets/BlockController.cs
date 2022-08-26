using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlockController : MonoBehaviour
{
    /// <summary>プレイヤーの場所を取得</summary>
    [SerializeField] Transform _player;
    /// <summary>Rigidbody2Dを格納する場所</summary>
    Rigidbody2D _rb;
    Player _playerScript;
    /// <summary>ブロックの静止と移動の判定</summary>
    bool _isBlockMove;
    Vector3 _basePositon;
    // Start is called before the first frame update
    void Start()
    {
        _playerScript = GameObject.FindObjectOfType<Player>();
        //最初のポジションを変数に代入
        _basePositon = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        //PlayerスクリプトのPositionReset変数にBlockPositionResetメソッドを代入
        _playerScript.PositionReset += BlockPositionReset;
    }
    //ブロックを移動させるためのメソッド
    public void Move(int MoveSpeed)
    {
        //もう1度実行されると静止と移動の判定が逆になる
        _isBlockMove = !_isBlockMove;
        //_isBlockMoveがtrueの場合はプレイヤーに向かって移動する
        if (_isBlockMove)
        {
            //bodyTypeがDynamicになる
            _rb.bodyType = RigidbodyType2D.Dynamic;
            //プレイヤーに向かって移動する
            _rb.velocity = (_player.position - transform.position).normalized * MoveSpeed;
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
    //ブロックのポジションをReset
    void BlockPositionReset()
    {
        transform.position = _basePositon;
    }
}
