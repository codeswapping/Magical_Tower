using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BarrageProjecttile : MonoBehaviour
{
    private int damage;
    private Vector3 direction;
    private float fireSpeed;
    private Rigidbody thisRb;
    public void Initiate(Vector3 Dir, int damage, float speed)
    {
        thisRb = GetComponent<Rigidbody>();
        direction = Dir ;
        this.damage = damage;
        fireSpeed = speed;
        gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        GameManager.OnFixedUpdate += OnFixedUpdate;
    }
    private void OnDisable()
    {
        GameManager.OnFixedUpdate -= OnFixedUpdate;
    }

    private void OnFixedUpdate()
    {
        thisRb.position += Time.fixedDeltaTime * fireSpeed * direction;
    }

    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.CompareTag("Enemy"))
        {
            c.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
        else if(c.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);    
        }
    }
}
