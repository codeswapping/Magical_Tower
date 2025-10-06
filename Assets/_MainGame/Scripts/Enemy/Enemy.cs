using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    public Transform damagePopupTrans;

    public EnemyData EnemyData => enemyData;

    [SerializeField]
    private int currentHealth;
    private float currentAttackTime;
    private Rigidbody enemyRb;
    private bool isNearPlayer = false;
    public void Initiate()
    {
        enemyRb = GetComponent<Rigidbody>();
        currentHealth = enemyData.health;
        transform.LookAt(TowerController.Instance.transform);
        isNearPlayer = false;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.OnUpdate += OnUpdate;
        GameManager.OnFixedUpdate += OnFixedUpdate;
    }

    private void OnDisable()
    {
        GameManager.OnUpdate -= OnUpdate;
        GameManager.OnFixedUpdate -= OnFixedUpdate;
    }

    private void OnUpdate()
    {
        currentAttackTime -= Time.deltaTime;
        if(isNearPlayer)
        {
            if(currentAttackTime <= 0)
            {
                currentAttackTime = enemyData.attackTime;
                TowerController.Instance.TakeDamage(enemyData.damage);
            }
        }
    }

    private void OnFixedUpdate()
    {
        if(isNearPlayer)
            return;
        if(Vector3.Distance(transform.position, TowerController.Instance.transform.position) > 2f)
        {
            var dir = (TowerController.Instance.transform.position - transform.position).normalized;
            enemyRb.position += dir * Time.fixedDeltaTime * enemyData.walkSpeed;
        }
        else
        {
            isNearPlayer = true;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy Taking Damage!");
        currentHealth -= damage;
        var damagePopup = GameManager.Instance.GetDamagePopup();
        damagePopup.transform.position = damagePopupTrans.position;
        damagePopup.ShowPopup(damage.ToString());
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
