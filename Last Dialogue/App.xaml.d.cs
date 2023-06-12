using System;
using Xamarin.Forms;
namespace CSharp_Shell
{
	public partial class App
	{
		public void InitializeComponent()
		{
			Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(App));
		}
	}
}
