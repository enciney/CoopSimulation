using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoopSimulation
{
	public class AnimalHelper
	{
		private static int NameCounter = 1;
		private static AnimalSettingsDto AnimalSettings;

		private static  IList<KeyValuePair<Animal, int>> BornActions;
		public delegate IList<Animal> Born(Animal mom);

		public Born BornEvent;
		public AnimalHelper(AnimalSettingsDto animalSettings)
		{
			AnimalSettings = animalSettings;
			BornActions = new List<KeyValuePair<Animal, int>>();
		}

		public static string GetSpeciesName()
		{
			return AnimalSettings.SpeciesName;
		}

		public static string GetNewAnimalName()
		{
			return AnimalSettings.SpeciesName + NameCounter++.ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="males"></param>
		/// <param name="females"></param>
		/// <returns>number of mating</returns>
		public static int ScheduleMatingAndBorns(IList<Animal> males, IList<Animal> females, int currentMonth)
		{

			var numberOfMating = Math.Min(males.Count, females.Count);
			if(numberOfMating == 0)
			{
				return 0;
			}

			var readyMales = males.Take(numberOfMating);
			var readyFemales = females.Take(numberOfMating);
			foreach(var parent in readyMales.Zip(readyFemales))
			{
				if(ValidateMating(parent.Second, parent.First))
				{
					BornActions.Add(KeyValuePair.Create(parent.Second, currentMonth + AnimalSettings.PregnancyTime));
					parent.Second.IsPregnant = true;
				}
				else
				{
					numberOfMating--;
				}
			}

			return numberOfMating;

		}

		public static int GetCountOfChildForBorn()
		{
			var num = GetRandomNumberInRange();
			int nonExpectedBornPossibility = (int)(AnimalSettings.PossibilityOfNonExpectedCountOfBorn * 100);
			int expectedBornCount = AnimalSettings.ExpectedChildCountAtBorn;
			// we have possiblity of borning child count, below we calculate the child count
			// according to "PossibilityOfNonAverageBreeding" settings
			if (num > nonExpectedBornPossibility)
			{
				return expectedBornCount;
			}
			var extraChildCount = AnimalSettings.MaxChildCountAtBorn - expectedBornCount;
			var stepForExtraChild = nonExpectedBornPossibility / (double)extraChildCount;

			return expectedBornCount + (int)((num) / stepForExtraChild) + 1;
		}

		public static Gender GetGenderOfBabe()
		{
			var num = GetRandomNumberInRange();
			if(num < AnimalSettings.PercentageOfBornMale* 100)
			{
				return Gender.Male;
			}
			
			return Gender.Female;


		}

		public static IList<Animal> ApplyBorn(int currentMonth)
		{
			IList<Animal> totalChilds = new List<Animal>();
			var borns = BornActions.Where(act => act.Value == currentMonth).ToList();
			BornActions.RemoveAll(borns);
			Parallel.ForEach(borns, act =>
			{
				IList<Animal> childs = new List<Animal>();
				act.Key.TryBorn(out childs);
				act.Key.IsPregnant = false;
				lock (totalChilds)
				{
					totalChilds.AddRange(childs);
				}
			});

			//borns.ForEach(act =>
			//{
			//	IList<Animal> childs = new List<Animal>();
			//	act.Key.TryBorn(out childs);
			//	act.Key.IsPregnant = false;
			//	totalChilds.AddRange(childs);
			//});
			return totalChilds;
		}

		private static bool ValidateMating(Animal female, Animal male)
		{
			return female.IsReadyToMating() && male.IsReadyToMating() &&
			female.Gender == Gender.Female && male.Gender == Gender.Male &&
			female.Generation == male.Generation;
		}

		public static bool IsReadyMating(Animal animal)
		{
			return
				animal.Age >= AnimalSettings.ReadyToMating &&
				animal.Age < AnimalSettings.EndOfMatingAge &&
				animal.IsPregnant == false;
		}
		// need to check delete wrong
		public static int CheckAndApplyDie(IList<Animal> animals)
		{
			var diedAnimal = animals.Where(a => a.Age >= AnimalSettings.LifeTime);
			var diedAnimalCount = diedAnimal.Count();
			animals.RemoveAll(diedAnimal.ToList());
			//animals.RemoveAll(a => a.Age >= AnimalSettings.LifeTime);
			return diedAnimalCount;
		}

		public static void IncreaseAge(IList<Animal> animals)
		{
			animals.ForEach(anim =>
			{
				anim.Age++;
			});
		}

		private static int GetRandomNumberInRange(int min = 1, int max = 101)
		{
			Random rnd = new Random();
			return rnd.Next(min, max);
		}
	}
}
