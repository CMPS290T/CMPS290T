using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace hr4eInterface
{
    class XMLReader
    {
        Form1 mainForm;
        FormData patientForm;
        ClinicData clinicData;

        string loadCurrentPatientDataPath;
        string loadClinicSettingsPath;
        string loadPatientTemplatePath;

        string pathType;

        public XMLReader(Form1 MainForm, FormData PatientForm, ClinicData Clinic)
        {
            pathType = "linux";//by default set path to windows, change to "osx" for mac, "linux" for linux based systems
            patientForm = PatientForm;
            mainForm = MainForm;
            clinicData = Clinic;

            Initializer();
            
        }

        private void Initializer()
        {
            SetStoragePaths();
            //ReadXML();
            //WriteXML();
        }

        public void SetStorageSystem(string newSystem)
        {
            if (newSystem == "windows" || newSystem == "osx" || newSystem == "linux")
            {
                pathType = newSystem;
                SetStoragePaths();
            }
        }

        private void SetStoragePaths()
        {

            switch (pathType)
            {
                case "windows":
                    string desktopPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    loadCurrentPatientDataPath = Path.Combine(desktopPath, "current_patient.xml");
                    loadClinicSettingsPath = Path.Combine(desktopPath, "hr4e_clinic_settings.xml");
                    loadPatientTemplatePath = Path.Combine(desktopPath, "hr4e_patient_template.xml");
                    break;
                case "osx":
                    break;
                case "linux":
                    string currentDirectory = Directory.GetCurrentDirectory();
                    string rootDirectory = "/home/phil/hr4e/";
                    //string usbDirectory = "/media/SUPER/";
                    //string rootDirectory = "/home/ben/hr4e/";
                    //string usbDirectory = "/media/hr4e/";
                    //Directory.SetCurrentDirectory(usbDirectory);
                    //Handle all of the USB File Paths:
                    loadCurrentPatientDataPath = "/hr4e/data/patient_data/current_patient_data.xml";

                    //Handle all of the local file paths:
                    Directory.SetCurrentDirectory(rootDirectory);
				    currentDirectory = Directory.GetCurrentDirectory();
                    loadClinicSettingsPath = currentDirectory + "/templates/hr4e_clinic_settings.xml";
                    loadPatientTemplatePath = currentDirectory + "/templates/hr4e_patient_template.xml";
                    break;
            }
        }

        public void LoadClinicSettings()
        {
            // Read a document
            XmlTextReader textReader = new XmlTextReader(loadClinicSettingsPath);
            textReader.WhitespaceHandling = WhitespaceHandling.None;

            textReader.Read();
            // If the node has value


            while (textReader.Read())
            {
                if (textReader.NodeType.Equals(XmlNodeType.Element))
                {
                    switch (textReader.LocalName)
                    {
                        case "stationName":
                            textReader.Read();
                            clinicData.stationName = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "clinicData":
                            LoadSettingsClinicData(textReader);
                            break;
                        case "formData":
                            LoadSettingsFormData(textReader);
                            break;
                        case "Languages":
                            #region load languages
                            bool inLanguages = true;
                            while (inLanguages)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "Languages":
                                            inLanguages = false;
                                            break;
                                        case "language":
                                            textReader.Read();
                                            textReader.Read();
                                            string langname = textReader.Value.ToString();
                                            textReader.Read();
                                            textReader.Read();
                                            textReader.Read();
                                            string langcode = textReader.Value.ToString();
                                            textReader.Read();
                                            textReader.Read();
                                            clinicData.AddLanguageInfo(langname, langcode);
                                            break;
                                    }
                                }
                            }
                            #endregion end load languages
                            break;
                        case "schoolStatus":
                            #region load school status list
                            bool inSchoolStatus = true;
                            while (inSchoolStatus)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "schoolStatus":
                                            inSchoolStatus = false;
                                            break;
                                        case "status":
                                            textReader.Read();
                                            string status = textReader.Value.ToString();
                                            textReader.Read();
                                            clinicData.AddSchoolStatus(status);
                                            break;
                                    }
                                }
                            }
                            #endregion end load school status list
                            break;
                        case "yearInSchool":
                            #region get years in school
                            textReader.Read();
                            textReader.Read();
                            clinicData.minYearInSchool = textReader.Value.ToString();
                            textReader.Read();
                            textReader.Read();
                            textReader.Read();
                            clinicData.maxYearInSchool = textReader.Value.ToString();
                            textReader.Read();
                            textReader.Read();
                            #endregion end get school year range
                            break;
                        case "tribes":
                            #region load tribes list
                            bool inTribes = true;
                            while (inTribes)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "tribes":
                                            inTribes = false;
                                            break;
                                        case "tribe":
                                            textReader.Read();
                                            string tribe = textReader.Value.ToString();
                                            textReader.Read();
                                            clinicData.AddTribes(tribe);
                                            break;
                                    }
                                }
                            }
                            #endregion end load tribes list
                            break;
                        case "villages":
                            #region load villages list
                            bool inVillages = true;
                            while (inVillages)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "villages":
                                            inVillages = false;
                                            break;
                                        case "village":
                                            textReader.Read();
                                            string village = textReader.Value.ToString();
                                            textReader.Read();
                                            clinicData.AddVillages(village);
                                            break;
                                    }
                                }
                            }
                            #endregion end load villages list
                            break;
                        case "schools":
                            #region load schools
                            bool inSchools = true;
                            while (inSchools)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "schools":
                                            inSchools = false;
                                            break;
                                        case "school":
                                            textReader.Read();
                                            textReader.Read();
                                            string schoolname = textReader.Value.ToString();
                                            textReader.Read();
                                            textReader.Read();
                                            textReader.Read();
                                            string schoolcity = textReader.Value.ToString();
                                            textReader.Read();
                                            textReader.Read();
                                            clinicData.AddSchoolData(schoolname, schoolcity);
                                            break;
                                    }
                                }
                            }
                            #endregion end load schools
                            break;
                        case "supports":
                            #region load supports
                            bool inSupports = true;
                            while (inSupports)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "supports":
                                            inSupports = false;
                                            break;
                                        case "support":
                                            bool inSupport = true;
                                            string fname = "NI";
                                            string lname = "NI";
                                            string school = "NI";
                                            string city = "NI";
                                            string comment = "NI";
                                            while (inSupport)
                                            {
                                                textReader.Read();
                                                {
                                                    switch (textReader.LocalName)
                                                    {
                                                        case "support":
                                                            inSupport = false;
                                                            break;
                                                        case "firstName":
                                                            textReader.Read();
                                                            fname = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "lastName":
                                                            textReader.Read();
                                                            lname = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "school":
                                                            textReader.Read();
                                                            school = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "city":
                                                            textReader.Read();
                                                            city = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "comment":
                                                            textReader.Read();
                                                            comment = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                    }
                                                }
                                            }
                                            clinicData.AddSupport(fname, lname, school, city, comment);
                                            break;
                                    }
                                }
                            }
                            #endregion end load supports
                            break;
                        case "medications":
                            #region load medications
                            bool inMedications = true;
                            while (inMedications)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "medications":
                                            inMedications = false;
                                            break;
                                        case "medication":
                                            bool inMedication = true;
                                            string displayName =  "";
                                            string text = "";
                                            string code = "";
                                            while (inMedication)
                                            {
                                                textReader.Read();
                                                {
                                                    
                                                    switch (textReader.LocalName)
                                                    {
                                                        case "medication":
                                                            inMedication = false;
                                                            break;
                                                        case "displayName":
                                                            textReader.Read();
                                                            displayName = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "text":
                                                            textReader.Read();
                                                            text = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "code":
                                                            textReader.Read();
                                                            code = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                    }
                                                }
                                            }
                                            clinicData.AddMedInfo(displayName, code, text);
                                            break;
                                    }
                                }
                            }
                            #endregion end load medications
                            break;
                        case "immunizations":
                            #region load immunizations
                            bool inimmunizations = true;
                            while (inimmunizations)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "immunizations":
                                            inimmunizations = false;
                                            break;
                                        case "immunization":
                                            bool inimmunization = true;
                                            string displayName =  "";
                                            string text = "";
                                            string code = "";
                                            while (inimmunization)
                                            {
                                                textReader.Read();
                                                {
                                                    switch (textReader.LocalName)
                                                    {
                                                        case "immunization":
                                                            inimmunization = false;
                                                            break;
                                                        case "displayName":
                                                            textReader.Read();
                                                            displayName = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "text":
                                                            textReader.Read();
                                                            text = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "code":
                                                            textReader.Read();
                                                            code = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                    }
                                                }
                                            }
                                            clinicData.AddMedInfo(displayName, code, text);
                                            break;
                                    }
                                }
                            }
                            #endregion end load immunizations
                            break;
                        case "providers":
                            #region load providers
                            bool inProviders = true;
                            while (inProviders)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "providers":
                                            inProviders = false;
                                            break;
                                        case "provider":
                                            bool inProvider = true;
                                            string roleCode = "NI";
                                            string roleText = "NI";
                                            string startDate = "NI";
                                            string endDate = "NI";
                                            string id = "NI";
                                            string namePrefix = "NI";
                                            string nameFirst = "NI";
                                            string nameLast = "NI";
                                            string organization = "NI";
                                            string comment = "NI";
                                            while (inProvider)
                                            {
                                                textReader.Read();
                                                {
                                                    switch (textReader.LocalName)
                                                    {
                                                        case "provider":
                                                            inProvider = false;
                                                            break;
                                                        case "roleCode":
                                                            textReader.Read();
                                                            roleCode = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "roleText":
                                                            textReader.Read();
                                                            roleText = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "start":
                                                            textReader.Read();
                                                            startDate = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "end":
                                                            textReader.Read();
                                                            endDate = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "id":
                                                            textReader.Read();
                                                            id = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "prefix":
                                                            textReader.Read();
                                                            namePrefix = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "first":
                                                            textReader.Read();
                                                            nameFirst = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "last":
                                                            textReader.Read();
                                                            nameLast = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "organization":
                                                            textReader.Read();
                                                            organization = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                        case "comment":
                                                            textReader.Read();
                                                            comment = textReader.Value.ToString();
                                                            textReader.Read();
                                                            break;
                                                    }
                                                }
                                            }
                                            clinicData.AddProvider(roleCode, roleText, startDate, endDate, id,
                                                        namePrefix, nameFirst, nameLast, organization, comment);
                                            break;
                                    }
                                }
                            }
                            #endregion end load providers
                            break;
                        
                    }
                }
            }

            textReader.Close();
        }

        #region load clinic settings
        private void LoadSettingsFormData(XmlTextReader textReader)
        {
            bool inFormData = true;
            while (inFormData)
            {
                textReader.Read();
                {
                    switch (textReader.LocalName)
                    {
                        case "formData":
                            inFormData = false;
                            break;
                        case "confidentialityCode":
                            textReader.Read();
                            clinicData.confidentialityCode = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "xmlns":
                            textReader.Read();
                            clinicData.greenCCDxmlns = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "xsi":
                            textReader.Read();
                            clinicData.greenCCDxmlns_xsi = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "hr4e":
                            textReader.Read();
                            clinicData.greenCCDxmlns_hr4e = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "schemaLocation":
                            textReader.Read();
                            clinicData.greenCCDxsi_schemaLocation = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "title":
                            textReader.Read();
                            clinicData.docTitle = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "formVersion":
                            textReader.Read();
                            clinicData.docVersion = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "hr4eUid":
                            textReader.Read();
                            clinicData.hr4eOID = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "timeZone":
                            textReader.Read();
                            clinicData.timeZone = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "encounterCode":
                            textReader.Read();
                            clinicData.encountertype = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "encounterText":
                            textReader.Read();
                            clinicData.encountertext = textReader.Value.ToString();
                            textReader.Read();
                            break; //hemoglobinNormalRange
                        case "hemoglobinNormalRange":
                            textReader.Read();
                            clinicData.hemoglobinRange = textReader.Value.ToString();
                            textReader.Read();
                            break; //hemoglobinNormalRange
                        case "codeSystems":
                            bool inCodeSystems = true;
                            while (inCodeSystems)
                            {
                                textReader.Read();
                                {
                                    switch (textReader.LocalName)
                                    {
                                        case "codeSystems":
                                            inCodeSystems = false;
                                            break;
                                        case "confidentiality":
                                            textReader.Read();
                                            clinicData.confidentialityCodeSystem = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "gender":
                                            textReader.Read();
                                            clinicData.genderCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "role":
                                            textReader.Read();
                                            clinicData.roleCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "resultsInterpret":
                                            textReader.Read();
                                            clinicData.resultsInterpretCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "resultType":
                                            textReader.Read();
                                            clinicData.resultTypeCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "encounterType":
                                            textReader.Read();
                                            clinicData.encounterTypeCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "medication":
                                            textReader.Read();
                                            clinicData.medicationCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "medicationCodeSystemName":
                                            textReader.Read();
                                            clinicData.medicationCodeSystemName = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "immunization":
                                            textReader.Read();
                                            clinicData.immunizationCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "vitalSign":
                                            textReader.Read();
                                            clinicData.vitalSignCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;
                                        case "problemCode":
                                            textReader.Read();
                                            clinicData.problemCode = textReader.Value.ToString();
                                            textReader.Read();
                                            break;

                                    }
                                }
                            }
                            break;
                    }

                }
            }
        }

        private void LoadSettingsClinicData(XmlTextReader textReader)
        {
            bool inClinicData = true;
            while (inClinicData)
            {
                textReader.Read();
                {
                    switch (textReader.LocalName)
                    {
                        case "clinicData":
                            inClinicData = false;
                            break;
                        case "clinicOid":
                            textReader.Read();
                            clinicData.hr4eOID = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "gps":
                            textReader.Read();
                            clinicData.gps = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "name":
                            textReader.Read();
                            clinicData.clinicName = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "details":
                            textReader.Read();
                            clinicData.clinicDetails = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "custodianName":
                            textReader.Read();
                            clinicData.custodianName = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "custodianID":
                            textReader.Read();
                            clinicData.custodianID = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "startDate":
                            textReader.Read();
                            clinicData.dateStart = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "endDate":
                            textReader.Read();
                            clinicData.dateEnd = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "authorPrefix":
                            textReader.Read();
                            clinicData.authorPrefix = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "authorFirstName":
                            textReader.Read();
                            clinicData.authorFirstName = textReader.Value.ToString();
                            textReader.Read();
                            break;
                        case "authorLastName":
                            textReader.Read();
                            clinicData.authorLastName = textReader.Value.ToString();
                            textReader.Read();
                            break;
                    }

                }
            }
        }
        #endregion end load clinic Settings
    }
}
