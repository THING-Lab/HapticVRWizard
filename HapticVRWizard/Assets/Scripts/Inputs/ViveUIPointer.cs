﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class ViveUIPointer : MonoBehaviour {
	public float _thickness = 0.002f;
	public float _cursorScale = 0.02f;
	public Color _color;
	private int _menuLayerMask = (1 << 5);

	private GameObject _hitPoint;
	private GameObject _pointer;

	private SteamVR_TrackedObject _trackedObj;

    // Use this for initialization
    void Start()
    {
        try
        {
            _trackedObj = GetComponent<SteamVR_TrackedObject>();

            // MAYBE create some prefabs for these
            // Create laser ray visualization mesh
            _pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _pointer.transform.SetParent(transform, false);
            _pointer.transform.localScale = new Vector3(_thickness, _thickness, 100.0f);
            _pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);

            // Create cursor for pointer menu
            _hitPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _hitPoint.transform.SetParent(transform, false);
            _hitPoint.transform.localScale = new Vector3(_cursorScale, _cursorScale, _cursorScale);
            _hitPoint.transform.localPosition = new Vector3(0.0f, 0.0f, 100.0f);

            _pointer.SetActive(false);
            _hitPoint.SetActive(false);

            UnityEngine.Object.DestroyImmediate(_pointer.GetComponent<BoxCollider>());
            UnityEngine.Object.DestroyImmediate(_hitPoint.GetComponent<SphereCollider>());

            Material pointerMat = new Material(Shader.Find("Unlit/Texture"));
            pointerMat.SetColor("_Color", _color);
            _pointer.GetComponent<MeshRenderer>().material = pointerMat;
            _hitPoint.GetComponent<MeshRenderer>().material = pointerMat;
        }

        catch (Exception e)
        {
            Debug.Log("!!!!!!!Exception occured!!!!!!!");
            Debug.LogException(e, this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            Vector3 rayPos = transform.position + new Vector3(0f, 0f, 0f);
            Vector3 rayDirection = new Vector3(0f, -5f, 1f).normalized;

            Ray ray = new Ray(rayPos, transform.TransformDirection(Vector3.forward));
            RaycastHit hitInfo;
            // Make this not infinity
            bool hasHit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _menuLayerMask);

            if (hasHit && hitInfo.distance < 0.35f)
            {
                _pointer.SetActive(true);
                _hitPoint.SetActive(true);

                _pointer.transform.localScale = new Vector3(_thickness, _thickness, hitInfo.distance);
                _pointer.transform.localPosition = new Vector3(0.0f, 0.0f, hitInfo.distance * 0.5f);

                _hitPoint.transform.localPosition = new Vector3(0.0f, 0.0f, hitInfo.distance);

                // Let the stroke reader know that the menu is in control
                GetComponent<ViveStrokeReader>().SetPointerMode(true);

                // Find first hit button
                // This'll need to change
                if (hitInfo.collider.tag == "MenuButton")
                {
                    // Only start drawing if not pointing at menu
                    if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        // Should I create an Interface for this?
                        hitInfo.collider.gameObject.GetComponent<ViveMenuButton>().Execute();
                    }
                }

                if (hitInfo.collider.tag == "MenuToggle")
                {
                    // Only start drawing if not pointing at menu
                    if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        // Should I create an Interface for this?
                        print("In UIPointer MenuToggle");
                        hitInfo.collider.gameObject.GetComponent<ViveMenuToggle>().Execute();
                    }
                }
            }
            else
            {
                _pointer.SetActive(false);
                _hitPoint.SetActive(false);

                GetComponent<ViveStrokeReader>().SetPointerMode(false);
            }
        }

        catch (Exception e)
        {
            Debug.Log("!!!!!!!Exception occured!!!!!!!");
            Debug.LogException(e, this);
        }
    }
}
