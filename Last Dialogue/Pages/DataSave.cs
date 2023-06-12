using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharp_Shell
{
	public class DataSave
	{
		protected static readonly string savesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		
		public string currentPageName;
		public Dictionary<string, object> variables = new Dictionary<string, object>();
		
		public void Save(string fileName = "Autosave")
		{
			using (var sw = new StreamWriter(savesDirectory + "/" + fileName + ".json"))
			{
				string json = JsonConvert.SerializeObject(this, Formatting.Indented);
				sw.WriteLineAsync(json);
			}
		}
		
		public DataSave Load(string fileName = "Autosave")
		{
			using (var sr = new StreamReader(savesDirectory + "/" + fileName + ".json"))
			{
				string json = sr.ReadToEnd();
				return JsonConvert.DeserializeObject<DataSave>(json);
			}
		}
	}
}