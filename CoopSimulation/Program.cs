using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CoopSimulation
{
	
	public class Program
	{
		public const string CONFIGNAME = "AnimalConfig";
		public static IConfigurationRoot AppConfig { get; set; }
		public static void Main(string[] args)
		{
			ServiceCollection serviceCollection = new ServiceCollection();
			Initialize(serviceCollection);
		}

		public static void Initialize(IServiceCollection serviceCollection)
		{

			AppConfig = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true).Build();
			//var configSection = AppConfig.GetSection(CONFIGNAME).AsEnumerable().ToDictionary(s => s.Key, v => v.Value);
			var configSection = AppConfig.GetSection(CONFIGNAME);
			serviceCollection.AddSingleton<IConfigurationSection>(configSection);

			var serviceProvider = serviceCollection.BuildServiceProvider();


		}
	}
}
