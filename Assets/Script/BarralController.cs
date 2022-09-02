using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarralController : MonoBehaviour
{
    [SerializeField] Transform _cursor;
    void Update()
    {
        transform.up = _cursor.position - transform.position;
    }
}
