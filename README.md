# CoopSimulation
Simulate Specified Species of animal reproduction in coop. 

# Configuration
All of the configuration stored in **AppSettings.json** file even simulation cycle
Below the configuration parameter of CoopSimulation;

* **SpeciesName**; name of the animal species
* **LifeTime**; total life stamp of the animal as month
* **PercentageOfBornMale**; probability of born as male 
* **ExpectedChildCountAtBorn**; average child count in a born
* **MaxChildCountAtBorn**; maximum cunt of borning child
* **PossibilityOfNonExpectedCountOfBorn**; Probability of non-expected child count.
* **PregnancyTime**; Pregnancy time of animal as month (only female)
* **ReadyToMating**;  REady month of the mating for animal (both male and female)
* **EndOfMatingAge**; Specified as month and meaning that after this month both female and male can not be mating

And some parameters for Coop Management;

* **InitialAgeForStartup**; we can specify our ancestor animals age in coop as initial
* **SimulationTime**; specify the simulation cycle as month
* **DetailedDebug**; with this paramater we can debug statistic end of each month


```json
{
	"AnimalConfig": {
		"SpeciesName": "Rabbit",
		"LifeTime": "60",
		"PercentageOfBornMale": "0.5",
		"ExpectedChildCountAtBorn": "2",
		"MaxChildCountAtBorn": "6",
		"PossibilityOfNonExpectedCountOfBorn": "0.5",
		"PregnancyTime": "3",
		"ReadyToMating": "12",
		"EndOfMatingAge": "36"
	},

	"CoopConfig": {
		"InitialAgeForStartup": "6",
		"SimulationTime": "60",
		"DetailedDebug": "false"
	}

}
```
# Classes

**AnimalSettingsDto** ; store AnimalConfig sections configuration inside 

**CoopSettingsDto** ; store CoopConfig sections configuration inside

**Gender** ; store Male,Female information as enumaration

**Animal**; store animal; name, gender, age, generation, pregnancy info

**AnimalHelper**; store Animal settings inside as private and manage the all Animal related job inside

**CoopManager**; schedule the animation and let time pass month by month, also collect the statistic

**EnumerableExtensions** ; extension methods for IEnumerable for increase efficiency and code-reuse

**Program.cs** ; Arrange the configuration and also dependency injection

# Dependency
 * At startup, configuration json file bind to related Dtos.
 * AnimalHelper has AnimalSettingsDto injection inside 
 * CoopManager has CoopSettingsDto and AnimalHelper injection inside
 * At startup only CoopManager object requested and as automagically other classes ready to use (thanks to IOC container option of .Net core)

# Dies
If a animal age is equals to **LifeTime** parameter then unfortunately dies and deleted from coop

# Mating 
for mating happens first of all we need male and female and also below conditions should be done;
Male and Female;
* Should be same generation
* Age should be lower than **EndOfMatingAge** param
* Age should be bigger tha **ReadyToMating** param
Female;
* Should not be pregnant

**Logic** : 
* Collect all of ready to mating animals as female and male then mating each other.
* Born actions stored in a list and should have Mother animal and month that born happen at which month

  For this **PregnancyTime** param is being used , For ex. : if current age is 5  and **PregnancyTime** is 3 then filled as 8 

# Born
Born means we have new baby animals in coop :) and we can have baby count between  **ExpectedChildCountAtBorn** - **MaxChildCountAtBorn**
* For born we use **PercentageOfBornMale** possibility for Gender
* **PossibilityOfNonExpectedCountOfBorn** parameter arrange of the borning babe count and if probability coming as bigger then this parameter then code found the child count     between **ExpectedChildCountAtBorn** - **MaxChildCountAtBorn**

# Statistic
Statistic sample info is as below;
```
Statictic of Month 60;
Total count of Rabbit : 870
Number of female 446, Number of Male 424
Total mating  count : 360, Last month mating count : 5
Total Dies : 2, Last month died count : 0
Count of Adolescent animal (0 - 11) : 596
Count of Adult animal (12 - 35) : 253
Count of Old animal (36 - 60) : 21
```

# TO DO
* Inside code I did not use event handlers and task, would be good to wire a event for Born actions
* Thread is only used as Parallel.ForEach for purpose of Born and here need to use lock since lots of thread trying to manuplate same list.
* I made a console application only, not ASP.Net core , so I did not use multi-threading and thread-safe code here
* Semantic check of configuration parameters
