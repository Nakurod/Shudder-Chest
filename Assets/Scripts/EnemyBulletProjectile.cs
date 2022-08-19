using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBulletProjectile : MonoBehaviour
{

    [SerializeField] private float lifeTime;
    private Rigidbody rigidBody;
    private float shootForce;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Init(float shootForce)
    {
        this.shootForce = shootForce;
        Destroy(gameObject, lifeTime);
    }

    public void Launch()
    {
        rigidBody.AddRelativeForce(Vector3.forward * shootForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FUL"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.FrontUpperLeft);
            Destroy(gameObject);
        }
        else if (other.CompareTag("FUR"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.FrontUpperRight);
            Destroy(gameObject);
        }
        else if (other.CompareTag("FLL"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.FrontLowerLeft);
            Destroy(gameObject);
        }
        else if (other.CompareTag("FLR"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.FrontLowerLeft);
            Destroy(gameObject);
        }
        else if (other.CompareTag("BUL"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.BackUpperLeft);
            Destroy(gameObject);
        }
        else if (other.CompareTag("BUR"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.BackUpperRight);
            Destroy(gameObject);
        }
        else if (other.CompareTag("BLL"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.BackLowerLeft);
            Destroy(gameObject);
        }
        else if (other.CompareTag("BLR"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(HitBox.BackLowerRight);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
