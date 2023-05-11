using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace CA1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Ward> ward = new ObservableCollection<Ward>();
        public ObservableCollection<Patient> patient = new ObservableCollection<Patient>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {   //disable buttons until ward/patient names entered
            btnAddWard.IsEnabled = false;
            btnAddPatient.IsEnabled = false;

            //setting slider text starts at 0
            tblkCapacity.Text = "0";

            Ward w1 = new Ward("Marx Brothers", 3);
            Ward w2 = new Ward("Planet Express", 6);

            ward.Add(w1);
            ward.Add(w2);

            Patient p1 = new Patient("Chico", 32, BloodType.A);
            Patient p2 = new Patient("Graucho", 57, BloodType.AB);
            Patient p3 = new Patient("Harpo", 46, BloodType.B);

            w1.Patients.Add(p1);
            w1.Patients.Add(p2);
            w1.Patients.Add(p3);

            //increase number of patients in ward for each Patient object
            foreach (Patient patient in w1.Patients)
            {
                w1.NumberOfPatients++;
            }

            Patient p4 = new Patient("Fry", 47, BloodType.O);
            Patient p5 = new Patient("Leela", 46, BloodType.AB);
            Patient p6 = new Patient("Professor", 181, BloodType.B);
            Patient p7 = new Patient("Bender", 26, BloodType.O);

            w2.Patients.Add(p4);
            w2.Patients.Add(p5);
            w2.Patients.Add(p6);
            w2.Patients.Add(p7);

            //increase number of patients in ward for each Patient object
            foreach (Patient patient in w2.Patients)
            {
                w2.NumberOfPatients++;
            }

            //display on screen
            lbxWards.ItemsSource = ward;

            //begin ward list count at 2
            foreach (Ward ward in ward)
            {
                Ward.NumberOfWards++;
            }

            int totalWards = Ward.NumberOfWards;
            tblkNumberOfWards.Text = string.Format("({0})", totalWards);
        }

        //to show the capacity chosen in numbers when adjusting slider
        private void sliderCapacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tblkCapacity.Text = string.Format("{0:F0}", sliderCapacity.Value);
        }

        //setting source of patients listbox to the patients of selected ward in ward listbox
        private void lbxWards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ward selectedWard = lbxWards.SelectedItem as Ward;

            if (selectedWard != null)
            {
                lbxPatients.ItemsSource = selectedWard.Patients;
            }
        }

        private void btnAddWard_Click(object sender, RoutedEventArgs e)
        {
            //read data from screen
            string wardName = tbxWardName.Text;
            double capacity = sliderCapacity.Value;

            //create ward object
            Ward newWard = new Ward(wardName, capacity);

            //add to collection of wards
            ward.Add(newWard);

            //increase number of wards
            Ward.NumberOfWards++;
            int totalWards = Ward.NumberOfWards;

            //update number of wards on ward list
            tblkNumberOfWards.Text = string.Format("({0})", totalWards);

            ClearScreen();
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)

        {
            Ward selectedWard = lbxWards.SelectedItem as Ward;

            //show error if DOB not selected
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Please enter date of birth", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

            }

            //show error if no ward selected
            else if (lbxWards.SelectedItem == null)
            {
                MessageBox.Show("Please select ward to add patient to", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //show error if ward capacity reached
            else if (selectedWard.NumberOfPatients >= selectedWard.Capacity)
            {
                MessageBox.Show("Cannot add patient - ward is at capacity", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            //else allow patient to be added

            else

            {
                AddPatient();
            }
        }

        //enable add ward and patient buttons when text is entered
        private void tbxWardName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnAddWard.IsEnabled = !string.IsNullOrEmpty(tbxWardName.Text);

        }

        private void tbxPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {

            btnAddPatient.IsEnabled = !string.IsNullOrEmpty(tbxPatientName.Text);
        }

        //to show blood type in details section
        private void lbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient selectedPatient = new Patient();
            selectedPatient = (Patient)lbxPatients.SelectedItem;

            //if else on blood 
            if (selectedPatient != null)
            {
                if (selectedPatient.BloodType == BloodType.A)
                {
                    imgBloodType.Source = new BitmapImage(new Uri("\\Images\\Picture1.png", UriKind.Relative));
                }

                else if (selectedPatient.BloodType == BloodType.AB)
                {
                    imgBloodType.Source = new BitmapImage(new Uri("\\Images\\Picture2.png", UriKind.Relative));
                }

                else if (selectedPatient.BloodType == BloodType.B)
                {
                    imgBloodType.Source = new BitmapImage(new Uri("\\Images\\Picture3.png", UriKind.Relative));
                }

                else if (selectedPatient.BloodType == BloodType.O)
                {
                    imgBloodType.Source = new BitmapImage(new Uri("\\Images\\Picture4.png", UriKind.Relative));
                }
            }
        }

        //method to add new patient
        private void AddPatient()
        {
            string patientName = tbxPatientName.Text;

            DateTime dob = dpDate.SelectedDate.Value;

            //calculate age
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;

            if (dob > today.AddYears(-age))
            {
                age--;
            }

            //assign blood types to radio buttons

            BloodType bloodType = 0;

            if (rbA.IsChecked.Value == true)
                bloodType = BloodType.A;

            else if (rbB.IsChecked.Value == true)
                bloodType = BloodType.B;

            else if (rbAB.IsChecked.Value == true)
                bloodType = BloodType.AB;

            else if (rbO.IsChecked.Value == true)
                bloodType = BloodType.O;

            //create patient object
            Patient newPatient = new Patient(patientName, age, bloodType);

            //when a ward is selected, add the new patient to that ward
            Ward selectedWard = lbxWards.SelectedItem as Ward;

            //add to collection of patients
            selectedWard.Patients.Add(newPatient);
            lbxPatients.ItemsSource = patient;
            lbxPatients.ItemsSource = selectedWard.Patients;

            //increase number of patients in the ward
            selectedWard.NumberOfPatients++;

            ClearScreen();
        }

        //resets values after entry
        private void ClearScreen()
        {
            tbxWardName.Clear();
            tbxPatientName.Clear();
            dpDate.SelectedDate = null;
            sliderCapacity.Value = 0;
        }

        //save expense objects to JSON
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //get string of objects - JSON formatted
            string json = JsonConvert.SerializeObject(ward, Formatting.Indented);

            //write that to file

            using (StreamWriter sw = new StreamWriter(@"c:\temp\patients.json"))
            {
                sw.Write(json);
            }
        }

        //load JSON file
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            // connect to a file
            using (StreamReader sr = new StreamReader(@"c:\temp\patients.json"))
            {
                // read text
                string json = sr.ReadToEnd();

                // convert from JSON to objects
                ward = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);

                // reset number of wards to 0 to avoid counting duplicates in JSON file
                Ward.NumberOfWards = 0;

                // iterate over each ward to update its patient count and the total ward count
                foreach (Ward ward in ward)
                {
                    // reset number of patients
                    ward.NumberOfPatients = ward.Patients.Count;

                    // increase the number of wards
                    Ward.NumberOfWards++;
                }

                // update ward count
                int totalWards = Ward.NumberOfWards;
                tblkNumberOfWards.Text = string.Format("({0})", totalWards);

                // refresh display
                lbxWards.ItemsSource = ward;

                // Clear patient list
                lbxPatients.ItemsSource = null;
            }
        }

    }

}
