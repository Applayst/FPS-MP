using Colyseus;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MultyplayerManager : ColyseusManager<MultyplayerManager>
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemy;

    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();

        Connect();
    }

    private async void Connect()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "speed", _player.Speed},
            { "sitM", _player.SitMultiplier}
        };
       _room = await Instance.client.JoinOrCreate<State>("state_handler", data); 

        _room.OnStateChange += OnChange;

        _room.OnMessage<string>("Shoot", ApplyShoot);
    }

    private void ApplyShoot(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

        if (_enemies.ContainsKey(shootInfo.key))
        {
            _enemies[shootInfo.key].Shoot(shootInfo);
        }
        else
        {
            Debug.Log("Врага нет. кто Стрелял?");
        }
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == false) return;
        
        state.players.ForEach((key, player) => {
            if (key == _room.SessionId)
                CreatePlayer(player);
            else
                CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player)
    {        
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);

        Instantiate(_player, position, Quaternion.identity);
    }

    private void CreateEnemy(string key, Player player)
    {
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);

        EnemyController enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(player);

        _enemies.Add(key, enemy);
    }

    private void RemoveEnemy(string key, Player value)
    {
        if (_enemies.ContainsKey(key))
        {
            EnemyController enemy = _enemies[key];
            enemy.Destroy();
            _enemies.Remove(key);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _room.Leave();
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }
    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    public string GetSessionID()
    {
        return _room.SessionId;
    }
}
