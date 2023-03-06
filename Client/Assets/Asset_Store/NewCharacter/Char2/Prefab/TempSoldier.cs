using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSoldier : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float jumpPower;
	public float jumpChargeTime;
	public Transform[] rayP;
	public float[] rayLen;
	
    Rigidbody rigid;
    Animator anim;
	bool isOnGround;
	bool isWall;
	bool jumpSet;
	float chargeTime;

	void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
		isOnGround = false;
		isWall = false;
        jumpSet = false;
		chargeTime = 0;
    }

	void Update()
	{
		MoveAndJump();
    }

	void MoveAndJump() {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		float groundAngle = RayF(rayP[0].position, rayP[1].position, rayLen[0], "Down");
		float wallAngle = RayF(rayP[2].position, rayP[3].position, rayLen[1], "Front");
		float angle;

		if (!isWall) angle = groundAngle;
		else angle = wallAngle / 4;

		Debug.Log("ground: " + groundAngle + ", wall: " + wallAngle + ", isWall: " + isWall);

		if (isWall) {
			anim.SetBool("IsClimb", true);
			h *= 0;
		}
		else anim.SetBool("IsClimb", false);
		anim.SetBool("IsOnGround", isOnGround);
		if (v != 0) anim.SetBool("JumpSet", false);

		if (!isOnGround && !anim.GetBool("IsClimb"))
		{
			v *= 0.05f;
			h *= 0.05f;
		}

		else if (Input.GetButton("Dash") && v > 0) v *= 5;

		if (jumpSet) v *= 0.01f;

		Vector3 vel;

		float cos = Mathf.Cos(Mathf.Deg2Rad * angle);
		float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
		float tan = 1;
		if (cos*sin != 0) tan = cos / sin;

		
		if (anim.GetBool("IsClimb"))
		{
			if (angle > 0 && angle < 180)
				vel = new Vector3(0, v * tan, v * (1 - tan));
			else vel = new Vector3(0, 0, v);
		}
		else
		{
			//rigid.useGravity = true;
			if (angle > 0 && angle < 180)
				vel = new Vector3(0, v * tan, v * (1 - tan));
			else vel = new Vector3(0, 0, v);
		}
		
		vel = transform.TransformDirection(vel) * speed;

		transform.localPosition += vel * Time.fixedDeltaTime;
		anim.SetFloat("Speed", v);

		if (isOnGround)
		{
			if (Input.GetButtonDown("Jump"))
			{
				anim.SetBool("JumpSet", true);
			}
		}

		if (!anim.GetBool("JumpSet"))
		{
			transform.Rotate(0, h * rotateSpeed, 0);
			chargeTime = 0;
		}

		else
		{
			if(chargeTime < jumpChargeTime)
				chargeTime += Time.deltaTime;
		}

		if (isOnGround && Input.GetButtonUp("Jump") && anim.GetBool("JumpSet")) {
			rigid.AddForce(new Vector3(0, jumpPower * chargeTime / jumpChargeTime, 0), ForceMode.Impulse);
			anim.SetBool("IsOnGround", false);
			anim.SetBool("JumpSet", false);
		}

	}

	IEnumerator Jump()
	{
		anim.SetBool("IsJump", true);
		jumpSet = true;
		yield return new WaitForSeconds(0.75f);
		rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
		jumpSet = false;
		yield return new WaitForSeconds(1f);
		jumpSet = true;
		yield return new WaitForSeconds(0.5f);
		jumpSet = false;
		anim.SetBool("IsJump", false);
	}

	float RayF(Vector3 front, Vector3 back, float len, string dir)
	{
		RaycastHit hit1, hit2;
		bool fh = false, bh = false;
		Vector3 vec;

		if (dir == "Front")
		{
			vec = transform.forward;
		}
		else vec = Vector3.down;

		Debug.DrawRay(front, vec * len, Color.red, 0.1f);
		if (Physics.Raycast(front, vec, out hit1, len))
		{
			fh = true;
		}

		if (dir == "Front")
		{
			len *= rayLen[2];
		}
		Debug.DrawRay(back, vec * len, Color.red, 0.1f);
		if (Physics.Raycast(back, vec, out hit2, len))
		{
			bh = true;
		}
		float y;
		float z;
		if (dir == "Front")
		{
			y = Mathf.Sqrt((hit1.point.z - front.z) * (hit1.point.z - front.z) + (hit1.point.x - front.x) * (hit1.point.x - front.x))
				- Mathf.Sqrt((hit2.point.z - back.z) * (hit2.point.z - back.z) + (hit2.point.x - back.x) * (hit2.point.x - back.x));
			z = hit1.point.y - hit2.point.y;
		}
		else
		{
			y = hit1.point.y - hit2.point.y;
			z = hit1.point.z - hit2.point.z;
		}

		if (fh && bh)
		{
			if (dir == "Front") isWall = true;
			else isOnGround = true;
			
			float a = Mathf.Sqrt(y * y + z * z);
			if (y * a != 0)
			{
				float angle = (1 / Mathf.Sin(Mathf.Deg2Rad * y / a)) % 360;
				return angle;
			}

			//if (dir == "Front") Debug.Log("Wangle = " + angle);
			//else Debug.Log("Gangle = " + angle);
			else
				return 0;
		}
		
		else
		{
			if (dir == "Front") isWall = false;
			else isOnGround = false;
			return 0;
		}
		/*
		//45도 이상 90도 이상일 경우 tan 값으로 y축, z축으로 힘 분산
		if (angle > 45 && angle < 90)
		{
			float tan = Mathf.Cos(Mathf.Deg2Rad * angle) / Mathf.Sin(Mathf.Deg2Rad * angle);
			velocity = new Vector3(0, v * tan, v * (1 - tan));
		}

		*/

		//절벽 각도에 따라 Climbing 애니메이션 실행시킬 경우 캐릭터의 몸의 각도 변화
		//gameObject.transform.Rotate(90 - angle, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
	}

	
}
