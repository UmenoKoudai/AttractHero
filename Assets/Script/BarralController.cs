using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarralController : MonoBehaviour
{
    [SerializeField] Transform _muzzlePosition;
    Vector3 _mousePositon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = transform.position - _mousePositon;
    }
    public void Shoot(GameObject Bullet)
    {
        Instantiate(Bullet, _muzzlePosition.position, transform.rotation);
    }
}
