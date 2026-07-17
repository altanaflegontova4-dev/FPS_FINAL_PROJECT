using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed, gravityModifier, jumpPower, runSpeed;
    public CharacterController charCon;
    private Vector3 moveInput;
    public Transform camTrans;
    private int jumpAgain;
    public Animator anim;

    public float mouseSensitivity;

    public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List <Gun> allGuns = new List<Gun> ();
    public int currentGun;
    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //charCon = GetComponent<CharacterController>();//one way to get character controller component
        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive (true);
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

    }

   //no physics in character, so no fix update
    void Update()
    {
        //  moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //  moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        float yStore = moveInput.y;//store initial position to prevent gravity confusion


        //move based on player's facing direction
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical"); //Z axis
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal"); //X axis

        moveInput=vertMove + horiMove;
        moveInput.Normalize(); //horizontal movement is normal and not fast


        if (Input.GetKey(KeyCode.LeftShift))
        {
            //running speed
            moveInput = moveInput * runSpeed;

        }
        else
        {
            //walking speed
            moveInput = moveInput * moveSpeed;
        }
        
     
        moveInput.y = yStore; //continue journey

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime; //aplying gravity to character

        if (charCon.isGrounded)//detect ground
        {
            moveInput.y = -1f; //to neutralize position of the player
            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime; //aply gravity again when touching ground
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveInput.y = jumpPower;
                jumpAgain = 2;
            }
            
        }

        if (jumpAgain > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;
            jumpAgain--;
        }

     

        charCon.Move(moveInput * Time.deltaTime);

        float horizontalSpeed = new Vector3(charCon.velocity.x,0, charCon.velocity.z).magnitude;

        anim.SetFloat("moveSpeed", horizontalSpeed);

        //Player looking rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity; //mouse is moving in 2d - left/right and up/down
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));


        //Handle the shooting
        if (Input.GetMouseButtonDown(0) && activeGun.fireCounter<=0) //left click mouse button
        {
            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f)) //if it hits something
            {
                firePoint.LookAt(hit.point);//make firepoint looking at the object 
            }

            else //pointing to the sky
            {
                firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
                //looking at the centre of the screen
            }
            fireShot();
 
             //Instantiate(bullet, firePoint.position, firePoint.rotation);
        }


        if (Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            if (activeGun.fireCounter > 0)
            {
                fireShot();
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switchGun();
        }
    }

    public void fireShot()
    {
        if (activeGun.currentAmmo>0)
        {
            activeGun.currentAmmo--;
           
            activeGun.fireCounter = activeGun.fireRate;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
        }

    }

    public void switchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGun++;
        
        if(currentGun>=allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
    }
}
