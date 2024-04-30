using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPB.BusinessLogic.Managers
{
    internal class PatientManager
    {
        public PatientManager() { }

        public Patient CrearPatient(string name, string lastname, int CI) {
            string sangre = randomBlood();

            Patient patient = new Patient()
            {
                Name = name,
                LastName = lastname,
                CI = CI,
                BloodType = sangre
            };
            return patient;
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
