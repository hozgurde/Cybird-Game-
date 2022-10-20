using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Inputs")]
    public KeyCode jump = KeyCode.LeftArrow;
    public KeyCode dash = KeyCode.RightArrow;

    [Header("Jump Variables")]
    public float jumpForce = 1.0f;
    public float maxFallingSpeed = 1.0f;
    public float gravity = 1.0f;
    public float maxStickTime = 1.0f;

    [Header("Dash Variables")]
    public float dashSpeed = 1.0f;
    public float dashDistance = 1.0f;
    public float returnSpeed = 0.5f;
    private float returnTime;
    private float dashTime;
    private float dashTimer = 0f;
    private int inDash = 0; //0 not in dash, 1 dashing, 2 returning

    [Header("Animation")]
    public Animator frontwing_animator;
    public Animator backwing_animator;
    public Animator torso_animator;

    private Rigidbody2D rb;

    private float stickTopTimer = 0f;
    private bool isInLowerWall = false;
    private bool isInUpperWall = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        dashTime = dashDistance / dashSpeed;
        returnTime = dashDistance / returnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        /*Check In Dash*/
        
        if( inDash == 1 )
        {
            dashTimer -= Time.deltaTime;
            if(dashTimer <= 0)
            {
                inDash = 2;
                dashTimer = returnTime;
                rb.velocity = new Vector2(-returnSpeed, 0);
                torso_animator.SetBool("IsDashing", false);
                backwing_animator.SetBool("IsDashing", false);
                frontwing_animator.SetBool("IsDashing", false);
            }
        }
        else {
            if (inDash == 2)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0)
                {
                    inDash = 0;
                    dashTimer = 0;
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            /*Check Dash*/
            if (Input.GetKeyDown(dash) && inDash == 0)
            {
                inDash = 1;
                rb.velocity = new Vector2(dashSpeed, 0);
                dashTimer = dashTime;
                torso_animator.SetBool("IsDashing", true);
                backwing_animator.SetBool("IsDashing", true);
                frontwing_animator.SetBool("IsDashing", true);
            }
            /*Check Jump Input*/
            if (Input.GetKeyDown(jump) && stickTopTimer <= 0 && !isInUpperWall)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                PlayFlyAnimation();
                //rb.rotation = (jumpForce / maxFallingSpeed) * 45f;
            }
            else if(isInUpperWall && Input.GetKeyDown(jump))
            {
                stickTopTimer = maxStickTime;
                PlayFlyAnimation();
            }
            /*Check Falling Speed*/
            if (stickTopTimer > 0)
            {
                stickTopTimer -= Time.deltaTime;
            }
            else if (rb.velocity.y < -maxFallingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -maxFallingSpeed);
                //rb.rotation = -45f;
            }
            else if (!isInLowerWall)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - gravity * Time.deltaTime);
                //rb.rotation = (rb.velocity.y / maxFallingSpeed) * 45f;
            }
                /*Check Collision*/
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "UpperWall" && rb.velocity.y > 0)
        {
            float timeRequired = rb.velocity.y / gravity;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            //rb.rotation = (rb.velocity.y / maxFallingSpeed) * 45f;
            stickTopTimer = Mathf.Clamp(timeRequired, 0, maxStickTime);
            Debug.Log(stickTopTimer);
            isInUpperWall = true;
        }
        else if (other.gameObject.CompareTag("Pipe"))
        {
            Debug.Log("Hit");
            Destroy(this.gameObject);
            FindObjectOfType<Buttons>().MenuButton();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        
        if(col.gameObject.name == "LowerWall" && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            //rb.rotation = (rb.velocity.y / maxFallingSpeed) * 45f;
            Debug.Log(rb.velocity);
            isInLowerWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "LowerWall")
        {
            isInLowerWall = false;
        }
        else if (other.gameObject.name == "UpperWall")
        {
            isInUpperWall = false;
        }

    }

    private void PlayFlyAnimation()
    {
        backwing_animator.SetTrigger("TriggerFly");
        frontwing_animator.SetTrigger("TriggerFly");
    }

}
