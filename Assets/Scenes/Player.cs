using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] int _moveSpeed;
    [SerializeField] Transform _mousePosition;
    [SerializeField] BlockController _blockMove;
    Rigidbody2D _rb;
    float _x;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_x * _moveSpeed, _rb.velocity.y);
    }
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        FlipX(_x);
        Vector3 MousePosition = _mousePosition.position;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + MousePosition);
        Debug.DrawLine(transform.position, MousePosition);
        if(Input.GetButton("Fire1"))
        {
            if(hit.collider)
            {
                _blockMove.Move();
            }
        }
    }
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
}
