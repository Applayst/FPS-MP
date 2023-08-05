using Colyseus;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MultyplayerManager : ColyseusManager<MultyplayerManager>
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private EnemyController _enemyPrefab;

    private ColyseusRoom<State> _room;

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();

        Connect();
    }

    private async void Connect()
    {
       _room = await Instance.client.JoinOrCreate<State>("state_handler");

        _room.OnStateChange += OnChange;
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

        Instantiate(_playerPrefab, position, Quaternion.identity);
    }

    private void CreateEnemy(string key, Player player)
    {
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);

        EnemyController enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        player.OnChange += enemy.OnChange;
    }
    private void RemoveEnemy(string key, Player value)
    {
        
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
}
