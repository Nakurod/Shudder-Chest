using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour, ITakeDamage
{
    const string RUN_TRIGGER = "Run";
    const string CROUCH_TRIGGER = "Crouch";
    const string SHOOT_TRIGGER = "Shoot";

    [SerializeField] private float startingHealth;
    [SerializeField] private float minTimeUnderCover;
    [SerializeField] private float maxTimeUnderCover;
    [SerializeField] private int minShotsToTake;
    [SerializeField] private int maxShotsToTake;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float shootForce;
    [Range(0, 10)]
    [SerializeField] private float shootingAccuracy;

    [SerializeField] private Transform shootingPosition;
    [SerializeField] private EnemyBulletProjectile bulletPrefab;

    private bool isCover;
    private bool isShooting;
    private int currentShotsTaken;
    private int currentMaxShotsToTake;
    private NavMeshAgent agent;
    private Player player;
    private Transform occupiedCoverSpot;
    private Animator animator;

    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, startingHealth);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animator.SetTrigger(RUN_TRIGGER);
        _health = startingHealth;
    }

    public void Init(Player player, Transform coverSpot)
    {
        occupiedCoverSpot = coverSpot;
        this.player = player;
        GetToCover();
    }

    private void GetToCover()
    {
        agent.isStopped = false;
        agent.SetDestination(occupiedCoverSpot.position);
    }

    private void Update()
    {
        if (agent.isStopped == false && (transform.position - occupiedCoverSpot.position).sqrMagnitude <= 0.1f)
        {
            agent.isStopped = true;
            isCover = true;
            StartCoroutine(InitializeShootingCO());
        }
        if (isShooting || isCover)
        {
            RotateTowardsPlayer();
        }
    }
    private IEnumerator InitializeShootingCO()
    {
        HideBehindCover();
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeUnderCover, maxTimeUnderCover));
        StartShooting();
    }


    private void HideBehindCover()
    {
        animator.SetTrigger(CROUCH_TRIGGER);
    }
    private void StartShooting()
    {
        isCover = false;
        isShooting = true;
        currentMaxShotsToTake = UnityEngine.Random.Range(minShotsToTake, maxShotsToTake);
        currentShotsTaken = 0;
        animator.SetTrigger(SHOOT_TRIGGER);
    }

    public void Shoot()
    {
        Quaternion spread = Quaternion.Euler(transform.rotation.eulerAngles + 
            new Vector3(UnityEngine.Random.Range(shootingAccuracy, -shootingAccuracy), UnityEngine.Random.Range(shootingAccuracy, -shootingAccuracy), 0f));
        EnemyBulletProjectile projectile = Instantiate(bulletPrefab, shootingPosition.position, spread);
        projectile.Init(shootForce);
        projectile.Launch();

        currentShotsTaken++;
        if (currentShotsTaken >= currentMaxShotsToTake)
        {
            StartCoroutine(InitializeShootingCO());
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = player.GetHeadPosition() - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    public void TakeDamage(Weapon weapon, Projectile projectile, Vector3 contactPoint)
    {
        health -= weapon.GetDamage();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }


}