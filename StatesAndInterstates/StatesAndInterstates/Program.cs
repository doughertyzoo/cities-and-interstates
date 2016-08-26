using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StatesAndInterstates
{
    class Program
    {
        #region Constants

        private const string InputArgumentFilepath = "/f:";
        private const string OutputFilenameCity = "Cities_By_Population.txt";
        private const string OutputFilenameInterstate = "Interstates_By_City.txt";
		private const string OutputFilenameDegrees = "Degrees_From_Chicago.txt";

        private const int InputIndexPopulation = 0;
        private const int InputIndexCity = 1;
        private const int InputIndexState = 2;
        private const int InputIndexInterstates = 3;

        private const char DelimiterInput = '|';
        private const char DelimiterInterstates = ';';
		private const int MaxDegreesOfSeparation = 10;

        #endregion

        #region Privates

        private static string _inputFileName;
		private static string _outputPath;
        private static string _message;
        private static List<FileInput> _inputs;

        #endregion

        public static void Main(string[] args)
        {
            if (!ValidateInput(args) || 
				!ReadFile() ||
				!OutputCitiesByPopulation() ||
				!OutputInterstatesByCity() ||
				!OutputDegreesFromChicago())
            {
                WriteMessage();
                return;
            }

            _message = $"Files {OutputFilenameCity} and {OutputFilenameInterstate} created.";
            WriteMessage();
        }

        #region Methods

        private static bool ValidateInput(string[] args)
        {
            var fileSwitch = args.FirstOrDefault(x => x.StartsWith(InputArgumentFilepath));

            if (fileSwitch == null)
            {
                _message = "Call this application using the following format:\nStatesAndInterstates.exe /f:{[pathname]}[filename]";
                return false;
            }

            _inputFileName = fileSwitch.Substring((InputArgumentFilepath).Length);

            if (string.IsNullOrEmpty(_inputFileName))
            {
                _message = "Missing file to import.";
                return false;
            }

            if (!File.Exists(_inputFileName))
            {
                _message = "File to import does not exist.";
                return false;
            }

			_outputPath = Path.GetDirectoryName(_inputFileName);


			return true;
        }

        private static bool ReadFile()
        {
            bool result;

            try
            {
                _inputs = new List<FileInput>();
                var lines = File.ReadAllLines(_inputFileName);
                foreach (var line in lines)
                {
                    var inputArray = line.Split(DelimiterInput);
					var input = new FileInput(
						int.Parse(inputArray[InputIndexPopulation]),
						inputArray[InputIndexCity],
						inputArray[InputIndexState]);

                    var interstateArray = inputArray[InputIndexInterstates].Split(DelimiterInterstates);
                    foreach (var interstateString in interstateArray)
                    {
                        input.Interstates.Add(new Interstate(interstateString));
                    }

                    _inputs.Add(input);
                }

                result = true;
            }
            catch (Exception ex)
            {
                _inputs = null;
                _message = $"Error reading from {_inputFileName}.\nError: {ex.Message}";
                result = false;
            }

            return result;
        }

        private static bool OutputCitiesByPopulation()
        {
            bool result;

            try
            {
                using (var output = new StreamWriter(Path.Combine(_outputPath, OutputFilenameCity)))
                {
                    var outputQuery = _inputs
                        .GroupBy(input => input.Population)
                        .Select(
                            group =>
                                new
                                {
                                    Population = group.Key,
                                    Cities = group.OrderBy(x => x.State).ThenBy(x => x.City)
                                })
                        .OrderByDescending(group => group.Population);

                    foreach (var group in outputQuery)
                    {
                        output.WriteLine(group.Population + "\n");
                        foreach (var city in group.Cities)
                        {
                            output.WriteLine(city.City + ", " + city.State);

                            var interstateString = String.Join(", ", city.Interstates.OrderBy(i => i.Number).Select(x => x.DisplayName).ToArray());
                            output.WriteLine("Interstates: " + interstateString + "\n");
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                _message = $"Error outputting {OutputFilenameCity}.\nError: {ex.Message}";
                result = false;
            }

            return result;
        }

        private static bool OutputInterstatesByCity()
        {
            bool result;

            try
            {
                var interstates = new Dictionary<int, int>();

                foreach (var input in _inputs.SelectMany(x => x.Interstates))
                {
                    if (!interstates.ContainsKey(input.Number))
                    {
                        interstates[input.Number] = 0;
                    }
                    interstates[input.Number]++;
                }

                using (var output = new StreamWriter(Path.Combine(_outputPath, OutputFilenameInterstate)))
                {
                    foreach (var interstate in interstates.OrderBy(sort => sort.Key))
                    {
                        output.WriteLine(Interstate.InterstatePrefix + interstate.Key + " " + interstate.Value);
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                _message = $"Error outputting {OutputFilenameInterstate}.\nError: {ex.Message}";
                result = false;
            }

            return result;
        }

		private static bool OutputDegreesFromChicago()
		{
			bool result;

			try
			{
				for (int i = 0; i < MaxDegreesOfSeparation; i++)
				{
					var baseInterstates = _inputs.Where(x => x.DegreeRemovedFromChicago == i).Select(x => x.Interstates);
					var uncheckedCities = _inputs.Where(x => x.DegreeRemovedFromChicago == -1);

					if (!uncheckedCities.Any())
					{
						break;
					}

					foreach (var interstate in baseInterstates)
					{
						foreach (var city in uncheckedCities)
						{
							var intersection = interstate.Intersect(city.Interstates, new InterstateComparer());
							if (intersection.Count() > 0)
							{
								city.DegreeRemovedFromChicago = i + 1;
							}
						}
					}
				}

				using (var output = new StreamWriter(Path.Combine(_outputPath, OutputFilenameDegrees)))
				{
					foreach (var input in _inputs.OrderByDescending(sort => sort.DegreeRemovedFromChicago).ThenBy(sort => sort.City).ThenBy(sort => sort.State))
					{
						output.WriteLine(input.DegreeRemovedFromChicago + " " + input.City + ", " + input.State);
					}
				}

				result = true;
			}
			catch (Exception ex)
			{
				_message = $"Error outputting {OutputFilenameDegrees}.\nError: {ex.Message}";
				result = false;
			}

			return result;
		}

		private static void WriteMessage()
        {
            Console.WriteLine(_message);
        }
        
        #endregion
    }
}
