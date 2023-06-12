using System;
using System.IO;
using Xamarin.Forms;
using Android.Views;
using Android.App;
using Plugin.CurrentActivity;

namespace CSharp_Shell
{
	public partial class MainPage : ContentPage
	{
		public static Profile profile = new Profile();
		
		public MainPage()
		{
			InitializeComponent();
			CrossCurrentActivity.Current.Activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
			Navigation.PushModalAsync(new Game(), false);
		}
	}
}