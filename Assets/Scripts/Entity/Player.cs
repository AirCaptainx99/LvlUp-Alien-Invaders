using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float speed;
    public int level;
    [SerializeField]
    private float horizontalLimit = 2.5f;
    private Rigidbody2D rb;

    public float cooldownTimer;
    public float firingCooldown;
    public float projectileSpeed;

    private AudioSource audioSource;
    [SerializeField] private AudioClip laserAudio;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // Player Movement
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, 0f);
        if (transform.position.x > horizontalLimit)
        {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(horizontalLimit, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -horizontalLimit)
        {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(-horizontalLimit, transform.position.y, transform.position.z);
        }

        // Player Attack
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            if (cooldownTimer < 0)
            {
                cooldownTimer = firingCooldown;
                audioSource.PlayOneShot(laserAudio);

                float degree = 180 / (level + 1);
                for (int i = 1; i <= level; i++)
                {
                    GameObject laser = ObjectPool.Instance.GetGameObjectFromPool("Player Laser", 2f).gameObject;
                    laser.transform.position = transform.position;
                    laser.transform.rotation = Quaternion.Euler(Vector3.forward * (i * degree - 90f));
                    laser.GetComponent<Rigidbody2D>().velocity = laser.transform.up * projectileSpeed;
                }
            }
        }
    }

    protected override GameObject OnDie()
    {
        GameObject explosion = base.OnDie();

        GameManager.Instance.GameOver(explosion.GetComponent<ParticleSystem>());

        return null;
    }
}
