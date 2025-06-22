using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class MultiplayerManager : Node
{
    const int SERVER_PORT = 25565;
    const string SERVER_IP = "localhost";

    public static MultiplayerManager Instance { get; protected set; }

    public long LocalId { get; set; } = 1;

    public override void _Ready()
    {
        Instance = this;
    }

    protected async Task WaitForScene()
    {
        int frames = 0;
        Action onFrame = () =>
        {
            frames++;
        };

        GetTree().ProcessFrame += onFrame;

        while (frames < 2)
        {
            await Task.Delay(25);
        }

        GetTree().ProcessFrame -= onFrame;
    }

    public async void HostGame()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");
        await WaitForScene();

        var server = new ENetMultiplayerPeer();
        server.CreateServer(SERVER_PORT);

        Multiplayer.MultiplayerPeer = server;
        Multiplayer.PeerConnected += OnPlayerConnect;
        Multiplayer.PeerDisconnected += OnPlayerDisconnect;

        var world = (World)GetTree().CurrentScene.GetNode("World");

        var newPlayer = ResourceLoader.Load<PackedScene>($"res://script/entity/Player.tscn").Instantiate<Player>();
        newPlayer.Id = 1;
        newPlayer.Name = "1";
        newPlayer.Position = world.LeftSpawn.Position;

        world.EntitiesContainer.AddChild(newPlayer);
    }

    public async void JoinGame()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");
        await WaitForScene();

        var client = new ENetMultiplayerPeer();
        client.CreateClient(SERVER_IP, SERVER_PORT);

        Multiplayer.MultiplayerPeer = client;
        Multiplayer.PeerDisconnected += OnServerDisconnect;
    }

    public async void QuitGame(bool causedByError = false)
    {
        if (Multiplayer.IsServer())
        {
            Multiplayer.MultiplayerPeer.Dispose();
            Multiplayer.MultiplayerPeer = null;
        }
        else
        {
            Multiplayer.MultiplayerPeer.DisconnectPeer(1);
            Multiplayer.MultiplayerPeer.Dispose();
            Multiplayer.MultiplayerPeer = null;
        }

        if (!causedByError)
        {
            GetTree().ChangeSceneToFile("res://script/MainMenu.tscn");
            await WaitForScene();
        }
        else
        {
            var packed = ResourceLoader.Load<PackedScene>($"res://script/MessageScene.tscn");
            GetTree().ChangeSceneToPacked(packed);
            await WaitForScene();

            var msg = (MessageScene)GetTree().CurrentScene;
            msg.Message = "Connection lost";
        }

    }

    protected void OnPlayerConnect(long id)
    {
        GD.Print($"Player {id} connected");

        var world = (World)GetTree().CurrentScene.GetNode("World");

        var newPlayer = ResourceLoader.Load<PackedScene>($"res://script/entity/Player.tscn").Instantiate<Player>();
        newPlayer.Id = id;
        newPlayer.Name = $"{id}";
        newPlayer.Position = world.RightSpawn.Position;

        world.EntitiesContainer.AddChild(newPlayer);
        world.GameActive = true;
    }

    protected void OnPlayerDisconnect(long id)
    {
        GD.Print($"Player {id} disconnected");

        var world = (World)GetTree().CurrentScene.GetNode("World");
        var player = world.EntitiesContainer
            .GetChildren()
            .Where(c => c.GetType().IsAssignableTo(typeof(Player)))
            .Select(c => (Player)c)
            .Where(p => p.Id == id)
            .First();

        player.QueueFree();
    }

    protected void OnServerDisconnect(long id)
    {
        GD.Print($"Server with id={id} disconnected");

        GetTree().Paused = false;
        QuitGame(true);
    }
}
