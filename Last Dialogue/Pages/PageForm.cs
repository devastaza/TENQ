using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CSharp_Shell
{
    public class PageForm 
    {
        public string subtitle;
        public string link;
        public string imgSource;
        public List <string> backgroundImageSource;
        public string backgroundGifSource;
        public string music;
        public string fx;
        public List<Button> buttons;
        public List<string> texts;
        public PageFormSettings settings;
        public delegate void ExtendedFeatures (Game page);
        public ExtendedFeatures extendedOptions;
    }
}