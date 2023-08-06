using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;

    [SerializeField] private float _mouseSensitivity = 1f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        bool space = Input.GetKeyDown(KeyCode.Space);

        _player.SetInput(h, v, mouseX * _mouseSensitivity);
        _player.RotateX(-mouseY * _mouseSensitivity);

        if (space)
            _player.Jump();

        SendMove();
    }

    private void SendMove()
    {
        _player.GetPosition(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x}, 
            { "pY", position.y},
            { "pZ", position.z},

            { "vX", velocity.x},
            { "vY", velocity.y},
            { "vZ", velocity.z},

            { "rX", rotateX},
            { "rY", rotateY}
        };

        MultyplayerManager.Instance.SendMessage("move", data);
    }
}
