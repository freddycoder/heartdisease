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

        static void Print(object thing = null, bool endl = true)
        {
            if (thing != null) Console.Out.Write(thing.ToString());
            if (endl) Console.Out.WriteLine();
        }

        static void Main(string[] args)
        {
            var data = DeserialiseData("../../../heart.csv");

            int nbGoodPrediction = 0;

            data = new List<HealthInfo>()
            {
                data.First(d => d.Target == 0),
                data.First(d => d.Target == 1)
            };

            var agent = new Agent<HealthInfo>(data);

            int nbLoop = 0;

            double lastBestScore = 0;

            while (nbGoodPrediction != data.Count)
            {
                int goodAnswer = 0;
                foreach (var d in data)
                {
                    if (agent.Predict(d) == d.Target > 0)
                    {
                        goodAnswer++;
                    }
                }

                if (lastBestScore < (double)goodAnswer / data.Count)
                {
                    Print("The agent beat is score at " + DateTime.Now.ToString());

                    Print("The score of the agent is : " + (double)goodAnswer / data.Count);

                    Print(agent);

                    Print();

                    lastBestScore = (double)goodAnswer / data.Count;
                }

                agent.ReceiveReward(new AI.Reinforcement.Reward(goodAnswer, data.Count));

                agent.Train();

                nbGoodPrediction = goodAnswer;

                nbLoop++;

                if (nbLoop % 250 == 0 || true)
                {
                    Print("The agent state at " + DateTime.Now.ToString());

                    Print("The score of the agent is : " + (double)goodAnswer / data.Count);

                    Print(agent);

                    Print();
                }

                goodAnswer = 0;
            }

            Print("The agent succed! After " + nbLoop);

            Console.ReadLine();
        }
    }
}
