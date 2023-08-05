using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _player.SetInput(h, v);

        SendMove();
    }

    private void SendMove()
    {
        _player.GetPosition(out Vector3 position, out Vector3 velocity);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x}, 
            { "pY", position.y},
            { "pZ", position.z},

            { "vX", velocity.x},
            { "vY", velocity.y},
            { "vZ", velocity.z}
        };

        MultyplayerManager.Instance.SendMessage("move", data);
    }
}
