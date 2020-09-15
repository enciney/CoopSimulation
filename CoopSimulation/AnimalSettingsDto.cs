using System;
using System.Collections.Generic;
using System.Text;

namespace CoopSimulation
{
	public class AnimalSettingsDto
	{

		public string SpeciesName { get; set; }
		public int LifeTime { get; set; }
		public double PercentageOfBornMale { get; set; }
		public int ExpectedChildCountAtBorn { get; set; }
		public int MaxChildCountAtBorn { get; set; }
		public double PossibilityOfNonExpectedCountOfBorn { get; set; }
		public int PregnancyTime { get; set; }
		public int ReadyToMating { get; set; }
		public int EndOfMatingAge { get; set; }



	}
}
