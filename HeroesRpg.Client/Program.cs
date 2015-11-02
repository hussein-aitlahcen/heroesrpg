using System;

using CocosSharp;

namespace HeroesRpg.Client
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThread]
        static void Main(string[] args)
        {                
            CCApplication application = new CCApplication(false, new CCSize(1024f, 768f));
            application.ApplicationDelegate = new AppDelegate();
            try
            {
                application.StartGame();
            }
            catch(Exception e)
            {
            }
        }
    }
}

