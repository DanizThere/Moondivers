using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public enum typeofAmmo
    {
        pistol,
        shotgun,
        rifle,
        machinegun
    }
    public typeofAmmo typeofammo;
    public float freeAmmo = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (typeofammo == typeofAmmo.pistol)
            {
                Pistol pistol = other.gameObject.GetComponentInChildren<Pistol>();
                if( pistol != null )
                {
                    pistol.AddAmmo(freeAmmo);
                }
            }


            Destroy(gameObject);
        }
    }
}
