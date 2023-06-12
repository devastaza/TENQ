using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CSharp_Shell
{

	public class PageUpdater
	{
		public static Task UpdateSettings(PageForm newPage)
		{
			if (newPage.settings == null) //даем стандартные настройки, если кастомных нет
			{
				newPage.settings = new PageFormSettings();
			}

			Console.WriteLine("Настройки обновлены.");
			return Task.CompletedTask;
		}

		public static Task UpdateBackgroundImage(Game page, in PageForm newPage)
		{
			if ((newPage.backgroundImageSource != null && newPage.backgroundImageSource.Count > 0) ||
				(Game.pageFromSaves && MainPage.profile.lastBgImgSource.Count > 0))
			{
				page.backgroundImages = newPage.backgroundImageSource;

				if (Game.pageFromSaves)
				{
					page.backgroundImages = MainPage.profile.lastBgImgSource;
				}

				MainPage.profile.lastBgImgSource = SomeMethods.CopyStringList(page.backgroundImages); //разъединяем ссылки
				SomeMethods.ChangeBackgroundImage(page);
			}
			
			Console.WriteLine("Задний фон обновлен.");

			if (newPage.settings.isBgImageParallaxing)
			{
				Animations.BgImageParallaxEffect(page.backgroundImage);
			}
			return Task.CompletedTask;
		}

		public static async Task UpdateBackgroundGif(Game page, PageForm newPage)
		{
			if (newPage.backgroundGifSource != null)
			{
				await Animations.BgImageShowHide(page.backgroundGif, "resources.Gifs." + newPage.backgroundGifSource);
			}
			else await Animations.BgImageShowHide(page.backgroundGif, "");
		}

		public static Task UpdateLink(Game page, PageForm newPage)
		{
			if (newPage.link != null && newPage.link != "") //берем ссылку на новую страницу, если есть
			{
				page.link = newPage.link;
			}
			else
			{
				page.link = null; //или обнуляем текущую (чтобы прршлая стерлась, если была)
			}
			return Task.CompletedTask;
		}

		public static Task UpdateSubtitle(Game page, PageForm newPage)
		{
			if ((newPage.subtitle != null && newPage.subtitle != "") ||
				(Game.pageFromSaves && MainPage.profile.lastSubtitle != null)) //если новый заголовок --> меняем на него
			{
				page.subtitle.IsVisible = newPage.settings.IsSubtitleVisible;

				if (Game.pageFromSaves) //если берем страницу из сохранений, то достаем инфу из них
				{
					page.subtitle.Text = MainPage.profile.lastSubtitle;
				}
				else page.subtitle.Text = newPage.subtitle;  //либо из словаря

				Animations.RefreshSubtitle(page.subtitleLine, newPage.settings.subtitleLineAnimation);

				if (newPage.settings.hasSubtitleSaveInHistory)
				{
					SomeMethods.AddLabelInHistory(page, subtitle: page.subtitle.Text);
				}

				MainPage.profile.lastSubtitle = page.subtitle.Text;
			}
			return Task.CompletedTask;
		}

		public static Task UpdateImage(Game page, in PageForm newPage)
		{
			if (newPage.imgSource == "none") //пусто - значит та же картина
			{
				Animations.ImageShowHide(page, newPage.settings.imageSize, newPage.settings.imageXYoptions);
				MainPage.profile.lastImgSource = newPage.imgSource;
			}
			else if ((newPage.imgSource != null) ||
			(Game.pageFromSaves && MainPage.profile.lastImgSource != null))//есть картинка? Вставляем!
			{
				try
				{
					if (Game.pageFromSaves)
					{
						Animations.ImageShowHide(page, newPage.settings.imageSize, newPage.settings.imageXYoptions, "resources.Images." + MainPage.profile.lastImgSource);
						Console.WriteLine("Новое изображение: " + MainPage.profile.lastImgSource);
					}
					else
					{
						Console.WriteLine("Загрузка изображения " + newPage.imgSource);
						Animations.ImageShowHide(page, newPage.settings.imageSize, newPage.settings.imageXYoptions, "resources.Images." + newPage.imgSource);
						Console.WriteLine("Новое изображение: " + newPage.imgSource + " загружено.");
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("ALERT. Ошибка загрузки фото. " + e.ToString());
				}
				
				MainPage.profile.lastImgSource = newPage.imgSource == null ? MainPage.profile.lastImgSource : newPage.imgSource;
			}

			if (newPage.settings.hasImageSaveInHistory)
			{
				SomeMethods.AddLabelInHistory(page, img: page.image);
			}
			
			return Task.CompletedTask;
		}

		public static Task UpdateText(Game page, in PageForm newPage)
		{
			if (newPage.texts != null && newPage.texts.Count > 0) //если в экземпляре есть текст --> копируем его
			{
				page.textField.BackgroundColor = Color.FromHex(newPage.settings.textFieldColor);
				page.texts = newPage.texts;
				page.textField.IsVisible = true;
			}
			else
			{
				page.textField.IsVisible = false;
				page.texts = null; //или же обнуляем список текстов
			}
			
			page.textİndex = newPage.texts == null? -1: 0;
			
			return Task.CompletedTask;
		}

		public static async Task UpdateButtons(Game page, PageForm newPage)
		{
			if (newPage.buttons != null && newPage.buttons.Count > 0) //добавляем кнопкам обработчик клика и их самих на страницу
			{
				await SomeMethods.InsertButtons(page, newPage.buttons);
				if (page.texts == null)
				{
					page.buttonsField.IsVisible = true; //
					await Animations.ShowButtons(page);
				}
			}
		}

		public static Task UpdateMusic(Game page, in PageForm newPage)
		{
			if ((newPage.music != null) ||
				(Game.pageFromSaves && MainPage.profile.lastMusicSource != null))
			{
				if (Game.pageFromSaves)
				{
					SomeMethods.PreparePlayer(page, App.musicPlayer, MainPage.profile.lastMusicSource);
				}
				else
				{
					SomeMethods.PreparePlayer(page, App.musicPlayer, newPage.music);
				}

				MainPage.profile.lastMusicSource = newPage.music == null ? MainPage.profile.lastMusicSource : newPage.music;
			}
			return Task.CompletedTask;
		}

		public static Task UpdateFX(Game page, in PageForm newPage)
		{
			if ((newPage.fx != null) ||
				(Game.pageFromSaves && MainPage.profile.lastFxSource != null))
			{
				if (Game.pageFromSaves)
				{
					SomeMethods.PreparePlayer(page, App.fxPlayer, MainPage.profile.lastFxSource, newPage.settings.isFXlooping);
				}
				else
				{
					SomeMethods.PreparePlayer(page, App.fxPlayer, newPage.fx, newPage.settings.isFXlooping);
				}

				MainPage.profile.lastFxSource = newPage.fx == null ? MainPage.profile.lastFxSource : newPage.fx;
			}
			return Task.CompletedTask;
		}

		public static Task RunExtendedOptions(Game page, PageForm newPage)
		{
			if (newPage.extendedOptions != null)
			{
				PageForm.ExtendedFeatures features;
				features = newPage.extendedOptions;
				features(page);
			}
			return Task.CompletedTask;
		}
	}
}