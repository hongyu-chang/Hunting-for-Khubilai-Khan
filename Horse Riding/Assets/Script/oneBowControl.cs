﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading;

public class OneBowControl : MonoBehaviour
{
	
	public GameObject arrow;
	private GameObject arrowClone;
	private bowMiddle bowMiddle;

	// Arduino connection
	private CommunicateWithArduino Uno = new CommunicateWithArduino();

	void Start()
	{
		new Thread(Uno.connectToArdunio).Start();
	}

	void Update()
	{

		switch (PlayerStatus())
		{
			case 1://拉弓 : 產生箭 箭往後(根據rotary encoder)
				if (Input.GetKeyDown(KeyCode.A))//產生箭
				{
					bowMiddle = gameObject.GetComponentInChildren<bowMiddle>();
					arrowClone = Instantiate(arrow, bowMiddle.transform.position, bowMiddle.transform.rotation);
					arrowClone.active = true;
					arrowClone.transform.up = bowMiddle.transform.forward;
				}


				break;

			case 2://放箭 : 射

				break;

			case 3://縮 : 箭消失

				break;

			default:

				break;
		}


	}

	public int PlayerStatus()
	{
		/**
		 * 利用收到的資料去判斷下列三個狀態arduinoController.ReadLine();
		 * 1. 拉弓 : 產生箭 箭往後(根據rotary encoder)
		 * 2. 放箭 : 射
		 * 3. 縮 : 箭消失
		 * */
		return 1;
	}

	class CommunicateWithArduino
	{
		public bool connected = true;
		public bool mac = false;
		public string choice = "cu.usbmodem1421";
		private SerialPort arduinoController;

		public void connectToArdunio()
		{

			if (connected)
			{
				string portChoice = "COM5";
				if (mac)
				{
					int p = (int)Environment.OSVersion.Platform;
					// Are we on Unix?
					if (p == 4 || p == 128 || p == 6)
					{
						List<string> serial_ports = new List<string>();
						string[] ttys = Directory.GetFiles("/dev/", "cu.*");
						foreach (string dev in ttys)
						{
							if (dev.StartsWith("/dev/tty."))
							{
								serial_ports.Add(dev);
								Debug.Log(String.Format(dev));
							}
						}
					}
					portChoice = "/dev/" + choice;
				}
				arduinoController = new SerialPort(portChoice, 9600, Parity.None, 8, StopBits.One);
				arduinoController.Handshake = Handshake.None;
				arduinoController.RtsEnable = true;
				arduinoController.Open();
				Debug.LogWarning(arduinoController);
			}

		}
		public void SendData(object obj)
		{
			string data = obj as string;
			Debug.Log(data);
			if (connected)
			{
				if (arduinoController != null)
				{
					arduinoController.Write(data);
					arduinoController.Write("\n");
				}
				else
				{
					Debug.Log(arduinoController);
					Debug.Log("nullport");
				}
			}
			else
			{
				Debug.Log("not connected");
			}
			Thread.Sleep(500);
		}

		public String ReceiveData()
		{
			return arduinoController.ReadLine();
		}
	}
}

