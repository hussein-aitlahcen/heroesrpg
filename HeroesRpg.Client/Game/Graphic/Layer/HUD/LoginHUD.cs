using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
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
    public sealed class LoginHUD : WrappedHUD<LoginHUD, BasicUI>
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
            m_log.Debug($"HUD::connection user={UI.TxtBoxAccount.Text} password={UI.PwdBoxPassword.Password}");            
        }
    }
}
