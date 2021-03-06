﻿using AI;
using AI.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Heart
{
    public class HealthInfo : IDataModel<int>
    {
        public HealthInfo(string textLine, char separator)
        {
            var text = textLine.Split(separator);

            Debug.Assert(text.Length == this.GetType().GetProperties().Length);

            Age = int.Parse(text[0]);
            Sex = int.Parse(text[1]);
            Cp = int.Parse(text[2]);
            Trestbps = int.Parse(text[3]);
            Chol = int.Parse(text[4]);
            Fbs = int.Parse(text[5]);
            Restecg = int.Parse(text[6]);
            Thalach = int.Parse(text[7]);
            Exang = int.Parse(text[8]);
            Oldpeak = double.Parse(text[9].Replace(".", ","));
            Slope = int.Parse(text[10]);
            Ca = int.Parse(text[11]);
            Thal = int.Parse(text[12]);
            Target = int.Parse(text[13]);
        }

        public int Age { get; set; }
        public int Sex { get; set; }
        public int Cp { get; set; }
        public int Trestbps { get; set; }
        public int Chol { get; set; }
        public int Fbs { get; set; }
        public int Restecg { get; set; }
        public int Thalach { get; set; }
        public int Exang { get; set; }
        public double Oldpeak { get; set; }
        public int Slope { get; set; }
        public int Ca { get; set; }
        public int Thal { get; set; }
        public int Target { get; set; }

        public override bool Equals(object obj)
        {
            var info = obj as HealthInfo;
            return info != null &&
                   Age == info.Age &&
                   Sex == info.Sex &&
                   Cp == info.Cp &&
                   Trestbps == info.Trestbps &&
                   Chol == info.Chol &&
                   Fbs == info.Fbs &&
                   Restecg == info.Restecg &&
                   Thalach == info.Thalach &&
                   Exang == info.Exang &&
                   Oldpeak == info.Oldpeak &&
                   Slope == info.Slope &&
                   Ca == info.Ca &&
                   Thal == info.Thal &&
                   Target == info.Target;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Age);
            hash.Add(Sex);
            hash.Add(Cp);
            hash.Add(Trestbps);
            hash.Add(Chol);
            hash.Add(Fbs);
            hash.Add(Restecg);
            hash.Add(Thalach);
            hash.Add(Exang);
            hash.Add(Oldpeak);
            hash.Add(Slope);
            hash.Add(Ca);
            hash.Add(Thal);
            hash.Add(Target);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var @this = GetType().GetProperties();

            foreach (var prop in @this)
            {
                if (prop.Name == "Target")
                {
                    int val = (int)prop.GetValue(this);
                    if (val == 0) sb.Append("False\t");
                    else sb.Append("True\t");
                }
                else
                {
                    sb.Append($"{prop.GetValue(this).ToString()}\t");
                }

                if (prop.Name == nameof(Trestbps)) sb.Append("\t");
            }

            return sb.ToString();
        }

        public string Header()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var prop in GetType().GetProperties())
            {
                sb.Append($"{prop.Name}\t");
            }

            return sb.ToString();
        }

        public static List<HealthInfo> DeserialiseData(string path, char separator = ',')
        {
            StreamReader file = new StreamReader(path);

            var infos = new List<HealthInfo>();

            Debug.Assert(EveryColumnHasIsProperty(file, typeof(HealthInfo), separator));

            while (!file.EndOfStream)
            {
                infos.Add(new HealthInfo(file.ReadLine(), separator));
            }

            return infos;
        }
        static bool EveryColumnHasIsProperty(StreamReader streamReader, Type type, char separator)
        {
            var columns = streamReader.ReadLine().Split(separator);

            bool isCorrect = columns.Length == type.GetProperties().Length;

            for (int i = 0; i < columns.Length && isCorrect; i++)
            {
                isCorrect = columns.Contains(type.GetProperties().ElementAt(i).Name.ToLower());
            }

            return isCorrect;
        }

        public Matrix GetFeatures()
        {
            var props = GetType().GetProperties().Where(p => p.Name != "Target").ToArray();
            Matrix m = new Matrix(1, props.Length);

            for (int i = 0; i < m.ColumnsCount; i++)
            {
                m[0][i] = double.Parse(props[i].GetValue(this).ToString());
            }

            return m;
        }
    }
}
