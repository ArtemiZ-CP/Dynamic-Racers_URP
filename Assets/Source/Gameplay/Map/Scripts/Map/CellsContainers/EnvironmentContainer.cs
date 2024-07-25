using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentContainer", menuName = "Containers/EnvironmentContainer")]
public class EnvironmentContainer : ScriptableObject
{
    private const float ChanceToAddLowInsteadHigh = 0.1f;

    [SerializeField] private List<Environment> _environments;

    public Environment GetRandomEnvironment(Environment.Type type, int lenth, int wigth, bool isHigh)
    {
        List<Environment> environments = new();

        foreach (Environment environment in _environments)
        {
            if (environment.Lenth <= lenth && environment.Wigth <= wigth && environment.EnvironmentType == type)
            {
                if (isHigh)
                {
                    if (environment.IsHigh)
                    {
                        environments.Add(environment);
                    }
                    else if (Random.value < ChanceToAddLowInsteadHigh)
                    {
                        environments.Add(environment);
                    }
                }
                else if (environment.IsHigh == isHigh)
                {
                    environments.Add(environment);
                }
            }
        }

        if (environments.Count == 0)
        {
            return null;
        }

        return environments[Random.Range(0, environments.Count)];
    }
}
