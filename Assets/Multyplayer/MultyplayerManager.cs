using Colyseus;
using System.Collections.Generic;
using UnityEngine;

public class MultyplayerManager : ColyseusManager<MultyplayerManager>
{
    [field: SerializeField] public ScoreCounter ScoreCounter { get; private set; }
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private EnemyController _enemyController;

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
            { "speed", _playerCharacter.Speed},
            { "hp", _playerCharacter.MaxHealth},
            { "sitM", _playerCharacter.SitMultiplier},
            { "team", _playerCharacter.Team}
        };
       _room = await Instance.client.JoinOrCreate<State>("state_handler", data); 

        _room.OnStateChange += OnChange;

        _room.OnMessage<string>("Shoot", ApplyShoot);

        _room.OnMessage<string>("score", UpdateScore);
    }

    private void UpdateScore(string jsoneScore)
    {
        ScoreInfo score = JsonUtility.FromJson<ScoreInfo>(jsoneScore);
        ScoreCounter.SetTeamsScore(score.a, score.b);
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
                
        PlayerCharacter playerCharacter = Instantiate(_playerCharacter, position, Quaternion.identity);
        playerCharacter.SetTeam(player.team);

        player.OnChange += playerCharacter.OnChange;

        _room.OnMessage<string>("Restart", playerCharacter.GetComponent<PlayerController>().Restart);
    }

    private void CreateEnemy(string key, Player player)
    {
        Vector3 position = new Vector3(player.pX, player.pY, player.pZ);

        EnemyController enemy = Instantiate(_enemyController, position, Quaternion.identity);
        enemy.Init(key, player);

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
public struct ScoreInfo
{
    public int a;
    public int b;
}
