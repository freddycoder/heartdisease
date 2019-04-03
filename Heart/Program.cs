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
            var data = HeartBeatSound.DeserialiseData(MIT_HEARTBEAT_DATASET, ',');

            while (data.Count > 10000)
            {
                data.RemoveAt(_randEngine.Next(data.Count - 1));
            }

            var testData = HeartBeatSound.DeserialiseData(MIT_HEARTBEAT_TEST_DATASET, ',');

            var agent = new Agent<HeartBeatSound>();

            int Nbsucces = 0;

            int bestNbSuccess = 0;

            int run = 0;

            int nbRunSinceLastSuccess = 0;

            int nbAgent = 1;

            while ((double)Nbsucces / data.Count < 0.99)
            {
                Nbsucces = 0;

                for (int i = 0; i < 1; i++)
                {
                    agent.TrainOnDatas(data);
                }

                foreach (var item in data)
                {
                    if ((agent.MakePrediction(item) > 0) == (item.Target >= 1))
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

                if (nbRunSinceLastSuccess >= 10)
                {
                    Random r = new Random();
                    if (r.Next() % 3 > 0)
                    {
                        agent = new Agent<HeartBeatSound>(agent);
                        Console.WriteLine($"New agent with transfert of knowledge");
                    }
                    else
                    {
                        agent = new Agent<HeartBeatSound>();
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
