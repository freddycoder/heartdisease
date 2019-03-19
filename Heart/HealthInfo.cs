using AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Heart
{
    public class HealthInfo : IData
    {
        public HealthInfo() { }
        public HealthInfo(string textLine)
        {
            var text = textLine.Split(",");

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
            var reflectionOfMyslf = GetType().GetProperties();

            foreach (var prop in reflectionOfMyslf)
            {
                sb.Append(prop.Name + ": " + prop.GetValue(this).ToString() + " | ");
            }

            return sb.ToString();
        }
    }
}
