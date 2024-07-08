using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bgJoystick"))
        {
            _playerController.SetIsRunning(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bgJoystick"))
        {
            _playerController.SetIsRunning(false);
        }
    }
}
