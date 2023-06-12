using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CSharp_Shell
{

	public class Profile
	{
		public string lastSubtitle;
		public string lastImgSource;
		public string lastMusicSource;
		public List<string> lastBgImgSource;
		public string lastFxSource;

		public Dictionary<string, object> objects = new Dictionary<string, object>
		{
			{"presentPage","mainPage"},
			{"лояльность", 0},
			{"scrollBack", true}
		};
	}
}