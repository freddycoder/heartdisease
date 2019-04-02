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
        static bool EveryColumnHasIsProperty(StreamReader streamReader, Type type)
        {
            var columns = streamReader.ReadLine().Split(";");

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

        static readonly Random _randEngine = new Random();

        static void Main(string[] args)
        {
            var data = DeserialiseData("../../../heart.csv");

            List<HealthInfo> testData = new List<HealthInfo>();

            for (int i = 0; i < 14; i++)
            {
                int index = _randEngine.Next(data.Count);

                testData.Add(data[index]);

                data.RemoveAt(index);
            }

            var agent = new Agent<HealthInfo>();

            int Nbsucces = 0;

            int bestNbSuccess = 0;

            int run = 0;

            int nbRunSinceLastSuccess = 0;

            int nbAgent = 1;

            while ((double)Nbsucces / data.Count < 0.84)
            {
                Nbsucces = 0;

                for (int i = 0; i < 1; i++)
                {
                    agent.TrainOnDatas(data);
                }

                foreach (var item in data)
                {
                    if ((agent.MakePrediction(item) > 0) == (item.Target == 1))
                    {
                        Nbsucces++;
                    }
                }

                if (Nbsucces > bestNbSuccess)
                {
                    Console.WriteLine($"Last best result at : {DateTime.Now.ToString()}");
                    Console.WriteLine($"The agent made {Nbsucces} prediction succefully on a total of {data.Count} at run {run}.");
                    Console.WriteLine($"This is accurate at {(double)Nbsucces / data.Count}");

                    Console.WriteLine($"The Agent spec");
                    Console.WriteLine(agent.ToString());

                    Console.WriteLine("Result");
                    Console.WriteLine($"{data[0].Header()} Prediction");
                    foreach (var item in data)
                    {
                        Console.WriteLine($"{item.ToString()} {agent.MakePrediction(item)}");
                    }

                    Console.WriteLine($"This is accurate at {(double)Nbsucces / data.Count}");

                    bestNbSuccess = Nbsucces;

                    nbRunSinceLastSuccess = -1;
                }
                nbRunSinceLastSuccess++;

                if (nbRunSinceLastSuccess >= 2500)
                {
                    Random r = new Random();
                    if (r.Next() % 3 > 0)
                    {
                        agent = new Agent<HealthInfo>(agent);
                        Console.WriteLine($"New agent with transfert of knowledge");
                    }
                    else
                    {
                        agent = new Agent<HealthInfo>();
                        Console.WriteLine($"New Agent at run {run}");
                    }
                    
                    
                    nbRunSinceLastSuccess = 0;
                    nbAgent++;
                }
                
                run++;
            }

            Console.WriteLine($"The tranning is complete after {run} run and {nbAgent} agents");
            int dataEvaluation = 0;
            foreach (var d in testData)
            {
                if ((agent.MakePrediction(d) > 0) == (d.Target > 0))
                {
                    dataEvaluation++;
                }
            }
            Console.WriteLine($"Final score is {(double)dataEvaluation/testData.Count} or {dataEvaluation} / {testData.Count}");
            Console.ReadLine();
        }
    }
}
