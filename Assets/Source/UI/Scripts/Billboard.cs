using UnityEngine;

public class Billboard : MonoBehaviour
{
	[SerializeField] private bool fixX = false;
	[SerializeField] private bool fixY = false;
	[SerializeField] private bool fixZ = false;

	private void Update()
	{
		Vector3 forward = Camera.main.transform.forward;

		if (fixX)
			forward.x = 0;
		if (fixY)
			forward.y = 0;
		if (fixZ)
			forward.z = 0;

		if (forward == Vector3.zero)
		{
			transform.rotation = Quaternion.identity;
		}
		else
		{
			transform.rotation = Quaternion.LookRotation(forward);
		}
	}
}
