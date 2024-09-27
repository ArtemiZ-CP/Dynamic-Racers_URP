using UnityEngine;

public class Billboard : MonoBehaviour
{
	[SerializeField] private bool _fixX = false;
	[SerializeField] private bool _fixY = false;
	[SerializeField] private bool _fixZ = false;

	private void Update()
	{
		Vector3 forward = Camera.main.transform.forward;

		if (_fixX)
			forward.x = 0;
		if (_fixY)
			forward.y = 0;
		if (_fixZ)
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
