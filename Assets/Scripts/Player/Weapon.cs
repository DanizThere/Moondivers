using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected private float impactForce, fireRate, nextTimeToShoot, maxBullets, timeToReload;
    protected float bullets;
    protected bool canShoot;
    protected Ray ray;
    public float ammo;
    public float maxAmmo;
    public AudioSource shootSound;
    public enum typeofGun
    {
        pistol,
        shotgun,
        rifle,
        machinegun
    }
    public typeofGun type;
    [SerializeField] private TMPro.TMP_Text text;

    private void Start()
    {
        ammo = maxBullets * 2;
        bullets = maxBullets;
        canShoot = true;
    }

    public void Check()
    {
        bullets = Math.Clamp(bullets, 0, maxBullets);

        text.text = $"Ammo: {bullets} / {ammo}";

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToShoot && canShoot)
        {
            nextTimeToShoot = Time.time + 1 / fireRate;
            Shoot();
            bullets--;
            shootSound.Play();
        }
        if (bullets == 0)
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }
        if (Input.GetKeyDown(KeyCode.R) && bullets < maxBullets)
        {
            if (ammo > 0)
            {
                StartCoroutine(nameof(Reload));
                ammo -= (maxBullets - bullets);
            }
        }

    }
    public void AddAmmo(float add)
    {
        ammo += add;
    }
    public abstract void Shoot();
    public abstract IEnumerator Reload();
}

