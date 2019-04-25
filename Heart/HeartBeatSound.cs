using AI;
using AI.Mathematics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Heart
{
    public class HeartBeatSound : IDataModel<int>
    {
        private LinkedList<double> _soundBytes;

        public HeartBeatSound(string line, char separator)
        {
            _soundBytes = new LinkedList<double>();

            string[] columns = line.Split(separator);

            for (int i = 0; i < columns.Length; i++)
            {
                _soundBytes.AddLast(double.Parse(columns[i].Replace(".", ",")));
            }
        }
        public double[] SoundBytes
        {
            get
            {
                return _soundBytes.SkipLast(1).ToArray();
            }
            set
            {
                _soundBytes = new LinkedList<double>(value);
            }
        }
        public void Add(double value)
        {
            _soundBytes.AddLast(value);
        }

        public int Target
        {
            get
            {
                return (int)_soundBytes.Last();
            }
            set
            {
                _soundBytes.RemoveLast();
                Add(value);
            }
        }
        public static List<HeartBeatSound> DeserialiseData(string path, char separator)
        {
            StreamReader sr = new StreamReader(path);

            LinkedList<HeartBeatSound> data = new LinkedList<HeartBeatSound>();

            while (sr.EndOfStream == false)
            {
                data.AddLast(new HeartBeatSound(sr.ReadLine(), separator));
            }

            return data.ToList();
        }

        public string Header()
        {
            return "Actual\t";
        }

        public override string ToString()
        {
            return Target.ToString();
        }

        public Matrix GetFeatures()
        {
            throw new System.NotImplementedException();
        }
    }
}
