using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile()
    {
        Debug.Log("Firing projectile");
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
        Debug.Log("Projectile instantiated at " + launchPoint.position);

        Vector3 origScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1,
            origScale.y,
            origScale.z
        );
    }

}