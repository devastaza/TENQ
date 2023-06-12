using System;
using Xamarin.Forms;
namespace CSharp_Shell
{
	public partial class Game
	{
		internal Image backgroundImage;
		internal Image backgroundGif;
		internal ScrollView pageScroll;
		internal StackLayout majorLayout;
		internal StackLayout subtitleField;
		internal Label subtitle;
		internal BoxView subtitleLine;
		internal StackLayout addonLayout;
		internal ScrollView imgField;
		internal Frame imgFrame;
		internal Image image;
		internal Frame textField;
		internal Label text;
		internal StackLayout buttonsField;
		internal StackLayout bgDarkness;
		internal Frame menu;
		internal BoxView closeButtonBorder;
		internal ImageButton closeButton;
		internal Frame textHistoryField;
		internal ScrollView textHistoryScroll;
		internal StackLayout textHistory;
		public void InitializeComponent()
		{
			Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(Game));
				backgroundImage = this.FindByName<Image>("backgroundImage");
				backgroundGif = this.FindByName<Image>("backgroundGif");
				pageScroll = this.FindByName<ScrollView>("pageScroll");
				majorLayout = this.FindByName<StackLayout>("majorLayout");
				subtitleField = this.FindByName<StackLayout>("subtitleField");
				subtitle = this.FindByName<Label>("subtitle");
				subtitleLine = this.FindByName<BoxView>("subtitleLine");
				addonLayout = this.FindByName<StackLayout>("addonLayout");
				imgField = this.FindByName<ScrollView>("imgField");
				imgFrame = this.FindByName<Frame>("imgFrame");
				image = this.FindByName<Image>("image");
				textField = this.FindByName<Frame>("textField");
				text = this.FindByName<Label>("text");
				buttonsField = this.FindByName<StackLayout>("buttonsField");
				bgDarkness = this.FindByName<StackLayout>("bgDarkness");
				menu = this.FindByName<Frame>("menu");
				closeButtonBorder = this.FindByName<BoxView>("closeButtonBorder");
				closeButton = this.FindByName<ImageButton>("closeButton");
				textHistoryField = this.FindByName<Frame>("textHistoryField");
				textHistoryScroll = this.FindByName<ScrollView>("textHistoryScroll");
				textHistory = this.FindByName<StackLayout>("textHistory");
		}
	}
}
