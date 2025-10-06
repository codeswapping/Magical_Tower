using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    public static TowerController Instance;

    [Header("Projectile Setup")]
    [SerializeField]
    private float FireballSpellTime;
    [SerializeField]
    private float fireballSpellRange, fireballSpellSpeed, fireballExplodeRadius; 
    [SerializeField]
    private int fireballSpellDamage;
    [SerializeField] 
    private float barrageSpellTime, barrageSpellRange, barrageSpellSpeed;
    [SerializeField]
    private int barrageSpellDamage;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private Transform projectileSpawnTrans;

    [Header("Tower Attributes")]
    [SerializeField]
    private int towerMaxHealth;

    [Header("UI")]
    [SerializeField]
    private UnityEngine.UI.Image healthBarImg;


    private float currentBarrageWait, currentFireballWait;
    private int currentHealth;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
    }

    public void Initiate()
    {
        currentHealth = towerMaxHealth;
        healthBarImg.fillAmount = (float)currentHealth / (float)towerMaxHealth;
        GameManager.OnUpdate += OnUpdate;
    }

    private void OnUpdate()
    {
        currentBarrageWait -= Time.deltaTime;
        currentFireballWait -= Time.deltaTime;
        if(currentBarrageWait <= 0)
        {
            currentBarrageWait = barrageSpellTime;
            var enemiesInRadius = Physics.OverlapSphere(transform.position, barrageSpellRange, enemyLayer);
            foreach(Collider c in enemiesInRadius)
            {
                var bp = GameManager.Instance.GetBarrageProjectile();
                bp.transform.position = projectileSpawnTrans.position;
                bp.Initiate((c.transform.position - bp.transform.position).normalized, barrageSpellDamage, barrageSpellSpeed);
            }
        }
        if(currentFireballWait <= 0)
        {
            currentFireballWait = FireballSpellTime;
            var enemiesInRadius = Physics.OverlapSphere(transform.position, fireballSpellRange, enemyLayer);
            var closes = Mathf.Infinity;
            Collider nearEnemy = null;
            foreach(Collider c in enemiesInRadius)
            {
                var distance = Vector3.Distance(c.transform.position, transform.position);
                if(distance < closes)
                {
                    nearEnemy = c;
                    closes = distance;
                }
            }
            if(nearEnemy != null)
            {
                Debug.Log("Near Enemy Found");
                var fireball = GameManager.Instance.GetFireballProjectile();
                fireball.transform.position = projectileSpawnTrans.position;
                fireball.Initiate((nearEnemy.transform.position - fireball.transform.position).normalized, fireballSpellDamage, fireballSpellSpeed, fireballExplodeRadius);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBarImg.fillAmount = (float)currentHealth / (float)towerMaxHealth;
        if(currentHealth <=0)
        {
            //Game over.
            GameManager.OnUpdate -= OnUpdate;
        }
    }

}
