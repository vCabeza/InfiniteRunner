using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AndroidPlayerController: MonoBehaviour {

    public float moveSpeed = .1f;
    public float jumpForce = .1f;
    public Text scoreText;

    private Animator animator;
    private float move;
    private float climb;
    private bool isJumping;
    private bool isClimable;
    private bool isFalling;
    private bool isDead;

    private int carrots;
    private int lives;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        
        isJumping = false;
        isClimable = false;
        isFalling = false;
        isDead = false;

        carrots = 0;
        lives = 5;

        InvokeRepeating("addPoints", 1.0f, 1.0f);
    }

    private void Update() {animator.SetFloat("isRunning", Mathf.Abs(move));
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        
        if (isClimable && !isJumping) {
            animator.SetBool("isClimbing", true);
            move = 0;
        } else {
            animator.SetBool("isClimbing", false);
            move = 1;
        }
        if (isDead == false & this.transform.position.y < -1.2f) {
            isDead = true;
            animator.Play("PlayerDie");

            move = 0;
            Destroy(GetComponent<Rigidbody2D>());

            SceneManager.LoadScene(0);
        }

        scoreText.text = carrots.ToString();
    }

    // Update is called once per frame
    void FixedUpdate() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if (isClimable) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0.8f * moveSpeed);
        }

        if (Input.touchCount > 0) {
            if (!isJumping && !isClimable) {
                isJumping = true;
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        /*if (Input.GetKey(KeyCode.Space) && !isJumping && !isClimable) {            
            isJumping = true;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }*/

        if (GetComponent<Rigidbody2D>().velocity.y < -0.1) {
            isFalling = true;
        } else {
            isFalling = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Ground")) {
            isJumping = false;
            isFalling = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Ladder")) {
            isClimable = true;
            isJumping = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }

        if (collision.gameObject.tag.Equals("Carrot")) {
            Destroy(collision.gameObject);
            carrots += 5;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Ladder")) {
            isClimable = false;
            GetComponent<Rigidbody2D>().gravityScale = .7f;
        }
    }

    private void addPoints() {
        carrots += 1;
    }
}
