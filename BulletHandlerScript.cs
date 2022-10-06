using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandlerScript : MonoBehaviour
{
    public static BulletHandlerScript Instance
    {
        get; private set;
    }

    public GameObject bullet;
    public Transform GunPointHead { get; set; }
    public Transform bulletContainer;
    public float bullet_speed = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void CreateBullet()
    {
        if(GunPointHead == null)
        {
            return;
        }
        GameObject bullet_new = Instantiate(bullet, GunPointHead.position, GunPointHead.rotation, bulletContainer);
        Rigidbody bulletRB = bullet_new.GetComponent<Rigidbody>();
        bulletRB.AddForce(GunPointHead.forward * bullet_speed, ForceMode.VelocityChange);
    }

}
