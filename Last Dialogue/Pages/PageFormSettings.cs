using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CSharp_Shell
{

    public class PageFormSettings 
    {
        public bool subtitleLineAnimation = true;
        public bool IsSubtitleVisible = true;
        public bool textAnimation = true;
        public bool isFXlooping = false;
        public bool hasImageSaveInHistory = false;
        public bool hasSubtitleSaveInHistory = true;
        public bool isBgImageParallaxing = false;
        public int[] imageSize = new int[]{400, 250};
        public LayoutOptions[] imageXYoptions = new LayoutOptions []{LayoutOptions.Center, LayoutOptions.Start};
        public LayoutOptions[] buttonsXYoptions = new LayoutOptions []{LayoutOptions.Center, LayoutOptions.Start};
        public int[] buttonTranslation = new int[]{0, 0};
        public string textFieldColor = "#33000000";
    }
}