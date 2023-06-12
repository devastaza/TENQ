using System;
using Xamarin.Forms;
namespace CSharp_Shell
{
	public partial class MainPage
	{
		internal Button btn;
		public void InitializeComponent()
		{
			Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(MainPage));
				btn = this.FindByName<Button>("btn");
		}
	}
}
