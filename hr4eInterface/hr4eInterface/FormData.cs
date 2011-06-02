using System;
using System.Collections.Generic;
using System.Text;


namespace hr4eInterface
{
    #region structs
    public struct PatientData
    {
        #region patientInfo
        public string @id;

        #region address
        public string @streetNull;
        public string presentVillage; // labeled as city
        public string @stateNull;
        public string @postalCodeNull;
        #endregion

        public string @telecomNull;

        #region personInformation
        #region Name
        public string firstName; // labeled as given
        public string lastName;  // labeled as family
        #endregion

        public string @genderCodeSystem;
        public string @genderCode;

        #region hr4eAgeInformation
        public string @dateOfBirth;
        public string statedAge;
        public string estimatedAge;
        public string yearInSchool;
        public string statusInSchool;
        #endregion
        #endregion

        #region hr4eLiving Situation
        public string homeVillage;
        public string tribe;
        #endregion
        #endregion

        public List<String> Languages;
        public List<Support> Supports;
    }

    public struct EncounterData
    {
        public List<LabResult> labResults;
        public Encounter encounter;
        public List<Medication> medications;
        public List<Immunization> immunizations;
        public List<VitalSign> vitalSigns;
        public List<Condition> conditions;
        public List<PlanOfCare> planofCares;
        
    }

    public struct ClinicInstanceData
    {
        public string @xmlns;
        public string @xmlns_xsi;
        public string @xmlns_hr4e;
        public string @xsi_schemaLocation;

        public string docID;
        public string title;
        public string versionNumber;
        public string @confidentialityCodeSystem;
        public string @confidentialityCode;
        public string @documentTimeStamp;

        #region custodian
        public string custodianID;
        public string custodianName;
        #endregion

        #region ClinicDates
        public string @clinicStartDate;
        public string @clinicEndDate;
        #endregion

        public List<Provider> providers;

        public Author author;

        #region clinic data
        public string gpsCoordinates;
        public string clinicName;
        public string clinicDetails;
        #endregion
    }

    public struct Author
    {
        public string @authorTimeStamp;
        public string authorNamePrefix;
        public string authorNameLast;
        public string authorNameFirst;
    }

    public struct Condition
    {
        public string @startYear;
        public string problemName;
        public string @code;
        public string @codeSystem;
        public string @displayName;
        public string comment;
    }

    public struct PlanOfCare
    {
        public string @id;
        public string @codeSystem;
        public string @code;
        public string @displayName;
        public string text;
    }

    public struct VitalSign
    {
        public string @id;
        public string @timeStamp;
        public string @codeSystem;
        public string @code;
        public string @displayName;
        public string @status;
        public string @value;
        public string @unit;
        public string comment;
    }

    public struct Immunization
    {
        public string @dateAdministered;
        public string @codeSystem;
        public string @code;
        public string @displayName;
        public string text;
        public string comment;
    }

    public struct Medication
    {
        public string @codeSystem;
        public string @code;
        public string @codeSystemName;
        public string @displayName;
        public string text;
    }

    public struct Encounter
    {
        public string @id;
        public string @codeSystem;
        public string @code;
        public string textDescription;
        public string @timeStamp;
        public string notes;
        public string reason;
        public string @reasonCode;
    }

    public struct LabResult
    {
        public string @id;
        public string @timeStamp;
        public string @typeCodeSystem;
        public string @typeCode;
        public string @displayName;
        public string @status;
        public string @value;
        public string @unit;
        public string @interpretCodeSystem;
        public string @interpretCode;
        public string referenceRange;
        public string comment;
    }

    public struct Provider
    {
        #region role
        public string @roleCode;
        public string @roleCodeSystem;
        public string roleDescription;
        #endregion

        #region participation dates
        public string @dateStart;
        public string @dateEnd;
        #endregion

        public string @id;

        #region name
        public string prefix;
        public string firstName;
        public string lastName;
        #endregion

        public string organizationName;

        public string comment;
    }

    public struct Support
    {
        public string @dateNull;

        #region Contact

        #region address
        public string @streetNull;
        public string city;
        public string @stateNull;
        public string @postalCodeNull;
        #endregion

        public string @telecomNull;

        #region name
        public string firstName; //given
        public string lastname; //family
        #endregion

        public string schoolName;

        #endregion

        public string comment;
    }
    #endregion

    class FormData
    {
        public PatientData patientData;
        public EncounterData encounterData;
        public ClinicInstanceData clinicData;

        public FormData()
        {
            patientData = new PatientData();
            encounterData = new EncounterData();
            clinicData = new ClinicInstanceData();
            patientData.Languages = new List<string>();
            patientData.Supports = new List<Support>();
            encounterData.labResults = new List<LabResult>();
            encounterData.medications = new List<Medication>();
            encounterData.immunizations = new List<Immunization>();
            encounterData.vitalSigns = new List<VitalSign>();
            encounterData.conditions = new List<Condition>();
            encounterData.planofCares = new List<PlanOfCare>();
            clinicData.providers = new List<Provider>();
        }

        public void ResetForm()
        {
            patientData = new PatientData();
            encounterData = new EncounterData();
            clinicData = new ClinicInstanceData();
            patientData.Languages = new List<string>();
            patientData.Supports = new List<Support>();
            encounterData.labResults = new List<LabResult>();
            encounterData.medications = new List<Medication>();
            encounterData.immunizations = new List<Immunization>();
            encounterData.vitalSigns = new List<VitalSign>();
            encounterData.conditions = new List<Condition>();
            encounterData.planofCares = new List<PlanOfCare>();
            clinicData.providers = new List<Provider>();
        }

        public void LoadForm()
        {

        }

        public string GetUUID()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }

        public string GetTimeStamp()
        {
            DateTime d = DateTime.Now;
            string day = d.Day.ToString();
            if (day.Length == 1) day = '0' + day;
            string month = d.Month.ToString();
            if (month.Length == 1) month = '0' + month;
            string hour = d.Hour.ToString();
            if (hour.Length == 1) hour = '0' + hour;
            string minute = d.Minute.ToString();
            if (minute.Length == 1) minute = '0' + minute;
            string second = d.Second.ToString();
            if (second.Length == 1) second = '0' + second;
            string formattedDate = d.Year.ToString() + month + day + hour + second;
            return formattedDate;
        }      
    }
}
