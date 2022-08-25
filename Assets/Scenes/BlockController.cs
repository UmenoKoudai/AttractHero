using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlockController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] float _playerArea;
    [SerializeField] int _moveSpeed;
    Rigidbody2D _rb;
    bool _isBlockMove;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void Move()
    {
        _isBlockMove = !_isBlockMove;
        //float dir = Vector2.Distance(transform.position, _player.transform.position);
        if (_isBlockMove)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = (_player.position - transform.position).normalized * _moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
