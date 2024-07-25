using UnityEngine;

[ExecuteAlways]
public class RandomRotation : MonoBehaviour
{
    private void Awake()
    {
        Environment environment = GetComponentInParent<Environment>();

        if (environment.EnvironmentType == Environment.Type.Ground)
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
        else if (environment.EnvironmentType == Environment.Type.Wall)
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }
}
