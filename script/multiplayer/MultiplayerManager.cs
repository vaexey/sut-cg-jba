using Godot;
using System;
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

        var newPlayer = ResourceLoader.Load<PackedScene>($"res://script/entity/RemotePlayer.tscn").Instantiate<RemotePlayer>();
        newPlayer.Id = 1;
        newPlayer.Name = "1";

        world.EntitiesContainer.AddChild(newPlayer);
    }

    public async void JoinGame()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");
        await WaitForScene();

        var client = new ENetMultiplayerPeer();
        client.CreateClient(SERVER_IP, SERVER_PORT);

        Multiplayer.MultiplayerPeer = client;
    }

    protected void OnPlayerConnect(long id)
    {
        GD.Print($"Player {id} connected");

        var world = (World)GetTree().CurrentScene.GetNode("World");

        var newPlayer = ResourceLoader.Load<PackedScene>($"res://script/entity/RemotePlayer.tscn").Instantiate<RemotePlayer>();
        newPlayer.Id = id;
        newPlayer.Name = $"{id}";

        world.EntitiesContainer.AddChild(newPlayer);
    }

    protected void OnPlayerDisconnect(long id)
    {
        GD.Print($"Player {id} disconnected");
    }
}
