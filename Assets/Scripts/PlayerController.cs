using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Swipe { None, Up, Down, Left, Right };

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    public AudioSource hitSound;
    public AudioSource boostSound;

    public GameObject[] players;
    private Animator anim;

    private float xPos;

    private bool touchingGround;
    private bool isDead = false;

    public LayerMask groundLayer;

    public float jumpForce;
    public float acceleration;
    public float maxSpeed;
    public float groundCheckRadius;

    private int character;

    // Start is called before the first frame update
    void Start()
    {
        //get the character id from playerprefs (or default to 0)
        character = PlayerPrefs.GetInt("character", 0);

        //enable the character
        if (character == 0)
            players[0].SetActive(true);
        else
            players[1].SetActive(true);

        //get the animator of that character
        anim = players[character].GetComponent<Animator>();

        //get the attached rigidbody and collider
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //if not game over
        if (!isDead)
        {
            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            //move left
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Move(-1);
            }
            //move right
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(1);
            }
        }

        //check if player is touching the ground by creating a sphere at the bottom of player
        touchingGround = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);

        //update animator parameter
        anim.SetBool("touchingGround", touchingGround);
    }

    void FixedUpdate()
    {
        //if not game over
        if (!isDead)
        {
            //forward movement
            if (rb.velocity.magnitude < maxSpeed)
            {
                transform.position += transform.forward * acceleration * Time.deltaTime;
            }

            //define the new position as a vector
            Vector3 newPos = new Vector3(xPos, transform.position.y, transform.position.z);

            //smoothly lerp to that new position
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5);

            //update the score display
            GameManager.instance.UpdateScore(Time.deltaTime);
        }  
    }

    public void Move(int dir)
    {
        //left
        if (dir == -1)
        {
            //if can move left
            if (xPos >= 0)
            {
                //move left
                xPos -= 6f;
            }
        }
        //right
        else if (dir == 1)
        {
            //if can move right
            if (xPos <= 0)
            {
                //move right
                xPos += 6f;
            }
        }
    }

    void Jump()
    {
        //only jump if on ground
        if (touchingGround)
        {
            //set animation trigger
            anim.SetTrigger("jump");

            //apply upwards velocity
            rb.velocity = Vector3.up * jumpForce;
        }
    }

    public void Die()
    {
        //set animation trigger
        anim.SetTrigger("die");

        //disable the rigidbody physics
        rb.isKinematic = true;

        //disable the collider
        col.enabled = false;

        isDead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //coin
        if (other.CompareTag("Coin"))
        {
            GameManager.instance.CollectCoin();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            hitSound.Play();
            Die();
            GameManager.instance.Die();
        }
    }
}
