using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private PlayerGun _playerGun;

    [SerializeField] private float _mouseSensitivity = 1f;

    private MultyplayerManager _multyplayerManager;

    private void Start()
    {
        _multyplayerManager = MultyplayerManager.Instance;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        bool space = Input.GetKeyDown(KeyCode.Space);

        bool mouseLeftBtn = Input.GetMouseButtonDown(0);

        _playerCharacter.SetInput(h, v, mouseX * _mouseSensitivity);
        _playerCharacter.RotateX(-mouseY * _mouseSensitivity);

        if (space)
            _playerCharacter.Jump();

        if (mouseLeftBtn && _playerGun.TryShoot(out ShootInfo shootInfo))
        {
            SendShoot(ref shootInfo);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) _playerCharacter.SitDown();
        if (Input.GetKeyUp(KeyCode.LeftControl)) _playerCharacter.StandUp();

        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multyplayerManager.GetSessionID();
        string json = JsonUtility.ToJson(shootInfo);

        _multyplayerManager.SendMessage("shoot", json);
    }

    private void SendMove()
    {
        _playerCharacter.GetPosition(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY, out bool isSit);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x}, 
            { "pY", position.y},
            { "pZ", position.z},

            { "vX", velocity.x},
            { "vY", velocity.y},
            { "vZ", velocity.z},

            { "rX", rotateX},
            { "rY", rotateY},

            { "s", isSit}
        };

        _multyplayerManager.SendMessage("move", data);
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string key;
    public float dX;
    public float dY;
    public float dZ;
    public float pX;
    public float pY;
    public float pZ;
}