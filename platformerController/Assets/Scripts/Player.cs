using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region Var

    bool isDebug = true;
    bool testBool = true;


    [SerializeField] public float speed = 20f;
    [SerializeField] public float slowRatio = .8f;

    [SerializeField] public float jumpVelocityCutOff = 14f;
    [SerializeField] public float jumpMultiplyer = 16f;
    [SerializeField] public float fallMultiplyer = 15f;

    [SerializeField] public float coyoteTime = .02f;
    [SerializeField] public float coyoteTimeCounter;

    [SerializeField] public float jumpBufferTime = .1f;
    [SerializeField] public float jumpBufferTimeCounter;

    public bool onGround;
    public bool onWallLeft;
    public bool onWallRight;
    public bool isJumping = false;
    public bool isFalling = false;
    public RaycastHit2D rayUp;
    public float rayUpYOffSet = -.5f;
    public RaycastHit2D rayDown;
    public bool isGroundfixing;

    public Vector3 rayCastOffSet = new Vector3(0f, 0f,0f);
    public Vector3 playerRayCastOffSet = new Vector3(0f, 0f, 0f);

    public int Ground;
    float collisionRadius = .25f;
    Vector2 bottomOffset = new Vector2(0f, -.5f);
    Vector2 topOffset = new Vector2(0f, .5f);
    Vector2 leftOffset = new Vector2(-.5f, 0f);
    Vector2 rightOffset = new Vector2(.5f, 0f);

    public float horizontalInput;

    private Rigidbody2D m_Rigidbody2D;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Ground = LayerMask.GetMask("ground");

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //checks
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, Ground);
        onWallLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, Ground);
        onWallRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, Ground);

        ClipFix();
        Movement();
        Jump();
    }

    public void ClipFix()
    {

        //Clip Fixes
        //fix Down
        if (transform.position.y - rayUpYOffSet < rayDown.point.y)
        {
            transform.position = new Vector3(transform.position.x, rayDown.point.y - rayUpYOffSet, transform.position.z);
            isGroundfixing = true;
            Debug.Log("warning Ground clip");
        }
        else
        {
            isGroundfixing = false;
        }

        rayDown = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, Ground);

        //fix up

        if (transform.position.y + rayUpYOffSet > rayUp.point.y && isGroundfixing == false && onGround == false)
        {
            transform.position = new Vector3(transform.position.x, rayUp.point.y + rayUpYOffSet, transform.position.z);
            Debug.Log("warning Roof clip");
        }

        rayUp = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, Ground);

    }

    public void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput < 0)
        {
            if (onWallLeft == false)
            {
                transform.Translate(new Vector2(horizontalInput, 0f) * Time.deltaTime * speed);
            }
        }
        else if (onWallRight == false)
        {
            {
                transform.Translate(new Vector2(horizontalInput, 0f) * Time.deltaTime * speed);
            }
        }
        else if (horizontalInput == 0)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x * slowRatio, m_Rigidbody2D.velocity.y * Time.deltaTime);
        }
    }

    public void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }

        if (onGround == true)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0 && isJumping == false && jumpBufferTimeCounter > 0)
        {

            isJumping = true;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);

            m_Rigidbody2D.velocity += Vector2.up * jumpMultiplyer;
        }
        else
        {
            if (m_Rigidbody2D.velocity.y <= 0 && onGround)
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && m_Rigidbody2D.velocity.y > 0f)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * .5f);

            jumpBufferTimeCounter = 0;
            coyoteTimeCounter = 0;
        }

    }

    private void FixedUpdate()
    {


        // gravity

        if (m_Rigidbody2D.velocity.y < jumpVelocityCutOff || m_Rigidbody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            isFalling = true;
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplyer * Time.deltaTime;
        }
        else
        {
            isFalling = false;
        }

    }

    #region Dedugging

    private void OnDrawGizmos()    { if (isDebug == true) {



            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);

            Debug.DrawRay(transform.position , -Vector2.up, Color.red);


        }
    }



    #endregion
}
