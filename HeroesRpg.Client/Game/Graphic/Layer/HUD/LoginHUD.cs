using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Themes;
using HeroesRpg.Client.Game.Graphic.Scene;
using HeroesRpg.Client.Game.Sound;
using HeroesRpg.Client.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroesRpg.Protocol;
using HeroesRpg.Protocol.Impl.Connection.Client;
using HeroesRpg.Protocol.Impl.Connection.Server;

namespace HeroesRpg.Client.Game.Graphic.Layer.HUD
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginHUD : WrappedHUD<LoginHUD, LoginUI>, IGameFrame
    {
        public TextBox TxtAccount => UI.TxtBoxAccount;
        public PasswordBox TxtPassword => UI.PwdBoxPassword;
        public Button BtnConnection => UI.BtnConnection;

        private string m_connectionError;
        private float m_connectionTimeout;

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

            // register back to the client
            GameClient.Instance.AddFrame(this);

            ResetConnection();     
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetTimeout()
        {
            m_connectionTimeout = -1f;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetConnection()
        {
            GameClient.Instance.Disconnect();
            m_connectionError = "Impossible de se connecter au serveur";
            ResetTimeout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnection_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer.Instance.PlayButtonClick();

            GameClient.Instance.Connect(GlobalConfig.GAME_HOST, GlobalConfig.GAME_PORT);

            SetInputEnabled(false);

            m_connectionTimeout = 5f;

            m_log.Debug($"HUD::connection user={UI.TxtBoxAccount.Text} password={UI.PwdBoxPassword.Password}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public bool ProcessMessage(NetMessage message)
        {
            var welcomeConnect = message as WelcomeConnectMessage;
            var identificationResult = message as IdentificationResultMessage;

            if(welcomeConnect != null)
            {
                GameClient.Instance.Send(new IdentificationMessage()
                {
                    Username = TxtAccount.Text,
                    Password = TxtPassword.Text
                });
                return true;
            }
            else if(identificationResult != null)
            {
                switch(identificationResult.Code)
                {
                    case IdentificationResultEnum.SUCCESS:
                        Director.ReplaceScene(GameScene.Instance);
                        break;

                    case IdentificationResultEnum.WRONG_CREDENTIALS:
                        m_connectionError = "Nom de compte ou mot de passe incorrect";
                        m_connectionTimeout = 0f;
                        break;
                }
                return true;
            }
            return false;
        }

        private void SetInputEnabled(bool enabled = true)
        {
            TxtAccount.IsEnabled = enabled;
            TxtPassword.IsEnabled = enabled;
            BtnConnection.IsEnabled = enabled;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (m_connectionTimeout != -1f)
            {
                if (m_connectionTimeout <= 0f)
                {
                    SetInputEnabled();
                    ResetTimeout();
                    MessageBox.Show(m_connectionError, "Connexion impossible", MessageBoxButton.OK, new RelayCommand((obj) =>
                    {
                        ResetConnection();
                    }), false);
                }
                else
                {
                    m_connectionTimeout -= dt;
                }
            }
        }
    }
}
