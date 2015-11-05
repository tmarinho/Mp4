using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class FlightController : MonoBehaviour {
	[SerializeField] private bool m_IsFlying;
	[SerializeField] private float m_FlySpeedMin;
	[SerializeField] private float m_FlySpeed;
	[SerializeField] private float m_FlySpeedMax;
	[SerializeField] private float m_PitchSpeed;
	[SerializeField] private float m_RollSpeed;
	
	private Camera m_Camera;
	private float m_YRotation;
	private Vector2 m_Input;
	private Vector3 m_MoveDir = Vector3.zero;
	private CharacterController m_CharacterController;
	private CollisionFlags m_CollisionFlags;
	private Vector3 m_OriginalCameraPosition;
	private Vector3 movement;
	// Use this for initialization
	void Start () {
		m_CharacterController = GetComponent<CharacterController>();
		movement = Vector3.zero;
		m_IsFlying = true;
	}
	
	// Update is called once per frame
	void Update () {
		float speed;
		GetInput(out speed);
		UpdateOrientation ();
		movement = transform.forward;
		movement *= speed*Time.deltaTime;
		m_CharacterController.Move(movement);
	}

	private void GetInput(out float speed){
		if (Input.GetKey ("up"))
			m_FlySpeed += (float)0.1;
		if (Input.GetKey ("down"))
			m_FlySpeed -= (float)0.1;

		// Saturation
		if (m_FlySpeed < m_FlySpeedMin)
			m_FlySpeed = m_FlySpeedMin;
		if (m_FlySpeed > m_FlySpeedMax)
			m_FlySpeed = m_FlySpeedMax;

		// Hover
		if (Input.GetKeyDown(KeyCode.Space)) 
			m_IsFlying = !m_IsFlying;	

		if (m_IsFlying == false) {
			speed = (float)0;
			m_FlySpeed = m_FlySpeedMin;
		}
		else	
			speed = m_FlySpeed;
	}

	private void UpdateOrientation (){
		Quaternion pitch;
		Quaternion roll;
		Quaternion orientation;

		pitch = Quaternion.identity;
		roll = Quaternion.identity;
		orientation = Quaternion.identity;

		if (Input.GetKey (KeyCode.W)){
			pitch.w = (float)Math.Cos (0.5 * m_PitchSpeed * Time.deltaTime);
			pitch.x = (float)Math.Sin (0.5 * m_PitchSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)){
			pitch.w = (float)Math.Cos (0.5 * -m_PitchSpeed * Time.deltaTime);
			pitch.x = (float)Math.Sin (0.5 * -m_PitchSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.A)){
			roll.w = (float)Math.Cos (0.5 * m_RollSpeed * Time.deltaTime);
			roll.z = (float)Math.Sin (0.5 * m_RollSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)){
			roll.w = (float)Math.Cos (0.5 * -m_RollSpeed * Time.deltaTime);
			roll.z = (float)Math.Sin (0.5 * -m_RollSpeed * Time.deltaTime);
		}
		orientation = pitch * roll;
		transform.Rotate(orientation.eulerAngles);
		
	}
}
