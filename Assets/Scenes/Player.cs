using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] int _moveSpeed;
    [SerializeField] int _jumpPower;
    //[SerializeField] BlockController _blockMove;
    [SerializeField] LayerMask _block;
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
        if(Input.GetButtonDown("Jump"))
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        FlipX(_x);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Input.GetButton("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _block);
            Debug.DrawLine(ray.origin, ray.direction, Color.red);
            if (hit)
            {
                //BlockController bloackMove = 
                //Debug.Log("HIT");
                //_blockMove.Move();
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
