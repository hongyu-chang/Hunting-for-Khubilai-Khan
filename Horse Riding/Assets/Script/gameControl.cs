﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameControl : MonoBehaviour {

	public GameObject scoreGroup;
	public GameObject timeGroup;
	public Text introduction;
	public Text labelScore;
	public Text labelTime;
	public Text popMessage;
	private int NowScore = 0;
	private int time = 0;
	private float timer_f = 0f;
	private bool TimerOn = false;
	private string animalName;
	private int score = 0;
	private Vector3 popMessagePosition;
	private Color popMessageColor;
	private bool showPopMessage = false;

	// Use this for initialization
	void Start ()
	{
		popMessagePosition = popMessage.rectTransform.localPosition; 
		popMessageColor = popMessage.color; //white
		scoreGroup.gameObject.SetActive(false);
		timeGroup.gameObject.SetActive(false);
		popMessage.gameObject.SetActive(false);
		introduction.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateTime(Time.deltaTime);
		UpdateUI(Time.deltaTime);

		//Button N: go to yurt scene
		if (Input.GetKeyDown(KeyCode.N))
		{
			StartCoroutine(yurtScene());
			Debug.Log("Button N: go to yurt scene");
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel("Hunt");
			Debug.Log("Button R: Restart");
		}

	}

	public void AddScore(string animalName)
	{
		//print(animalName);
		animalName = animalName.Replace("(Clone)", "");
		this.animalName = animalName;
		//print(String.Compare(animalName, "Terrain") + " " +animalName);
		if (String.Compare(animalName, "Terrain") != 0)
		{
			score = 20;
			NowScore += score;
			showPopMessage = true;
		}

		labelScore.text = string.Format("{0:D2}", NowScore);
		
	}

	public void StartGame()
	{
		introduction.gameObject.SetActive(false);
		TimerOn = true;
		NowScore = 0;
		time = 100;
		timer_f = 100f;
		labelScore.text = string.Format("{0:D2}", NowScore);
		scoreGroup.gameObject.SetActive(true);
		timeGroup.gameObject.SetActive(true);
		popMessage.gameObject.SetActive(true);
	}

	private void UpdateTime(float num)
	{
		if (TimerOn)
		{
			timer_f -= num;
			time = (int)timer_f;
			labelTime.text = string.Format("{0:D2}", time);
			if (time == 0)
			{
				introduction.text = "遊戲結束！！ 總得分為：" + NowScore;
				scoreGroup.gameObject.SetActive(false);
				timeGroup.gameObject.SetActive(false);
				popMessage.gameObject.SetActive(false);
				introduction.gameObject.SetActive(true);
				TimerOn = false;
				StartCoroutine(yurtScene());
				//這裡跳下一個場景 要傳送總分數
			}
		}
		
	}

	private void UpdateUI(float num)
	{
		popMessage.rectTransform.localPosition = new Vector3(popMessage.rectTransform.localPosition.x, popMessage.rectTransform.localPosition.y
		+ 100f * Time.deltaTime, popMessage.rectTransform.localPosition.z);

		popMessageColor.a -= 0.025f;
		popMessage.color = popMessageColor;
		//Debug.Log(animalName);
		if(animalName == "StartAnimal")
			popMessage.text = "";
		else
			popMessage.text = "plus " + score;
		if (showPopMessage)
		{
			popMessage.rectTransform.localPosition = popMessagePosition;
			popMessageColor.a = 1f;
			showPopMessage = false;
		}
	}

	IEnumerator yurtScene()
	{
		yield return new WaitForSeconds(0.8f);
		float fadeTime = GameObject.Find("Canvas").GetComponent<fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene("Yurt-V2");
	}
}
