using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;    

    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private float _mouseSensitivity = 1f;

    private MultyplayerManager _multyplayerManager;

    private bool _hold = false;

    private PlayerGun _currentPlayerGun;
    private int _currentGunIndex = 0;

    private void Start()
    {
        _multyplayerManager = MultyplayerManager.Instance;
        _currentPlayerGun = _playerCharacter.GetCurrentGun(_currentGunIndex, out int newIndex);
        _currentGunIndex = newIndex;
    }

    void Update()
    {
        if (_hold) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        bool jump = Input.GetKeyDown(KeyCode.Space);

        bool shot = Input.GetMouseButtonDown(0);

        bool changeGun = Input.GetKeyDown(KeyCode.E);

        _playerCharacter.SetInput(h, v, mouseX * _mouseSensitivity);
        _playerCharacter.RotateX(-mouseY * _mouseSensitivity);

        if (jump)
            _playerCharacter.Jump();

        if (shot && _currentPlayerGun.TryShoot(out ShootInfo shootInfo))
        {
            SendShoot(ref shootInfo);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) _playerCharacter.SitDown();
        if (Input.GetKeyUp(KeyCode.LeftControl)) _playerCharacter.StandUp();

        if (changeGun)
        {            
            _currentPlayerGun = _playerCharacter.GetCurrentGun(_currentGunIndex+1, out int newIndex);
            _currentGunIndex = newIndex;
            SendGunIndex(_currentGunIndex);
        }

        SendMove();
    }

    private void SendGunIndex(int index)
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "iGun", index}
        };
        _multyplayerManager.SendMessage("changeGun", data);
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

    public void Restart(string jsonRestartInfo)
    {
        ResrtartInfo info = JsonUtility.FromJson<ResrtartInfo>(jsonRestartInfo);
        StartCoroutine(Hold());

        _playerCharacter.SetInput(0, 0, 0);
        _playerCharacter.transform.position = new Vector3(info.x, 0, info.z);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", info.x},
            { "pY", 0},
            { "pZ", info.z},

            { "vX", 0},
            { "vY", 0},
            { "vZ", 0},

            { "rX", 0},
            { "rY", 0},

            { "s", false}
        };

        _multyplayerManager.SendMessage("move", data);
    }

    private IEnumerator Hold()
    {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;
    }
}

[Serializable]
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

[Serializable]
public struct ResrtartInfo
{    
    public float x;    
    public float z;
}