using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Media;

namespace CSharp_Shell
{
	public partial class App : Application
	{
		public static MediaPlayer musicPlayer = new MediaPlayer();
		public static MediaPlayer fxPlayer = new MediaPlayer();
		public App()
		{
			InitializeComponent();
			musicPlayer.SetVolume (0.8f, 0.8f);
			this.MainPage = new MainPage();

		}
		protected override void OnStart()
		{
			
		}

		protected override void OnSleep()
		{
			try
			{
				musicPlayer.Pause();
				fxPlayer.Pause();
			}
			catch { }
		}

		protected override void OnResume()
		{
			try
			{
				musicPlayer.Start();
				
				if (fxPlayer.Looping)
				{
					fxPlayer.Start();
				}
			}
			catch { }
		}
	}
}