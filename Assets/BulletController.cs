using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] int _shootSpeed;
    Rigidbody2D _rb;
    CursorController _cursor;
    void Start()
    {
        _cursor = GetComponent<CursorController>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(_cursor.transform.position * _shootSpeed, ForceMode2D.Impulse);
    }
}
