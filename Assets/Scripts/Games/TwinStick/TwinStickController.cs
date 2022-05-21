using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TwinStickInput))]
public class TwinStickController : MonoBehaviour
{
    public TwinStickInput input;
    public PlayerInput playerInput;
    public Camera cam;

    public TwinStickBulletPool bulletPool;
    public float shotInterval = 0.25f;
    public float shotTimer;

    public float baseDamage = 5f;
    public float baseSpeed = 2.5f;
    
    public float bulletSpeed;
    public float bulletDamage;
    public float playerSpeed = 3f;
    public float rotationSpeed = 4f;

    private Vector3 direction;
    public Transform bulletSpawnPoint;

    private Rigidbody rigid;

    private bool canPlay = false;
    
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void SetupPlayer(float damageModifier = 1f, float speedModifier = 1f)
    {
        bulletDamage = baseDamage * damageModifier;
        bulletSpeed = baseSpeed * speedModifier;
        canPlay = true;
    }
    
    void Update()
    {
        if (!canPlay) return;
        shotTimer -= Time.fixedDeltaTime;
        Move();
        Rotate();
        Shoot();
    }

    void Move()
    {
        rigid.velocity = new Vector3(input.move.normalized.x * (playerSpeed * Time.fixedDeltaTime), 0f, input.move.normalized.y * (playerSpeed * Time.fixedDeltaTime));
    }

    void Rotate()
    {
        if (input.rotate == Vector2.zero)
            return;
        if (playerInput.currentControlScheme == "KeyboardMouse")
        {
            MouseRotate();
            return;
        }
        
        Vector3 rotVector = Quaternion.LookRotation(new Vector3(input.rotate.x, 0f, -input.rotate.y)).eulerAngles;
        rotVector.x = rotVector.z = 0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotVector), rotationSpeed * Time.fixedDeltaTime);
    }

    void MouseRotate()
    {
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(input.rotate.x, input.rotate.y, cam.transform.position.y));
        //mouseWorldPosition.y = transform.position.y;
        Vector3 rotationVector = Quaternion.LookRotation(mouseWorldPosition - transform.position).eulerAngles;
        rotationVector.x = rotationVector.z = 0;

        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(rotationVector), 1f);

    }

    void Shoot()
    {
        if (input.shoot)
        {
            input.shoot = false;
            if (shotTimer <= 0)
            {
                shotTimer = shotInterval;
                bulletPool.FireBullet(bulletSpawnPoint.transform.forward, bulletSpawnPoint.position, bulletSpawnPoint.rotation, bulletSpeed, bulletDamage);
            }
        }
    }
}
