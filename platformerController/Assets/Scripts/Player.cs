using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isDebug = true;
    bool testBool = true;


    [SerializeField] public float speed = 20f;
    [SerializeField] public float jumpForce = 80f;
    [SerializeField] public float jumpVelocityCutOff = 14f;
    [SerializeField] public float jumpMultiplyer = 16f;
    [SerializeField] public float fallMultiplyer = 15f;

    public bool onGround;
    public bool onWall;
    public bool isJumping = false;
    public bool isFalling = false;

    public int Ground;
    float collisionRadius = .25f;
    Vector2 bottomOffset = new Vector2(0f, -.5f);
    Vector2 topOffset = new Vector2(0f, .5f);
    Vector2 leftOffset = new Vector2(-.5f, 0f);
    Vector2 rightOffset = new Vector2(.5f, 0f);




    public float horizontalInput;

    private Rigidbody2D m_Rigidbody2D;

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
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, Ground)
        || Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, Ground);



        //Movement

        horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(new Vector2(horizontalInput, 0) * Time.deltaTime * speed);

        //jumping

        if (isJumping == false && onGround == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            m_Rigidbody2D.AddForce(transform.up * jumpForce, ForceMode2D.Force);
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);

            m_Rigidbody2D.velocity += Vector2.up * jumpMultiplyer;

            Debug.Log(m_Rigidbody2D.velocity);


        }


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

        if (!Input.GetKeyUp(KeyCode.Space) && isJumping == true)
            {
                isJumping = false;
            } 

    }

    #region Dedugging

    private void OnDrawGizmos()    { if (isDebug == true) {



            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);




        }
    }



    #endregion
}
