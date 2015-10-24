// -----------------------------------------------------------
//  
//  This file was generated, please do not modify.
//  
// -----------------------------------------------------------
namespace EmptyKeys.UserInterface.Generated {
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.ObjectModel;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Data;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Controls.Primitives;
    using EmptyKeys.UserInterface.Input;
    using EmptyKeys.UserInterface.Media;
    using EmptyKeys.UserInterface.Media.Animation;
    using EmptyKeys.UserInterface.Media.Imaging;
    using EmptyKeys.UserInterface.Shapes;
    using EmptyKeys.UserInterface.Renderers;
    using EmptyKeys.UserInterface.Themes;
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "1.10.0.0")]
    public partial class BasicUI : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private TextBox txtBoxAccount;
        
        private TextBlock e_3;
        
        private PasswordBox pwdBox;
        
        private Button btnConnection;
        
        public BasicUI() : 
                base() {
            this.Initialize();
        }
        
        public BasicUI(int width, int height) : 
                base(width, height) {
            this.Initialize();
        }
        
        private void Initialize() {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.FontFamily = new FontFamily("Segoe UI");
            this.FontSize = 13.33333F;
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.Foreground = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            this.e_2.Text = "Nom de compte";
            this.e_2.TextWrapping = TextWrapping.Wrap;
            this.e_2.FontFamily = new FontFamily("Segoe UI");
            this.e_2.FontSize = 20F;
            this.e_2.FontStyle = FontStyle.Bold;
            // txtBoxAccount element
            this.txtBoxAccount = new TextBox();
            this.e_1.Children.Add(this.txtBoxAccount);
            this.txtBoxAccount.Name = "txtBoxAccount";
            this.txtBoxAccount.Width = 120F;
            this.txtBoxAccount.HorizontalAlignment = HorizontalAlignment.Center;
            this.txtBoxAccount.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.txtBoxAccount.Text = "";
            // e_3 element
            this.e_3 = new TextBlock();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_3.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_3.VerticalAlignment = VerticalAlignment.Center;
            this.e_3.Foreground = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            this.e_3.Text = "Mot de passe";
            this.e_3.TextWrapping = TextWrapping.Wrap;
            this.e_3.FontFamily = new FontFamily("Segoe UI");
            this.e_3.FontSize = 20F;
            this.e_3.FontStyle = FontStyle.Bold;
            // pwdBox element
            this.pwdBox = new PasswordBox();
            this.e_1.Children.Add(this.pwdBox);
            this.pwdBox.Name = "pwdBox";
            this.pwdBox.Width = 120F;
            this.pwdBox.HorizontalAlignment = HorizontalAlignment.Center;
            this.pwdBox.VerticalAlignment = VerticalAlignment.Top;
            this.pwdBox.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            // btnConnection element
            this.btnConnection = new Button();
            this.e_1.Children.Add(this.btnConnection);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Width = 120F;
            this.btnConnection.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.btnConnection.HorizontalAlignment = HorizontalAlignment.Center;
            this.btnConnection.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.btnConnection.Content = "Connexion";
            FontManager.Instance.AddFont("Segoe UI", 13.33333F, FontStyle.Regular, "Segoe_UI_10_Regular");
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Bold, "Segoe_UI_15_Bold");
            FontManager.Instance.AddFont("Times New Roman", 13.33333F, FontStyle.Regular, "Times_New_Roman_10_Regular");
        }
    }
}
