using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace PorfirioPartida.Workshop
{
    public class UIManager : MonoBehaviourPunCallbacks
    {
        public TMPro.TMP_InputField playerNameField;
        public Button connectButton;
        public Button startButton;
        public GameObject UIPanel;

        public Button nextColor;
        public Button prevColor;
        public int selectedColor = 0;
        public Image selectedColorImage;
        
        #region Workshop UI Manager
        private void Start()
        {
            connectButton.onClick.AddListener(ConnectButtonPressed);
            startButton.onClick.AddListener(StartButtonPressed);
            nextColor.onClick.AddListener(NextColorPressed);
            prevColor.onClick.AddListener(PrevColorPressed);
            
            var lastName = PlayerPrefs.GetString(Constants.PlayerName, "");
            playerNameField.text = lastName;
            
            var lastColor = PlayerPrefs.GetInt(Constants.SelectedColor, 0);
            selectedColor = lastColor;
            
            UpdateColor();
        }

        private void PrevColorPressed()
        {
            selectedColor--;
            
            if (selectedColor < 0)
            {
                selectedColor = SceneManager.Instance.materials.Length - 1;
            }
            
            UpdateColor();
        }

        private void NextColorPressed()
        {
            selectedColor++;
            
            if (selectedColor >= SceneManager.Instance.materials.Length)
            {
                selectedColor = 0;
            }
            
            UpdateColor();
        }
        private void UpdateColor()
        {
            
            if (selectedColor < 0 || selectedColor >= SceneManager.Instance.materials.Length)
            {
                selectedColor = 0;
            }
            
            selectedColorImage.color = SceneManager.Instance.materials[selectedColor].color;
        }

        private void ConnectButtonPressed()
        {
            var cleanName = CleanName(playerNameField.text).Trim();
            PlayerPrefs.SetString(Constants.PlayerName, cleanName);
            PlayerPrefs.SetInt(Constants.SelectedColor, selectedColor);
            Debug.Log($"Test Starting with name: {cleanName}");
            PhotonNetwork.ConnectUsingSettings();
        }

        private void StartButtonPressed()
        {
            UIPanel.SetActive(false);
            SceneManager.Instance.StartGame();
        }

        #endregion

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connected to master server.");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.Log("No Room was found, trying to create one.");
            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = maxPlayers;

            PhotonNetwork.JoinOrCreateRoom(RandomString(5), roomOptions, TypedLobby.Default);
        }
        private static string RandomString(int length)
        {
            var random = new System.Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            FixConnectedUI();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            FixConnectedUI();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            FixConnectedUI();
        }
        public TMPro.TMP_Text connectedPlayers;
        private const int maxPlayers = 20;
        private const string ConnectedPlayersCounter = "Players Online -- {0}/{1}";

        private void FixConnectedUI()
        {
            connectedPlayers.text = string.Format(ConnectedPlayersCounter, PhotonNetwork.PlayerList.Length, maxPlayers);
        }

        #region Clean Name
        
        private static string CleanName(string name)
        {
            var validFileName = MakeValidFileName(name).Trim();
            var truncated = Truncate(validFileName, 11);
            var notEmpty = randomIfEmpty(truncated.Trim());
            var cleanName = notEmpty.Trim();
                
            return cleanName;
        }

        private static string randomIfEmpty(string value)
        {
            var random = new System.Random();

            if (string.IsNullOrEmpty(value))
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, 11)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return value;
        }

        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        private static string MakeValidFileName( string name )
        {
            var invalidChars = System.Text.RegularExpressions.Regex.Escape( new string( System.IO.Path.GetInvalidFileNameChars() ) );
            var invalidRegStr = string.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );

            return System.Text.RegularExpressions.Regex.Replace( name, invalidRegStr, "_" );
        }

        #endregion
    }
}