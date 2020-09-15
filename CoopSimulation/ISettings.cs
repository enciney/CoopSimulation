using System;
using System.Collections.Generic;
using System.Text;

namespace CoopSimulation
{
	public interface ISettings
	{
		T GetOption<T>(string optionName);
		bool TryGetOption<T>(string optionName, out T value);

		Dictionary<string, string> GetSettings();
	}
}
