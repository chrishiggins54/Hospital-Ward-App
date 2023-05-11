using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA1
{
    public class Ward
    {
        public List<Patient> Patients { get; set; }
        public string WardName { get; set; }
        public double Capacity { get; set; }
        public static int NumberOfWards { get; set; }
        public int NumberOfPatients { get; set; }

        public Ward(string wardName, double capacity)
        {
            WardName = wardName;
            Capacity = capacity;
            Patients = new List<Patient>();
        }


        public override string ToString()
        {
            return string.Format("{0} Ward \t (Limit: {1})", WardName, Capacity);
        }

    }
}