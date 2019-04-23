using AI;
using System;
using System.Collections.Generic;

namespace Heart
{
    class Program
    {
        static readonly Random _randEngine = new Random();
        static readonly string HEART_DATASET = "../../../Dataset/heart.csv";
        static readonly string MIT_HEARTBEAT_DATASET = "../../../Dataset/mitbih_train.csv";
        static readonly string MIT_HEARTBEAT_TEST_DATASET = "../../../Dataset/mitbih_test.csv";

        static void Main(string[] args)
        {
            var data = HealthInfo.DeserialiseData(HEART_DATASET, ';');

            var testData = new List<HealthInfo>();

            while (testData.Count < 25)
            {
                int index = _randEngine.Next(data.Count - 1);

                testData.Add(data[index]);

                data.RemoveAt(index);
            }

            var agent = new Boost<HealthInfo>();

            agent.Add(new Agent<HealthInfo>(100));

            int Nbsucces = 0;

            int bestNbSuccess = 0;

            int run = 0;

            int nbRunSinceLastSuccess = 0;

            int nbAgent = 1;

            while ((double)Nbsucces / data.Count < 0.85)
            {
                Nbsucces = 0;

                for (int i = 0; i < 1; i++)
                {
                    agent.Fit(data);
                }

                foreach (var item in data)
                {
                    if ((agent.MakePrediction(item) > 0) == (item.Target > 0))
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
                    int lineToBePrintedCount = 303;
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (lineToBePrintedCount > 0 && (_randEngine.Next() % 303 == 0 || data.Count - i < lineToBePrintedCount))
                        {
                            Console.WriteLine($"{data[i].ToString()} {agent.MakePrediction(data[i])}");
                            lineToBePrintedCount--;
                        }
                    }

                    Console.WriteLine($"This is accurate at {(double)Nbsucces / data.Count}");

                    bestNbSuccess = Nbsucces;

                    nbRunSinceLastSuccess = -1;
                }
                nbRunSinceLastSuccess++;

                if (nbRunSinceLastSuccess >= 500)
                {
                    if (_randEngine.Next() % 2 == 0)
                    {
                        agent = new Boost<HealthInfo>();
                        agent.Add(new Agent<HealthInfo>(100));
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
