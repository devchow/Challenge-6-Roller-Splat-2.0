using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallController : MonoBehaviour
{
    [Header("Physics Settings")]
    public Rigidbody rb;
    public float speed = 15f;

    private bool _isMoving;
    private Vector3 _moveDirection;
    private Vector3 _nextCollisionPos;

    [Header("Swipe Settings")]
    public int minSwipe = 500;
    private Vector2 _swipePosLastFrame;
    private Vector2 _swipePosCurrentFrame;
    private Vector2 _currentSwipe;

    [Header("Colors")]
    private Color _solveColor;
    
    [Header("Audio")]
    public AudioSource leftRight;
    public AudioSource upDown;

    [Header("Particles")]
    public ParticleSystem trails;

    private void Start()
    {
        // Get Random Color
        _solveColor = Random.ColorHSV(0.5f, 1);
        
        // Change Ball Color to Random-Color
        GetComponent<MeshRenderer>().material.color = _solveColor;
    }
    

    private void FixedUpdate()
    {

        // If Ball is moving, move it to the x direction
        if (_isMoving)
        {
            rb.velocity = _moveDirection * speed;
            
            // Play ball move sound here
        }
        
        // Apply Color to Ground Pieces
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            // Get the ground which collides with ball
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            
            // if the ground is not colored, color it
            if (ground && !ground.isColored)
            {
                ground.ChangeColor(_solveColor);
            }
        
            i++;
        }
        
        if (_nextCollisionPos != Vector3.zero)
        {   // Compare ball position with wall ahead of ball | if Ball hits wall, stop it
            if (Vector3.Distance(transform.position, _nextCollisionPos) < 1)
            {
                _isMoving = false;
                _moveDirection = Vector3.zero;
            }
        }
        
        if(_isMoving)
            return;
        
        // Swipe Mechanism

        // When Mouse Button is pressed 
        if (Input.GetMouseButton(0)) // If swiped
        {   
            // Get the Current Swipe Position
            _swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // if swipe was not at pos 0
            if (_swipePosLastFrame != Vector2.zero)
            {
                _currentSwipe = _swipePosCurrentFrame - _swipePosLastFrame;

                // If user Accidentally Swipes
                if (_currentSwipe.sqrMagnitude < minSwipe)
                {
                    return;
                }
                
                _currentSwipe.Normalize();  // Get Direction 
                
                // If swipe is UP - DOWN
                if (_currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    // Move Ball Up or Down
                    SetDestination(_currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                    
                    upDown.Play();
                }
                
                //If swipe is LEFT - RIGHT
                if (_currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    // Move Ball Left or Right 
                    SetDestination(_currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                    
                    leftRight.Play();
                }
            }
            // Updating swipe pos frame
            _swipePosLastFrame = _swipePosCurrentFrame;
        }
        
        // When Mouse Button is Released
        if (Input.GetMouseButtonUp(0))
        {   // Stop the moving Ball
            _swipePosLastFrame = Vector2.zero;
            _currentSwipe = Vector2.zero;
            trails.Play();
        }

    } // end of Fixed Update

    private void SetDestination(Vector3 direction)
    {
        _moveDirection = direction;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            _nextCollisionPos = hit.point;
        }

        _isMoving = true;
        
    } // end of SetDestination
}



