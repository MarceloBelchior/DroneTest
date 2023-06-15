using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DroneDelivery
{



   public class Program
    {



        public static void Main(string[] args)
        {
            string inputFilePath = "input.txt";
            string outputFilePath = "output.txt";

            // Step 1: Parse the input data
            string[] lines = File.ReadAllLines(inputFilePath);

            string[] drones = lines[0].Split(',').Select(x => x.Trim()).ToArray();
            List<Tuple<string, int>> dtuple = new List<Tuple<string, int>>();
            //Step 2: Get the drone capacity
            for (int i = 0; i < drones.Length; i++)
            {
                int capacity = 0;
                if (drones[i].Contains("Drone") && int.TryParse(ReplaceSquareBrackets(drones[i + 1]), out capacity))
                {
                    dtuple.Add(ConvertToTuple(ReplaceSquareBrackets(drones[i]), capacity));
                }

            }


            string[][] locations = lines.Skip(1).Select(line => line.Split(',').Select(x => x.Trim()).ToArray()).ToArray();
            List<Tuple<string, int>> ltuple = new List<Tuple<string, int>>();
            //step 3: Get the location capacity
            for (int i = 0; i < locations.Length; i++)
            {
                int capacity = 0;
                if (locations[i][0].Contains("Location") && int.TryParse(ReplaceSquareBrackets(locations[i][1]), out capacity))
                {
                    ltuple.Add(ConvertToTuple(ReplaceSquareBrackets(locations[i][0]), capacity));
                }

            }

            // Step 4: Create a dictionary to track deliveries for each drone

            Dictionary<string, List<string>> deliveries = new Dictionary<string, List<string>>();

            // Step 5: Assign locations to drones
            ltuple.OrderByDescending(c => c.Item2).ToList().ForEach(y =>
            {
                for (int i = 0; i < dtuple.Count; i++)
                {
                    if (dtuple[i].Item2 >= y.Item2)
                    {
                        if (!deliveries.ContainsKey(dtuple[i].Item1))
                        {
                            deliveries.Add(dtuple[i].Item1, new List<string>());
                        }
                        deliveries[dtuple[i].Item1].Add(y.Item1);
                        dtuple[i] = Tuple.Create(dtuple[i].Item1, dtuple[i].Item2 - y.Item2);
                        break;
                    }
                }
            });

            // Step 6: Generate the output file
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (string drone in dtuple.Select(x => x.Item1))
                {
                    writer.WriteLine($"[{drone}]");
                    if (deliveries.ContainsKey(drone))
                    {

                        List<string> trips = deliveries[drone];
                        int vp = 0;
                        foreach (var item in trips)
                        {
                            writer.WriteLine($"Trip #{vp + 1}");
                            writer.WriteLine(string.Join(", ", "[" + item + "]"));
                            vp++;
                        }
                    }

                }
            }
        }
        //utils to remove square brackets
        static string ReplaceSquareBrackets(string input)
        {
            string pattern = @"\[|\]";
            string replacement = "";
            return Regex.Replace(input, pattern, replacement);
        }
        //utils to convert to tuple
        static Tuple<string, int> ConvertToTuple(string item, int capacity)
        {
            return Tuple.Create(ReplaceSquareBrackets(item), capacity);
        }


    }
}