using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPB.BusinessLogic.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients;
        public PatientManager() { 
            _patients = new List<Patient>();
            leerPatient();
        }

        public Patient CrearPatient(Patient patient) {
            string sangre = randomBlood();
            Patient createdp;
            createdp = new Patient(patient.Name, patient.LastName, patient.CI, sangre);
            _patients.Add(createdp);
            escribirPatient();
            return createdp;
        }
        public Patient ActualizarPatient(int ci, Patient p_actualizado) {
            Patient patient = _patients.Find(x => x.CI == ci);

            if (patient == null)
            {
                throw new NotImplementedException();
            }
            patient.Name = p_actualizado.Name;
            patient.LastName = p_actualizado.LastName;
            patient.BloodType = p_actualizado.BloodType;
            escribirPatient();
            return patient;
        }
        public List<Patient> GetPatients()
        {
            return _patients;
        }
        public List<Patient> DeletePatients(int ci) {
            Patient patient = _patients.Find(x => x.CI == ci);
            if (patient == null)
            {
                throw new NotImplementedException();
            }
            _patients.Remove(patient);
            escribirPatient();
            return _patients;
        }
        public Patient GetPatientByCI(int ci)
        {
            Patient patient = _patients.Find(x => x.CI == ci);
            if (patient == null) { 
                throw new NotImplementedException();
            }
            return patient;
        }
        private void leerPatient() { 
            StreamReader sr = new StreamReader("D:\\Git\\practicaDos\\Practica-2\\Patients.txt");
            _patients.Clear();
            while (!sr.EndOfStream) { 
                string line = sr.ReadLine();
                string[] info_pacientes = line.Split(",");

                Patient new_patient = new Patient()
                {
                    Name = info_pacientes[0],
                    LastName = info_pacientes[1],
                    CI = int.Parse(info_pacientes[2]),
                    BloodType = info_pacientes[3]
                };
                _patients.Add(new_patient);
            }
            sr.Close();
        }

        private void escribirPatient() {
            StreamWriter writer = new StreamWriter("D:\\Git\\practicaDos\\Practica-2\\Patients.txt");
            foreach (var patient in _patients) {
                string[] patientInfo = new[] { patient.Name, patient.LastName, $"{patient.CI}", patient.BloodType };
                writer.WriteLine(string.Join(",", patientInfo));
            }
            writer.Close();
        }
        public string randomBlood() { 
            Random rnd = new Random();
            int random = rnd.Next(1, 81);
            if (random < 10) {
                return "A+";
            }
            if (random < 20) {
                return "A-";
            }
            if (random < 30) {
                return "B+";
            }
            if (random < 40) {
                return "B-";
            }
            if (random < 50) {
                return "AB+";
            }
            if (random < 60)
            {
                return "AB-";
            }
            if (random < 70)
            {
                return "O+";
            }
            if (random < 80)
            {
                return "O-";
            }
            return "O+";

        }
    }
}
