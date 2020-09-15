using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace CoopSimulation
{
	
	public class Program
	{
		public const string ANIMALCONFIG = "AnimalConfig";
		public const string COOPCONFIG = "CoopConfig";
		public static IConfigurationRoot AppConfig { get; set; }
		public static ServiceProvider Provider;
		public static void Main(string[] args)
		{
			ServiceCollection serviceCollection = new ServiceCollection();
			Initialize(serviceCollection);
			var coopManager = Provider.GetRequiredService<CoopManager>();
			coopManager.StartSimulation();

		}

		public static void Initialize(IServiceCollection serviceCollection)
		{
			
			AppConfig = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true).Build();

			var animalSection = AppConfig.GetSection(ANIMALCONFIG);
			var animalSettings = new AnimalSettingsDto();
			animalSection.Bind(animalSettings);

			var coopSection = AppConfig.GetSection(COOPCONFIG);
			var coopSettings = new CoopSettingsDto();
			coopSection.Bind(coopSettings);

			serviceCollection.AddSingleton<AnimalSettingsDto>(animalSettings);
			serviceCollection.AddSingleton<CoopSettingsDto>(coopSettings);
			serviceCollection.AddSingleton<CoopManager>();
			serviceCollection.AddSingleton<AnimalHelper>();
			//serviceCollection.AddSingleton<ISettings,SettingsManager>();
			Provider = serviceCollection.BuildServiceProvider();
		}
	}
}
