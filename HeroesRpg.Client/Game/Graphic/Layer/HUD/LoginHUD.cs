using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Themes;
using HeroesRpg.Client.Game.Graphic.Scene;
using HeroesRpg.Client.Game.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Layer.HUD
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginHUD : WrappedHUD<LoginHUD, LoginUI>
    {
        public TextBox TxtAccount => UI.TxtBoxAccount;
        public PasswordBox TxtPassword => UI.PwdBoxPassword;
        public Button BtnConnection => UI.BtnConnection;

        /// <summary>
        /// 
        /// </summary>
        public LoginHUD()
        {
            BtnConnection.Click += BtnConnection_Click;

            var binding = new KeyBinding(new RelayCommand((obj) =>
            {
                BtnConnection_Click(BtnConnection, null);
            }), new KeyGesture(KeyCode.Enter, ModifierKeys.None));
            UI.InputBindings.Add(binding);
            TxtAccount.InputBindings.Add(binding);
            TxtPassword.InputBindings.Add(binding);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
            // Reset ui because of the singleton
            TxtAccount.Text = string.Empty;
            TxtPassword.Password = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnection_Click(object sender, EmptyKeys.UserInterface.RoutedEventArgs e)
        {
            SoundPlayer.Instance.PlayButtonClick();

            //TODO: impl server connection logic
            MessageBox.Show("Impossible de se connecter au serveur", "Connexion", MessageBoxButton.OK, new RelayCommand((obj) =>
            {
                Director.ReplaceScene(GameScene.Instance);
            }), false);
            m_log.Debug($"HUD::connection user={UI.TxtBoxAccount.Text} password={UI.PwdBoxPassword.Password}");            
        }
    }
}
