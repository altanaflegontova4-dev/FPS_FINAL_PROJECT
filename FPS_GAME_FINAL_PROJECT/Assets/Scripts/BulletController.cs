using UnityEngine;
using UnityEngine.VFX;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;
    public Rigidbody rb;
    public ParticleSystem impact;
    public bool damageEnemy, damagePlayer;
    public int damageAmount;

    public bool attackPlayer;
    public int damage;

    void Start()
    {
        
    }

   
    void Update()
    {
        rb.linearVelocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;//counting down

        if (lifeTime <= 0 )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Enemy") && damageEnemy)
        {
            other.transform.parent.GetComponent<EnemyHealthController>().damage(damageAmount);
        }

        if (other.CompareTag("headshot") && damageEnemy) 
        {
            other.transform.parent.GetComponent<EnemyHealthController>().damage(damageAmount * 3);
        }


        if (other.CompareTag("Player") && damagePlayer)
        {
            //Debug.Log("Hit Player At " + transform.position);
            PlayerHealthController.instance.DamagePlayer(damageAmount);
        }
        */
        IDamagable damagable = other.GetComponentInParent<IDamagable>();//if that component have damagable implemented
        if (damagable != null )
        {
            damagable.TakeDamage(damage, attackPlayer);
        }

        Destroy(gameObject); //destroy object when it hits something

        float offset=0.7f;
        Vector3 newPosition = transform.position - transform.forward * offset;

        Instantiate(impact, newPosition, transform.rotation);

    }


}
