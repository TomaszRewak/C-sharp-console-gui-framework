using System;

namespace ConsoleGUI.Example
{
	internal class Player
	{
		public string Name { get; }
		public string Surname { get; }
		public DateTime BirthDate { get; }
		public int Points { get; }

		public Player(string name, string surname, DateTime birthDate, int points)
		{
			Name = name;
			Surname = surname;
			BirthDate = birthDate;
			Points = points;
		}
	}
}
