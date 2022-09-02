using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] int _shootSpeed;
    Rigidbody2D _rb;
    BarralController _cursor;
    void Start()
    {
        _cursor = GameObject.FindObjectOfType<BarralController>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _cursor.transform.up * _shootSpeed;
    }
}
