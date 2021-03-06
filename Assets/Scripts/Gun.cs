﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

	//銃に入れれる弾の限界数
	public int bullet;

	//弾倉
	public int bulletBox;
	float coolTime;

	//残弾数・リロードする弾数
	public int residualBullet, reloadBullet;

	//銃声・リロード音
	[SerializeField] AudioClip gunfireSound, reloadSound;
	AudioSource audioSource;

	//スナイパー機能
	[SerializeField] GameObject scopeImage;
	bool isSniper = false;
	private Vector3 scopePosition;
	//スコープ倍率
	[SerializeField] private float magnification;
	[SerializeField] private Camera camera;

	void Start () {
		//残弾数 = 初期弾数
		residualBullet = bullet;
		audioSource = transform.GetComponent<AudioSource> ();

		//スナイパー機能
		scopeImage.SetActive(false);
	}
	
	void Update () {

		Snipe ();

		//クーリング
		coolTime -= Time.deltaTime;

		//リロードする弾数
		reloadBullet = bullet - residualBullet;

		//クリックしてショットする
		if (Input.GetMouseButton(0) && residualBullet != 0) {
			//クールタイム機能
			if (coolTime <= 0f){
				Shoot();
			}
		}

		if (Input.GetKeyDown (KeyCode.R) && residualBullet < bullet) {
			Reload ();
		}
	}

	//ショット機能
	void Shoot () {
		residualBullet -= 1;

		//クールタイム設定
		coolTime = 0.5f;
		audioSource.PlayOneShot (gunfireSound);
	}

	//リロード機能
	void Reload () {
		audioSource.PlayOneShot (reloadSound);
		//残弾数 = 初期弾数
		residualBullet += reloadBullet;
		//リロードすると弾倉内弾数が減る
		bulletBox -= reloadBullet;		
	}

	//スナイパー機能
	void Snipe () {
		if (Input.GetMouseButtonDown (1)) {
			if (!isSniper) {
				this.camera.fieldOfView -= this.magnification;
				scopeImage.SetActive (true);
				isSniper = true;
			} else {
				this.camera.ResetFieldOfView ();
				scopeImage.SetActive (false);
				isSniper = false;
			}
		}
	}
}