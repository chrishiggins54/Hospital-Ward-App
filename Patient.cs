using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA1
{
    public enum BloodType
    {
        A,
        B,
        AB,
        O,
    }

    public class Patient
    {
        public string PatientName { get; set; }

        public DateTime DOB { get; set; }

        public int Age { get; set; }

        public BloodType BloodType { get; set; }

        public Patient(string patientName, int age, BloodType bloodType)
        {
            PatientName = patientName;
            Age = age;
            BloodType = bloodType;
        }


        public Patient()
        {

        }


        public override string ToString()
        {
            return string.Format("{0} ({1}) Type: {2}", PatientName, Age, BloodType);
        }

    }
}