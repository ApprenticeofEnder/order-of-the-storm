using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    #region variables

    public int health;

    private float speed;
    private float originSpeed = 10.0f;
    private float jumpForce = 10.0f;
    private float doubleJumpForce = 8.0f;
    private float gravity = 20.0f;
    private float sprintSpeed = 15.0f;
    private float crouchSpeed = 5.0f;
    float strafe;
    float translation;
    private float verticalVelocity;
    private bool doubleJump = false;
    private bool crouchJump = false;
    private bool sliding = false;
    private bool isBlocking = false;
    private float slideStart;
    private CharacterController player;
    public CapsuleCollider playerColl;
    public Transform cameraHolder;
    private Vector3 crouchJumpDir;
    public bool isPaused;
    public Image pauseMenu;
    public Sword swordScript;
    private Vector3 moveDirection = Vector3.zero;

    #endregion 

    public void Start()
    {
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        doubleJump = false;
        speed = originSpeed;
        isPaused = false;
    }

    public void Update()
    {
        #region playingCode

        if (health > 0)
        {
            Sprinting();
            Crouching();
            Movement();
            Jumping();
            if (Input.GetButton("Block") && Input.GetButtonDown("Attack"))
            {
                if (!swordScript.isThrown)
                {
                    swordScript.SetThrowTrajectory();
                } 
            }
            else if (Input.GetButtonDown("Attack"))
            {
                if (!swordScript.isSwung)
                {
                    swordScript.Attack();
                }
            }
            else if (Input.GetButton("Block"))
            {
                isBlocking = true;
            }
            else
            {

            }
        }
        else
        {
            Death();
        }

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.gameObject.activeInHierarchy == false)
            {
                Cursor.lockState = CursorLockMode.None;
                isPaused = true;
                Cursor.visible = true;
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
                
            }
            else
            {
                Time.timeScale = 1;
            }
        }*/

        

        #endregion
    }

    public void PlayerDamage(int damage)
    {
        if (!isBlocking)
        {
            health -= damage;
        }
    }

    private void Jumping()
    {
        if (isGrounded())
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump") && !Input.GetButton("Crouch"))
            {
                verticalVelocity = jumpForce;
                doubleJump = true;
            }
        }
        else if (doubleJump && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = doubleJumpForce;
            doubleJump = false;
        }
        else if (!crouchJump)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 moveVector = new Vector3(0, verticalVelocity, 0);

        player.Move(moveVector * Time.deltaTime);
    }

    private void Sprinting()
    {
        if (Input.GetButton("Sprint"))
        {
            speed = sprintSpeed;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            speed = originSpeed;
        }
    }

    private void Crouching()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                speed = sprintSpeed;
                sliding = true;
                slideStart = Time.time;
            }
            playerColl.height = 1;
            player.height = 1;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            sliding = false;
            playerColl.height = 2;
            player.height = 2;
            speed = originSpeed;
        }
        if (Input.GetButton("Crouch"))
        {
            if (sliding)
            {
                if (speed > 0)
                {
                    speed = sprintSpeed - (Time.time - slideStart) * 10;
                    if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
                    {
                        sliding = false;
                        speed = crouchSpeed;
                    }
                }
                else
                {
                    sliding = false;
                    speed = crouchSpeed;
                }
            }
            else
            {
                speed = crouchSpeed;
            }
            if (Input.GetButtonDown("Jump") && !crouchJump)
            {
                if (player.isGrounded)
                {
                    crouchJump = true;
                    crouchJumpDir = cameraHolder.forward;
                    StartCoroutine("CrouchJump");
                }
            }
        }
    }

    private void Movement()
    {
        if (crouchJump)
        {
            player.Move(crouchJumpDir * 17 * Time.deltaTime);
        }
        else
        {
            translation = Input.GetAxis("Vertical") * speed;
            strafe = Input.GetAxis("Horizontal") * speed;
            translation *= Time.deltaTime;
            strafe *= Time.deltaTime;
            moveDirection = new Vector3(strafe, 0, translation);
            moveDirection = transform.TransformDirection(moveDirection);               
            player.Move(moveDirection);
        }
    }

    private void Death()
    {

    }

    private void Throw()
    {
        swordScript.SetThrowTrajectory();
    }

    private void Attack()
    {
        swordScript.Attack();
    }

    private bool isGrounded()
    {
        if (player.isGrounded)
        {
            return true;
        }

        Vector3 bottom = player.transform.position - new Vector3(0, player.height / 2, 0);

        RaycastHit hit;
        if (Physics.Raycast(bottom, new Vector3(0, -1, 0), out hit, 0.1f))
        {
            player.Move(new Vector3(0, -hit.distance, 0));
            return true;
        }

        return false;
    }

    IEnumerator CrouchJump()
    {
        yield return new WaitForSeconds(0.05f);
        doubleJump = true;
        yield return new WaitForSeconds(1f);
        crouchJump = false;
    }
}
