using System;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace hr4eInterface
{
    class XMLHandler
    {
        Form1 mainForm;
        FormData patientForm;

        string loadCurrentPatientDataPath;
        string loadClinicSettingsPath;
        string loadPatientTemplatePath;

        string storeCurrentPatientDataPath;
        string storeClinicSettingsPath;
        string storePatientTemplatePath;
        string storePatientBackupPath;
        string storePatientExitPath;

        string pathType;


        public XMLHandler(Form1 MainForm, FormData PatientForm)
        {
            pathType = "linux";//by default set path to windows, change to "osx" for mac, "linux" for linux based systems
            patientForm = PatientForm;
            mainForm = MainForm;

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

                    storeCurrentPatientDataPath = Path.Combine(desktopPath, "current_patient.xml");
                    storeClinicSettingsPath = Path.Combine(desktopPath, "hr4e_clinic_settings.xml");
                    storePatientTemplatePath = Path.Combine(desktopPath, "hr4e_patient_template.xml");
                    storePatientBackupPath = Path.Combine(desktopPath, "hr4e\\data\\patient_data");
                    storePatientExitPath = Path.Combine(desktopPath, "hr4e_patient.xml");
                    break;
                case "osx":
                    break;
                case "linux":
                    string currentDirectory = Directory.GetCurrentDirectory();
                    //string rootDirectory = "/home/phil/hr4e/";
                    //string usbDirectory = "/media/hr4e/";
                    //string rootDirectory = "/home/ben/hr4e/";
              		//string usbDirectory = "/media/hr4e/";
                    //Directory.SetCurrentDirectory(usbDirectory);
					//currentDirectory = Directory.GetCurrentDirectory();
                    //Handle all of the USB File Paths:
                    storeCurrentPatientDataPath = "/media/SUPER/hr4e/data/patient_data/current_patient.xml";
                    loadCurrentPatientDataPath = "/media/SUPER/hr4e/data/patient_data/current_patient.xml";

                    //Handle all of the local file paths:
                    //Directory.SetCurrentDirectory(rootDirectory);
					//currentDirectory = Directory.GetCurrentDirectory();
                    loadClinicSettingsPath = "/home/phil/hr4e/templates/hr4e_clinic_settings.xml";
                    loadPatientTemplatePath = "/home/phil/hr4e/templates/hr4e_patient_template.xml";
                    storeClinicSettingsPath = "/home/phil/hr4e/templates/hr4e_clinic_settings.xml";
                    storePatientTemplatePath = "/home/phil/hr4e/templates/hr4e_patient_template.xml";
                    storePatientBackupPath = "/home/phil/hr4e/data/patient_data";
                    storePatientExitPath = "/home/phil/hr4e/data/patient_data/hr4e_patient.xml";
                    break;
            }
        }

        #region XML READERS
        public void LoadClinicSettings()
        {

        }

		#region readers
        public void LoadCurrentPatientXML()
        {
            ReadXMLPatientFile(loadCurrentPatientDataPath);
        }

        public void LoadPatientTemplateXML()
        {
            ReadXMLPatientFile(loadPatientTemplatePath);
        }

        public void ReadXMLPatientFile(string filePath)
        {
            // Read a document
            XmlTextReader textReader = new XmlTextReader(filePath);
            textReader.WhitespaceHandling = WhitespaceHandling.None;
            
            textReader.Read();
            // If the node has value


            while (textReader.Read())
            {
                if (textReader.NodeType.Equals(XmlNodeType.Element))
                {
                    switch (textReader.LocalName)
                    {
                        case "greenCCD":
                            patientForm.clinicData.xmlns = textReader.GetAttribute(0);
                            patientForm.clinicData.xmlns_xsi = textReader.GetAttribute(1);
                            patientForm.clinicData.xmlns_hr4e = textReader.GetAttribute(2);
                            patientForm.clinicData.xsi_schemaLocation = textReader.GetAttribute(3);
                            break;
                        case "header":
                            ReadHeader(textReader);
                            break;
                        case "body":
                            ReadBody(textReader);
                            break;
                    }
                }
            }

            textReader.Close();
        }

        private void ReadBody(XmlTextReader textReader)
        {
            bool readingBody = true;
            while (readingBody)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "body":
                        readingBody = false;
                        break;
                    case "results":
                        ReadLabResults(textReader);
                        break;
                    case "encounters":
                        ReadEncounters(textReader);
                        break;
                    case "medications":
                        ReadMedication(textReader);
                        break;
                    case "immunizations":
                        ReadImmunizations(textReader);
                        break;
                    case "vitalSigns":
                        ReadVitalSigns(textReader);
                        break;
                    case "conditions":
                        ReadConditions(textReader);
                        break;
                    case "planOfCare":
                        ReadPlanOfCare(textReader);
                        break;
                }
            }
        }

        private void ReadPlanOfCare(XmlTextReader textReader)
        {
            bool readingPlanOfCare = true;
            while (readingPlanOfCare)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "planOfCare":
                        readingPlanOfCare = false;
                        break;
                    case "plannedObservation":
                        PlanOfCare newPlanOfCare = new PlanOfCare();
                        bool readingPlannedObservation = true;
                        while (readingPlannedObservation)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "plannedObservation":
                                    readingPlannedObservation = false;
                                    break;
                                case "planId":
                                    newPlanOfCare.id = textReader.GetAttribute(0);
                                    break;
                                case "planType":
                                    newPlanOfCare.code = textReader.GetAttribute(0);
                                    newPlanOfCare.codeSystem = textReader.GetAttribute(1);
                                    newPlanOfCare.displayName = textReader.GetAttribute(2);
                                    break;
                                case "planFreeText":
                                    textReader.Read();
                                    newPlanOfCare.text = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.encounterData.planofCares.Add(newPlanOfCare);
                        break;
                }
            }
        }

        private void ReadConditions(XmlTextReader textReader)
        {
            bool readingConditions = true;
            while (readingConditions)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "conditions":
                        readingConditions = false;
                        break;
                    case "condition":
                        Condition newCondition = new Condition();
                        bool readingCondition = true;
                        while (readingCondition)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "condition":
                                    readingCondition = false;
                                    break;
                                case "low":
                                    newCondition.startYear = textReader.GetAttribute(0);
                                    break;
                                case "problemName":
                                    textReader.Read();
                                    newCondition.problemName = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "problemCode":
                                    newCondition.code = textReader.GetAttribute(0);
                                    newCondition.codeSystem = textReader.GetAttribute(1);
                                    newCondition.displayName = textReader.GetAttribute(2);
                                    break;
                                case "comment":
                                    textReader.Read();
                                    textReader.Read();
                                    newCondition.comment = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.encounterData.conditions.Add(newCondition);
                        break;
                }
            }
        }

        private void ReadVitalSigns(XmlTextReader textReader)
        {
            bool readingVitalSigns = true;
            while (readingVitalSigns)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "vitalSigns":
                        readingVitalSigns = false;
                        break;
                    case "vitalSign":
                        VitalSign newVitalSign = new VitalSign();
                        bool readingVitalSign = true;
                        while (readingVitalSign)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "vitalSign":
                                    readingVitalSign = false;
                                    break;
                                case "resultID":
                                    newVitalSign.id = textReader.GetAttribute(0);
                                    break;
                                case "resultDateTime":
                                    newVitalSign.timeStamp = textReader.GetAttribute(0);
                                    break;
                                case "resultType":
                                    newVitalSign.codeSystem = textReader.GetAttribute(0);
                                    newVitalSign.displayName = textReader.GetAttribute(1);
                                    newVitalSign.code = textReader.GetAttribute(2);
                                    break;
                                case "resultStatus":
                                    newVitalSign.status = textReader.GetAttribute(0);
                                    break;
                                case "physicalQuantity":
                                    newVitalSign.value = textReader.GetAttribute(0);
                                    newVitalSign.unit = textReader.GetAttribute(1);
                                    break;
                                case "comment":
                                    textReader.Read();
                                    textReader.Read();
                                    newVitalSign.comment = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.encounterData.vitalSigns.Add(newVitalSign);
                        break;
                }
            }
        }

        private void ReadEncounters(XmlTextReader textReader)
        {
            bool readingEncounters = true;
            while (readingEncounters)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "encounters":
                        readingEncounters = false;
                        break;
                    case "encounter":
                        Encounter newEncounter = new Encounter();
                        bool readingEncounter = true;
                        while (readingEncounter)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "encounter":
                                    readingEncounter = false;
                                    break;
                                case "encounterID":
                                    newEncounter.id = textReader.GetAttribute(0);
                                    break;
                                case "encounterType":
                                    newEncounter.codeSystem = textReader.GetAttribute(0);
                                    newEncounter.code = textReader.GetAttribute(1);
                                    textReader.Read();
                                    textReader.Read();
                                    newEncounter.textDescription = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                                case "reasonForVisit":
                                    textReader.Read();
                                    textReader.Read();
                                    newEncounter.reason = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    newEncounter.reasonCode = textReader.GetAttribute(0);
                                    textReader.Read();
                                    break;
                                case "encounterDateTime":
                                    newEncounter.timeStamp = textReader.GetAttribute(0);
                                    break;
                                case "encounterNotes":
                                    textReader.Read();
                                    newEncounter.notes = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.encounterData.encounter = newEncounter;
                        break;
                }
            }
        }

        private void ReadImmunizations(XmlTextReader textReader)
        {
            bool readingImmunizations = true;
            while (readingImmunizations)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "immunizations":
                        readingImmunizations = false;
                        break;
                    case "immunization":
                        Immunization newImmunization = new Immunization();
                        bool readingEncounter = true;
                        while (readingEncounter)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "immunization":
                                    readingEncounter = false;
                                    break;
                                case "administeredDate":
                                    newImmunization.dateAdministered = textReader.GetAttribute(0);
                                    break;
                                case "codedProductName":
                                    newImmunization.codeSystem = textReader.GetAttribute(0);
                                    newImmunization.displayName = textReader.GetAttribute(1);
                                    newImmunization.code = textReader.GetAttribute(2);
                                    break;
                                case "freeTextProductName":
                                    textReader.Read();
                                    newImmunization.text = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "comment":
                                    textReader.Read();
                                    textReader.Read();
                                    newImmunization.comment = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.encounterData.immunizations.Add(newImmunization);
                        break;
                }
            }
        }

        private void ReadMedication(XmlTextReader textReader)
        {
            bool readingMedications = true;
            while (readingMedications)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "medications":
                        readingMedications = false;
                        break;
                    case "medication":
                        Medication newMedication = new Medication();
                        bool readingMedication = true;
                        while (readingMedication)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "medication":
                                    readingMedication = false;
                                    break;
                                case "codedProductName":
                                    newMedication.codeSystem = textReader.GetAttribute(0);
                                    newMedication.codeSystemName = textReader.GetAttribute(1);
                                    newMedication.displayName = textReader.GetAttribute(2);
                                    newMedication.code = textReader.GetAttribute(3);
                                    break;
                                case "freeTextProductName":
                                    textReader.Read();
                                    newMedication.text = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.encounterData.medications.Add(newMedication);
                        break;
                }
            }
        }

        private void ReadLabResults(XmlTextReader textReader)
        {
            bool readingResults = true;
            while (readingResults)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "results":
                        readingResults = false;
                        break;
                    case "result":
                        LabResult newResult = new LabResult();
                        bool readingResult = true;
                        while (readingResult)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "result":
                                    readingResult = false;
                                    break;
                                case "resultID":
                                    newResult.id = textReader.GetAttribute(0);
                                    break;
                                case "resultDateTime":
                                    newResult.timeStamp = textReader.GetAttribute(0);
                                    break;
                                case "resultType":
                                    newResult.typeCodeSystem = textReader.GetAttribute(0);
                                    newResult.typeCode = textReader.GetAttribute(1);
                                    newResult.displayName = textReader.GetAttribute(2);
                                    break;
                                case "resultStatus":
                                    newResult.status = textReader.GetAttribute(0);
                                    break;
                                case "physicalQuantity":
                                    newResult.value = textReader.GetAttribute(0);
                                    newResult.unit = textReader.GetAttribute(1);
                                    break;
                                case "resultInterpretation":
                                    newResult.interpretCodeSystem = textReader.GetAttribute(0);
                                    newResult.interpretCode = textReader.GetAttribute(1);
                                    break;
                                case "resultReferenceRange":
                                    textReader.Read();
                                    newResult.referenceRange = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "comment":
                                    textReader.Read();
                                    textReader.Read();
                                    newResult.comment = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.encounterData.labResults.Add(newResult);
                        break;
                }
            }
        }

        private void ReadHeader(XmlTextReader textReader)
        {
            bool readingHeader = true;
            while (readingHeader)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "header":
                        readingHeader = false;
                        break;
                    case "documentID":
                        patientForm.clinicData.docID = textReader.GetAttribute(0);
                        break;
                    case "title":
                        //patientForm.clinicData.title = textReader.Value;
                        textReader.MoveToContent();
                        patientForm.clinicData.title = textReader.ReadElementContentAsString();
                        break;
                    case "number":
                        //patientForm.clinicData.title = textReader.Value;
                        textReader.MoveToContent();
                        patientForm.clinicData.versionNumber = textReader.ReadElementContentAsString();
                        break;
                    case "version":
                        textReader.Read();
                        textReader.MoveToContent();
                        patientForm.clinicData.versionNumber = textReader.ReadElementContentAsString();
                        break;
                    case "confidentiality":
                        patientForm.clinicData.confidentialityCodeSystem = textReader.GetAttribute(0);
                        patientForm.clinicData.confidentialityCode = textReader.GetAttribute(1);
                        break;
                    case "documentTimestamp":
                        patientForm.clinicData.documentTimeStamp = textReader.GetAttribute(0);
                        break;
                    case "personalInformation":
                        ReadPersonalInformation(textReader);
                        break;
                    case "supports":
                        ReadSupports(textReader);
                        break;
                    case "custodian":
                        textReader.Read();
                        patientForm.clinicData.custodianID = textReader.GetAttribute(0);
                        textReader.Read();
                        textReader.MoveToContent();
                        patientForm.clinicData.custodianName = textReader.ReadElementContentAsString();
                        break;
                    case "healthcareProviders":
                        ReadCareProviders(textReader);
                        break;
                    case "informationSource":
                        ReadInformationSource(textReader);
                        break;
                    case "clinics":
                        ReadClinicData(textReader);
                        break;
                    case "languagesSpoken":
                        textReader.Read();
                        bool findingLanguages = true;
                        while (findingLanguages)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "languagesSpoken":
                                    findingLanguages = false;
                                    break;
                                case "languageCode":
                                    if (!patientForm.patientData.Languages.Contains(textReader.GetAttribute(0)))
                                    patientForm.patientData.Languages.Add(textReader.GetAttribute(0));
                                    textReader.Read();
                                    break;
                            }
                        }
                        break;
                            
                }
            }
        }

        private void ReadClinicData(XmlTextReader textReader)
        {
            bool inClinic = true;
            textReader.Read();
            while (inClinic)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "clinicEntity":
                        inClinic = false;
                        break;
                    case "clinicGPSCoordinates":
                        //patientForm.clinicData.gpsCoordinates = textReader.GetAttribute(0);
                        textReader.Read();
                        patientForm.clinicData.gpsCoordinates = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "clinicName":
                        textReader.Read();
                        patientForm.clinicData.clinicName = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "clinicDetails":
                        textReader.Read();
                        patientForm.clinicData.clinicDetails = textReader.Value.ToString();
                        textReader.Read();
                        break;
                }
            }
            textReader.Read();
        }

        private void ReadInformationSource(XmlTextReader textReader)
        {
            bool inInformationSource = true;
            while (inInformationSource)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "informationSource":
                        inInformationSource = false;
                        break;
                    case "author":
                        Author newAuthor = new Author();
                        bool inNewAuthor = true;
                        while (inNewAuthor)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "author":
                                    inNewAuthor = false;
                                    break;
                                case "authorTime":
                                    newAuthor.authorTimeStamp = textReader.GetAttribute(0);
                                    break;
                                case "prefix":
                                    textReader.Read();
                                    newAuthor.authorNamePrefix = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "given":
                                    textReader.Read();
                                    newAuthor.authorNameFirst = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "family":
                                    textReader.Read();
                                    newAuthor.authorNameLast = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.clinicData.author = newAuthor;
                        break;
                    
                }
            }
        }

        private void ReadCareProviders(XmlTextReader textReader)
        {
            bool inCareProviders = true;
            while (inCareProviders)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "healthcareProviders":
                        inCareProviders = false;
                        break;
                    case "careProvisionDateRange":
                        textReader.Read();
                        patientForm.clinicData.clinicStartDate = textReader.GetAttribute(0);
                        textReader.Read();
                        patientForm.clinicData.clinicEndDate = textReader.GetAttribute(0);
                        textReader.Read();
                        break;
                    case "healthcareProvider":
                        Provider newProvider = new Provider();
                        bool buildProvider = true;
                        while (buildProvider)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "healthcareProvider":
                                    buildProvider = false;
                                    break;
                                case "role":
                                    newProvider.roleCode = textReader.GetAttribute(0);
                                    newProvider.roleCodeSystem = textReader.GetAttribute(1);
                                    textReader.Read();
                                    textReader.Read();
                                    newProvider.roleDescription = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                                case "dateRange":
                                    textReader.Read();
                                    newProvider.dateStart = textReader.GetAttribute(0);
                                    textReader.Read();
                                    newProvider.dateEnd = textReader.GetAttribute(0);
                                    textReader.Read();
                                    break;
                                case "comment":
                                    textReader.Read();
                                    textReader.Read();
                                    newProvider.comment = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                                case "providerEntity":
                                    bool inProvider = true;
                                    while (inProvider)
                                    {
                                        textReader.Read();
                                        switch (textReader.LocalName)
                                        {
                                            case "providerEntity":
                                                inProvider = false;
                                                break;
                                            case "providerID":
                                                newProvider.id = textReader.GetAttribute(0);
                                                break;
                                            case "prefix":
                                                textReader.Read();
                                                newProvider.prefix = textReader.Value.ToString();
                                                textReader.Read();
                                                break;
                                            case "given":
                                                textReader.Read();
                                                newProvider.firstName = textReader.Value.ToString();
                                                textReader.Read();
                                                break;
                                            case "family":
                                                textReader.Read();
                                                newProvider.lastName = textReader.Value.ToString();
                                                textReader.Read();
                                                break;
                                            case "providerOrganizationName":
                                                textReader.Read();
                                                newProvider.organizationName = textReader.Value.ToString();
                                                textReader.Read();
                                                break;
                                        }
                                    }
                                    
                                    break;
                            }
                        }
                        patientForm.clinicData.providers.Add(newProvider);
                        break;
                }
            }
        }

        private void ReadSupports(XmlTextReader textReader)
        {
            bool inSupports = true;
            while (inSupports)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "supports":
                        inSupports = false;
                        break;
                    case "support":
                        Support newSupport = new Support();
                        bool buildSupport = true;
                        while (buildSupport)
                        {
                            textReader.Read();
                            switch (textReader.LocalName)
                            {
                                case "support":
                                    buildSupport = false;
                                    break;
                                case "date":
                                    newSupport.dateNull = textReader.GetAttribute(0);
                                    break;
                                case "streetAddressLine":
                                    newSupport.streetNull = textReader.GetAttribute(0);
                                    break;
                                case "city":
                                    textReader.Read();
                                    newSupport.city = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "state":
                                    newSupport.stateNull = textReader.GetAttribute(0);
                                    break;
                                case "postalCode":
                                    newSupport.postalCodeNull = textReader.GetAttribute(0);
                                    break;
                                case "contactTelecom":
                                    newSupport.telecomNull = textReader.GetAttribute(0);
                                    break;
                                case "given":
                                    textReader.Read();
                                    newSupport.firstName = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "family":
                                    textReader.Read();
                                    newSupport.lastname = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "schoolName":
                                    textReader.Read();
                                    newSupport.schoolName = textReader.Value.ToString();
                                    textReader.Read();
                                    break;
                                case "comment":
                                    textReader.Read();
                                    textReader.Read();
                                    newSupport.comment = textReader.Value.ToString();
                                    textReader.Read();
                                    textReader.Read();
                                    break;
                            }
                        }
                        patientForm.patientData.Supports.Add(newSupport);
                        break;
                }
            }
        }

        private void ReadPersonalInformation(XmlTextReader textReader)
        {
            textReader.Read();
            bool findingPersonalInfo = true;
            while (findingPersonalInfo)
            {
                textReader.Read();
                switch (textReader.LocalName)
                {
                    case "personalInformation":
                        findingPersonalInfo = false;
                        break;
                    case "personID":
                        patientForm.patientData.id  = textReader.GetAttribute(0);
                        break;
                    case "streetAddressLine":
                        patientForm.patientData.streetNull = textReader.GetAttribute(0);
                        break;
                    case "city":
                        textReader.Read();
                        patientForm.patientData.presentVillage= textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "state":
                        patientForm.patientData.stateNull = textReader.GetAttribute(0);
                        break;
                    case "postalCode":
                        patientForm.patientData.postalCodeNull = textReader.GetAttribute(0);
                        break;
                    case "personPhone":
                        patientForm.patientData.telecomNull = textReader.GetAttribute(0);
                        break;
                    case "given":
                        textReader.Read();
                        patientForm.patientData.firstName = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "family":
                        textReader.Read();
                        patientForm.patientData.lastName = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "gender":
                        patientForm.patientData.genderCodeSystem = textReader.GetAttribute(0);
                        patientForm.patientData.genderCode = textReader.GetAttribute(1);
                        break;
                    case "personDateOfBirth":
                        patientForm.patientData.dateOfBirth = textReader.GetAttribute(0);
                        break;
                    case "statedAge":
                        textReader.Read();
                        patientForm.patientData.statedAge = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "estimatedAge":
                        textReader.Read();
                        patientForm.patientData.estimatedAge = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "yearInSchool":
                        textReader.Read();
                        patientForm.patientData.yearInSchool = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "statusInSchool":
                        textReader.Read();
                        patientForm.patientData.statusInSchool = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "homeVillage":
                        textReader.Read();
                        patientForm.patientData.homeVillage = textReader.Value.ToString();
                        textReader.Read();
                        break;
                    case "tribe":
                        textReader.Read();
                        patientForm.patientData.tribe = textReader.Value.ToString();
                        textReader.Read();
                        break;


                }
            }
        }
		#endregion
        #endregion

        public void WriteCurrentPatientXML()
        {
            WritePatientXML(storeCurrentPatientDataPath);
        }

        public void WriteFinalPatientXML()
        {
            WritePatientXML(storePatientExitPath);
        }

        public void WritePatientTemplateXML()
        {
            WritePatientXML(storePatientTemplatePath);
        }
        
        public void WritePatientXML(string path)
        {
            
            // Read until end of file
            XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("greenCCD");
            writer.WriteAttributeString("xmlns", patientForm.clinicData.xmlns);
            writer.WriteAttributeString("xmlns:xsi", patientForm.clinicData.xmlns_xsi);
            writer.WriteAttributeString("xmlns:hr4e", patientForm.clinicData.xmlns_hr4e);
            writer.WriteAttributeString("xsi:schemaLocation", patientForm.clinicData.xsi_schemaLocation);
            #region write header
            //header start
            writer.WriteStartElement("header");

            //doc ID info
            writer.WriteStartElement("documentID");
            writer.WriteAttributeString("root", patientForm.clinicData.docID);
            writer.WriteEndElement(); //end documentID

            writer.WriteElementString("title", patientForm.clinicData.title);

            writer.WriteStartElement("version");
            writer.WriteElementString("number", patientForm.clinicData.versionNumber);
            writer.WriteEndElement(); //end version

            writer.WriteStartElement("confidentiality");
            writer.WriteAttributeString("codeSystem", patientForm.clinicData.confidentialityCodeSystem);
            writer.WriteAttributeString("code", patientForm.clinicData.confidentialityCode);
            writer.WriteEndElement(); //end confidentiality

            writer.WriteStartElement("documentTimestamp");
            writer.WriteAttributeString("value", patientForm.clinicData.documentTimeStamp);
            writer.WriteEndElement(); //end documentTimestamp

            //personal info
            writer.WriteStartElement("personalInformation"); 
            writer.WriteStartElement("patientInformation");
            writer.WriteStartElement("personID");
            writer.WriteAttributeString("root", patientForm.patientData.id);
            writer.WriteEndElement(); //end personID

            writer.WriteStartElement("personAddress");
            writer.WriteStartElement("streetAddressLine");
            writer.WriteAttributeString("nullFlavor", patientForm.patientData.streetNull);
            writer.WriteEndElement(); //end streetAddressLine
            writer.WriteElementString("city", patientForm.patientData.presentVillage);
            writer.WriteStartElement("state");
            writer.WriteAttributeString("nullFlavor", patientForm.patientData.stateNull);
            writer.WriteEndElement(); //end state
            writer.WriteStartElement("postalCode");
            writer.WriteAttributeString("nullFlavor", patientForm.patientData.postalCodeNull);
            writer.WriteEndElement(); //end postalCode
            writer.WriteEndElement(); //end personAddress
            writer.WriteStartElement("personPhone");
            writer.WriteAttributeString("nullFlavor", patientForm.patientData.telecomNull);
            writer.WriteEndElement(); //end personPhone
            writer.WriteStartElement("personInformation");
            writer.WriteStartElement("personName");
            writer.WriteElementString("given", patientForm.patientData.firstName);
            writer.WriteElementString("family", patientForm.patientData.lastName);
            writer.WriteEndElement(); //end personName
            
            writer.WriteStartElement("gender");
            writer.WriteAttributeString("codeSystem", patientForm.patientData.genderCodeSystem);
            writer.WriteAttributeString("code", patientForm.patientData.genderCode);
            writer.WriteEndElement(); //end gender
            writer.WriteStartElement("personDateOfBirth");
            writer.WriteAttributeString("value", patientForm.patientData.dateOfBirth);
            writer.WriteEndElement(); //end personDateOfBirth
            writer.WriteStartElement("hr4e:ageInformation");
            writer.WriteElementString("hr4e:statedAge", patientForm.patientData.statedAge);
            writer.WriteElementString("hr4e:estimatedAge", patientForm.patientData.estimatedAge);
            writer.WriteElementString("hr4e:yearInSchool", patientForm.patientData.yearInSchool);
            writer.WriteElementString("hr4e:statusInSchool", patientForm.patientData.statusInSchool);
            writer.WriteEndElement(); //end hr4e:ageInformation
            writer.WriteEndElement(); //end personInformation
            writer.WriteStartElement("hr4e:livingSituation");
            writer.WriteElementString("hr4e:homeVillage", patientForm.patientData.homeVillage);
            writer.WriteElementString("hr4e:tribe", patientForm.patientData.tribe);
            writer.WriteEndElement(); //end hr4e:livingSituation
            writer.WriteEndElement(); //end patientInformation
            writer.WriteEndElement(); //end personalInformation

            writer.WriteStartElement("languagesSpoken");
            foreach (string lang in patientForm.patientData.Languages)
            {
                writer.WriteStartElement("languageSpoken");
                writer.WriteStartElement("languageCode");
                writer.WriteAttributeString("code", lang);
                writer.WriteEndElement(); //end languageCode
                writer.WriteEndElement(); //end languageSpoken
            }
            writer.WriteEndElement(); //end languagesSpoken

            writer.WriteStartElement("supports");
            foreach (Support s in patientForm.patientData.Supports)
            {
                writer.WriteStartElement("support");
                writer.WriteStartElement("date");
                writer.WriteAttributeString("nullFlavor", s.dateNull);
                writer.WriteEndElement(); //end date
                writer.WriteStartElement("contact");
                writer.WriteStartElement("contactAddress");
                writer.WriteStartElement("streetAddressLine");
                writer.WriteAttributeString("nullFlavor", s.streetNull);
                writer.WriteEndElement(); //end streetAddressLine
                writer.WriteElementString("city", s.city);
                writer.WriteStartElement("postalCode");
                writer.WriteAttributeString("nullFlavor", s.postalCodeNull);
                writer.WriteEndElement(); //end postalCode
                writer.WriteEndElement(); //end contactAddress
                writer.WriteStartElement("contactTelecom");
                writer.WriteAttributeString("nullFlavor", s.telecomNull);
                writer.WriteEndElement(); //end contactTelecom
                writer.WriteStartElement("contactName");
                writer.WriteElementString("given", s.lastname);
                writer.WriteElementString("family", s.firstName);
                writer.WriteEndElement(); //end contactName
                writer.WriteStartElement("comment");
                writer.WriteElementString("text", s.comment);
                writer.WriteEndElement(); //end comment
                writer.WriteElementString("hr4e:schoolName", s.schoolName);
                writer.WriteEndElement(); //end contact
                writer.WriteEndElement(); //end support
            }
            writer.WriteEndElement(); //end supports

            writer.WriteStartElement("custodian");
            writer.WriteStartElement("custodianID");
            writer.WriteAttributeString("root", patientForm.clinicData.custodianID);
            writer.WriteEndElement(); //end custodianID
            writer.WriteElementString("custodianName", patientForm.clinicData.custodianName);
            writer.WriteEndElement(); //end custodian

            writer.WriteStartElement("healthcareProviders");
            writer.WriteStartElement("careProvisionDateRange");
            writer.WriteStartElement("low");
            writer.WriteAttributeString("value", patientForm.clinicData.clinicStartDate);
            writer.WriteEndElement(); //end low
            writer.WriteStartElement("high");
            writer.WriteAttributeString("value", patientForm.clinicData.clinicEndDate);
            writer.WriteEndElement(); //end high
            writer.WriteEndElement(); //end careProvisionDateRange
            foreach (Provider p in patientForm.clinicData.providers)
            {
                writer.WriteStartElement("healthcareProvider");
                writer.WriteStartElement("role");
                writer.WriteAttributeString("code", p.roleCode);
                writer.WriteAttributeString("codeSystem", p.roleCodeSystem);
                writer.WriteElementString("originalText", p.roleDescription);
                writer.WriteEndElement(); //end role
                writer.WriteStartElement("dateRange");
                writer.WriteStartElement("low");
                writer.WriteAttributeString("value", p.dateStart);
                writer.WriteEndElement(); //end low
                writer.WriteStartElement("high");
                writer.WriteAttributeString("value", p.dateEnd);
                writer.WriteEndElement(); //end high
                writer.WriteEndElement(); //end dateRange
                writer.WriteStartElement("providerEntity");
                writer.WriteStartElement("providerID");
                writer.WriteAttributeString("root", p.id);
                writer.WriteEndElement(); //end providerID
                writer.WriteStartElement("providerName");
                writer.WriteElementString("prefix", p.prefix);
                writer.WriteElementString("given", p.firstName);
                writer.WriteElementString("family", p.lastName);
                writer.WriteEndElement(); //end providerName
                writer.WriteElementString("providerOrganizationName", p.organizationName);
                writer.WriteEndElement(); //end  providerEntity
                writer.WriteStartElement("comment");
                writer.WriteElementString("text", p.comment);
                writer.WriteEndElement(); //end comment
                writer.WriteEndElement(); //end healthcareProvider
            }
            writer.WriteEndElement(); //end healthcareProviders

            writer.WriteStartElement("informationSource");
            Author a = patientForm.clinicData.author;
            writer.WriteStartElement("author");
            writer.WriteStartElement("authorTime");
            writer.WriteAttributeString("value", a.authorTimeStamp);
            writer.WriteEndElement(); //end authorTime
            writer.WriteStartElement("authorName");
            writer.WriteElementString("prefix", a.authorNamePrefix);
            writer.WriteElementString("given", a.authorNameFirst);
            writer.WriteElementString("family", a.authorNameLast);
            writer.WriteEndElement(); //end authorName
            writer.WriteEndElement(); //end author
            writer.WriteEndElement(); //end informationSource

            writer.WriteStartElement("hr4e:clinics");
            writer.WriteStartElement("clinicEntity");
            writer.WriteElementString("hr4e:clinicGPSCoordinates", patientForm.clinicData.gpsCoordinates);
            writer.WriteElementString("hr4e:clinicName", patientForm.clinicData.clinicName);
            writer.WriteElementString("hr4e:clinicDetails", patientForm.clinicData.clinicDetails);
            writer.WriteEndElement(); //end clinicEntity
            writer.WriteEndElement(); //end hr4e:clinics

            writer.WriteEndElement(); //end header
            #endregion end Header
            #region write body
            writer.WriteStartElement("body");

            if (patientForm.encounterData.labResults.Count > 0)
            {
                writer.WriteStartElement("results");
                foreach (LabResult r in patientForm.encounterData.labResults)
                {
                    writer.WriteStartElement("result");
                    writer.WriteStartElement("resultID");
                    writer.WriteAttributeString("root", r.id);
                    writer.WriteEndElement(); //end resultID
                    writer.WriteStartElement("resultDateTime");
                    writer.WriteAttributeString("value", r.timeStamp);
                    writer.WriteEndElement(); //end resultDateTime
                    writer.WriteStartElement("resultType");
                    writer.WriteAttributeString("codeSystem", r.typeCodeSystem);
                    writer.WriteAttributeString("code", r.typeCode);
                    writer.WriteAttributeString("displayName", r.displayName);
                    writer.WriteEndElement(); //end resultType
                    writer.WriteStartElement("resultStatus");
                    writer.WriteAttributeString("code", r.status);
                    writer.WriteEndElement(); //end resultStatus
                    writer.WriteStartElement("resultValue");
                    writer.WriteStartElement("physicalQuantity");
                    writer.WriteAttributeString("value", r.value);
                    writer.WriteAttributeString("unit", r.unit);
                    writer.WriteEndElement(); //end physicalQuantity
                    writer.WriteEndElement(); //end resultValue
                    writer.WriteStartElement("resultInterpretation");
                    writer.WriteAttributeString("codeSystem", r.interpretCodeSystem);
                    writer.WriteAttributeString("code", r.interpretCode);
                    writer.WriteEndElement(); //end resultInterpretation
                    writer.WriteElementString("resultReferenceRange", r.referenceRange);
                    writer.WriteStartElement("comment");
                    writer.WriteElementString("text", r.comment);
                    writer.WriteEndElement(); //end comment
                    writer.WriteEndElement(); //end result
                }
                writer.WriteEndElement(); //end results
            }

            writer.WriteStartElement("encounters");
            Encounter e = patientForm.encounterData.encounter;
            writer.WriteStartElement("encounter");
            writer.WriteStartElement("encounterID");
            writer.WriteAttributeString("root", e.id);
            writer.WriteEndElement(); //end encounterID
            writer.WriteStartElement("encounterType");
            writer.WriteAttributeString("codeSystem", e.codeSystem);
            writer.WriteAttributeString("code", e.code);
            writer.WriteElementString("originalText", e.textDescription);
            writer.WriteEndElement(); //end encounterType
            writer.WriteStartElement("encounterDateTime");
            writer.WriteAttributeString("value", e.timeStamp);
            writer.WriteEndElement(); //end encounterDateTime
            writer.WriteStartElement("reasonForVisit");
            writer.WriteElementString("text", e.reason);
            writer.WriteStartElement("reason");
            writer.WriteAttributeString("code", e.reasonCode);
            writer.WriteEndElement(); //end reason
            writer.WriteEndElement(); //end reasonForVisit
            writer.WriteElementString("hr4e:encounterNotes", e.notes);
            writer.WriteEndElement(); //end encounter
            writer.WriteEndElement(); //end encounters

            if (patientForm.encounterData.medications.Count > 0)
            {
                writer.WriteStartElement("medications");
                foreach (Medication m in patientForm.encounterData.medications)
                {
                    writer.WriteStartElement("medication");
                    writer.WriteStartElement("medicationInformation");
                    writer.WriteStartElement("Replace");
                    writer.WriteAttributeString("codeSystem", m.codeSystem);
                    writer.WriteAttributeString("codeSystemName", m.codeSystemName);
                    writer.WriteAttributeString("displayName", m.displayName);
                    writer.WriteAttributeString("code", m.code);
                    writer.WriteEndElement(); //end Replace
                    writer.WriteElementString("freeTextProductName", m.text);
                    writer.WriteEndElement(); //end medicationInformation
                    writer.WriteEndElement(); //end medication
                }
                writer.WriteEndElement(); //end medications
            }

            if (patientForm.encounterData.immunizations.Count > 0)
            {
                writer.WriteStartElement("immunizations");
                foreach (Immunization i in patientForm.encounterData.immunizations)
                {
                    writer.WriteStartElement("immunization");
                    writer.WriteStartElement("administeredDate");
                    writer.WriteAttributeString("value", i.dateAdministered);
                    writer.WriteEndElement(); //end administeredDate
                    writer.WriteStartElement("medicationInformation");
                    writer.WriteStartElement("codedProductName");
                    writer.WriteAttributeString("codeSystem", i.codeSystem);
                    writer.WriteAttributeString("displayName", i.displayName);
                    writer.WriteAttributeString("code", i.code);
                    writer.WriteEndElement(); //end codedProductName
                    writer.WriteElementString("freeTextProductName", i.text);
                    writer.WriteEndElement(); //end medicationInformation
                    writer.WriteStartElement("comment");
                    writer.WriteElementString("text", i.comment);
                    writer.WriteEndElement(); //end comment
                    writer.WriteEndElement(); //end immunization
                }
                writer.WriteEndElement(); //end immunizations
            }

            writer.WriteStartElement("vitalSigns");
            foreach (VitalSign v in patientForm.encounterData.vitalSigns)
            {
                writer.WriteStartElement("vitalSign");
                writer.WriteStartElement("resultID");
                writer.WriteAttributeString("root", v.id);
                writer.WriteEndElement(); //end resultID
                writer.WriteStartElement("resultDateTime");
                writer.WriteAttributeString("value", v.timeStamp);
                writer.WriteEndElement(); //end resultDateTime
                writer.WriteStartElement("resultType");
                writer.WriteAttributeString("codeSystem", v.codeSystem);
                writer.WriteAttributeString("displayName", v.displayName);
                writer.WriteAttributeString("code", v.code);
                writer.WriteEndElement(); //end resultType
                writer.WriteStartElement("resultStatus");
                writer.WriteAttributeString("cade", v.status);
                writer.WriteEndElement(); //end resultStatus
                writer.WriteStartElement("resultValue");
                writer.WriteStartElement("physicalQuantity");
                writer.WriteAttributeString("value", v.value);
                writer.WriteAttributeString("unit", v.unit);
                writer.WriteEndElement(); //end physicalQuantity
                writer.WriteEndElement(); //end resultValue
                writer.WriteStartElement("comment");
                writer.WriteElementString("text", v.comment);
                writer.WriteEndElement(); //end comment
                writer.WriteEndElement(); //end vitalSign
            }
            writer.WriteEndElement(); //end vitalSigns

            if (patientForm.encounterData.conditions.Count > 0)
            {
                writer.WriteStartElement("conditions");
                foreach (Condition c in patientForm.encounterData.conditions)
                {
                    writer.WriteStartElement("condition");
                    writer.WriteStartElement("problemDate");
                    writer.WriteStartElement("low");
                    writer.WriteAttributeString("value", c.startYear);
                    writer.WriteEndElement(); //end low
                    writer.WriteEndElement(); //end problemDate
                    writer.WriteElementString("problemName", c.problemName);
                    writer.WriteStartElement("problemCode");
                    writer.WriteAttributeString("code", c.code);
                    writer.WriteAttributeString("codeSystem", c.codeSystem);
                    writer.WriteAttributeString("displayName", c.displayName);
                    writer.WriteEndElement(); //end problemCode
                    writer.WriteStartElement("comment");
                    writer.WriteElementString("text", c.comment);
                    writer.WriteEndElement(); //end comment
                    writer.WriteEndElement(); //end condition
                }
                writer.WriteEndElement(); //end conditions
            }

            if (patientForm.encounterData.planofCares.Count > 0)
            {
                writer.WriteStartElement("planOfCare");
                foreach (PlanOfCare p in patientForm.encounterData.planofCares)
                {
                    writer.WriteStartElement("plannedObservation");
                    writer.WriteStartElement("planId");
                    writer.WriteAttributeString("root", p.id);
                    writer.WriteEndElement(); //end planId
                    writer.WriteStartElement("planType");
                    writer.WriteAttributeString("code", p.code);
                    writer.WriteAttributeString("codeSystem", p.codeSystem);
                    writer.WriteAttributeString("displayName", p.displayName);
                    writer.WriteEndElement(); //end planType
                    writer.WriteElementString("planFreeText", p.text);
                    writer.WriteEndElement(); //end plannedObservation
                }
                writer.WriteEndElement(); //end planOfCare
            }

            writer.WriteEndElement(); //end body
            #endregion

            writer.WriteEndElement(); //end 
            writer.WriteEndDocument();  
            writer.Close();


            #region write template
            //Writer templates
            //comment
            //writer.WriteComment("REPLACE");

            //element with attribute
            //writer.WriteStartElement("Replace");
            //writer.WriteAttributeString("Replace", Replace);
            //writer.WriteEndElement(); //end Replace

            //parent element
            //writer.WriteStartElement("Replace");

            //writer.WriteEndElement(); //end Replace

            //Element with string
            //writer.WriteElementString("Replace", Replace);
            #endregion
        }
    }
}
