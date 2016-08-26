using System.Collections.Generic;

namespace StatesAndInterstates
{
    public class FileInput
    {
        #region Publics

        public int Population { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<Interstate> Interstates { get; set; }
		public int DegreeRemovedFromChicago { get; set; }


        #endregion

        public FileInput(int population, string city, string state)
        {
			Population = population;
			City = city;
			State = state;
			Interstates = new List<Interstate>();
			DegreeRemovedFromChicago = City.ToLower() == "chicago" ? 0 : -1;
		}
    }
}
