using BusinessLogic.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPB.BusinessLogic.Managers.Exceptions;

namespace UPB.BusinessLogic.Managers
{
    public class PatientManager
    {
        private List<Patient> _patients;
        private readonly IConfiguration _configuration;
        public PatientManager(IConfiguration configuration) { 
            _configuration = configuration;
            
            _patients = new List<Patient>();
            leerPatient();
        }

        public async Task<Patient> CrearPatient(Patient patient) {
            string sangre = randomBlood();
            Patient createdp;
            string codigo = await CreateCode(patient);

            createdp = new Patient(patient.Name, patient.LastName, patient.CI, sangre, codigo);
            _patients.Add(createdp);
            escribirPatient();
            Log.Information($"Se creo un paciente con CI: {patient.CI}");
            return createdp;
        }
        public Patient ActualizarPatient(int ci, Patient p_actualizado) {
            try
            {
                Patient patient = _patients.Find(x => x.CI == ci);
                if (patient == null)
                {
                    Log.Error("Se intentó actualizar un paciente que no existe en el método ActualizarPatient");
                    throw new PracticeException("El paciente fue null en ActualizarPatient");

                }
                patient.Name = p_actualizado.Name;
                patient.LastName = p_actualizado.LastName;
                patient.BloodType = p_actualizado.BloodType;
                escribirPatient();
                Log.Information($"Se actualizó la información del paciente con CI: {ci}");
                return patient;
            }
            catch (Exception ex)
            {
                
                Log.Error("Error en ActualizarPatient: " + ex.Message);
                Log.Error("Stacktrace de error en ActualizarPatient: " + ex.StackTrace);
                throw new PracticeException("Error al actualizar el paciente" +  ex);
            }

        }
        public List<Patient> GetPatients()
        {
            Log.Information("Alguien solicito a a todos los pacientes");
            return _patients;
        }
        public List<Patient> DeletePatients(int ci) {

            try
            {
                Patient patient = _patients.Find(x => x.CI == ci);
                if (patient == null)
                {
                    Log.Error("Se intentó borrar un paciente que no existe en el método DeletePatients");
                    throw new PracticeException("El paciente fue null en DeletePatients");
                }
                _patients.Remove(patient);
                escribirPatient();
                Log.Information($"Se borró al paciente con CI: {ci}");
                return _patients;
            }
            catch (Exception ex)
            {
                Log.Error("Error en DeletePatients: " + ex.Message);
                Log.Error("Stacktrace de error en DeletePatients: " + ex.StackTrace);
                throw new PracticeException("Error al borrar el paciente" + ex);
            }
        }
        public Patient GetPatientByCI(int ci)
        {
            try {
                Patient patient = _patients.Find(x => x.CI == ci);
                if (patient == null)
                {
                    Log.Error("Se busco un paciente que devolvio null en el metodo GetPatientByCI");
                    throw new PracticeException("El paciente fue null en GetPatientByCI");
                }
                Log.Information($"Alguien solicito al paciente con CI: {ci}");
                return patient;
            }
            catch (Exception ex)
            {
                Log.Error("Error en GetPatientByCI: " + ex.Message);
                Log.Error("Stacktrace de error en GetPatientByCI: " + ex.StackTrace);
                throw new PracticeException("Error al obtener paciente" + ex);
            }
            
        }
        private void leerPatient() {
            string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("textconnection").Value;

            if (string.IsNullOrEmpty(connectionString))
            {
                Log.Error("El valor de connectionString fue nulo o vacio en leerPatient");
                return;
            }

            StreamReader sr = new StreamReader(connectionString);
            _patients.Clear();
            while (!sr.EndOfStream) { 
                string line = sr.ReadLine();
                string[] info_pacientes = line.Split(",");

                Patient new_patient = new Patient()
                {
                    Name = info_pacientes[0],
                    LastName = info_pacientes[1],
                    CI = int.Parse(info_pacientes[2]),
                    BloodType = info_pacientes[3],
                    Codigo = info_pacientes[4]
                };
                _patients.Add(new_patient);
            }
            sr.Close();
        }

        private void escribirPatient() {
            string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("textconnection").Value;
            if (string.IsNullOrEmpty(connectionString))
            {
                Log.Error("El valor de connectionString fue nulo o vacio en escribirPatient");
                return;
            }
            StreamWriter writer = new StreamWriter(connectionString);
            foreach (var patient in _patients) {
                string[] patientInfo = new[] { patient.Name, patient.LastName, $"{patient.CI}", patient.BloodType, patient.Codigo};
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
        private async Task<string> CreateCode(Patient patient)
        {
            string dir_backserv = _configuration.GetSection("ConnectionStrings").GetSection("backingservice").Value;
            if (string.IsNullOrEmpty(dir_backserv))
            {
                Log.Error("El valor de la direccion del backing service fue nulo o vacio en CreateCode");
                return null;
            }

            var patientInfo = new { Name = patient.Name, LastName = patient.LastName, CI = patient.CI };
            var patientInfoJson = JsonConvert.SerializeObject(patientInfo);
            var contenido = new StringContent(patientInfoJson, System.Text.Encoding.UTF8, "application/json");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(dir_backserv);

                
                HttpResponseMessage responseMessage = await httpClient.PostAsync("api/PatientCode", contenido);

                
                if (responseMessage.IsSuccessStatusCode)
                {
                    
                    string responseContent = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                    return responseContent;
                }
                else
                {
                    
                    Console.WriteLine("Error al obtener el código del paciente.");
                    return null;
                }
            }
        }
    }
}
