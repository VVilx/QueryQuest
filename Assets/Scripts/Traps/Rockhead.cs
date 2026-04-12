using UnityEngine;
using UnityEngine.UIElements;

public class Rockhead : EnemyDamage 
{
    [Header("Spikehead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private Vector3[] directions = new Vector3[1];
    private Vector3 destination;
    [SerializeField] private float checkTimer;
    private bool attacking;
    private bool hitCeiling;
    
    [Header ("SFX")]
    [SerializeField] private AudioClip impactSound;

    [SerializeField] private LayerMask groundLayer;

    private void OnEnable()
    {
        destination = Vector3.zero; 
        attacking = false;
        hitCeiling = false;
    }
    
    private void Update()
    {
        // If destination points down, we're attacking
        if(destination.y < 0)
        {
            transform.Translate(destination * speed * Time.deltaTime);
        }
        // If destination points up, we're returning
        else if(destination.y > 0)
        {
            // Only move if we haven't hit ceiling and not at max height
            if(!hitCeiling && transform.localPosition.y < 5f)
            {
                transform.Translate(destination * speed * Time.deltaTime);
            }

            // Stop when back at starting height (y = 5) OR hit ceiling
            if(transform.localPosition.y >= 5f || hitCeiling)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 5, transform.localPosition.z);
                destination = Vector3.zero;
                attacking = false;
                hitCeiling = false; // Reset for next cycle
            }
        } 
        // Otherwise we're waiting to detect player
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
        }
    }
    
    private void CheckForPlayer()
    {
        CalculateDirections();

        for(int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red); 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if(hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i].normalized;
                checkTimer = 0f;              
            }
        }
    }
    
    private void CalculateDirections()
    {
        directions[0] = -transform.up * range;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D(collision);
        
        destination = transform.up;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If moving up and hit something (the ceiling)
        if(destination.y > 0 && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hitCeiling = true;
        }
    }
}