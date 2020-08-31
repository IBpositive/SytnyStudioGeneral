using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;


    }
    
    public bool isFiring = false;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0f;
    public ParticleSystem[] _muzzleFlash;
    public Transform _raycastOrigin;
    public Transform _raycastDestination;
    public ParticleSystem _hitEffect;
    public AnimationClip weaponAnimation;

    [SerializeField] private int _fireRate = 25;

    private Ray ray;
    private RaycastHit hitInfo;
    private float accumulateTime;
    List<Bullet> bullets = new List<Bullet>();
    private float despawnTime = 3f;

    Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + 
               (bullet.initialVelocity * bullet.time) +
               (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        return bullet;
    }
    
    public void StartFiring()
    {
        isFiring = true;
        accumulateTime = 0.0f;
        Fire();
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulateTime += deltaTime;
        float fireInterval = 1f / _fireRate;
        while (accumulateTime >= 0f)
        {
            Fire();
            accumulateTime -= fireInterval;
        }
    }

    public void UpdateBullet(float deltaTime)
    {
        SimulateBullets(Time.deltaTime);
        DestroyBullets();
    }

    private void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= despawnTime);
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = (end - start).magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 2.0f, false);
       
            _hitEffect.transform.position = hitInfo.point;
            _hitEffect.transform.forward = hitInfo.normal;
            _hitEffect.Emit(1);

            bullet.time = despawnTime;
        }
        
    }
    
    private void Fire()
    {
        foreach (var particle in _muzzleFlash)
        {
            particle.Emit(1);
        }

        Vector3 velocity = (_raycastDestination.position - _raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(_raycastOrigin.position, velocity);

        bullets.Add(bullet);
        // ray.origin = _raycastOrigin.position;
        // ray.direction = _raycastDestination.position - _raycastOrigin.position;
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
