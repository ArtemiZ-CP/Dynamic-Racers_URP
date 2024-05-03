using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private List<Transform> _movePoints;
	[SerializeField] private float _speed = 1f;
	[SerializeField] private float _delayAfterMove = 1f;

	private int _currentMovePointIndex = 0;

	[ContextMenu("Look At Target")]
	public void LookAtTarget()
	{
		if (_target != null)
		{
			transform.LookAt(_target);
		}
	}

	private void Start()
	{
		transform.position = _movePoints[0].position;
		StartCoroutine(Move());
	}

	private void LateUpdate()
	{
		LookAtTarget();
	}

	private Transform GetRandomTarget()
	{
		int index = Random.Range(0, _movePoints.Count);

		if (_currentMovePointIndex == index)
		{
			index = (index + 1) % _movePoints.Count;
		}

		_currentMovePointIndex = index;

		return _movePoints[index];
	}

	private IEnumerator Move()
	{
		while (true)
		{
			Transform movePoint = GetRandomTarget();

			while (Vector3.Distance(transform.position, movePoint.position) > 0.1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, movePoint.position, _speed * Time.deltaTime);
				yield return null;
			}

			yield return new WaitForSeconds(_delayAfterMove);
		}
	}
}
