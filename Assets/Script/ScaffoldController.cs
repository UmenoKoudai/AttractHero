using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldController : MonoBehaviour
{
    Player _player;
    private void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        //_player.PositionReset += ResetButton;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            StartCoroutine(BulletHit(collision));
        }
    }
    //public void ResetButton()
    //{
    //    if (FindObjectsOfType<ScaffoldController>().Length > 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    IEnumerator BulletHit(Collider2D collision)
    {
        _player.ScaffoldBlockList.Add(gameObject);
        yield return new WaitForEndOfFrame();
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
