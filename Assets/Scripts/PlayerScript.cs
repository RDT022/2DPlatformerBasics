using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public Text winText;

    public float speed;
    
    public Text score;

    private int scoreValue = 0;

    private bool isOnGround;

    public Transform groundcheck;

    public float checkRadius;

    public LayerMask allGround;

    private Animator anim;
    
    private bool facingRight = true;

    private float hozMovement;

    private float vertMovement;

    private bool warped = false;

    public AudioClip bgm;

    public AudioClip winFanfare;
    
    public AudioSource musicSource;

    private bool hasWon = false;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        anim = GetComponent<Animator>();
        winText.text = "";
        musicSource.clip = bgm;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hozMovement = Input.GetAxis("Horizontal");
        vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);    

        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("state", 1);
        }
        else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("state", 0);
        }
        if(facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if(facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if(scoreValue == 5 && warped == false)
        {
            transform.position = new Vector2(44.0f,-2.0f);
            warped = true;
        }
        if(scoreValue == 10 && hasWon == false)
        {
            winText.text = "Congratulations, you won! By: Adam Romanowski";
            musicSource.Stop();
            musicSource.clip = winFanfare;
            musicSource.loop = false;
            musicSource.Play();
            hasWon = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
