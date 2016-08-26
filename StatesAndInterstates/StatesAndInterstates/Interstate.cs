namespace StatesAndInterstates
{
    public class Interstate
    {
        #region Constants

        public const int InterstateNumberDefault = 0;
        public const string InterstatePrefix = "I-";

        #endregion

        #region Privates

        private int? _number;

        #endregion

        #region Publics

        public string DisplayName { get; set; }

        public int Number
        {
            get
            {
                if (!_number.HasValue)
                {
                    try
                    {
                        _number = !string.IsNullOrEmpty(DisplayName) ?
                            int.Parse(DisplayName.Substring(InterstatePrefix.Length)) :
                            InterstateNumberDefault;
                    }
                    catch
                    {
                        _number = InterstateNumberDefault;
                    }                }
                return _number.Value;
            }
        }

        #endregion

        public Interstate(string input)
        {
            DisplayName = input;
        }

        #region Methods

        public override string ToString()
        {
            return DisplayName;
        }

        #endregion
    }
}
