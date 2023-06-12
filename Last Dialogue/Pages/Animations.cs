using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CSharp_Shell
{

	public class Animations
	{
		public static bool textAnimationIsEnded = true;
		public static bool paralaxing = false;
		
		public static async void TextHistoryViewHide(Game page)
		{
			bool isViewed = page.textHistoryField.IsVisible;
			if (isViewed)
			{
				page.bgDarkness.FadeTo(0, 300, Easing.SinIn);
				page.textHistoryField.ScaleTo(1.5, 300, Easing.CubicIn);
				await page.textHistoryField.FadeTo(0, 300, Easing.SinOut);
				
				page.bgDarkness.IsVisible = false;
				page.textHistoryField.IsVisible = false;
			}
			
			else
			{
				page.bgDarkness.IsVisible = true;
				page.textHistoryField.IsVisible = true;
				page.textHistoryField.Scale = 1.5;
				
				page.textHistoryScroll.ScrollToAsync (page.textHistory, ScrollToPosition.End, true);
				page.bgDarkness.FadeTo(1, 300, Easing.CubicOut);
				page.textHistoryField.ScaleTo(1, 300, Easing.CubicOut);
				await page.textHistoryField.FadeTo(1, 300, Easing.CubicOut);
			}
		}
		
		public static async void MenuShowHide (Game page)
		{
			if (page.menu.IsVisible)
			{
				page.bgDarkness.FadeTo (0, 300, Easing.SinIn);
				page.menu.ScaleTo (0.5, 300, Easing.CubicOut);
				await page.menu.FadeTo (0, 300, Easing.SinOut);
				
				page.bgDarkness.IsVisible = false;
				page.menu.IsVisible = false;
			}
			
			else
			{
				page.bgDarkness.IsVisible = true;
				page.menu.IsVisible = true;
				page.menu.Scale = 1.5;
				
				page.bgDarkness.FadeTo (1, 300, Easing.CubicOut);
				page.menu.ScaleTo (1, 300, Easing.CubicOut);
				page.closeButton.RelRotateTo (720, 1500, Easing.CubicOut);
				await page.menu.FadeTo (1, 300, Easing.CubicOut);
			}
		}

		public static async Task WriteNewText(Game page, string text)
		{
			textAnimationIsEnded = false;
			page.text.Text = "";
			foreach (var letter in text)
			{
				if (textAnimationIsEnded == false)
				{
					page.text.Text += letter;
					try
					{
						await Task.Delay(5);
					}
					catch { }
				}
			}
			textAnimationIsEnded = true;
		}

		public static async Task PlaySettedButtonAnimation(Game page, Object button)
		{
			((Button)button).ScaleXTo(2, 50);
			((Button)button).ScaleYTo(0.1, 50);
			await ((Button)button).FadeTo(0, 50);

			for (int i = 0; i < page.buttonsField.Children.Count; i++)
			{
				await page.buttonsField.Children[i].FadeTo(0, 70);
			}
			((Button)button).ScaleX = 1;
			((Button)button).ScaleY = 1;
		}

		public static async Task ShowButtons(Game page)
		{
			page.pageScroll.ScrollToAsync (page.buttonsField, ScrollToPosition.End, true);
			for (int i = 0; i < page.buttonsField.Children.Count; i++)
			{
				page.buttonsField.Children[i].ScaleX = 0.1;
				page.buttonsField.Children[i].ScaleXTo(1, 100);
				await page.buttonsField.Children[i].FadeTo(1, 100);
			}
		}
		
		public static async Task FastRefreshFrameAnimation (Frame frame)
		{
			frame.ScaleTo (1.1, 70);
			await frame.FadeTo (0.5, 70);
			
			frame.ScaleTo (1, 200);
			await frame.FadeTo (1, 200);
		}
		
		public static async void RefreshSubtitle (BoxView line, bool order)
		{
			if (order)
			{
				line.ScaleX = 0;
				line.Opacity = 1;
			
				await line.ScaleXTo (1, 1000, Easing.CubicOut);
				await Task.Delay (2000);
				await line.FadeTo (0, 1500);
			}
		}
		
		public static async Task BgImageShowHide (Image backgroundImage, string path = null)
		{
			if (backgroundImage.BindingContext.ToString() != path)
			{
				await backgroundImage.FadeTo (0, 100);
				backgroundImage.Source = ImageSource.FromResource(path);
				backgroundImage.BindingContext = path;
				await backgroundImage.FadeTo (1);
			}
		}
		
		public static async Task ImageShowHide (Game page, int[] size, LayoutOptions[] layoutOptions, string path = null)
		{
			Console.WriteLine("Начало анимации изображения.");
			
			if (path != null && page.imgField.IsVisible) //обновление
			{
				await page.imgField.FadeTo (0, 300, Easing.CubicOut);
			
				page.imgField.WidthRequest = size.First();
				page.imgField.HeightRequest = size.Last();
				page.imgField.HorizontalOptions = layoutOptions.First();
				page.imgField.VerticalOptions = layoutOptions.Last();
			
				page.image.Source = ImageSource.FromResource(path);
				
				await page.imgField.FadeTo (1, 500, Easing.CubicIn);
			}
			else if (path != null) //установка
			{
				page.imgField.WidthRequest = size.First();
				page.imgField.HeightRequest = size.Last();
				page.imgField.HorizontalOptions = layoutOptions.First();
				page.imgField.VerticalOptions = layoutOptions.Last();

				page.image.Source = ImageSource.FromResource(path);
				page.imgField.Opacity = 0;
				page.imgField.ScaleY = 0;
				page.imgField.TranslationY = -100;
				page.imgField.IsVisible = true;
				
				page.imgField.TranslateTo (0, 0, 300, Easing.CubicOut);
				page.imgField.ScaleYTo (1, 300, Easing.CubicOut);
				await page.imgField.FadeTo (1, 300, Easing.CubicOut);
			}		
			else //скрытие
			{
				page.imgField.ScaleYTo (0, 200, Easing.SinOut);
				page.imgField.TranslateTo (0, -100, 200, Easing.SinOut);
				await page.imgField.FadeTo (0, 200, Easing.SinOut);
				
				page.imgField.IsVisible = false;
				page.imgField.ScaleY = 1;
				page.imgField.TranslationY = 0;
				page.imgField.Opacity = 1;
			}
			
			Console.WriteLine("Конец анимации изображения.");
		}
		
		public static async void CloseMenuWithButton(Game page)
		{
			page.closeButtonBorder.Scale = 1;
			page.closeButtonBorder.Opacity = 1;
			page.closeButtonBorder.ScaleTo (5, 200, Easing.CubicOut);
			page.closeButtonBorder.FadeTo (0, 200);
			
			MenuShowHide (page);
		}
		
		public static async Task ButtonFadingAnimation (Game page)
		{
			foreach (Button button in page.buttonsField.Children)
			{
				button.FadeTo (0.3, 50);
				button.ScaleXTo (0.8, 50);
				
				await Task.Delay (50);
				button.FadeTo (1, 50);
				button.ScaleXTo (1, 50);
			}
		}
		
		public static void ImageFocusing (Game page)
		{
			if (page.imgField.Scale == 1)
			{
				page.imgField.ScaleTo (1.20, 300, Easing.CubicIn);
				page.textField.TranslateTo (0, 30, 300);
				page.imgFrame.WidthRequest *= 2;
				page.imgFrame.HeightRequest *= 2;
			}
			else
			{
				NormalizeImageScale (page);
			}
		}
		
		public static async void NormalizeImageScale (Game page)
		{
			page.imgField.ScaleTo (1, 200, Easing.CubicOut);
			await page.textField.TranslateTo (0, -10, 200, Easing.CubicOut);
			page.textField.TranslateTo (0, 0, 150, Easing.CubicOut);
			page.imgFrame.WidthRequest /= 2;
			page.imgFrame.HeightRequest /= 2;
		}
		
		public static async void BgImageParallaxEffect (Image img)
		{
			paralaxing = true;
			img.ScaleTo (1.05, 1000, Easing.CubicInOut);
			
			int x, y;
			while (paralaxing)
			{
				x = SomeMethods.rndm.Next(-10, 10);
				y = SomeMethods.rndm.Next(-10, 10);
				await Task.Delay (1000);
				img.TranslateTo (x, y, 2000, Easing.CubicOut);

				await Task.Delay (1000);
				img.TranslateTo (0, 0, 2000, Easing.SinInOut);
			}
			
			await img.ScaleTo (1.015, 250, Easing.CubicInOut);
		}
	}
}