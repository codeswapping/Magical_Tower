using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireballProjectile : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyLayer;
    private int damage;
    private Vector3 direction;
    private float fireSpeed;
    private Rigidbody thisRb;
    private float explosionRadius;

    public void Initiate(Vector3 Dir, int damage, float speed, float explosionRadius)
    {
        thisRb = GetComponent<Rigidbody>();
        direction = Dir ;
        this.damage = damage;
        fireSpeed = speed;
        this.explosionRadius = explosionRadius;
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
        Debug.Log("On Collision Enter Fireball");
        if(c.gameObject.CompareTag("Enemy") || c.gameObject.CompareTag("Ground"))
        {
            //c.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            var enemiesInRadius = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer );
            foreach(var c1 in enemiesInRadius)
            {
                int dm = Mathf.RoundToInt(damage - Vector3.Distance(transform.position, c1.transform.position) / explosionRadius);
                dm = Mathf.Clamp(dm, 1, damage);
                c1.GetComponent<Enemy>().TakeDamage(dm);
            }
            gameObject.SetActive(false);
        }
    }
}
