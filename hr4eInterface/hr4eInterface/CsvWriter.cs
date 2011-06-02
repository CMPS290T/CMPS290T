using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace hr4eInterface
{
    class CsvWriter
    {
        FormData patientForm;
        public CsvWriter(FormData PatientForm)
        {
            patientForm = PatientForm;
        }

        public void UpdateCsv()
        {
            //string desktopPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string path  = Path.Combine(desktopPath, "hr4e\\data\\csv\\hr4e_patient_out.txt");

            string currentPath = Directory.GetCurrentDirectory();
            string path = currentPath + "/data/csv/hr4e_patient_out.txt";

            string current = "";

            // create reader & open file
            TextReader tr = new StreamReader(path);

            current = tr.ReadToEnd();

            // close the stream
            tr.Close();

            // create a writer and open the file
            TextWriter tw = new StreamWriter(path);

            tw.Write(current);

            string newPatientData = CreateString();
            // write a line of text to the file
            tw.WriteLine(newPatientData);

            // close the stream
            tw.Close();
        }

        private string CreateString()
        {
            string returnString = "";
            returnString += patientForm.patientData.firstName + ",<|>,";
            returnString += patientForm.patientData.lastName + ",<|>,";
            returnString += patientForm.clinicData.documentTimeStamp + ",<|>,";
            returnString += patientForm.patientData.id + ",<|>,";
            returnString += patientForm.patientData.presentVillage + ",<|>,";
            returnString += patientForm.patientData.genderCode + ",<|>,";
            returnString += patientForm.patientData.dateOfBirth + ",<|>,";
            returnString += patientForm.patientData.statedAge + ",<|>,";
            returnString += patientForm.patientData.estimatedAge + ",<|>,";
            returnString += patientForm.patientData.yearInSchool + ",<|>,";
            returnString += patientForm.patientData.statusInSchool + ",<|>,";
            returnString += patientForm.patientData.homeVillage + ",<|>,";
            foreach (String l in patientForm.patientData.Languages)
            {
                returnString += l + ",{|},";
            }
            returnString += "<|>,";

            foreach (Support s in patientForm.patientData.Supports)
            {
                returnString += s.firstName + ",";
                returnString += s.lastname + ",";
                returnString += s.schoolName;
                returnString += "{|},";
            }
            returnString += "<|>,";

            foreach (Provider p in patientForm.clinicData.providers)
            {
                returnString += p.firstName + ",";
                returnString += p.lastName + ",";
                returnString += "{|},";
            }
            returnString += "<|>,";

            foreach (LabResult r in patientForm.encounterData.labResults)
            {
                returnString += r.displayName + ",";
                returnString += r.value + " " + r.unit + ",";
                returnString += r.referenceRange + ",";
                returnString += "{|},";
            }
            returnString += "<|>,";

            foreach (Medication m in patientForm.encounterData.medications)
            {
                returnString += m.text;
                returnString += ",{|},";
            }
            returnString += "<|>,";

            foreach (VitalSign v in patientForm.encounterData.vitalSigns)
            {
                returnString += v.displayName + ",";
                returnString += v.value + " " + v.unit + ",";
                returnString += "{|},";
            }
            returnString += "<|>,";

            foreach (Condition d in patientForm.encounterData.conditions)
            {
                returnString += d.startYear + ",";
                returnString += d.problemName;
                returnString += ",{|},";
            }
            returnString += "<|>,";

            foreach (PlanOfCare c in patientForm.encounterData.planofCares)
            {
                returnString += c.displayName +",";
                returnString += c.text;
                returnString += ",{|},";
            }
            returnString += "<|>";

            return returnString;
        }
    }
}
