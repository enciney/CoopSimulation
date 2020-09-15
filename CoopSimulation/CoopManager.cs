using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoopSimulation
{
	public class CoopManager
	{
		static int TotalMating = 0;
		static int MatingCountThisMonth = 0;
		static int TotalDied = 0;
		static int DiedCountThisMonth = 0;
		CoopSettingsDto CoopSettings;
		AnimalHelper AnimalHelper;
		IList<Animal> AnimalList = new List<Animal>();
		IList<Animal> ReadyToMatingMale = new List<Animal>();
		IList<Animal> ReadyToMatingFemale = new List<Animal>();

		public CoopManager(CoopSettingsDto coopSettings, AnimalHelper animalHelper)
		{
			var AncestorMaleAnimal = new Animal(Gender.Male, 1, coopSettings.InitialAgeForStartup);
			var AncestorFemaleAnimal = new Animal(Gender.Female, 1, coopSettings.InitialAgeForStartup);
			AnimalList.Add(AncestorMaleAnimal);
			AnimalList.Add(AncestorFemaleAnimal);
			CoopSettings = coopSettings;
			AnimalHelper = animalHelper;
			
		}

		public void StartSimulation()
		{
			foreach (int i in Enumerable.Range(1, CoopSettings.SimulationTime))
			{
				PassOneMonth(i);
				PrintStat(i);
			}

			// print number of animal
			// female, male number / percantage
			// print number of below age;
			// 0- readytoMating 
			// 11- endOfMating
			// EndOfMating - Lifetime
			// Died animals
		}

		public void PrintStat(int currentMonth)
		{
			
			string txt = $"Statictic of Month {currentMonth};\n\n" +
				$"Total count of {AnimalHelper.GetSpeciesName()} : {AnimalList.Count}\n " +
			   $"Number of female {AnimalList.Where(a => a.Gender == Gender.Female).Count()} " +
			   $"number of Male {AnimalList.Where(a => a.Gender == Gender.Male).Count()}\n" +
			   $"Total mating  count : {TotalMating},  Last month mating count : {MatingCountThisMonth} " +
			   $"Total Dies : {DiedCountThisMonth}, Last month died count :{DiedCountThisMonth}\n\n\n";
			Console.WriteLine(txt);
		}

		public void PassOneMonth(int currentMonth)
		{
			// clear mating lists
			ReadyToMatingMale.Clear();
			ReadyToMatingFemale.Clear();
			// check die actions and remove
			DiedCountThisMonth = AnimalHelper.CheckAndApplyDie(AnimalList);
			TotalDied += DiedCountThisMonth;

			#region check born actions
			// check previous born actions 
			// make born and add new animals to list
			AnimalList.AddRange(AnimalHelper.ApplyBorn(currentMonth));

			// fill the mating list
			ReadyToMatingMale = AnimalList.Where(anim => anim.IsReadyToMating() && anim.Gender == Gender.Male).ToList();
			ReadyToMatingFemale = AnimalList.Where(anim => anim.IsReadyToMating() && anim.Gender == Gender.Female).ToList();
			// crate new born actions
			MatingCountThisMonth  = AnimalHelper.ScheduleMatingAndBorns(ReadyToMatingMale, ReadyToMatingFemale, currentMonth);
			TotalMating += MatingCountThisMonth;
			#endregion

			#region calculate age
			// increase age 1 
			AnimalHelper.IncreaseAge(AnimalList);
			#endregion

		}
	}
}
