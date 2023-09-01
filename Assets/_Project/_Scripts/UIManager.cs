using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PorfirioPartida.Workshop
{
    public class UIManager : MonoBehaviour
    {
        public InputField playerNameField;
        public Button startButton;

        #region Workshop UI Manager
        private void Start()
        {
            startButton.onClick.AddListener(StartButtonPressed);
        }

        private void StartButtonPressed()
        {
            var cleanName = CleanName(playerNameField.text).Trim();
            PlayerPrefs.SetString(Constants.PlayerName, cleanName);
        }
        #endregion
    
        //TODO: Add Photon set name and connect here.

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