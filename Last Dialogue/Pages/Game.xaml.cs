using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Media;

namespace CSharp_Shell
{
	public partial class Game : ContentPage
	{
		public List<string> texts;
		public List<string> backgroundImages;
		public string link;
		public static bool pageFromSaves = false;
		
		public DataSave playerData = new DataSave();

		public int textİndex = 0;

		public Game()
		{
			InitializeComponent();
			PrepareNewPage(Data.dictionary.Values.First());
		}

		public async void PrepareNewPage(PageForm newPage)
		{
			await PageUpdater.UpdateSettings(newPage);
			await PageUpdater.UpdateBackgroundImage(this, newPage);
			await PageUpdater.UpdateBackgroundGif(this, newPage);
			await PageUpdater.UpdateLink(this, newPage);
			await PageUpdater.UpdateSubtitle(this, newPage);
			await PageUpdater.UpdateImage(this, newPage);

			buttonsField.IsVisible = false;
			buttonsField.HorizontalOptions = newPage.settings.buttonsXYoptions.First();
			buttonsField.VerticalOptions = newPage.settings.buttonsXYoptions.Last();

			await PageUpdater.UpdateText(this, newPage);
				
			ClearButtonsEventHandler();
			buttonsField.Children.Clear(); //очищаем стек кнопок

			await PageUpdater.UpdateButtons(this, newPage);
			await PageUpdater.UpdateMusic(this, newPage);
			await PageUpdater.UpdateFX(this, newPage);
			await PageUpdater.RunExtendedOptions(this, newPage);

			if (texts != null) ChangeText();

			if ((bool)MainPage.profile.objects["scrollBack"] == true)
			{
				SomeMethods.SaveGame();
			}
			
			playerData.Save();

			pageFromSaves = false;
		}

		async void RefreshPage(Button button)
		{
			Animations.paralaxing = false;
			try
			{
				text.Text = "";
				MainPage.profile.objects["presentPage"] = button.BindingContext.ToString();
				PageForm newPageExample = Data.dictionary[button.BindingContext.ToString()];
				PrepareNewPage(newPageExample);
			}
			catch (Exception e)
			{
				DegressPage(e);
			}
		}

		public async void RefreshPage(string link)
		{
			Animations.paralaxing = false;
			try
			{
				text.Text = "";
				MainPage.profile.objects["presentPage"] = link;
				PageForm newPageExample = Data.dictionary[link];
				PrepareNewPage(newPageExample);
			}
			catch (Exception e)
			{
				DegressPage(e);
			}
		}

		void DegressPage(Exception e)
		{
			DisplayAlert("Oops...", $"Page not found.\n\n{e}", "ok");
			foreach (Button button in buttonsField.Children)
			{
				button.Clicked += ButtonClickEventHandler;
			}
			Animations.ShowButtons(this);
		}

		public async void ButtonClickEventHandler(Object sender, EventArgs e)
		{
			if (((Button)sender).BindingContext == "ExitTheGame")
			{
				ExitTheGame(sender, e);
			}
			else if (((Button)sender).BindingContext as string == "mainPage")
			{
				Animations.textAnimationIsEnded = true;
				Animations.FastRefreshFrameAnimation(textField);
				RefreshPage("mainPage");
				Animations.MenuShowHide(this);
			}
			else
			{
				await Animations.PlaySettedButtonAnimation(this, sender);
				
				SomeMethods.AddLabelInHistory(this, (Button)sender);
				RefreshPage((Button)sender);
			}
		}

		void ClearButtonsEventHandler()
		{
			foreach (Button button in buttonsField.Children) //снимаем обработчики со всех кнопок во избежания дублирования сценариев
			{
				button.Clicked -= ButtonClickEventHandler; //помянем мои три оставшиеся нервные клетки
			}
		}

		void OpenCloseTextHistory(Object sender, EventArgs e)
		{
			Animations.TextHistoryViewHide(this);
		}

		async void ChangeText()
		{
			if (!Animations.textAnimationIsEnded)  //skip text typing anim
			{
				text.Text = texts[textİndex];
				Animations.FastRefreshFrameAnimation(textField);
				Animations.textAnimationIsEnded = true;
			}
			else if (textİndex == -1 || textİndex == texts.Count) //there are no texts or we are at the last part
			{
				if (link != null) RefreshPage(link);
				else if (buttonsField.Children.Count > 0 && buttonsField.IsVisible)
				{
					await Animations.ButtonFadingAnimation(this); //focuse on buttons anim
				}
			}
			else if (textİndex <= texts.Count) //just set the next part of texts
			{
				await Animations.WriteNewText(this, texts[textİndex]);
				SomeMethods.AddLabelInHistory(this); //insert text in history

				if (textİndex == texts.Count -1 && buttonsField.Children.Count > 0)
				{
					buttonsField.IsVisible = true;
					Animations.ShowButtons(this);
				}
				
				textİndex++;
			}
		}

		async void ChangeTextOld()
		{
			try
			{
				if (backgroundImages.Count > 0)
				{
					if (backgroundImages.First() == "")
					{
						backgroundImages.RemoveAt(0);
					}
					else
					{
						SomeMethods.ChangeBackgroundImage(this);
					}
				}
			}
			catch { } //почти что детектинг быстрого скипа, экзепшн порой выпадает
		}

		void TextTapped(Object sender, EventArgs e)
		{
			//	if (textField.IsVisible)
			{
				if (imgField.Scale != 1)
				{
					Animations.NormalizeImageScale(this);
				}
				ChangeText();
			}
		}

		async void SaveGame(Object sender, EventArgs e)
		{
			bool rewriteGranted = true;
			Animations.MenuShowHide(this);
			string newFileName = await DisplayPromptAsync(null, "Дайте название сохранению", placeholder: "Введите что-нибудь...", maxLength: 20);

			newFileName = newFileName == "" ?  //тернарный оператор
			"Random #" + SomeMethods.rndm.Next(1000) :
			newFileName;

			if (File.Exists(SomeMethods.savesDirectory + "/" + newFileName + ".json"))
			{
				rewriteGranted = await DisplayAlert(null, "Сохранение с таким названием уже существует. Перезаписать его?", "Да", "Нет");
			}

			if (rewriteGranted)
			{
				SomeMethods.SaveGame(newFileName);
				DisplayAlert(null, $"Сохранение \"{newFileName}\" записано!", "ок");
			}
		}

		void MuteUnmuteMusic(Object sender, EventArgs e)
		{
			SomeMethods.MuteUnmuteMusic(sender);
		}

		void OpenCloseMenu(Object sender, EventArgs e)
		{
			Animations.MenuShowHide(this);
		}

		void CloseMenuWithButton(Object sender, EventArgs e)
		{
			Animations.CloseMenuWithButton(this);
		}

		void ImageFocusing(Object sender, EventArgs e)
		{
			Animations.ImageFocusing(this);
		}

		protected override bool OnBackButtonPressed()
		{
			if (textHistoryField.IsVisible)
			{
				Animations.TextHistoryViewHide(this);
			}
			else if (menu.IsVisible)
			{
				Animations.MenuShowHide(this);
			}
			else if (!menu.IsVisible)
			{
				Animations.MenuShowHide(this);
			}
			return true;
		}

		static void ExitTheGame(Object sender, EventArgs e)
		{
			Environment.Exit(1);
		}
	}
}