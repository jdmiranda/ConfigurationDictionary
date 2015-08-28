using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurationReplacePrototype
{
    internal static class Program
    {

        private static void Main()
        {

            var configsList = new List<string>
            {
                "Name: Link, Level: 1, Hp: 20",
                "Name: Zelda, Level: 1, Hp: 20 ",
                "Name: Zelda, Hp: 30",
                "Name: Mario, Hp: 30"
            };
            var gamesDic = new Dictionary<string, Dictionary<string,string>>();

            foreach (var configs in configsList)
            {
                UpdateDictionary(configs, gamesDic);
            }

            foreach (var kvp in gamesDic.SelectMany(games => games.Value))
            {
                Console.WriteLine($"{kvp.Key}:{kvp.Value}");
            }

              Console.ReadKey();
        }

        private static void UpdateDictionary(string configs, IDictionary<string, Dictionary<string, string>> gamesDic)
        {
            var overRides = GetParameterList(configs);
            var resource = FindDictionaryName(overRides);
            if (!gamesDic.ContainsKey(resource)) CreateDictionary(configs, gamesDic); //create
            foreach (var kvp in from o in overRides where o != overRides.First() select Split(o)) //update
            {
                gamesDic[resource][kvp.First()] = kvp.Last();
            }
        }

        private static string[] Split(string o)
        {
            return o.Split(':');
        }

        private static string FindDictionaryName(IReadOnlyList<string> overRides)
        {
            return overRides[0].Split(':').Last();
        }

        private static string[] GetParameterList(string configs)
        {
            return configs.Split(',');
        }

        private static void CreateDictionary(string config, IDictionary<string, Dictionary<string, string>> gamesDic)
        {
            var gameConfigs = new Dictionary<string, string>();

            var shouldAdd = false;
            var configs = config.Split(',');
            foreach (var c in configs)
            {
                var options = c.Split(':');

                var key = options.First();
                var value = options.Last();
                if (gamesDic.ContainsKey(key)) continue;
                gameConfigs.Add(key, value);
                shouldAdd = true;
            }
            if (shouldAdd)
                gamesDic.Add(gameConfigs.First().Value, gameConfigs);
        }

    }
}
