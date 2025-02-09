﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startAnimal : MonoBehaviour {

	private GameObject horse;
	private Animator _animator;
	private bool isDead;
	public bool tempStartButton = false;

	// Use this for initialization
	void Start()
	{
		_animator = this.GetComponent<Animator>();
		isDead = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			tempStartButton = true;
		}
		horse = GameObject.FindWithTag("horse");
		transform.forward += horse.transform.forward.normalized;
		if (!isDead) this.transform.position += this.transform.forward.normalized * 5 * Time.deltaTime;

		if (Mathf.Abs(this.transform.eulerAngles.z) >= 70 && Mathf.Abs(this.transform.eulerAngles.z) <= 290)
		{
			Destroy(this.gameObject);
		}
		if (this.transform.position.y <= -25)
		{
			Destroy(this.gameObject);
		}
		if (tempStartButton)
		{
			_animator.SetTrigger("isShot");
			isDead = true;
			StartCoroutine(animalDisappear());
			print("Start Game");
			SendMessageUpwards("StartGame");
			tempStartButton = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("arrow"))
		{
			_animator.SetTrigger("isShot");
			isDead = true;
			StartCoroutine(animalDisappear());
			print("Start Game");
			SendMessageUpwards("StartGame");
		}
	}
	IEnumerator animalDisappear()
	{
		yield return new WaitForSeconds(0.8f);
		Destroy(this.gameObject);
		//Score
	}


}
