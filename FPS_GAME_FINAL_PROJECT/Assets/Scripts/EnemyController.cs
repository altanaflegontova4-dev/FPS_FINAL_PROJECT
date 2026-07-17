using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //public float moveSpeed;
    //public Rigidbody rb;
    private bool chasing;
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop=2f;
    private Vector3 targetPoint;
    private Vector3 defaultPoint;

    public NavMeshAgent agent;//automatically look at target and adjust speed

    public float keepChasingTime = 5f;
    private float chaseCounter;

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate, waitBetweenShots=1f, timeToShoot=2f;
    private float fireCount, shotWaitCounter, ShootTimeCounter;

    public Animator anim;
    void Start()
    {
        defaultPoint=transform.position;
        ShootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
    }

    
    void Update()
    {
        targetPoint=PlayerController.instance.transform.position;
        targetPoint.y=transform.position.y;

        if (!chasing)
        {
            if (chaseCounter>0) //think for 5 seconds
            {
                agent.destination = transform.position;
                chaseCounter -= Time.deltaTime;//counting down
            }

            else
            {
                agent.destination=defaultPoint;
            }


            
            if (Vector3.Distance(transform.position, targetPoint)<distanceToChase)
            {
                chasing = true;

                ShootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;
            }
            if (agent.remainingDistance<0.25f)
            {
                anim.SetBool("isMoving", false);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }
        }

        else //if chasing is true
        {
            

            if (Vector3.Distance(transform.position, targetPoint) <= distanceToStop)
            {
                agent.destination = transform.position;
            }

            else
            {
                agent.destination = targetPoint;
            }

            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
                chaseCounter = keepChasingTime;
                
            }

            if (shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;

                if (shotWaitCounter <= 0)
                {
                    ShootTimeCounter = timeToShoot;
                }
                anim.SetBool("isMoving", true);
            }

            else if (PlayerController.instance.gameObject.activeInHierarchy) //proceed shooting only when player is active
            {
                ShootTimeCounter-= Time.deltaTime;

                if (ShootTimeCounter > 0)//shoot withing shootTimeCounter
                {
                    fireCount -= Time.deltaTime;

                    if (fireCount <= 0)
                    {
                        fireCount = fireRate;

                        firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f,0.4f,0f));

                        Vector3 targetDir = PlayerController.instance.transform.position - transform.position;//get direction

                        float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                        if (Mathf.Abs(angle)<=30)//only shoot when degree within 30
                        {
                            Instantiate(bullet, firePoint.position, firePoint.rotation);//to create copies object bullets
                            anim.SetTrigger("fireShot");
                        }
                        else
                        {
                            shotWaitCounter=waitBetweenShots;
                        }
                        
                    }

                    agent.destination = transform.position;//stop while shooting
                }

                else
                {
                    shotWaitCounter = waitBetweenShots;//reset to counter
                }
                anim.SetBool("isMoving", true);
            }

       

        }
        
    }
}
