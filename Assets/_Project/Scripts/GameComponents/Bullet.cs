using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    //[SerializeField] [Range(1, 50)]
    private float _speed = 17;
    private bool _move;
    private Vector3 _target;

    void Start()
    {
        StopMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameManager.Instance.GameStarted()) return;
        string tag = collision.transform.tag;
        print("------tag: " + tag);
        if (tag == "Ground" || tag == "Enemy")
        {
            FXManager.Instance.ShowFX(FX_Type.BulletCollision, transform.position);
            StopMove();
        }
         if (tag == "Finish")
        {
            FXManager.Instance.ShowFX(FX_Type.BulletCollision, transform.position);
            FXManager.Instance.ShowFX(FX_Type.Grass, transform.position);
            FXManager.Instance.ShowFX(FX_Type.Coin, transform.position);
            GameManager.Instance.AddCoin(10);
            AudioManager.Instance.PlaySound(AudioType.Coin);
            StopMove();
        }
         if (tag == "Obstacle")
        {
            FXManager.Instance.ShowFX(FX_Type.BulletCollision, transform.position);
            StopMove();
            collision.transform.position = new Vector3(0,-50,0); //Destroy(collision.gameObject); // collision.gameObject.SetActive(false);
        }
    }



    public void StartMove(Vector3 target)
    {
        gameObject.SetActive(true);
        _target = target;
        _move = true;
    }

    public void StopMove()
    {
        gameObject.SetActive(false);
        _move = false;
    }

    void Update()
    {
        if (_move)
        {
            transform.position = Vector3.Lerp(transform.position, _target, Time.deltaTime * _speed);
        }
    }
}
