﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{    
    // Handle animations
	private Animator animator;

    // Handle physics
    [Header("Physics & Movement")]
	[SerializeField]
        [Tooltip("How fast he'll walk/run")] 
        private float speed = 5;
    [SerializeField]
    [Tooltip("How much acceleration he gains as he moves continuously")]
    private float accelerationFactor = 0.5f;
	[SerializeField] 
        [Tooltip("How high he'll jump")]
        private float jumpForce = 2;
    [SerializeField]
        [Tooltip("What are to check contact with the ground")]
        private float groundCheckRadius = 0.2f;
	[SerializeField]
        [Tooltip("Speed rate at which flying is enabled")]
        private float flyingSpeedRate = 1.5f;
    [SerializeField]
        [Tooltip("How high he'll fly")]
        private float flyingForce = 20;
    [SerializeField]
	    [Tooltip("The gravity while flying")]
        private float flyingGravity = 1.5f;

    private Transform groundCheck; // get the Transform that we'll need to check whether the character is on the ground

    private Rigidbody2D rb; // get the character's Rigidbody to access its properties and apply force to it
    private float originalGravity = 0; // store the character's original Gravity so we can restore it after flying

	private bool isGrounded = true; // determine whether the character is on the ground
    private bool isFlying = false; // determine whether the character is flying
	private float direction = 0; // determine whether the character is moving left or right or stopped
	private bool isFlipping = false; // determine whether the character is flipping so we can trigger the Flip animation


    // Handle pick & release items
    [Header("Grab & Release")]
    [SerializeField]
        [Tooltip("The precision margin for grabbing an item")]
        private float grabbingDistance = 0.2f;
    [SerializeField]
        [Tooltip("The force used to release an item")]
        private float throwForce = 10;
    private LayerMask grabable = 0; // select which layers we consider grabable
    private Transform holdPoint; // get the Transform that will serve as reference for scanning for items as well as the position to hold them
    private RaycastHit2D scanForItem; // cast a ray to scan for items in grabbing distance
	private GameObject grabbedItem; // determine whether the character is holding an item and which item is being held
    private Rigidbody2D grabbedItemRB; // get the held item's Rigidbody to manipulate its mass while held
    private float grabbedItemOriginalMass = 0; // store the grabbed item original mass so we can restore it upon release

	// Handle additional controls
    [Header("Others")]
	[SerializeField]
		[Tooltip("The precision margin for hitting a block")]
		private Vector2 hitBlockDistance = new Vector2(0, 0.1f);
    [SerializeField]
        [Tooltip("How long the character stays invincible between hits")]
        private float HitRecoverTime = 0f;
	
    private bool isCrouching = false; // determine whether the character is crouching
	internal bool isAlive = true; // determine whether the character is alive
	internal bool hit = false; // to determine whether the character has just been hit
    private Transform sprite = null; // to assist in flashing the character
    private bool facingRight = true; // determine whether the character's sprite is flipped or not

    internal GameController.CharacterStates poweredUpState = GameController.CharacterStates.small; // determine the character's current state

    void Awake()
    {
        SetInitialState();
    }

    void Update()
    {
        // Call the appropriate animations
        ControlAnimations();
    }

    void FixedUpdate()
    {
		// determine whether the chracter is grounded based on the overlap of
		// groundCheck and the groundLayer, at groundCheckRadius size
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, GameController.instance.ground);

        // scan for items
        WatchForGrabbableItems();

		if (isFlying)
        {
            rb.gravityScale = flyingGravity;
        }
        else
        {
            rb.gravityScale = originalGravity;
        }
    }
    
    void SetInitialState()
    {
        // Set and Check Animation items
        animator = GetComponent<Animator>();
        GameController.instance.CheckRequiredComponent("Animator", animator, objectName: gameObject.name);

		// Set and Check Physics items
        GameController.instance.CheckRequiredFloat("speed", speed, objectName: gameObject.name);
        GameController.instance.CheckRequiredFloat("accelerationFactor", accelerationFactor, objectName: gameObject.name);
		GameController.instance.CheckRequiredFloat("jumpForce", jumpForce, objectName: gameObject.name);
        GameController.instance.CheckRequiredFloat("groundCheckRadius", groundCheckRadius, objectName: gameObject.name);
        GameController.instance.CheckRequiredFloat("flyingSpeedRate", flyingSpeedRate, objectName: gameObject.name);
        GameController.instance.CheckRequiredFloat("flyingForce", flyingForce, objectName: gameObject.name);
        GameController.instance.CheckRequiredFloat("flyingGravity", flyingGravity, objectName: gameObject.name);
        // find a groundCheck child in the character (if more than one, get the first)
		foreach (Transform t in transform)
		{
			if (t.tag == GameController.Tags.GroundCheck.ToString())
			{ groundCheck = t; break; }
		}
        GameController.instance.CheckRequiredChild("groundCheck", groundCheck, objectName: gameObject.name);
        // find Rigidbody and store gravity
        rb = GetComponent<Rigidbody2D>();
        GameController.instance.CheckRequiredComponent("RigidBody", rb, objectName: gameObject.name);
        originalGravity = rb.gravityScale;

		// Set and Check Grab & Release items
		GameController.instance.CheckRequiredFloat("grabbingDistance", grabbingDistance, objectName: gameObject.name);
		GameController.instance.CheckRequiredFloat("throwForce", throwForce, objectName: gameObject.name);
        // Enable grabable LayerMask only for what's defined in grabLayer
        grabable = 1 << LayerMask.NameToLayer(GameController.Layers.Grabable.ToString());
		// find a holdPoint child in the character (if more than one, get the first)
		foreach (Transform t in transform)
		{
			if (t.tag == GameController.Tags.HoldPoint.ToString())
			{ 
                holdPoint = t; 
                break; 
            }
		}
		GameController.instance.CheckRequiredChild("holdPoint", holdPoint, objectName: gameObject.name);

        // Set and Check Other items
        GameController.instance.CheckRequiredVector2("hitBlockDistance", hitBlockDistance, objectName: gameObject.name);

        // Find sprite
        foreach (Transform t in transform)
		{
			if (t.tag == GameController.Tags.PlayerSprite.ToString())
			{ 
                sprite = t; 
                break; 
            }
		}
		GameController.instance.CheckRequiredChild("sprite", sprite, objectName: gameObject.name);
    }

    void ControlAnimations()
    {
		// tie in our code variables to our Animator parameters
		animator.SetFloat("Velocity", Mathf.Abs(rb.velocity.x));
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Flipping", isFlipping);
        animator.SetBool("Crouching", isCrouching);
        animator.SetBool("Alive", isAlive);
        animator.SetBool("hasItem", grabbedItem);
        animator.SetBool("Flying", isFlying);

        // loop through animation layers activating the one that corresponded to the current powerUp and deactivating the rest
        for (int i = 1; i <= System.Enum.GetValues(typeof(GameController.CharacterStates)).Length; i++)
        {
            if (i == (int)poweredUpState)
            { 
                animator.SetLayerWeight(i, 1);
            }
            else
            {
                animator.SetLayerWeight(i, 0);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight; // toggle facingRight based on its previous state
        isFlipping = true; // set the Flipping variable to trigger flipping animation

        // invert the x scale to mirror the sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Move()
    {
        // determine acceleration based on current velocity and acceleration factor
        float acceleration = rb.velocity.x * accelerationFactor;
        
        // adjust velocity to account for speed, direction and acceleration
        rb.velocity = new Vector2((speed * direction) + acceleration, rb.velocity.y);
    }
    
    void Jump()
    {
        SoundController.instance.PlaySFX(SoundController.instance.sfxJump, 0.7f);
        
        // apply upwards force at the scale of jumpForce
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    void Fly()
    {
        isFlying = true; // toggle Flying to help animations and gravity control
        rb.velocity += Vector2.up * flyingForce; // adjust velocity to account for upwards force at the scale of flyingForce
    }
    
    public void TakeHit()
    {
        if (isAlive && !hit)
        {
            switch(poweredUpState)
            {
                case GameController.CharacterStates.racoon:
                {
                    SoundController.instance.PlaySFX(SoundController.instance.sfxLosePower);
                    poweredUpState = GameController.CharacterStates.big;
                    StartCoroutine(RecoverFromHit());
                    break;
                }
                case GameController.CharacterStates.big:
                {
                    SoundController.instance.PlaySFX(SoundController.instance.sfxLosePower);
                    poweredUpState = GameController.CharacterStates.small;
                    StartCoroutine(RecoverFromHit());
                    break;
                }
                case GameController.CharacterStates.small:
                default:
                {
                    TriggerDeath();
                    break;
                }
            }
        }
    }

    IEnumerator RecoverFromHit() 
    {
        hit = true;
        float flashTime = 0f;
        while (flashTime <= HitRecoverTime)
        {
            sprite.gameObject.SetActive(false);
            yield return new WaitForSeconds(.1f);
            sprite.gameObject.SetActive(true);
            yield return new WaitForSeconds(.1f);
            flashTime++;
        }
        hit = false;
    }

    public void TriggerDeath()
    {
        speed = 0;
        rb.velocity = new Vector2(0,0);
        rb.isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        SoundController.instance.PlaySFX(SoundController.instance.sfxLoseLife);
        // GameController.instance.lives--;
        StartCoroutine(Die());
        isAlive = false;
    }

    IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(4f);
        Destroy(gameObject);
        // if  (GameController.instance.lives > 0)
        // {
        //     SceneController.instance.Reload();
        // }
        // else
        // {
        //     SceneController.instance.GameOver();
        // }
    }

    // public void Die()
    // {
    //     //Debug.Log(System.DateTime.Now + ">> die");
    //     Destroy(gameObject);
    //     if  (GameController.instance.lives > 0)
    //     {
    //         SceneController.instance.Reload();
    //     }
    //     else
    //     {
    //         SceneController.instance.GameOver();
    //     }
    // }

    void WatchForGrabbableItems()
    {
		if (!(bool)grabbedItem) // if not holding an item
		{
            Physics2D.queriesStartInColliders = false; // ignore the raycast collision to the character
            // cast a ray from the holdPoint position to the front (as indicated by localScale.x sign),
            // the ray will be sized at grabbingDistance and only consider items in the grabable LayerMask
            scanForItem = Physics2D.Raycast(holdPoint.position, Vector3.right * transform.localScale.x, grabbingDistance, grabable);
		}
    }

    void GrabItem()
    {
        // if a grabable item was found
        if (scanForItem.collider != null)
        {
            grabbedItem = scanForItem.collider.gameObject; // grab the item
            grabbedItemRB = grabbedItem.GetComponent<Rigidbody2D>(); // get the item's Rigidbody
            grabbedItemOriginalMass = grabbedItemRB.mass; // store the item's mass
            grabbedItemRB.mass = 0; // set the item's mass to 0 as to not influence with the character's physics
        }
    }

    void HoldItem ()
    {
        // while holding the item, position it at holdPoint
        grabbedItem.transform.position = holdPoint.transform.position;
    }

    void ReleaseItem()
    {
        // restore the item's original mass
        grabbedItemRB.mass = grabbedItemOriginalMass;
        // push the item forward in the scale of throwForce
        grabbedItem.GetComponent<Rigidbody2D>().AddForce(Vector3.right * transform.localScale.x * throwForce, ForceMode2D.Impulse);
        // clean the grabbedItem object
        grabbedItem = null;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // if colliding with DeathPit, Die()
        if (coll.gameObject.tag == GameController.Tags.DeathPit.ToString() && isAlive)
        {
            TriggerDeath();
        }
    }

    // void OnTriggerEnter2D(Collider2D coll)
    // {
	// 	// if colliding with a Coin, Destroy it and Score Points
    //     if (coll.gameObject.GetComponent<Coin>())
	// 	{
    //         coll.gameObject.GetComponent<Item>().CollectItem();
	// 	}
    // }

    // public void GetPower(GameController.Powers p)
    // {
    //     switch(p)
    //     {
    //         // if the power is to grow, make the character big
    //         case GameController.Powers.grow:
    //         {
    //             SoundController.instance.PlaySFX(SoundController.instance.sfxGetBig);
    //             poweredUpState = GameController.CharacterStates.big;
    //             break;
    //         }
    //         // if the power is fly, make the character racoon
    //         case GameController.Powers.fly:
    //         {
    //             SoundController.instance.PlaySFX(SoundController.instance.sfxGetRacoon);
    //             poweredUpState = GameController.CharacterStates.racoon;
    //             break;
    //         }
    //         case GameController.Powers.extraLife:
    //         {
    //             SoundController.instance.PlaySFX(SoundController.instance.sfxGainLife);
    //             GameController.instance.lives++;
    //             break;
    //         }
    //         // if the power is something else, keep the character's state as is
    //         default:
    //         {
    //             break;
    //         }
    //     }
    // }

    public void GetImpulseFromHit(float force)
    {
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.magenta; 

        // Draw Raycast so we can see the range in which the character would reach items
        // Vector.up/2 is just to account for the character's pivot at the bootm and draw the line towards the middle of his body
        Gizmos.DrawLine(transform.position + Vector3.up/2, transform.position + transform.localScale.x * new Vector3(grabbingDistance, 0, 0) + Vector3.up/2);
    }
}