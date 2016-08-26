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

        #endregion

        public FileInput()
        {
            Interstates = new List<Interstate>();
        }
    }
}
