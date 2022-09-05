using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isDebug = true;

    [SerializeField] public float speed = 10f;
    [SerializeField] public float jumpForce = 6f;
    [SerializeField] public float jumpForceBoost = .5f;
    [SerializeField] public float jumpForceBoostMod;
    [SerializeField] public float jumpForceBoostCurve = .5f;


    public bool onGround;
    public bool onWall;
    public bool isJumping = false;

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

        jumpForceBoostMod = jumpForceBoost;
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
     

            if (Input.GetKeyUp(KeyCode.Space) & isJumping == true)
            {
                isJumping = false;
                jumpForceBoostMod = jumpForceBoost;
            }

            if (Input.GetKeyDown(KeyCode.Space) & isJumping == false & onGround == true)
            {

                m_Rigidbody2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;

            }

            if (isJumping == true & m_Rigidbody2D.velocity.y > 0)
            {
                m_Rigidbody2D.AddForce(transform.up * jumpForceBoostMod, ForceMode2D.Impulse);

            jumpForceBoostMod = jumpForceBoostCurve * jumpForceBoostMod;

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
