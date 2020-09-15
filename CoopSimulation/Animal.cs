using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CoopSimulation
{
	public class Animal
	{

		/// <summary>
		/// Name of the animal
		/// </summary>
		public string RealName;
		public Gender Gender;
		public bool IsPregnant;
		public int Age;
		/// <summary>
		/// A animal can be reproduce with same generation animal
		/// </summary>
		public int Generation;

		public Animal(Gender gender, int generation) : this(gender, generation, 0) { }

		public Animal(Gender gender, int generation, int age )
		{
			RealName = AnimalHelper.GetNewAnimalName();
			Gender = gender;
			Generation = generation;
			Age = age;
			IsPregnant = false;
		}



		public  void TryBorn(out IList<Animal> childs)
		{
			childs = new List<Animal>();
			var childCount = AnimalHelper.GetCountOfChildForBorn();
			var newGeneration = this.Generation + 1;
			foreach (int value in Enumerable.Range(1,childCount))
			{
				var gender =  AnimalHelper.GetGenderOfBabe();
				childs.Add(new Animal(gender, newGeneration));
			}
		}

		public bool IsReadyToMating()
		{
			return AnimalHelper.IsReadyMating(this);
		}



		
	}
}
