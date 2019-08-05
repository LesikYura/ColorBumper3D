using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	[SerializeField] private Camera _mainCamera;
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private Transform _tr;
	[SerializeField] private float _speed = 2f;
	[SerializeField] private float _wallDistance;
	[SerializeField] private float _minCamDistance;

	private Vector2 _lastMousePos;

	private void OnEnable()
	{
		if (_mainCamera == null)
		{
			_mainCamera = Camera.main;
		}
	}

	// Update is called once per frame
	void Update()
	{

		if (GameController.Instance != null && GameController.Instance.gameState == GameState.Started)
		{
			var deltaPos = Vector2.zero;

			if (Input.GetMouseButton(0))
			{
				Vector2 currentMousePos = Input.mousePosition;

				if (_lastMousePos == Vector2.zero)
				{
					_lastMousePos = currentMousePos;
				}

				deltaPos = currentMousePos - _lastMousePos;
				_lastMousePos = currentMousePos;

				Vector3 force = new Vector3(deltaPos.x, 0, deltaPos.y) * _speed;
				_rb.AddForce(force);
			}
			else
			{
				_lastMousePos = Vector2.zero;
			}
		}

	}

	private void LateUpdate()
	{

		if (GameController.Instance != null && GameController.Instance.gameState == GameState.Started)
		{
			// player saved
			Vector3 playerPos = _tr.position;

			if (playerPos.x < -_wallDistance)
			{
				playerPos.x = -_wallDistance;
			}
			else if (playerPos.x > _wallDistance)
			{
				playerPos.x = _wallDistance;
			}

			if (playerPos.z < _mainCamera.transform.position.z + _minCamDistance)
			{
				playerPos.z = _mainCamera.transform.position.z + _minCamDistance;
			}

			_tr.position = playerPos;
		}

	}

	private void OnCollisionEnter(Collision collision)
	{

		if (GameController.Instance != null && GameController.Instance.gameState == GameState.Started)
		{
			if (collision.gameObject.tag == "Enemy")
			{
				GameController.Instance.EndGame();
			}

			if (collision.gameObject.tag == "FinishLine")
			{
				GameController.Instance.WinGame();
			}
		}
	}

}
