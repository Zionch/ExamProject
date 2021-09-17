using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveVelocity = 5, jumpVelocity = 10;
    [SerializeField]
    private float groundedDistance = 0.005f;

    private new Rigidbody2D rigidbody;

    private Animator animator;
    private const float moveAnimationThreshold = 0.05f;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void UpdateMovement()
    {
        Vector2 v = rigidbody.velocity;

        if (Input.GetKey(KeyCode.A)) {
            v.x = -moveVelocity;
        }

        if (Input.GetKey(KeyCode.D)) {
            v.x = moveVelocity;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            v.y = jumpVelocity;
        }

        rigidbody.velocity = v;
    }

    private bool IsGrounded() {
        return Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, 1 << GameSettings.GroundLayer);
    }

    public void UpdateAnimation() {
        var scale = transform.localScale;
        if(rigidbody.velocity.x < -moveAnimationThreshold) {
            scale.x = 1;
        }else if(rigidbody.velocity.x > moveAnimationThreshold) {
            scale.x = -1;
        }
        transform.localScale = scale;

        if(Mathf.Abs(rigidbody.velocity.x) >= moveAnimationThreshold) {
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }
    }
}
