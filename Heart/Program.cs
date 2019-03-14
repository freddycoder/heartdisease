using AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Heart
{
    class Program
    {
        /// <summary>
        /// This gonna read the next line of a file and determine is each column 
        /// has a matching property
        /// </summary>
        static bool EveryColumnHasIsProperty(StreamReader streamReader, Type type)
        {
            var columns = streamReader.ReadLine().Split(",");

            bool isCorrect = columns.Length == type.GetProperties().Length;

            for (int i = 0; i < columns.Length && isCorrect; i++)
            {
                isCorrect = columns.Contains(type.GetProperties().ElementAt(i).Name.ToLower());
            }

            return isCorrect;
        }

        static List<HealthInfo> DeserialiseData(string path)
        {
            StreamReader file = new StreamReader(path);

            var infos = new List<HealthInfo>();

            Debug.Assert(EveryColumnHasIsProperty(file, typeof(HealthInfo)));

            while (!file.EndOfStream)
            {
                infos.Add(new HealthInfo(file.ReadLine()));
            }

            return infos;
        }

        static void Shuffle(List<HealthInfo> healthInfos)
        {
            var rand = new Random();
            for (int i = 0; i < healthInfos.Count; i++)
            {
                int firstIndex = rand.Next() % healthInfos.Count;
                var info = healthInfos[firstIndex];
                int secondIndex = rand.Next() % healthInfos.Count;
                healthInfos[firstIndex] = healthInfos[secondIndex];
                healthInfos[secondIndex] = info;
            }
        }

        static void Main(string[] args)
        {
            var data = DeserialiseData("../../../heart.csv");

            var agent = new Agent<HealthInfo>();

            int Nbsucces = 0;

            int bestNbSuccess = 0;

            int run = 0;

            int nbRunSinceLastSuccess = 0;

            int nbAgent = 1;

            while (Nbsucces != data.Count)
            {
                for (int i = 0; i < 10; i++)
                {
                    Shuffle(data);
                    agent.AddData(data);
                }

                foreach (var item in data)
                {
                    if (agent.MakePrediction(item) == (item.Target == 1))
                    {
                        Nbsucces++;
                    }
                }

                if (Nbsucces >= bestNbSuccess)
                {
                    Console.WriteLine($"Last best result at : {DateTime.Now.ToString()}");
                    Console.WriteLine($"The agent made {Nbsucces} prediction succefully on a total of {data.Count} at run {run}.");

                    Console.WriteLine($"The Agent spec");
                    Console.WriteLine(agent.ToString());

                    bestNbSuccess = Nbsucces;

                    nbRunSinceLastSuccess = -1;
                }
                nbRunSinceLastSuccess++;

                Console.WriteLine($"New Agent at run {run}");

                run++;
                Nbsucces = 0;
            }

            Console.Write($"The tranning is complete after {run} run and {nbAgent} agents");
            Console.ReadLine();
        }
    }
}
