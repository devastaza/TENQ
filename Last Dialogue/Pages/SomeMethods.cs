using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Android.Media;
using Newtonsoft.Json;

namespace CSharp_Shell
{

	public class SomeMethods
	{
		public static Random rndm = new Random();
		public delegate Button ButtonCode();
		public static string previousSound = "";
		public static string savesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

		public static Task InsertButtons(Game page, List<Button> buttons)
		{
			foreach (Button button in buttons)
			{
				button.Clicked += page.ButtonClickEventHandler; //настраиваем событие
				page.buttonsField.Children.Add(button); //добавляем в стек
			}

			return Task.CompletedTask;
		}

		public static Button CreateConditionalButton(ButtonCode buttonCode)
		{
			return buttonCode();
		}

		public static void MuteUnmuteMusic(Object sender)
		{
			if (((Button)sender).BindingContext.ToString() == "On")
			{
				App.musicPlayer.SetVolume(0, 0);
				((Button)sender).BindingContext = "Off";
				((Button)sender).Text = "Включить музыку";
			}
			else
			{
				App.musicPlayer.SetVolume(0.8f, 0.8f);
				((Button)sender).BindingContext = "On";
				((Button)sender).Text = "Отключить музыку";
			}
		}

		public static async void PreparePlayer(Game page, MediaPlayer player, string sound, bool isLooping = true)
		{
			if (sound == "stop")
			{
				try
				{
					if (player.IsPlaying)
					{
						player.Reset();
					}
				}
				catch { }
			}
			else if (sound != "" && previousSound != sound)
			{
				previousSound = sound;

				try
				{
					player.Reset();
				}
				catch { }

				try
				{
					await PrepareNewTrack(sound);
					player.SetDataSource(Android.OS.Environment.ExternalStorageDirectory + "/Last Dialog/cache/" + sound.Substring(sound.IndexOf("/")));
					player.Prepare();
					player.Start();
					File.Delete(Android.OS.Environment.ExternalStorageDirectory + "/Last Dialog/cache/" + sound.Substring(sound.IndexOf("/")));
					player.Looping = isLooping;
				}
				catch (Exception e)
				{
					page.DisplayAlert(null, e.ToString(), "ok");
				}
			}
		}

		static async Task PrepareNewTrack(string soundName)
		{
			byte[] track = Ui.GetAssetAsBytes(soundName);
			Directory.CreateDirectory(Android.OS.Environment.ExternalStorageDirectory + "/Last Dialog/cache/");
			string path = Android.OS.Environment.ExternalStorageDirectory + "/Last Dialog/cache/" + soundName.Substring(soundName.IndexOf("/"));
			await File.WriteAllBytesAsync(path, track);
		}

		public static async Task AddLabelInHistory(Game page, Button btn = null, Xamarin.Forms.Image img = null, string subtitle = null)
		{
			if (img != null)
			{
				var newImg = new Xamarin.Forms.Image();
				newImg.Source = img.Source;
				page.textHistory.Children.Add(newImg);
			}
			else
			{
				string text;
				if (btn != null) text = btn.Text;
				else if (subtitle != null) text = subtitle;
				else text = page.text.Text;

				var lbl = new Label
				{
					Text = text,
					Margin = new Thickness(20, 10, 20, 10)
				};

				var handler = new TapGestureRecognizer();
				handler.Tapped += (sender, e) =>
				{
					if ((bool)MainPage.profile.objects["scrollBack"])
					{
						Animations.TextHistoryViewHide(page);
						page.RefreshPage(lbl.BindingContext.ToString());
					}
				};

				if (btn != null)
				{
					lbl.BackgroundColor = Color.FromHex("#55000000");
					lbl.Margin = 0;
					lbl.Padding = 20;
					lbl.TextColor = Color.FromHex("#757590");
					lbl.HorizontalTextAlignment = TextAlignment.Center;
				}
				else if (subtitle != null)
				{
					lbl.BackgroundColor = Color.FromHex("#363652");
					lbl.HorizontalTextAlignment = TextAlignment.Center;
					lbl.Padding = 20;
					lbl.Margin = 0;
				}

				lbl.GestureRecognizers.Add(handler);
				lbl.BindingContext = MainPage.profile.objects["presentPage"].ToString();
				page.textHistory.Children.Add(lbl);
			}
		}
		
		public static async void ChangeBackgroundImage (Game page)
		{
			Animations.BgImageShowHide (page.backgroundImage, "resources.Images." + page.backgroundImages.First());
			page.backgroundImages.RemoveAt(0);
		}

		public static void SaveGame(string fileName = "Autosave")
		{
			using (var sw = new StreamWriter(savesDirectory + "/" + fileName + ".json"))
			{
				string json = JsonConvert.SerializeObject (MainPage.profile, Formatting.Indented);
				sw.WriteLineAsync (json);
			}
		}

		public static Task LoadGame(string fileName = "Autosave")
		{
			using (var sr = new StreamReader(savesDirectory + "/" + fileName + ".json"))
			{
				string json = sr.ReadToEnd();
				MainPage.profile = JsonConvert.DeserializeObject<Profile>(json);
			}
			Game.pageFromSaves = true;
			
			return Task.CompletedTask;
		}
		
        public static List<string> CopyStringList (List<string> originalList)
        {
        	var newList = new List<string>();

        	for (int i = 0; i < originalList.Count; i++)
        	{
        		newList.Insert(i, (string) originalList[i].Clone());
        	}
        	
        	return newList;
        }
    }
}
