using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8;
    public float acceleration = 5;
    public float deceleration = 20;

    [Header("Jump")]
    public float jumpPower = 8;
    public float extraGravity = 4;
    public int maxJump = 2;

    Rigidbody2D rb2D;
    float xVelocity;
    bool isGrounded = true;
    float feetOffset;
    int currentJumps = 0;
    Vector2 position;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        Physics2D.queriesStartInColliders = false;

        position = rb2D.position;
        feetOffset = GetComponent<Collider2D>().bounds.extents.y;// + 0.02f;
    }

    void Update()
    {
        AdjustGravity();
        Movement();
        GroundCheck();
        Jump();
    }

    private void AdjustGravity()
    {
        if(rb2D.velocity.y < 0)
        {
            rb2D.gravityScale = extraGravity;
        }
        else
        {
            rb2D.gravityScale = 1;
        }
    }

    private void GroundCheck()
    {
        Vector2 offset = position;              
        offset.y = position.y = feetOffset;



        RaycastHit2D hit = Physics2D.Raycast(offset, Vector2.down, 0.05f);

        Debug.DrawRay(offset, Vector2.down * 0.05f);
         
        if (hit.collider == null)
        {
            isGrounded = true;
            currentJumps = 0;
        }
        else
        {
            isGrounded= false;
        }
    }


    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && currentJumps < maxJump) 
        {
            currentJumps++;
            rb2D.velocity = new Vector2 (rb2D.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb2D.velocity.y > 0) 
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.5f);
        }
    }


    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        xVelocity += x * acceleration * Time.fixedDeltaTime;

        xVelocity = Mathf.Clamp(xVelocity, -maxSpeed, maxSpeed);

        if (x == 0 || (x < 0 == xVelocity > 0))
        {
            xVelocity *= 1 - (deceleration * Time.fixedDeltaTime);
        }
        rb2D.velocity = new Vector2(xVelocity, rb2D.velocity.y);
        Debug.Log("rb2d Position" + rb2D.position);
        Debug.Log("position" + position);
        position = rb2D.position;
    }
}