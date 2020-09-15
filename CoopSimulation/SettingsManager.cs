using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoopSimulation
{
	public class SettingsManager : ISettings
	{
		IConfigurationRoot configuration;
		public SettingsManager(IConfigurationRoot section)
		{
			configuration = section;
		}

		public T GetOption<T>(string optionName)
		{
			return configuration.GetValue<T>(optionName);
		}

		public Dictionary<string, string> GetSettings()
		{
			return configuration.AsEnumerable().ToDictionary(s => s.Key, v => v.Value);
		}

		public bool TryGetOption<T>(string optionName, out T value)
		{
			value = configuration.GetValue<T>(optionName);
			return IsOptionExist(optionName);
		}

		private bool IsOptionExist(string optionName)
		{
			return configuration.AsEnumerable().Count(s => s.Key == optionName) > 0;
		}
	}
}
