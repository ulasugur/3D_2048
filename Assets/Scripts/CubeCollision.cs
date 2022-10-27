using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    Cube cube;
    private void Awake()
    {
        cube = GetComponent<Cube>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        Cube otherCube = collision.gameObject.GetComponent<Cube>();
        if (otherCube != null && cube.CubeID > otherCube.CubeID)
        {
            if (cube.CubeNumber == otherCube.CubeNumber)
            {
                Vector3 contanctPoint = collision.contacts[0].point;
                if (otherCube.CubeNumber<CubeSpawner.instance.maxCubeNumber)
                {
                    Cube newCube = CubeSpawner.instance.Spawn(cube.CubeNumber * 2, contanctPoint + Vector3.up * 1.6f);
                    float pushForce = 2.5f;
                    newCube.CubeRigidbody.AddForce(new Vector3(0, .3f, 1f) * pushForce, ForceMode.Impulse);

                    float randomValue = Random.Range(-20f, 20f);
                    Vector3 randomDirection=Vector3.one* randomValue;
                    newCube.CubeRigidbody.AddTorque(randomDirection);
                }

                Collider[] surroundedCube = Physics.OverlapSphere(contanctPoint, 2f);
                float explosionForce = 400f;
                float explosionRadius = 1.5f;

                foreach(Collider coll in surroundedCube)
                {
                    if (coll.attachedRigidbody !=null)
                    {
                        coll.attachedRigidbody.AddExplosionForce(explosionForce, contanctPoint, explosionRadius);
                    }

                    FX.Instance.PlayCubeExplosionFX(contanctPoint, cube.CubeColor);

                    CubeSpawner.instance.DestoyCube(cube);
                    CubeSpawner.instance.DestoyCube(otherCube);
                }
            }
        }
    }
}
