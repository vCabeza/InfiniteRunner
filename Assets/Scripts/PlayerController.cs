using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = .1f;
    public float jumpForce = .1f;
    public GameObject grid;

    private Animator animator;
    private GameObject[] mapList;
    private Object mapGenerator;
    private float move;
    private float climb;
    private int nextMap;
    private bool facingRight;
    private bool isJumping;
    private bool isClimable;
    private bool isFalling;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        mapList = Resources.LoadAll("Map", typeof(GameObject))
            .Cast<GameObject>()
            .ToArray();
        nextMap = Random.Range(1, mapList.Length);
        mapGenerator = Resources.Load("MapGenerator");
        facingRight = true;
        isJumping = false;
        isClimable = false;
        isFalling = false;
    }

    private void Update() {
        animator.SetFloat("isRunning", Mathf.Abs(move));
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        if (isClimable && !isJumping) {
            animator.SetBool("isClimbing", true);
        } else {
            animator.SetBool("isClimbing", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        move = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if (move > 0 && !facingRight)
            flip();
        else if (move < 0 && facingRight)
            flip();

        if(isClimable) {
            climb = Input.GetAxis("Vertical");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, climb * moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isClimable) {
            isJumping = true;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (GetComponent<Rigidbody2D>().velocity.y < -0.1) {
            isFalling = true;
        } else {
            isFalling = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag.Equals("Ground")) {
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
        if (collision.gameObject.tag.Equals("MapGenerator")) {
            Vector3 position = collision.transform.position;
            Destroy(collision);
            GameObject newMap = instantiateNewMap(nextMap);
            newMap.transform.parent = grid.transform;
            newMap.transform.position = position;
            position.x += 5.5f;

            GameObject newMapGenerator = (GameObject)Instantiate(mapGenerator);
            newMapGenerator.transform.position = position;

            nextMap = Random.Range(1, mapList.Length);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Ladder")) {
            isClimable = false;
            GetComponent<Rigidbody2D>().gravityScale = .7f;
        }
    }

    void flip() {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public GameObject instantiateNewMap(int map) {
        GameObject goMap = Instantiate(mapList[map]);
        goMap.transform.parent = this.transform;

        return goMap;
    }
}
