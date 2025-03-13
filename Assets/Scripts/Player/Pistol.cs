using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public Animator anim;
    public float damage;
    public Transform shootpos;

    public override IEnumerator Reload()
    {
        anim.SetTrigger("Reloading");
        yield return new WaitForSeconds(timeToReload);
        bullets = maxBullets;
    }
    public override void Shoot()
    {
        RaycastHit hit;
        ray = new Ray(shootpos.position, -shootpos.forward);
        if(Physics.Raycast(ray,out hit, 35f)) 
        {
            Debug.Log(hit.transform.name);
        }
    }
    private void Start()
    {
        type = typeofGun.pistol;
    }
    private void Update()
    {
        Check();

        ammo = Mathf.Clamp(ammo, 0, maxAmmo);
    }
}
