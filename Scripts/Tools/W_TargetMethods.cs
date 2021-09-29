using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class W_TargetMethods
{
    public static bool InLineOfSight(Vector3 observer, Vector3 target)
    {
        RaycastHit hit;

        Physics.Linecast(observer, target, out hit);

        if (hit.collider == null) return false;
        bool pathBlocked = hit.collider.gameObject.layer != LayerMask.NameToLayer(Layers.DoomGuy);
        
        return !pathBlocked;
    }
    public static bool HitMapGeometry(RaycastHit hit, Vector3 playerPosition, Weapons.WeaponType type)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Map))
        {
            Vector3 offset = (hit.point - playerPosition).normalized * 2f;
            GameController.Instance.w_effects.PlayImpactEffect(hit.point - offset, type);

            return true;
        }
        return false;
    }
    public static float GetDistanceToTarget(Vector3 actor, Vector3 target)
    {
        return Vector3.Distance(actor, target);
    }
    public static RaycastHit WeaponHitScan(Vector3 observer, Vector3 direction, float range)
    {
        RaycastHit hit;

        Physics.Raycast(observer, direction, out hit, range);

        return hit;
    }
    public static Vector3 GetBulletSpread(Transform aimDirection, float projectileSpread)
    {
        Vector3 forwardVector = aimDirection.forward;

        var spread = projectileSpread / GameController.Instance.Rntable.P_Random();
        float height;
        float width;

        if (GameController.Instance.Rntable.P_Random() < 128)
            height = spread;
        else height = -spread;

        if (GameController.Instance.Rntable.P_Random() < 128)
            width = spread;
        else width = -spread;


        forwardVector += aimDirection.right * width;
        forwardVector += aimDirection.up * height;

        return forwardVector;
    }
    public static Vector3 GetBulletSpread(Vector3 aimDirection, float projectileSpread)
    {
        var spread = projectileSpread / GameController.Instance.Rntable.P_Random();
        float height;
        float width;

        if (GameController.Instance.Rntable.P_Random() < 128)
            height = spread;
        else height = -spread;

        if (GameController.Instance.Rntable.P_Random() < 128)
            width = spread;
        else width = -spread;

        aimDirection += Vector3.right * width;
        aimDirection += Vector3.up * height;

        return aimDirection;
    }
    public static Vector3 GetRandomImpactOffset(Vector3 targetPosition, float targetWidth, float targetHeight)
    {
        float height;
        float width;

        if (GameController.Instance.Rntable.P_Random() < 128)
            height = targetHeight;
        else height = -targetHeight;

        if (GameController.Instance.Rntable.P_Random() < 128)
            width = targetWidth;
        else width = -targetWidth;

        return new Vector3(targetPosition.x + width, targetPosition.y + height, targetPosition.z); 
    }
    public static IHittable AttackableFromCollider(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Attackable))
        {
            IHittable target = hit.collider.GetComponent<IHittable>();
            if (target != null)
            {
                return target;
            }
        }
        return null;
    }    
}