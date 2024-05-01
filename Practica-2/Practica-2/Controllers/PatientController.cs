using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPB.BusinessLogic.Managers;


namespace Practica_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientManager _patientManager;
        public PatientController() {
            _patientManager = new PatientManager();
        }

        [HttpGet]
        public List<Patient> Get() {
            return _patientManager.GetPatients();
        }

        [HttpGet]
        [Route("{ci}")]
        public Patient Get(int ci) { 
            return _patientManager.GetPatientByCI(ci);
        }

        [HttpPost]
        public void Post([FromBody] Patient value) {
            _patientManager.CrearPatient(value);
        }
        
        [HttpPut("{ci}")]
        public void Put(int ci, [FromBody] Patient value) {
            _patientManager.ActualizarPatient(ci, value);
        }
        
        [HttpDelete("{ci}")]
        public void Delete(int ci)
        {
            _patientManager.DeletePatients(ci);
        }
        
        
    }
}
