using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SimpleGame.Helpers;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using SimpleGame.ViewModel;

namespace SimpleGame.ViewModels
{
    public class BoardViewModel:ViewModelBase
    {
        private  SimpleTcpClient _client;
        private readonly string _host;
        private readonly string _port;

        public ObservablePairCollection<int,ImageSource> Fields { get; set; }
        public ObservablePairCollection<int, ImageSource> EnemyFields { get; set; }

        public RelayCommand<object> MoveCommand { get; }
        public RelayCommand NewGame { get; }
        public RelayCommand LoadCommand { get; }
      
        private Player _enemyPlayer;
        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (Equals(_currentPlayer, value))
                {
                    return;
                }

                _currentPlayer = value;
                RaisePropertyChanged();
            }
        }

        private bool _isPlayerTurn;
        public bool IsPlayerTurn
        {
            get => _isPlayerTurn;
            set
            {
                if (Equals(_isPlayerTurn, value))
                    return;
                _isPlayerTurn = value;

                RaisePropertyChanged();
            }
        }

        private bool _isAnimatedWaitingImage;
        public bool IsAnimatedWaitingImage
        {
            get => _isAnimatedWaitingImage;
            set
            {
                if (Equals(_isAnimatedWaitingImage, value))
                    return;
                _isAnimatedWaitingImage = value;
                RaisePropertyChanged();
            }
        }

        private string _waitingBoxMessage;
        public string WaitingBoxMessage
        {
            get => _waitingBoxMessage;
            set
            {
                if (Equals(_waitingBoxMessage, value))
                    return;
                _waitingBoxMessage = value;
                RaisePropertyChanged();
            }
        }

        private ImageSource _waitingBoxImage;
        public ImageSource WaitingBoxImage
        {
            get => _waitingBoxImage;
            set
            {
                if (Equals(_waitingBoxImage, value))
                {
                    return;
                }
                _waitingBoxImage = value;
                RaisePropertyChanged();
            }
        }     
    
        public BoardViewModel(string host,string port)
        {
            _host = host;
            _port = port;

            Fields =new ObservablePairCollection<int, ImageSource>();
            EnemyFields=new ObservablePairCollection<int, ImageSource>();
            for (int i = 1; i <= 64; i++)
            {
                Fields.Add(i,new BitmapImage());
                EnemyFields.Add(i,new BitmapImage());
            }                           
            MoveCommand = new RelayCommand<object>(PlayerTurn);
            NewGame=new RelayCommand(InformServerToStartNewGame);
            LoadCommand =new RelayCommand(Loaded);   
          
            Application.Current.Exit += DisconnectClient;
        }

        private void DisconnectClient(object sender, ExitEventArgs e)
        {
            try
            {
                _client.Write("Client_disconnected");
                _client.Disconnect();
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }         
        }

        private void Loaded()
        {          
            try
            {
                _client = new SimpleTcpClient { StringEncoder = Encoding.UTF8 };
                _client.DataReceived += Client_DataReceived;
                _client.Connect(_host, Convert.ToInt32(_port));
                _client.Write("Hello");
            }
            catch (Exception)
            {           
                ViewModelLocator vm = new ViewModelLocator();
                vm.Main.CurrentViewModel = new ModalBoxViewModel("Cannot connect to given host and port!!!");
            }
        }

        private void Client_DataReceived(object sender, Message e)
        {
            var msg = e.MessageString.Split(';');
            switch (msg.Length)
            {
                case 1 when msg[0] == "NewGame":
                    Application.Current.Dispatcher.BeginInvoke(new Action(StartNewGame));
                    break;
                case 1 when msg[0] == "Ready":
                    Application.Current.Dispatcher.BeginInvoke(new Action(ReadyToPlay));
                    break;
                case 1 when msg[0] == "Client_disconnected":
                    Application.Current.Dispatcher.BeginInvoke(new Action(ClientDisconnected));
                    break;
                case 1 when msg[0] == "Server_disconnected":
                    Application.Current.Dispatcher.BeginInvoke(new Action(ServerDisconnected));
                    break;
                case 4 when msg[0] == "Assign_player":
                    Application.Current.Dispatcher.BeginInvoke(msg[1] == "Player1"
                        ? new Action(() => AssignPlayer(Players.Player1,msg[2],msg[3]))
                        : () => AssignPlayer(Players.Player2,msg[2],msg[3]));
                    break;
                default:
                    if (CurrentPlayer != null && msg[0] == CurrentPlayer.PlayerType.ToString())
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => EnemyTurn(Int32.Parse(msg[1]))));
                    }

                    break;
            }
        }

        private void ServerDisconnected()
        {
            ViewModelLocator vm = new ViewModelLocator();
            vm.Main.CurrentViewModel = new ModalBoxViewModel("Server not running");
        }

        private void ClientDisconnected()
        {     
            ViewModelLocator vm = new ViewModelLocator();
            vm.Main.CurrentViewModel = new ModalBoxViewModel("Client disconnected");
        }

        private void ReadyToPlay()
        {
            IsAnimatedWaitingImage = true;            
            if (CurrentPlayer.PlayerType == Players.Player1)
            {
                IsPlayerTurn = true;
                WaitingBoxMessage = "Your turn";
                WaitingBoxImage = CurrentPlayer.Flag;
            }
            else
            {
                IsPlayerTurn = false;
                WaitingBoxMessage = "Second player thinking...";
                WaitingBoxImage = _enemyPlayer.Flag;
            }
            SetUpGrid(CurrentPlayer,Fields);
        }

        public static ObservablePairCollection<int,ImageSource> SetUpGrid(Player player,ObservablePairCollection<int,ImageSource> Fields)
        {
            foreach (var ship in player.Ships)
            {
                foreach (var shipField in ship.Fields)
                {
                    foreach (var field in Fields)
                    {
                        if (field.Key == shipField)
                            field.Value = ship.GetShipImage();
                    }
                }
            }

            return Fields;
        }

        private void AssignPlayer(Players player,string currentPlayerShipsJson,string enemyPlayerShipsJson)
        {
            JsonConverter[] converters = { new ShipConverter() };
            var currentPlayerShips = JsonConvert.DeserializeObject<List<Ship>>(currentPlayerShipsJson, new JsonSerializerSettings() { Converters = converters });
            var enemyPlayerShips = JsonConvert.DeserializeObject<List<Ship>>(enemyPlayerShipsJson, new JsonSerializerSettings() { Converters = converters });
            if (player == Players.Player1)
            {
                CurrentPlayer = new FirstPlayer(Players.Player1);                
                _enemyPlayer = new SecondPlayer(Players.Player2);
                WaitingBoxMessage = "Waiting for second player to join...";              
            }
            else
            {
                CurrentPlayer = new SecondPlayer(Players.Player2);
                _enemyPlayer = new FirstPlayer(Players.Player1);
                IsAnimatedWaitingImage = true;
            }

            CurrentPlayer.AddShips(currentPlayerShips);
            _enemyPlayer.AddShips(enemyPlayerShips);
            _enemyPlayer.Lost += PlayerWin;
            _currentPlayer.Lost += EnemyWin;

            if(player==Players.Player2)
                _client.Write("Ready");
        }       

        private void PlayerWin(object sender, EventArgs e)
        {
            WaitingBoxMessage = CurrentPlayer.PlayerType+" is the winner";
            WaitingBoxImage = CurrentPlayer.Flag;           
            IsPlayerTurn = false;
            IsAnimatedWaitingImage = false;          
        }

        private void EnemyWin(object sender, EventArgs e)
        {
            WaitingBoxMessage = _enemyPlayer.PlayerType + " is the winner";
            WaitingBoxImage = _enemyPlayer.Flag;          
            IsPlayerTurn = false;
            IsAnimatedWaitingImage = false;      
        }

        private void InformServerToStartNewGame()
        {        
            _client.Write("NewGame");
        }

        private void StartNewGame()
        {           
           // for (var i = 0; i < 9; i++)
           //     Fields[i].Value = new BitmapImage();
           // Player.IsGameOver = false;
           // Player.FieldsTakenByBothPlayers.Clear();
           // CurrentPlayer.EmptyFields();
           // _enemyPlayer.EmptyFields();
           //ReadyToPlay();
        }

        private void EnemyTurn(int index)
        {
            Fields.FirstOrDefault(f => f.Key == index).Value =
                _enemyPlayer.Attack(CurrentPlayer, index) ? Ship.RedShip : Ship.BlueShip;
            _enemyPlayer.Attack(CurrentPlayer, index);           
            SwitchWaitingBoxPlayer();
        }

        private void PlayerTurn(object p)
        {
            var index = Convert.ToInt32(p); 
            if (Player.IsGameOver||CurrentPlayer.AlreadyAttackedFields.Contains(index)) return;
            EnemyFields.FirstOrDefault(f => f.Key == index).Value =
                CurrentPlayer.Attack(_enemyPlayer,index)?Ship.RedShip:Ship.BlueShip;
            _client.Write(_enemyPlayer.PlayerType + ";" + index);
            SwitchWaitingBoxPlayer();
        }

        private void SwitchWaitingBoxPlayer()
        {
            if (Player.IsGameOver) return;
            if (IsPlayerTurn)
            {
                IsPlayerTurn = false;
                WaitingBoxMessage = "Second player thinking...";
                WaitingBoxImage = _enemyPlayer.Flag;
            }
            else
            {
                IsPlayerTurn = true;
                WaitingBoxMessage = "Your turn";
                WaitingBoxImage = CurrentPlayer.Flag;
            }
        }
    }
}
