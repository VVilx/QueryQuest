using UnityEngine;
using UnityEngine.UIElements;

public class Spikehead : EnemyDamage 
{
    [Header("Spikehead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private Vector3[] directions = new Vector3[4];
    private Vector3 destination;
    [SerializeField] private float checkTimer;
    private bool attacking;
    
    [Header ("SFX")]
    [SerializeField] private AudioClip impactSound;

    private void OnEnable()
    {
        Stop();
    }
    private void Update()
    {
        //Move spikehead to destination only if attacking
        if(attacking)
        {
            transform.Translate(destination * speed * Time.deltaTime);
        }
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

        //Check if spikehead sees player
        for(int i =0; i < directions.Length; i++)
        {
          Debug.DrawRay(transform.position, directions[i], Color.red); 
          RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

          if(hit.collider != null)
          {
              attacking = true;
              destination = directions[i].normalized;
              checkTimer = 0f;              
          }
        }
    }
    private void CalculateDirections()
    {
        directions[0] = transform.right * range; //Right
        directions[1] = -transform.right * range; //Left
        directions[2] = transform.up * range; //Up
        directions[3] = -transform.up * range; //Down
    }
    private void Stop()
    {
        destination = transform.position; //Stop moving
        attacking = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D(collision);
        //Stop spikehead once he hits something
        Stop();
    }
}
