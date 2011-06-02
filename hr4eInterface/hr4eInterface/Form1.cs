using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace hr4eInterface
{
    public partial class Form1 : Form
    {
        #region structs
        struct IntakeTabStatus
        {

            public bool newPatient;
            public bool hasDOB;

            public bool goodDOByear;
            public bool goodDOBmonth;
            public bool goodDOBday;

        }

        struct TriageTabStatus
        {

            public bool showFlags;
            public List<RedFlags> redFlags;
            public List<WhoVaccine> whoVaccines;
            public List<TriageVitals> triageVitals;
            public int displayIndexStart;
            public List<DisplayTwoElements> displayList;
        }

        struct ClinicTabStatus
        {
            public List<ClinicAssesment> assessmentList;
            public int assessmentDisplayStartIndex;
            public int planDisplayStartIndex;
            public bool showAllAssessments;
            public List<string[]> plansOfCareList;
        }

        struct PharmacyTabStatus
        {
            public List<PharmacyPlan> activePlanList;
            public List<string[]> allPlanList;
            public int planDisplayStartIndex;
        }
        struct PharmacyPlan
        {
            public int index;
            public string[] plan;
        }

        public struct ClinicAssesment
        {
            public string name;
            public string note;
            public int assessmentIndex;
            public int arrayIndex;
            public bool checkedForDeletion;

        }

        struct RedFlags
        {
            public string name;
            public string comment;
        }

        struct WhoVaccine
        {
            public string name;
            public string date;
        }

        struct TriageVitals
        {
            public string situation;
            public string t;
            public string p;
            public string bp;
            public string o2sat;
        }

        struct DisplayTwoElements
        {
            public string Element1;
            public string Element2;
        }
        #endregion

        #region globals

        List<GroupBox> adminGroups;
        XMLHandler xmlHandler;
        FormData patientForm;
   
        ClinicData clinicData;
        XMLReader xmlReader;
        IntakeTabStatus intakeStatus;
        TriageTabStatus triageStatus;
        ClinicTabStatus clinicTabStatus;
        PharmacyTabStatus pharmacyTabStatus;
        Provider stationScribe;

        #endregion

        public Form1()
        {

            //this.FormBorderStyle = FormBorderStyle.None; //uncomment to make completely full screen, so close button
            //this.WindowState = FormWindowState.Maximized; //uncomment to make full screen
            InitializeComponent();
            intakeStatus.newPatient = true;
            patientForm = new FormData();
            clinicData = new ClinicData();

            xmlHandler = new XMLHandler(this, patientForm);
            xmlReader = new XMLReader(this, patientForm, clinicData);
			xmlReader.LoadClinicSettings();
			
            intakeStatus = new IntakeTabStatus();
            triageStatus = new TriageTabStatus();
            clinicTabStatus = new ClinicTabStatus();
            pharmacyTabStatus = new PharmacyTabStatus();

            

            //set scribe list
            string[] providers = clinicData.GetProviders();
            for (int i = 0; i < providers.Length; i++)
            {
                comboBoxScribeList.Items.Add(providers[i]);
            }
            comboBoxScribeList.Text = "NI";

            InitializeIntakeData();
            InitializeTriageData();
            InitializeClinicTabData();
            InitializeAdminTab();

        }

        private Support CreateSupport(string name, string school, string city, string comment)
        {
            string name1 = name.Replace(" ", "");
            string[] names = name1.Split(',');

            Support s = new Support();
            s.dateNull = "NI";
            s.streetNull = "NI";
            s.city = city;
            s.stateNull = "NI";
            s.postalCodeNull = "NI";
            s.telecomNull = "NI";
            s.firstName = names[1];
            s.lastname = names[0];
            s.comment = comment;
            s.schoolName = school;
            return s;
        }

       
        #region ---------------------------------Intake tab---------------------------------

        private void InitializeIntakeData()
        {
            intakeStatus = new IntakeTabStatus();
            intakeStatus.newPatient = true;
            intakeStatus.hasDOB = false;
            intakeStatus.goodDOByear = false;
            intakeStatus.goodDOBmonth = false;
            intakeStatus.goodDOBday = false;

            LabelIntakeTabDOB.Visible = false;
            TextBoxIntakeTabDOBYear.Visible = false;
            TextBoxIntakeTabDOBDay.Visible = false;
            TextBoxIntakeTabDOBMonth.Visible = false;

            ListBoxPathType.Items.Clear();
            //Add Elements to Path ListBox
            ListBoxPathType.Items.Add("linux");
            ListBoxPathType.Items.Add("windows");
            ListBoxPathType.Items.Add("osx");
            ListBoxPathType.SetSelected(0, true);

            GenderComboBox.Items.Clear();
            GenderComboBox.Items.Add("M");
            GenderComboBox.Items.Add("F");
            GenderComboBox.Text = "NI";

            ComboBoxIntakeTabLanguageList.Items.Clear();
            string[] languages = clinicData.GetLanguageList();
            for (int i = 0; i < languages.Length; i++)
            {
                ComboBoxIntakeTabLanguageList.Items.Add(languages[i]);
            }
            ComboBoxIntakeTabLanguageList.Text = "NI";

            ComboBoxIntakeTabStatusInSchool.Items.Clear();
            string[] Statii = clinicData.GetSchoolStatusList();
            for (int i = 0; i < Statii.Length; i++)
            {
                ComboBoxIntakeTabStatusInSchool.Items.Add(Statii[i]);
            }
            ComboBoxIntakeTabStatusInSchool.Text = "NI";

            comboBox1.Items.Clear();
            int[] range = clinicData.GetSchoolYearRange();
            for (int i = range[0]; i <= range[1]; i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.Text = "NI";

            ComboBoxIntakeTabHomeVillage.Items.Clear();
            ComboBoxIntakeTabPresentVillage.Items.Clear();
            string[] villages = clinicData.GetVillageList();
            for (int i = 0; i < villages.Length; i++)
            {
                ComboBoxIntakeTabHomeVillage.Items.Add(villages[i]);
                ComboBoxIntakeTabPresentVillage.Items.Add(villages[i]);
            }
            ComboBoxIntakeTabHomeVillage.Text = "NI";
            ComboBoxIntakeTabPresentVillage.Text = "NI";

            ComboBoxIntakeTabTribe.Items.Clear();
            string[] tribes = clinicData.GetTribesList();
            for (int i = 0; i < tribes.Length; i++)
            {
                ComboBoxIntakeTabTribe.Items.Add(tribes[i]);
            }
            ComboBoxIntakeTabTribe.Text = "NI";

            ComboBoxIntakeTabSchoolList.Items.Clear();
            string[] schools = clinicData.GetSchoolNames();
            for (int i = 0; i < schools.Length; i++)
            {
                ComboBoxIntakeTabSchoolList.Items.Add(schools[i]);
            }
            ComboBoxIntakeTabSchoolList.Text = "NI";

            comboBox28.Items.Clear();
            string[] teachers = clinicData.GetSupports();
            for (int i = 0; i < teachers.Length; i++)
            {
                comboBox28.Items.Add(teachers[i]);
            }
            comboBox28.Text = "NI";

            TextBoxIntakeTabPatientLastNameIn.Text = "Last Name";
            TextBoxIntakeTabPatientFirstNameIn.Text = "First Name";
            TextBoxIntakeTabDOBYear.Text = "YYYY";
            TextBoxIntakeTabDOBMonth.Text = "MM";
            TextBoxIntakeTabDOBDay.Text = "DD";

            TextBoxIntakeTabEstAgeYears.Text = "YY";
            TextBoxIntakeTabEstAgeMonths.Text = "MM";
            TextBoxIntakeTabStatedAgeYears.Text = "YY";
            TextBoxIntakeTabStatedAgeMonths.Text = "MM";

            textBox27.Text = "Z-Score";
            textBox28.Text = "BMI";
            textBox26.Text = "Weight";
            textBox29.Text = "Height";

            TextBoxIntakeTabEstAgeYears.Text = "YY";
            TextBoxIntakeTabEstAgeMonths.Text = "MM";
            TextBoxIntakeTabStatedAgeYears.Text = "YY";
            TextBoxIntakeTabStatedAgeMonths.Text = "MM";

            // data displays
            label46.Text = label47.Text = label51.Text = label88.Text = label89.Text = "";
            label90.Text = label91.Text = label92.Text = label93.Text = label94.Text = "";
            label95.Text = label96.Text = label97.Text = label98.Text = "";
        }

        #region buttons

        #region read/write patient data forms
        //intake form reload patient button
        private void IntakeTabReloadPatientButton_Click(object sender, EventArgs e)
        {
            intakeStatus.newPatient = false;
            xmlHandler.LoadCurrentPatientXML();
            ReloadPatientDataIntakeTab();
            IntakeTabResetLabelColors();
            LoadExitReviewPatient();
        }

        //intake form, creat new patient
        private void ButtonIntakeTabBeginNewForm_Click(object sender, EventArgs e)
        {
            intakeStatus.newPatient = true;
            patientForm.ResetForm();
            InitializeIntakeData();
        }

        //intake form write new patient file button
        private void ButtonIntakeTabWritePatientFile_Click(object sender, EventArgs e)
        {
            bool checkData = IntakeTabCopyFormToPatientData();
            if (checkData)
            {
                xmlHandler.WriteCurrentPatientXML();
                InitializeIntakeData();
            }
            LoadExitReviewPatient();
        }
        #endregion end read write patient data form

        #region add/remove languages
        private void ButtonIntakeTabAddLanguage_Click(object sender, EventArgs e)
        {
            if (ComboBoxIntakeTabLanguageList.Text != "NI")
            {
                if (label91.Text.Length == 0)
                {
                    label91.Text = ComboBoxIntakeTabLanguageList.Text;
                    label91.BackColor = System.Drawing.Color.LightYellow;
                }
                else if (!label91.Text.Contains(ComboBoxIntakeTabLanguageList.Text))
                {
                    label91.Text += "," + ComboBoxIntakeTabLanguageList.Text;
                    label91.BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }

        private void IntakeTabResetLanguageButton_Click(object sender, EventArgs e)
        {
            label91.BackColor = System.Drawing.Color.MistyRose;
            string languages = "";
            foreach (string l in patientForm.patientData.Languages)
            {
                if (languages.Length > 0)
                {
                    languages += ", ";
                    languages += l;
                }
                else languages = l;
            }
            label91.Text = languages;

        }
        #endregion

        private void ButtonSelectAgeType_Click(object sender, EventArgs e)
        {
            intakeStatus.hasDOB = !intakeStatus.hasDOB;

            if (intakeStatus.hasDOB)
            {
                LabelIntakeTabDOB.Visible = true;
                TextBoxIntakeTabDOBYear.Visible = true;
                TextBoxIntakeTabDOBDay.Visible = true;
                TextBoxIntakeTabDOBMonth.Visible = true;

                label1.Visible = false;
                LabelIntakeTabStatedAge.Visible = false;
                TextBoxIntakeTabEstAgeYears.Visible = false;
                TextBoxIntakeTabEstAgeMonths.Visible = false;
                TextBoxIntakeTabStatedAgeYears.Visible = false;
                TextBoxIntakeTabStatedAgeMonths.Visible = false;
                ButtonSelectAgeType.Text = "Enter Estimated/Stated Age";
            }
            else
            {
                LabelIntakeTabDOB.Visible = false;
                TextBoxIntakeTabDOBYear.Visible = false;
                TextBoxIntakeTabDOBDay.Visible = false;
                TextBoxIntakeTabDOBMonth.Visible = false;

                label1.Visible = true;
                LabelIntakeTabStatedAge.Visible = true;
                TextBoxIntakeTabEstAgeYears.Visible = true;
                TextBoxIntakeTabEstAgeMonths.Visible = true;
                TextBoxIntakeTabStatedAgeYears.Visible = true;
                TextBoxIntakeTabStatedAgeMonths.Visible = true;
                ButtonSelectAgeType.Text = "Enter Date Of Birth";
            }
        }
        #endregion

        private void ReloadPatientDataIntakeTab()
        {

            TextBoxIntakeTabPatientFirstNameIn.Text = label46.Text = patientForm.patientData.firstName;

            TextBoxIntakeTabPatientLastNameIn.Text = label47.Text = patientForm.patientData.lastName;

            GenderComboBox.Text = label51.Text = patientForm.patientData.genderCode;


            label88.Text = patientForm.patientData.dateOfBirth;
            if (patientForm.patientData.dateOfBirth != "NI")
            {
                TextBoxIntakeTabDOBYear.Text = patientForm.patientData.dateOfBirth.Substring(0, 4);
                TextBoxIntakeTabDOBMonth.Text = patientForm.patientData.dateOfBirth.Substring(4, 2);
                TextBoxIntakeTabDOBDay.Text = patientForm.patientData.dateOfBirth.Substring(6, 2);
            }
            else
            {
                TextBoxIntakeTabDOBYear.Text = "YYYY";
                TextBoxIntakeTabDOBMonth.Text = "MM";
                TextBoxIntakeTabDOBDay.Text = "DD";
                label88.Text = "NI";
            }

            label89.Text = "Years:" + patientForm.patientData.estimatedAge.Substring(0, 2) + " Months:" + patientForm.patientData.estimatedAge.Substring(2);
            TextBoxIntakeTabEstAgeYears.Text = patientForm.patientData.estimatedAge.Substring(0, 2);
            TextBoxIntakeTabEstAgeMonths.Text = patientForm.patientData.estimatedAge.Substring(2);

            label90.Text = "Years:" + patientForm.patientData.statedAge.Substring(0, 2) + " Months:" + patientForm.patientData.statedAge.Substring(2);
            TextBoxIntakeTabStatedAgeYears.Text = patientForm.patientData.statedAge.Substring(0, 2);
            TextBoxIntakeTabStatedAgeMonths.Text = patientForm.patientData.statedAge.Substring(2);

            string languages = "";
            foreach (string l in patientForm.patientData.Languages)
            {
                if (languages.Length > 0)
                {
                    languages += ", ";
                    languages += l;
                }
                else languages = l;
            }
            label91.Text = languages;
            ComboBoxIntakeTabLanguageList.Text = "Enter Language";

            string tribeVillage = patientForm.patientData.tribe + "/" + patientForm.patientData.presentVillage;
            label92.Text = tribeVillage;
            ComboBoxIntakeTabTribe.Text = patientForm.patientData.tribe;
            ComboBoxIntakeTabHomeVillage.Text = patientForm.patientData.homeVillage;
            ComboBoxIntakeTabPresentVillage.Text = patientForm.patientData.presentVillage;

            string schoolName_Teacher = "";
            foreach (Support s in patientForm.patientData.Supports)
            {
                if (schoolName_Teacher.Length > 0)
                {
                    schoolName_Teacher += ", ";
                    schoolName_Teacher += s.schoolName;
                    schoolName_Teacher += "/" + s.firstName + " " + s.lastname;
                }
                else
                {
                    schoolName_Teacher = s.schoolName;
                    schoolName_Teacher += "/" + s.firstName + " " + s.lastname;
                }
            }
            Support support1 = patientForm.patientData.Supports[0];
            ComboBoxIntakeTabSchoolList.Text = support1.schoolName;
            ComboBoxIntakeTabStatusInSchool.Text = patientForm.patientData.statusInSchool;
            comboBox28.Text = support1.firstName + " " + support1.lastname;
            label93.Text = schoolName_Teacher;


            label94.Text = patientForm.patientData.yearInSchool;

            patientForm.clinicData.documentTimeStamp = patientForm.GetTimeStamp();
            patientForm.clinicData.docID = patientForm.GetUUID();

            label95.Text = patientForm.encounterData.vitalSigns[0].value;
            label96.Text = patientForm.encounterData.vitalSigns[1].value;
            label97.Text = patientForm.encounterData.vitalSigns[2].value;
            label98.Text = patientForm.encounterData.vitalSigns[3].value;

            textBox29.Text = patientForm.encounterData.vitalSigns[0].value; //height text box
            textBox26.Text = patientForm.encounterData.vitalSigns[1].value; //weight text box
            textBox28.Text = patientForm.encounterData.vitalSigns[2].value; //bmi text box
            textBox27.Text = patientForm.encounterData.vitalSigns[3].value; //z-score text box
        }
        
        private void IntakeTabResetLabelColors()
        {
            label46.BackColor = System.Drawing.Color.MistyRose;
            label47.BackColor = System.Drawing.Color.MistyRose;
            label51.BackColor = System.Drawing.Color.MistyRose;
            label88.BackColor = System.Drawing.Color.MistyRose;
            label89.BackColor = System.Drawing.Color.MistyRose;
            label90.BackColor = System.Drawing.Color.MistyRose;
            label91.BackColor = System.Drawing.Color.MistyRose;
            label92.BackColor = System.Drawing.Color.MistyRose;
            label93.BackColor = System.Drawing.Color.MistyRose;
            label94.BackColor = System.Drawing.Color.MistyRose;
            label95.BackColor = System.Drawing.Color.MistyRose;
            label96.BackColor = System.Drawing.Color.MistyRose;
            label97.BackColor = System.Drawing.Color.MistyRose;
            label98.BackColor = System.Drawing.Color.MistyRose;
        }

        private bool IntakeTabCopyFormToPatientData()
        {
            bool checkDataGood = true;
            bool checkScribeAssigned = true;
            if (comboBoxScribeList.Text == "NI") checkScribeAssigned = false;
            if (label46.Text == "" || label47.Text == "" || label51.Text == "" || 
                label88.Text == "" || label89.Text == "" || label90.Text == "" || 
                label91.Text == "" || label92.Text == "" || label93.Text == "" || 
                label94.Text == "" || label95.Text == "" || label97.Text == "" ||
                label98.Text == "" || label89.Text == "" || !checkScribeAssigned) checkDataGood = false;

            if (checkScribeAssigned)
            {
                if (checkDataGood)
                {
                    if (intakeStatus.newPatient)
                    {
                        #region form settings
                        //set patient ID
                        DateTime dt = DateTime.Now;
                        string patientOID = clinicData.hr4eOID + ".P" + clinicData.stationName + dt.Day.ToString()
                                + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                        patientForm.patientData.id = patientOID;

                        patientForm.clinicData.xmlns = clinicData.greenCCDxmlns;
                        patientForm.clinicData.xmlns_xsi = clinicData.greenCCDxmlns_xsi;
                        patientForm.clinicData.xmlns_hr4e = clinicData.greenCCDxmlns_hr4e;
                        patientForm.clinicData.xsi_schemaLocation = clinicData.greenCCDxsi_schemaLocation;

                        patientForm.clinicData.docID = patientForm.GetUUID();
                        patientForm.clinicData.title = clinicData.docTitle;
                        patientForm.clinicData.versionNumber = clinicData.docVersion;
                        patientForm.clinicData.confidentialityCodeSystem = clinicData.confidentialityCodeSystem;
                        patientForm.clinicData.confidentialityCode = clinicData.confidentialityCode;
                        patientForm.clinicData.documentTimeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;

                        #region Null Data
                        patientForm.patientData.streetNull = "NI";
                        patientForm.patientData.stateNull = "NI";
                        patientForm.patientData.postalCodeNull = "NI";
                        patientForm.patientData.telecomNull = "NI";
                        #endregion

                        patientForm.patientData.genderCodeSystem = clinicData.genderCode;
                        patientForm.clinicData.custodianID = clinicData.custodianID;
                        patientForm.clinicData.custodianName = clinicData.custodianName;

                        patientForm.clinicData.clinicStartDate = clinicData.dateStart;
                        patientForm.clinicData.clinicEndDate = clinicData.dateEnd;
                        patientForm.clinicData.author.authorNamePrefix = clinicData.authorPrefix;
                        patientForm.clinicData.author.authorNameFirst = clinicData.authorFirstName;
                        patientForm.clinicData.author.authorNameLast = clinicData.authorLastName;
                        patientForm.clinicData.author.authorTimeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                        patientForm.clinicData.gpsCoordinates = clinicData.gps;
                        patientForm.clinicData.clinicName = clinicData.clinicName;
                        patientForm.clinicData.clinicDetails = clinicData.clinicDetails;
                        #endregion
                    }

                    #region patient data
                    patientForm.patientData.presentVillage = ComboBoxIntakeTabPresentVillage.Text;
                    patientForm.patientData.firstName = TextBoxIntakeTabPatientFirstNameIn.Text;
                    patientForm.patientData.lastName = TextBoxIntakeTabPatientLastNameIn.Text;
                    patientForm.patientData.genderCode = label51.Text;
                    patientForm.patientData.dateOfBirth = label88.Text;

                    string statedYear;
                    string statedMonth;
                    if (TextBoxIntakeTabStatedAgeYears.Text.Length == 1) statedYear = "0" + TextBoxIntakeTabStatedAgeYears.Text;
                    else statedYear = TextBoxIntakeTabStatedAgeYears.Text;
                    if (TextBoxIntakeTabStatedAgeMonths.Text.Length == 1) statedMonth = "0" + TextBoxIntakeTabStatedAgeMonths.Text;
                    else statedMonth = TextBoxIntakeTabStatedAgeMonths.Text;
                    string estYear;
                    string estMonth;
                    if (TextBoxIntakeTabEstAgeYears.Text.Length == 1) estYear = "0" + TextBoxIntakeTabEstAgeYears.Text;
                    else estYear = TextBoxIntakeTabEstAgeYears.Text;
                    if (TextBoxIntakeTabEstAgeMonths.Text.Length == 1) estMonth = "0" + TextBoxIntakeTabEstAgeMonths.Text;
                    else estMonth = TextBoxIntakeTabEstAgeMonths.Text;

                    patientForm.patientData.statedAge = statedYear + statedMonth;
                    patientForm.patientData.estimatedAge = estYear + estMonth;

                    patientForm.patientData.yearInSchool = label94.Text;
                    patientForm.patientData.statusInSchool = ComboBoxIntakeTabStatusInSchool.Text;
                    patientForm.patientData.homeVillage = ComboBoxIntakeTabHomeVillage.Text;
                    patientForm.patientData.tribe = ComboBoxIntakeTabTribe.Text;
                    #endregion patient data

                    #region add languages
                    string[] languages = label91.Text.Split(',');
                    patientForm.patientData.Languages.Clear();
                    foreach (string l in languages)
                    {
                        patientForm.patientData.Languages.Add(clinicData.GetLanguageCode(l));
                    }
                    #endregion

                    #region add scribe
                    //add scribe to provider list
                    bool scribeAlreadyInProviderList = false;
                    foreach (Provider p in patientForm.clinicData.providers)
                    {
                        if (p.id == stationScribe.id) scribeAlreadyInProviderList = true;
                    }

                    if (!scribeAlreadyInProviderList)
                    {
                        stationScribe.comment = "Intake Station Scribe";
                        patientForm.clinicData.providers.Add(stationScribe);
                    }
                    #endregion

                    #region add teacher
                    Support teacher = clinicData.GetSupportStruct(comboBox28.Text);
                    if (teacher.firstName != null)
                    {
                        bool supportAlreadyInSupportList = false;
                        foreach (Support s in patientForm.patientData.Supports)
                        {
                            if (s.lastname == teacher.lastname && s.firstName == teacher.firstName &&
                                s.schoolName == teacher.schoolName) supportAlreadyInSupportList = true;
                        }

                        if (!supportAlreadyInSupportList)
                        {
                            patientForm.patientData.Supports.Add(teacher);
                        }
                    }
                    else
                    {
                        teacher = CreateSupport(comboBox28.Text, ComboBoxIntakeTabSchoolList.Text, "NI", "unlisted school");
                    }
                    #endregion end add teacher

                    #region add initial vitals
                    VitalSign height = new VitalSign();
                    height.id = "NI";//DEBUG need height ID;
                    height.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                    height.status = "completed";
                    height.unit = "cm";
                    height.value = textBox29.Text;
                    height.code = "NI";//DEBUG need code
                    height.codeSystem = clinicData.vitalSignCode;
                    height.displayName = "Body Height";//DEBUG need official display names
                    height.comment = "Intake Vitals";

                    VitalSign weight = new VitalSign();
                    weight.id = "NI";//DEBUG need height ID;
                    weight.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                    weight.status = "completed";
                    weight.unit = "kg";
                    weight.value = textBox26.Text;
                    weight.code = "NI";//DEBUG need code
                    weight.codeSystem = clinicData.vitalSignCode;
                    weight.displayName = "Body Weight";//DEBUG need official display names
                    weight.comment = "Intake Vitals";

                    VitalSign bmi = new VitalSign();
                    bmi.id = "NI";//DEBUG need height ID;
                    bmi.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                    bmi.status = "completed";
                    bmi.unit = "NI";
                    bmi.value = textBox28.Text;
                    bmi.code = "NI";//DEBUG need code
                    bmi.codeSystem = clinicData.vitalSignCode;
                    bmi.displayName = "BMI";//DEBUG need official display names
                    bmi.comment = "Intake Vitals";

                    VitalSign zScore = new VitalSign();
                    zScore.id = "NI";//DEBUG need height ID;
                    zScore.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                    zScore.status = "completed";
                    zScore.unit = "NI";
                    zScore.value = textBox27.Text;
                    zScore.code = "NI";//DEBUG need code
                    zScore.codeSystem = clinicData.vitalSignCode;
                    zScore.displayName = "Z-Score";//DEBUG need official display names
                    zScore.comment = "Intake Vitals";

                    if (patientForm.encounterData.vitalSigns.Count < 1) patientForm.encounterData.vitalSigns.Add(height);
                    else patientForm.encounterData.vitalSigns[0] = height;
                    if (patientForm.encounterData.vitalSigns.Count < 2) patientForm.encounterData.vitalSigns.Add(weight);
                    else patientForm.encounterData.vitalSigns[1] = weight;
                    if (patientForm.encounterData.vitalSigns.Count < 3) patientForm.encounterData.vitalSigns.Add(bmi);
                    else patientForm.encounterData.vitalSigns[2] = bmi;
                    if (patientForm.encounterData.vitalSigns.Count < 4) patientForm.encounterData.vitalSigns.Add(zScore);
                    else patientForm.encounterData.vitalSigns[3] = zScore;

                    #endregion end add initial vitals

                    #region initialize encounter
                    patientForm.encounterData.encounter.id = patientForm.GetUUID();
                    patientForm.encounterData.encounter.codeSystem = clinicData.encounterTypeCode;
                    patientForm.encounterData.encounter.code = clinicData.encountertype;
                    patientForm.encounterData.encounter.textDescription = clinicData.encountertype;
                    patientForm.encounterData.encounter.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                    patientForm.encounterData.encounter.reason = "Chief Complaint:";
                    patientForm.encounterData.encounter.notes = "Encounter Notes:";
                    #endregion

                    return true;
                }
                else
                {
                    MessageBox.Show("Could Not Write, Data Missing", "Print Error");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please Assign A Scribe","Print Error");
                return false;
            }
        }

        #region update intake display

        #region vitals
        //height
        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            double height = 0.000;
            double weight = 0.000;
            try
            {
                height = Convert.ToDouble(textBox29.Text);
                weight = Convert.ToDouble(textBox26.Text);
                double bmi = (weight / ((height / 100) * (height / 100)));
                textBox28.Text = bmi.ToString();

            }
            catch { }

            try
            {
                height = Convert.ToDouble(textBox29.Text);
                label95.Text = textBox29.Text;
                label95.BackColor = System.Drawing.Color.LightYellow;
            }
            catch
            {
                label95.Text = "";
                label95.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        //weight
        private void textBox26_TextChanged(object sender, EventArgs e)
        {
            double height = 0.000;
            double weight = 0.000;
            try
            {
                height = Convert.ToDouble(textBox29.Text);
                weight = Convert.ToDouble(textBox26.Text);
                double bmi = (weight / ((height / 100) * (height / 100)));
                string bmiText = bmi.ToString();
                textBox28.Text = bmiText.Substring(0, 5);
            }
            catch { }


            try
            {
                weight = Convert.ToDouble(textBox26.Text);
                label96.Text = textBox26.Text;
                label96.BackColor = System.Drawing.Color.LightYellow;
            }
            catch
            {
                label96.Text = "";
                label96.BackColor = System.Drawing.Color.MistyRose;
            }
        }


        //bmi
        private void textBox28_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double temp = Convert.ToDouble(textBox28.Text);
                label97.Text = textBox28.Text;
                label97.BackColor = System.Drawing.Color.LightYellow;
            }
            catch
            {
                label97.Text = "";
                label97.BackColor = System.Drawing.Color.MistyRose;
            }

        }

        //z-score
        private void textBox27_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double temp = Convert.ToDouble(textBox27.Text);
                label98.Text = textBox27.Text;
                label98.BackColor = System.Drawing.Color.LightYellow;
            }
            catch
            {
                label98.Text = "";
                label98.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        #endregion end vitals

        #region name/gender
        private void TextBoxIntakeTabPatientFirstNameIn_TextChanged(object sender, EventArgs e)
        {
            label46.Text = TextBoxIntakeTabPatientFirstNameIn.Text;
            label46.BackColor = System.Drawing.Color.LightYellow;
        }

        private void TextBoxIntakeTabPatientLastNameIn_TextChanged(object sender, EventArgs e)
        {
            label47.Text = TextBoxIntakeTabPatientLastNameIn.Text;
            label47.BackColor = System.Drawing.Color.LightYellow;
        }

        private void GenderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            label51.Text = GenderComboBox.Text;
            label51.BackColor = System.Drawing.Color.LightYellow;
        }
        #endregion

        #region school/teacher
        private void comboBox28_TextChanged(object sender, EventArgs e)
        {
            string[] supportData = clinicData.GetSupportData(comboBox28.Text);
            if (supportData != null) ComboBoxIntakeTabSchoolList.Text = supportData[2];

            label93.Text = ComboBoxIntakeTabSchoolList.Text + "/" + comboBox28.Text;
            label93.BackColor = System.Drawing.Color.LightYellow;
        }

        private void ComboBoxIntakeTabSchoolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            label93.Text = ComboBoxIntakeTabSchoolList.Text + "/" + comboBox28.Text;
            label93.BackColor = System.Drawing.Color.LightYellow;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label94.Text = comboBox1.Text;
            label94.BackColor = System.Drawing.Color.LightYellow;
        }
        #endregion

        #region age
        private void TextBoxIntakeTabDOBYear_TextChanged(object sender, EventArgs e)
        {
            if (intakeStatus.hasDOB)
            {
                if (TextBoxIntakeTabDOBYear.Text.Length == 4)
                {
                    int killjoy = 0;
                    int die;
                    try
                    {
                        int year = Convert.ToInt16(TextBoxIntakeTabDOBYear.Text);
                        if (year < 1950 || year > DateTime.Now.Year)
                        {
                            intakeStatus.goodDOByear = false;
                            die = year / killjoy;
                        }
                        else intakeStatus.goodDOByear = true;
                    }
                    catch
                    {
                        intakeStatus.goodDOByear = false;
                        label88.Text = "";
                        label88.BackColor = System.Drawing.Color.MistyRose;
                    }
                }
                else intakeStatus.goodDOByear = false;

                if (intakeStatus.goodDOByear && intakeStatus.goodDOBmonth && intakeStatus.goodDOBday)
                {
                    SetEstStateAgeWithDOB();
                }
                else
                {
                    label88.Text = "";
                    label88.BackColor = System.Drawing.Color.MistyRose;
                    label89.Text = "";
                    label89.BackColor = System.Drawing.Color.MistyRose;
                    label90.Text = "";
                    label90.BackColor = System.Drawing.Color.MistyRose;
                }
            }
        }

        private void TextBoxIntakeTabDOBDay_TextChanged(object sender, EventArgs e)
        {
            if (intakeStatus.hasDOB)
            {
                if (TextBoxIntakeTabDOBDay.Text.Length >= 1 && TextBoxIntakeTabDOBDay.Text.Length <= 2)
                {
                    int killjoy = 0;
                    int die;
                    try
                    {
                        int day = Convert.ToInt16(TextBoxIntakeTabDOBDay.Text);
                        if (day < 1 || day > 32)
                        {
                            intakeStatus.goodDOBday = false;
                            die = day / killjoy;
                        }
                        else intakeStatus.goodDOBday = true;
                    }
                    catch
                    {
                        intakeStatus.goodDOBday = false;
                        label88.Text = "";
                        label88.BackColor = System.Drawing.Color.MistyRose;
                    }
                }
                else intakeStatus.goodDOBday = false;

                if (intakeStatus.goodDOByear && intakeStatus.goodDOBmonth && intakeStatus.goodDOBday)
                {
                    SetEstStateAgeWithDOB();
                }
                else
                {
                    label88.Text = "";
                    label88.BackColor = System.Drawing.Color.MistyRose;
                    label89.Text = "";
                    label89.BackColor = System.Drawing.Color.MistyRose;
                    label90.Text = "";
                    label90.BackColor = System.Drawing.Color.MistyRose;
                }
            }
        }

        private void TextBoxIntakeTabDOBMonth_TextChanged(object sender, EventArgs e)
        {
            if (intakeStatus.hasDOB)
            {
                if (TextBoxIntakeTabDOBMonth.Text.Length >= 1 && TextBoxIntakeTabDOBMonth.Text.Length <= 2)
                {
                    int killjoy = 0;
                    int die;
                    try
                    {
                        int month = Convert.ToInt16(TextBoxIntakeTabDOBMonth.Text);
                        if (month < 1 || month > 12)
                        {
                            intakeStatus.goodDOBmonth = false;
                            die = month / killjoy;
                        }
                        else intakeStatus.goodDOBmonth = true;
                    }
                    catch
                    {
                        intakeStatus.goodDOBmonth = false;
                        label88.Text = "";
                        label88.BackColor = System.Drawing.Color.MistyRose;
                    }
                }
                else intakeStatus.goodDOBmonth = false;

                if (intakeStatus.goodDOByear && intakeStatus.goodDOBmonth && intakeStatus.goodDOBday)
                {
                    SetEstStateAgeWithDOB();
                }
                else
                {
                    label88.Text = "";
                    label88.BackColor = System.Drawing.Color.MistyRose;
                    label89.Text = "";
                    label89.BackColor = System.Drawing.Color.MistyRose;
                    label90.Text = "";
                    label90.BackColor = System.Drawing.Color.MistyRose;
                }
            }
        }

        private void SetEstStateAgeWithDOB()
        {
            string tempMonth;
            string tempDay;

            if (TextBoxIntakeTabDOBMonth.Text.Length == 1) tempMonth = "0" + TextBoxIntakeTabDOBMonth.Text;
            else tempMonth = TextBoxIntakeTabDOBMonth.Text;
            if (TextBoxIntakeTabDOBDay.Text.Length == 1) tempDay = "0" + TextBoxIntakeTabDOBDay.Text;
            else tempDay = TextBoxIntakeTabDOBDay.Text;

            int year = Convert.ToInt16(TextBoxIntakeTabDOBYear.Text);
            int mm = Convert.ToInt16(tempMonth);
            int day = Convert.ToInt16(tempDay);
            DateTime dob = new DateTime(year, mm, 1);
            int adjust = 0;
            if (DateTime.Today.Month < dob.Month) adjust = -1;
            int AgeInYears = DateTime.Today.Year - dob.Year + adjust;
            int AgeInMonths = (DateTime.Today.Month - dob.Month) * adjust;

            string ageYear = AgeInYears.ToString();
            string ageMonth = AgeInMonths.ToString(); ;
            if (ageYear.Length == 1) ageYear = "0" + ageYear;
            if (ageMonth.Length == 1) ageMonth = "0" + ageMonth;

            TextBoxIntakeTabEstAgeYears.Text = ageYear;
            TextBoxIntakeTabStatedAgeYears.Text = ageYear;
            TextBoxIntakeTabEstAgeMonths.Text = ageMonth;
            TextBoxIntakeTabStatedAgeMonths.Text = ageMonth;


            label88.Text = TextBoxIntakeTabDOBYear.Text + tempMonth +
                tempDay + clinicData.timeZone;
            label88.BackColor = System.Drawing.Color.LightYellow;
        }

        private void TextBoxIntakeTabEstAgeYears_TextChanged(object sender, EventArgs e)
        {
            DisplayEstimatedAge();
        }

        private void TextBoxIntakeTabEstAgeMonths_TextChanged(object sender, EventArgs e)
        {
            DisplayEstimatedAge();
        }

        private void TextBoxIntakeTabStatedAgeYears_TextChanged(object sender, EventArgs e)
        {
            DisplayStatedAge();
        }

        private void TextBoxIntakeTabStatedAgeMonths_TextChanged(object sender, EventArgs e)
        {
            DisplayStatedAge();
        }

        private void DisplayEstimatedAge()
        {
            int intOfDeath = 0;
            try
            {
                string tempMonth;
                string tempYear;
                if (TextBoxIntakeTabEstAgeYears.Text.Length < 1 || TextBoxIntakeTabEstAgeYears.Text.Length > 2) intOfDeath = 19 / intOfDeath;
                if (TextBoxIntakeTabEstAgeMonths.Text.Length < 1 || TextBoxIntakeTabEstAgeMonths.Text.Length > 2) intOfDeath = 19 / intOfDeath;

                if (TextBoxIntakeTabEstAgeYears.Text.Length == 1) tempYear = "0" + TextBoxIntakeTabEstAgeYears.Text;
                else tempYear = TextBoxIntakeTabEstAgeYears.Text;
                if (TextBoxIntakeTabEstAgeMonths.Text.Length == 1) tempMonth = "0" + TextBoxIntakeTabEstAgeMonths.Text;
                else tempMonth = TextBoxIntakeTabEstAgeMonths.Text;

                int testYear = Convert.ToInt16(TextBoxIntakeTabEstAgeYears.Text);
                int testMonth = Convert.ToInt16(TextBoxIntakeTabEstAgeMonths.Text);

                label89.Text = "Years: " + tempYear + " Months: " + tempMonth;
                label89.BackColor = System.Drawing.Color.LightYellow;
            }
            catch
            {
                label89.Text = "";
                label90.BackColor = System.Drawing.Color.MistyRose;
            }
            if (!intakeStatus.hasDOB)
            {
                label88.Text = "NI";
                label88.BackColor = System.Drawing.Color.LightYellow;
            }
        }

        private void DisplayStatedAge()
        {
            int intOfDeath = 0;
            try
            {
                string tempMonth;
                string tempYear;
                if (TextBoxIntakeTabStatedAgeYears.Text.Length < 1 || TextBoxIntakeTabStatedAgeYears.Text.Length > 2) intOfDeath = 19 / intOfDeath;
                if (TextBoxIntakeTabStatedAgeMonths.Text.Length < 1 || TextBoxIntakeTabStatedAgeMonths.Text.Length > 2) intOfDeath = 19 / intOfDeath;

                if (TextBoxIntakeTabStatedAgeYears.Text.Length == 1) tempYear = "0" + TextBoxIntakeTabStatedAgeYears.Text;
                else tempYear = TextBoxIntakeTabStatedAgeYears.Text;
                if (TextBoxIntakeTabStatedAgeMonths.Text.Length == 1) tempMonth = "0" + TextBoxIntakeTabStatedAgeMonths.Text;
                else tempMonth = TextBoxIntakeTabStatedAgeMonths.Text;

                int testYear = Convert.ToInt16(TextBoxIntakeTabStatedAgeYears.Text);
                int testMonth = Convert.ToInt16(TextBoxIntakeTabStatedAgeMonths.Text);

                label90.Text = "Years: " + tempYear + " Months: " + tempMonth;
                label90.BackColor = System.Drawing.Color.LightYellow;

            }
            catch
            {
                label90.Text = "";
                label90.BackColor = System.Drawing.Color.MistyRose;
            }
            if (!intakeStatus.hasDOB)
            {
                label88.Text = "NI";
                label88.BackColor = System.Drawing.Color.LightYellow;
            }
        }

        #endregion

        private void ComboBoxIntakeTabTribe_textChanged(object sender, EventArgs e)
        {
            label92.Text = ComboBoxIntakeTabTribe.Text + "/" + ComboBoxIntakeTabPresentVillage.Text;
            label92.BackColor = System.Drawing.Color.LightYellow;
        }

        #endregion end update intake display

        #endregion------------------------------End Intake tab-------------------------------

        #region ---------------------------------Triage tab---------------------------------
        private void InitializeTriageData()
        {
            triageStatus.showFlags = true;
            triageStatus.redFlags = new List<RedFlags>();
            groupBox6.Visible = true;
            label33.Visible = true;
            TriageTabChiefComplaintInput.Visible = true;
            groupBox10.Visible = false;
            groupBox12.Visible = false;
            triageStatus.displayIndexStart = 0;
            triageStatus.displayList = new List<DisplayTwoElements>();
            triageStatus.triageVitals = new List<TriageVitals>();
            triageStatus.whoVaccines = new List<WhoVaccine>();
            TriageUpdateDisplay();
            label75.Text = label100.Text = label101.Text = label102.Text = "";
            label103.Text = label104.Text = label105.Text = label106.Text = "";
            label107.Text = label108.Text = label109.Text = label110.Text = "";
            label111.Text = label112.Text = label113.Text = label114.Text = "";
            label115.Text = label116.Text = "";
            //label115.Text = label116.Text = label24.Text = label25.Text = "";
            label31.Text = label32.Text = label38.Text = label39.Text = "";
            //label30.Text = label31.Text = label32.Text = label38.Text = label39.Text = "";
            label34.Text = label35.Text = label36.Text = label37.Text = "";
            textBox22.Text = textBox21.Text = textBox20.Text = TriageTabChiefComplaintInput.Text = "";
            textBox23.Text = textBox18.Text = textBox17.Text = textBox16.Text = textBox19.Text = "";
            button11.Visible = false;
            button12.Visible = false;
        }

        #region triage buttons

        private void TriageTabToggle_Click(object sender, EventArgs e)
        {
            triageStatus.showFlags = !triageStatus.showFlags;
            if (triageStatus.showFlags)
            {
                groupBox6.Visible = true;
                label33.Visible = true;
                TriageTabChiefComplaintInput.Visible = true;
                groupBox10.Visible = false;
                groupBox12.Visible = false;
                TriageTabToggle.Text = "Edit Vaccines/Vitals";
            }
            else
            {
                groupBox6.Visible = false;
                label33.Visible = false;
                TriageTabChiefComplaintInput.Visible = false;
                groupBox10.Visible = true;
                groupBox12.Visible = true;
                TriageTabToggle.Text = "Edit Red Flags/Chief Complaint";
            }
        }

        //load data in triage tab
        private void button8_Click(object sender, EventArgs e)
        {
            InitializeTriageData();
            patientForm.ResetForm();
            xmlHandler.LoadCurrentPatientXML();

            label34.Text = label38.Text = label36.Text = label39.Text = label37.Text = label35.Text = "";


            label34.Text = "Patient: " + patientForm.patientData.lastName + ", " + patientForm.patientData.firstName;

            string gender = "";
            if (patientForm.patientData.genderCode == "M" || patientForm.patientData.genderCode == "m") gender = "male";
            else if (patientForm.patientData.genderCode == "F" || patientForm.patientData.genderCode == "f") gender = "female";
            else gender = patientForm.patientData.genderCode;

            label38.Text = patientForm.patientData.estimatedAge.Substring(0, 2) + "yr " + patientForm.patientData.estimatedAge.Substring(2) + "mo old " + gender;

            label36.Text = "Ht: " + patientForm.encounterData.vitalSigns[0].value + " cm Wt: " + patientForm.encounterData.vitalSigns[1].value + " kg BMI: " + patientForm.encounterData.vitalSigns[2].value + " Z-Score: " + patientForm.encounterData.vitalSigns[3].value;

            label35.Text = "Grade: " + patientForm.patientData.yearInSchool;

            string school_teacher = "";
            foreach (Support s in patientForm.patientData.Supports)
            {
                if (school_teacher.Length > 0)
                {
                    school_teacher += ", " + s.schoolName + "/" + s.firstName + " " + s.lastname;
                }
                else school_teacher = s.schoolName + "/" + s.firstName + " " + s.lastname;
            }
            label37.Text = school_teacher;

            string languages = "";
            foreach (string l in patientForm.patientData.Languages)
            {
                if (languages.Length > 0)
                {
                    languages += ", " + l;
                }
                else languages = l;
            }
            label39.Text = "Languages: " + languages;

            LoadStoredTriageData();

            TriageDisplayListUpdate();
            TriageUpdateDisplay();
            LoadExitReviewPatient();
        }

        //store triage tab data
        private void button9_Click(object sender, EventArgs e)
        {
            StoreTriageData();
            xmlHandler.WriteCurrentPatientXML();
            LoadExitReviewPatient();
            InitializeTriageData();
        }

        private void LoadStoredTriageData()
        {
            triageStatus.triageVitals = new List<TriageVitals>();
            label75.Text = patientForm.encounterData.encounter.reason;

            #region add vitals
            foreach (VitalSign v in patientForm.encounterData.vitalSigns)
            {
                if (v.comment.Contains("Triage"))
                {
                    string situation = v.comment.Replace("Triage: ", "");
                    situation = situation.Substring(8);
                    bool triageVitalCreated = false;
                    int index = -1;

                    TriageVitals t = new TriageVitals();
                    for( int i = 0; i < triageStatus.triageVitals.Count; i++)
                    {
                        TriageVitals tv = triageStatus.triageVitals[i]; ;
                        if (tv.situation == situation)
                        {
                            triageVitalCreated = true;
                            t = tv;
                            index = i;
                            break;
                        }
                    }

                    if (!triageVitalCreated)
                    {
                        t = new TriageVitals();
                        t.situation = situation;
                    }

                    if (v.displayName == "Tempurature")
                    {
                        t.t = v.value;
                    }
                    else if (v.displayName == "pulse")
                    {
                        t.p = v.value;
                    }
                    else if (v.displayName == "blood pressure")
                    {
                        t.bp = v.value;
                    }
                    else if (v.displayName == "o2 saturation")
                    {
                        t.o2sat = v.value;
                    }

                    if (triageVitalCreated)
                    {
                        triageStatus.triageVitals[index] = t;
                    }
                    else triageStatus.triageVitals.Add(t);
                }
            }
            #endregion


        }

        private void StoreTriageData()
        {
            #region store vitals
            foreach (TriageVitals tv in triageStatus.triageVitals)
            {
                string timestamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                string hourminute = tv.situation.Substring(1, 2) + tv.situation.Substring(4, 2) + "00" + clinicData.timeZone;
                timestamp += hourminute;

                if (tv.t != null)
                {
                    VitalSign t = new VitalSign();
                    t.id = "NI";//DEBUG need ID;
                    t.timeStamp = timestamp;
                    t.status = "completed";
                    t.unit = "c";
                    t.value = tv.t;
                    t.code = "NI";//DEBUG need code
                    t.codeSystem = clinicData.vitalSignCode;
                    t.displayName = "Tempurature";//DEBUG need official display names
                    t.comment = "Triage: " + tv.situation;
                    patientForm.encounterData.vitalSigns.Add(t);
                }

                if (tv.p != null)
                {
                    VitalSign p = new VitalSign();
                    p.id = "NI";//DEBUG need ID;
                    p.timeStamp = timestamp;
                    p.status = "completed";
                    p.unit = "NI";
                    p.value = tv.p;
                    p.code = "NI";//DEBUG need code
                    p.codeSystem = clinicData.vitalSignCode;
                    p.displayName = "pulse";//DEBUG need official display names
                    p.comment = "Triage: " + tv.situation;
                    patientForm.encounterData.vitalSigns.Add(p);
                }

                if (tv.bp != null)
                {
                    VitalSign b = new VitalSign();
                    b.id = "NI";//DEBUG need ID;
                    b.timeStamp = timestamp;
                    b.status = "completed";
                    b.unit = "NI";
                    b.value = tv.t;
                    b.code = "NI";//DEBUG need code
                    b.codeSystem = clinicData.vitalSignCode;
                    b.displayName = "blood pressure";//DEBUG need official display names
                    b.comment = "Triage: " + tv.situation;
                    patientForm.encounterData.vitalSigns.Add(b);
                }

                if (tv.o2sat != null)
                {
                    VitalSign o = new VitalSign();
                    o.id = "NI";//DEBUG need ID;
                    o.timeStamp = timestamp;
                    o.status = "completed";
                    o.unit = "NI";
                    o.value = tv.t;
                    o.code = "NI";//DEBUG need code
                    o.codeSystem = clinicData.vitalSignCode;
                    o.displayName = "o2 saturation";//DEBUG need official display names
                    o.comment = "Triage: " + tv.situation;
                    patientForm.encounterData.vitalSigns.Add(o);
                }
            }
            #endregion end vitals

            patientForm.encounterData.encounter.reason = label75.Text;

            #region create red flags
            foreach (RedFlags r in triageStatus.redFlags)
            {
                Condition c = new Condition();
                c.code = "NI";
                c.codeSystem = clinicData.problemCode;
                c.code = "00000"; //DEBUG need codes!
                c.problemName = r.name;
                c.displayName = "Red Flags";
                c.startYear = "1900";  //DEBUG start dates???
                c.comment = r.comment;
                patientForm.encounterData.conditions.Add(c);
            }
            #endregion

            #region create Assessments
            Condition cc = new Condition();
            cc.code = "NI";
            cc.codeSystem = clinicData.problemCode;
            cc.code = "1"; //using 1 for active assessments, 0 for deleted
            cc.problemName = label75.Text;
            cc.displayName = "Assessment";
            cc.startYear = "1900";  //DEBUG start dates???
            cc.comment = "Enter Notes Here";
            patientForm.encounterData.conditions.Add(cc);
            foreach (RedFlags r in triageStatus.redFlags)
            {
                Condition c = new Condition();
                c.code = "NI";
                c.codeSystem = clinicData.problemCode;
                c.code = "1"; //DEBUG need codes!
                c.problemName = r.name;
                c.displayName = "Assessment";
                c.startYear = "1900";  //DEBUG start dates???
                c.comment = r.comment;
                patientForm.encounterData.conditions.Add(c);
            }
            #endregion

            #region create immunizations
            foreach (WhoVaccine w in triageStatus.whoVaccines)
            {
                Immunization i = new Immunization();
                i.code = "000000"; //DEBUG need codes
                i.codeSystem = clinicData.immunizationCode;
                i.comment = "Clinic Tab";
                i.dateAdministered = w.date;
                i.displayName = w.name;
                i.text = "WHO Vaccines";
                patientForm.encounterData.immunizations.Add(i);
            }
            #endregion end immunizations


        }

        #endregion end triage buttons

        #region triage element entry loggers
        private void TriageTabChiefComplaintInput_TextChanged(object sender, EventArgs e)
        {
            TriageTabChiefComplaintInput.Text.Replace("|", "");
            label75.Text = TriageTabChiefComplaintInput.Text;
        }

        #region red flag textbox updates

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox20, "Cough/Dyspnea");
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox21, "Diarrhea +/- dehydration:");
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox22, "Fever and stiff neck:");
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox23, "e/o Measles:");
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox18, "Ear problems/mastoiditis:");
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox17, "Eye problems/FHx blindness/trachoma:");
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox16, "Severe pallor c/w anemia:");
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            UpdateRedFlag(textBox19, "Current/prior intestinal parasites:");
        }

        public void UpdateRedFlag(TextBox textbox, string name)
        {
            string removeSpaces = textbox.Text.Replace(" ", "");
            if (removeSpaces.Length != 0)
            {
                int editIndex = triageStatus.redFlags.Count;
                for (int i = 0; i < triageStatus.redFlags.Count; i++)
                {
                    RedFlags rf = triageStatus.redFlags[i];
                    if (rf.name == name)
                    {
                        editIndex = i;
                        break;
                    }
                }
                if (editIndex == triageStatus.redFlags.Count)
                {
                    RedFlags rf = new RedFlags();
                    rf.name = name;
                    rf.comment = textbox.Text;
                    triageStatus.redFlags.Add(rf);
                }
                else
                {
                    RedFlags rf = triageStatus.redFlags[editIndex];
                    rf.comment = textbox.Text;
                    triageStatus.redFlags[editIndex] = rf;
                }
            }
            else
            {
                int removeIndex = -1;
                for (int i = 0; i < triageStatus.redFlags.Count; i++)
                {
                    RedFlags rf = triageStatus.redFlags[i];
                    if (rf.name == name)
                    {
                        removeIndex = i;
                        break;
                    }
                }

                if (removeIndex >= 0 && triageStatus.redFlags.Count > 0)
                {
                    triageStatus.redFlags.RemoveAt(removeIndex);
                }
            }
            TriageDisplayListUpdate();
            TriageUpdateDisplay();
        }
        
        #endregion end red flag textbox updates

        #region who vaccine textbox/checkbox updates
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                WhoVaccine w = new WhoVaccine();
                w.name = "BCG, OPV-0";
                string date = " Date: NI";
                if (textBox33.Text.Length > 0) date = textBox33.Text;
                w.date = date;
                triageStatus.whoVaccines.Add(w);
            }
            else
            {
                int removeIndex = -1;
                for (int i = 0; i < triageStatus.whoVaccines.Count; i++)
                {
                    WhoVaccine w = triageStatus.whoVaccines[i];
                    if (w.name == "BCG, OPV-0")
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex >= 0) triageStatus.whoVaccines.RemoveAt(removeIndex);
            }
            TriageDisplayListUpdate();
            TriageUpdateDisplay();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                WhoVaccine w = new WhoVaccine();
                w.name = "DPT-1, OPV-1, HBV-1";
                string date = "NI";
                if (textBox34.Text.Length > 0) date = textBox34.Text;
                w.date = date;
                triageStatus.whoVaccines.Add(w);
            }
            else
            {
                int removeIndex = -1;
                for (int i = 0; i < triageStatus.whoVaccines.Count; i++)
                {
                    WhoVaccine w = triageStatus.whoVaccines[i];
                    if (w.name == "DPT-1, OPV-1, HBV-1")
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex >= 0) triageStatus.whoVaccines.RemoveAt(removeIndex);
            }
            TriageDisplayListUpdate();
            TriageUpdateDisplay();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                WhoVaccine w = new WhoVaccine();
                w.name = "DPT-2, OPV-2, HBV-2";
                string date = "NI";
                if (textBox35.Text.Length > 0) date = textBox35.Text;
                w.date = date;
                triageStatus.whoVaccines.Add(w);
            }
            else
            {
                int removeIndex = -1;
                for (int i = 0; i < triageStatus.whoVaccines.Count; i++)
                {
                    WhoVaccine w = triageStatus.whoVaccines[i];
                    if (w.name == "DPT-2, OPV-2, HBV-2")
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex >= 0) triageStatus.whoVaccines.RemoveAt(removeIndex);
            }
            TriageDisplayListUpdate();
            TriageUpdateDisplay();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                WhoVaccine w = new WhoVaccine();
                w.name = "DPT-3, OPV-3, HBV-3";
                string date = "NI";
                if (textBox36.Text.Length > 0) date = textBox36.Text;
                w.date = date;
                triageStatus.whoVaccines.Add(w);
            }
            else
            {
                int removeIndex = -1;
                for (int i = 0; i < triageStatus.whoVaccines.Count; i++)
                {
                    WhoVaccine w = triageStatus.whoVaccines[i];
                    if (w.name == "DPT-3, OPV-3, HBV-3")
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex >= 0) triageStatus.whoVaccines.RemoveAt(removeIndex);
            }
            TriageDisplayListUpdate();
            TriageUpdateDisplay();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                WhoVaccine w = new WhoVaccine();
                w.name = "Measles, Yellow Fever";
                string date = "NI";
                if (textBox37.Text.Length > 0) date = textBox37.Text;
                w.date = date;
                triageStatus.whoVaccines.Add(w);
            }
            else
            {
                int removeIndex = -1;
                for (int i = 0; i < triageStatus.whoVaccines.Count; i++)
                {
                    WhoVaccine w = triageStatus.whoVaccines[i];
                    if (w.name == "Measles, Yellow Fever")
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex >= 0) triageStatus.whoVaccines.RemoveAt(removeIndex);
            }
            TriageDisplayListUpdate();
            TriageUpdateDisplay();
        }

        private void textBox34_TextChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            checkBox2.Checked = true;
        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = false;
            checkBox3.Checked = true;
        }

        private void textBox36_TextChanged(object sender, EventArgs e)
        {
            checkBox4.Checked = false;
            checkBox4.Checked = true;
        }

        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            checkBox5.Checked = false;
            checkBox5.Checked = true;
        }

        #endregion end who vaccine textbox/checkbox updates

        //add vitals in triage
        private void button10_Click(object sender, EventArgs e)
        {
            bool recordVital = false;
            if (textBox31.Text.Length != 0) recordVital = true;
            if (textBox30.Text.Length != 0) recordVital = true;
            if (textBox32.Text.Length != 0) recordVital = true;
            if (textBox25.Text.Length != 0) recordVital = true;


            if (recordVital)
            {
                TriageVitals tv = new TriageVitals();
                DateTime d = DateTime.Now;
                string hour = d.Hour.ToString();
                if (hour.Length == 1) hour = "0" + hour;
                string minute = d.Minute.ToString();
                if (minute.Length == 1) minute = "0" + minute;
                tv.situation = "(" + hour + ":" + minute + ") " + textBox24.Text;
                if (textBox25.Text.Length > 0)
                {
                    tv.t = textBox25.Text;
                }

                if (textBox32.Text.Length > 0)
                {
                    tv.p = textBox32.Text;
                }

                if (textBox30.Text.Length > 0)
                {
                    tv.bp = textBox30.Text;
                }

                if (textBox31.Text.Length > 0)
                {
                    tv.o2sat = textBox31.Text;
                }

                triageStatus.triageVitals.Add(tv);

                TriageDisplayListUpdate();
                TriageUpdateDisplay();
            }


        }

        private void TriageDisplayListUpdate()
        {
            triageStatus.displayList.Clear();
            if (triageStatus.redFlags.Count > 0)
            {
                DisplayTwoElements d = new DisplayTwoElements();
                d.Element1 = "Red Flags";
                d.Element2 = "";
                triageStatus.displayList.Add(d);
                foreach (RedFlags rf in triageStatus.redFlags)
                {
                    DisplayTwoElements d2 = new DisplayTwoElements();
                    d2.Element1 = rf.name;
                    d2.Element2 = rf.comment;
                    triageStatus.displayList.Add(d2);
                }
            }

            if (triageStatus.whoVaccines.Count > 0)
            {
                DisplayTwoElements d = new DisplayTwoElements();
                d.Element1 = "Who Vaccines:";
                d.Element2 = "";
                triageStatus.displayList.Add(d);
                foreach (WhoVaccine w in triageStatus.whoVaccines)
                {
                    DisplayTwoElements d2 = new DisplayTwoElements();
                    d2.Element1 = w.name;
                    d2.Element2 = w.date;
                    triageStatus.displayList.Add(d2);
                }
            }

            if (triageStatus.triageVitals.Count > 0)
            {
                DisplayTwoElements d = new DisplayTwoElements();
                d.Element1 = "Vital Signs:";
                d.Element2 = "";
                triageStatus.displayList.Add(d);
                foreach (TriageVitals v in triageStatus.triageVitals)
                {
                    DisplayTwoElements d2 = new DisplayTwoElements();
                    d2.Element1 = v.situation;
                    string printString = "";
                    if (v.t != null) printString = "T: " + v.t;
                    if (v.p != null)
                    {
                        if (printString.Length != 0) printString += ". ";
                        printString += "P: " + v.p;
                    }
                    if (v.bp != null)
                    {
                        if (printString.Length != 0) printString += ". ";
                        printString += "BP: " + v.bp;
                    }
                    if (v.o2sat != null)
                    {
                        if (printString.Length != 0) printString += ". ";
                        printString += "O2 Sat: " + v.o2sat;
                    }

                    d2.Element2 = printString;
                    triageStatus.displayList.Add(d2);
                }
            }
        }

        private void TriageUpdateDisplay()
        {
            if (triageStatus.displayList.Count > 17)
            {
                button11.Visible = true;
                button12.Visible = true;
            }
            else
            {
                button11.Visible = false;
                button12.Visible = false;
            }
            
            if (triageStatus.displayList.Count <= 17) //checks if entire list will fits on 1 page
            {
                triageStatus.displayIndexStart = 0;
            }
            else if (triageStatus.displayList.Count - triageStatus.displayIndexStart <= 17)
            {
                triageStatus.displayIndexStart = triageStatus.displayList.Count - 17;
            }

            if (triageStatus.displayList.Count > 0)
            {
                label100.Text = triageStatus.displayList[0 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[0 + triageStatus.displayIndexStart].Element2;
            }
            else label100.Text = "";

            if (triageStatus.displayList.Count > 1)
            {
                label101.Text = triageStatus.displayList[1 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[1 + triageStatus.displayIndexStart].Element2;
            }
            else label101.Text = "";

            if (triageStatus.displayList.Count > 2)
            {
                label102.Text = triageStatus.displayList[2 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[2 + triageStatus.displayIndexStart].Element2;
            }
            else label102.Text = "";

            if (triageStatus.displayList.Count > 3)
            {
                label103.Text = triageStatus.displayList[3 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[3 + triageStatus.displayIndexStart].Element2;
            }
            else label103.Text = "";

            if (triageStatus.displayList.Count > 4)
            {
                label104.Text = triageStatus.displayList[4 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[4 + triageStatus.displayIndexStart].Element2;
            }
            else label104.Text = "";

            if (triageStatus.displayList.Count > 5)
            {
                label105.Text = triageStatus.displayList[5 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[5 + triageStatus.displayIndexStart].Element2;
            }
            else label105.Text = "";

            if (triageStatus.displayList.Count > 6)
            {
                label106.Text = triageStatus.displayList[6 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[6 + triageStatus.displayIndexStart].Element2;
            }
            else label106.Text = "";

            if (triageStatus.displayList.Count > 7)
            {
                label107.Text = triageStatus.displayList[7 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[7 + triageStatus.displayIndexStart].Element2;
            }
            else label107.Text = "";

            if (triageStatus.displayList.Count > 8)
            {
                label108.Text = triageStatus.displayList[8 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[8 + triageStatus.displayIndexStart].Element2;
            }
            else label108.Text = "";

            if (triageStatus.displayList.Count > 9)
            {
                label109.Text = triageStatus.displayList[9 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[9 + triageStatus.displayIndexStart].Element2;
            }
            else label109.Text = "";

            if (triageStatus.displayList.Count > 10)
            {
                label110.Text = triageStatus.displayList[10 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[10 + triageStatus.displayIndexStart].Element2;
            }
            else label110.Text = "";

            if (triageStatus.displayList.Count > 11)
            {
                label111.Text = triageStatus.displayList[11 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[11 + triageStatus.displayIndexStart].Element2;
            }
            else label111.Text = "";

            if (triageStatus.displayList.Count > 12)
            {
                label112.Text = triageStatus.displayList[12 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[12 + triageStatus.displayIndexStart].Element2;
            }
            else label112.Text = "";

            if (triageStatus.displayList.Count > 13)
            {
                label113.Text = triageStatus.displayList[13 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[13 + triageStatus.displayIndexStart].Element2;
            }
            else label113.Text = "";

            if (triageStatus.displayList.Count > 14)
            {
                label114.Text = triageStatus.displayList[14 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[14 + triageStatus.displayIndexStart].Element2;
            }
            else label114.Text = "";

            if (triageStatus.displayList.Count > 15)
            {
                label115.Text = triageStatus.displayList[15 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[15 + triageStatus.displayIndexStart].Element2;
            }
            else label115.Text = "";

            if (triageStatus.displayList.Count > 16)
            {
                label116.Text = triageStatus.displayList[16 + triageStatus.displayIndexStart].Element1 + ": " + triageStatus.displayList[16 + triageStatus.displayIndexStart].Element2;
            }
            else label116.Text = "";


            if (triageStatus.displayIndexStart == 0)
            {
                button11.Visible = false;
            }
            else if ((triageStatus.displayIndexStart + 17) == triageStatus.displayList.Count)
            {
                button12.Visible = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            triageStatus.displayIndexStart--;
            TriageUpdateDisplay();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            triageStatus.displayIndexStart++;
            TriageUpdateDisplay();
        }

        #endregion end triage element entry loggers

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox1.Checked = true;
        }

        #endregion ----------------------------End Triage tab-------------------------------

        #region ---------------------------------Clinic tab---------------------------------

        private void InitializeClinicTabData()
        {
            List<string> medImmList = new List<string>();
            string[] meds = clinicData.GetMedList();
            string[] imms = clinicData.GetImmList();

            foreach (string m in meds)
            {
                medImmList.Add(m);
            }

            foreach (string i in imms)
            {
                medImmList.Add(i);
            }


            comboBox24.Items.Clear();
            comboBox25.Items.Clear();
            comboBox26.Items.Clear();
            comboBox27.Items.Clear();
            comboBox29.Items.Clear();
            foreach (string mi in medImmList)
            {
                comboBox24.Items.Add(mi);
                comboBox25.Items.Add(mi);
                comboBox26.Items.Add(mi);
                comboBox27.Items.Add(mi);
                comboBox29.Items.Add(mi);
            }


            clinicTabStatus.showAllAssessments = true;
            groupBox14.Visible = false;
            label32.Visible = false;
            ClinicTabNotesEntry.Visible = false;
            label123.Visible = false;
            button17.Visible = false;
            richTextBox2.Visible = false;
            label124.Visible = false;
            label76.Visible = false;
            label121.Visible = false;
            label122.Visible = false;
            comboBox22.Items.Clear();
            comboBox22.Items.Add("Negative");
            comboBox22.Items.Add("Positive");
            comboBox22.Items.Add("Equivocal");
            comboBox23.Items.Clear();
            comboBox23.Items.Add("Normal");
            comboBox23.Items.Add("Abnormal");
            label119.Text = clinicData.hemoglobinRange;
            richTextBox3.ReadOnly = true;
            richTextBox3.BackColor = System.Drawing.Color.PaleTurquoise;
            richTextBox4.ReadOnly = true;
            richTextBox4.BackColor = System.Drawing.Color.PaleTurquoise;
            richTextBox5.ReadOnly = true;
            richTextBox5.BackColor = System.Drawing.Color.PaleTurquoise;



            richTextBox2.ReadOnly = true;
            richTextBox2.BackColor = System.Drawing.Color.PaleTurquoise;
            ClinicTabNotesEntry.Text = patientForm.encounterData.encounter.notes;
            richTextBox2.Text = patientForm.encounterData.encounter.notes;
            label76.Text = "";
            label121.Text = "";
            label122.Text = "";
            label52.Text = label53.Text = label54.Text = "";
            label55.Text = label56.Text = label57.Text = "";
            textBox51.ReadOnly = true;
            textBox52.ReadOnly = true;
            textBox53.ReadOnly = true;
            textBox54.ReadOnly = true;
            textBox55.ReadOnly = true;
            textBox51.BackColor = System.Drawing.Color.PaleTurquoise;
            textBox52.BackColor = System.Drawing.Color.PaleTurquoise;
            textBox53.BackColor = System.Drawing.Color.PaleTurquoise;
            textBox54.BackColor = System.Drawing.Color.PaleTurquoise;
            textBox55.BackColor = System.Drawing.Color.PaleTurquoise;

            checkBox6.Visible = false;
            checkBox7.Visible = false;
            checkBox8.Visible = false;
            checkBox9.Visible = false;
            checkBox10.Visible = false;
            textBox51.Visible = false;
            textBox52.Visible = false;
            textBox53.Visible = false;
            textBox54.Visible = false;
            textBox55.Visible = false;
            textBox42.Visible = false;
            textBox43.Visible = false;
            textBox44.Visible = false;
            textBox45.Visible = false;
            textBox46.Visible = false;
            checkBox11.Visible = false;
            button16.Visible = false;

            groupBox16.Visible = false;
            label132.Visible = false;
            label131.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;

            label133.Visible = comboBox24.Visible = textBox47.Visible = false;
            label134.Visible = comboBox25.Visible = textBox48.Visible = false;
            label135.Visible = comboBox26.Visible = textBox49.Visible = false;
            label136.Visible = comboBox27.Visible = textBox50.Visible = false;
            label137.Visible = comboBox29.Visible = textBox56.Visible = false;



        }

        private void ClinicTabLoadPatient_Click(object sender, EventArgs e)
        {
            
            InitializeClinicTabData();
            LoadCurrentPatientInClinic();
            ClinicTabNotesEntry.Text = patientForm.encounterData.encounter.notes;
            richTextBox2.Text = patientForm.encounterData.encounter.notes;
            clinicTabStatus.assessmentList = new List<ClinicAssesment>();
            clinicTabStatus.plansOfCareList = new List<string[]>();
            clinicTabStatus.assessmentDisplayStartIndex = 0;
            clinicTabStatus.planDisplayStartIndex = 0;
            GetAssessmentList();
            UpdateAssesmentsDisplay();

            richTextBox3.Text = richTextBox2.Text;

            clinicTabStatus.plansOfCareList.Clear();
            foreach (PlanOfCare p in patientForm.encounterData.planofCares)
            {
                string text = p.text;
                text += "|" + p.id;
                string[] plan = text.Split('|');
                if (plan[3] == "1")
                {
                    clinicTabStatus.plansOfCareList.Add(plan);
                }
            }

			//add old lab results
			foreach (LabResult l in patientForm.encounterData.labResults)
			{
				if (l.displayName == "Hemoglobin Test")
				{
					textBox38.Text = l.value;
					label119.Text = l.referenceRange;
				}
				else if (l.displayName == "Orasure Test")
				{
					comboBox22.Text = l.value;
				}
				else if (l.displayName == "Stool O&P Test")
				{
					int splitIndex = l.value.IndexOf(" Notes:");
					comboBox23.Text = l.value.Substring(0, splitIndex - 1);
					textBox39.Text = l.value.Substring(splitIndex + 8);
				}
			}
			
            LoadExitReviewPatient();
        }

        private void GetAssessmentList()
        {
            clinicTabStatus.assessmentList.Clear();
            int assessmentCount = 1;
            for (int i = 0; i < patientForm.encounterData.conditions.Count; i++)
            {
                Condition c = patientForm.encounterData.conditions[i];
                if (c.displayName == "Assessment")
                {
                    if (c.code == "1")
                    {
                        ClinicAssesment a = new ClinicAssesment();
                        a.assessmentIndex = assessmentCount;
                        a.name = c.problemName;
                        a.note = c.comment;
                        a.arrayIndex = i;
                        a.checkedForDeletion = false;
                        clinicTabStatus.assessmentList.Add(a);
                    }
                    assessmentCount++;
                }

            }
        }
        
        private void UpdateAssesmentsDisplay() 
        {
            //List<ClinicAssesments> assessmentList = new List<ClinicAssesments>();
            //int assessmentDisplayStartIndex = 0;

            if (clinicTabStatus.assessmentList.Count > 5)
            {
                button20.Visible = true;
                button19.Visible = true;
            }
            else
            {
                button20.Visible = false;
                button19.Visible = false;
            }

            if (clinicTabStatus.assessmentList.Count <= 5) //checks if entire list will fits on 1 page
            {
                clinicTabStatus.assessmentDisplayStartIndex = 0;
            }
            else if (clinicTabStatus.assessmentList.Count - clinicTabStatus.assessmentDisplayStartIndex <= 5)
            {
                clinicTabStatus.assessmentDisplayStartIndex = clinicTabStatus.assessmentList.Count - 5;
            }

            if (clinicTabStatus.assessmentDisplayStartIndex == 0)
            {
                button20.Visible = false;
            }
            else button20.Visible = true;

            if (clinicTabStatus.assessmentDisplayStartIndex + 5 >= clinicTabStatus.assessmentList.Count)
            {
                button19.Visible = false;
            }
            else button19.Visible = true;

            UpdateClinicAssessmentListCells(checkBox6, textBox51, textBox42, 0);
            UpdateClinicAssessmentListCells(checkBox7, textBox52, textBox43, 1);
            UpdateClinicAssessmentListCells(checkBox8, textBox53, textBox44, 2);
            UpdateClinicAssessmentListCells(checkBox9, textBox54, textBox45, 3);
            UpdateClinicAssessmentListCells(checkBox10, textBox55, textBox46, 4);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            clinicTabStatus.assessmentDisplayStartIndex--;
            UpdateAssesmentsDisplay();
            ClinicCheckIfChecked(checkBox6);
            ClinicCheckIfChecked(checkBox7);
            ClinicCheckIfChecked(checkBox8);
            ClinicCheckIfChecked(checkBox9);
            ClinicCheckIfChecked(checkBox10);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            clinicTabStatus.assessmentDisplayStartIndex++;
            UpdateAssesmentsDisplay();
            ClinicCheckIfChecked(checkBox6);
            ClinicCheckIfChecked(checkBox7);
            ClinicCheckIfChecked(checkBox8);
            ClinicCheckIfChecked(checkBox9);
            ClinicCheckIfChecked(checkBox10);
        }

        public void UpdateClinicAssessmentListCells(CheckBox checkBox, TextBox assessName, TextBox assessNote, int count)
        {
            if (clinicTabStatus.assessmentList.Count > count)
            {
                assessName.Text = clinicTabStatus.assessmentList[count + clinicTabStatus.assessmentDisplayStartIndex].name;
                assessName.Visible = true;
                assessNote.Text = clinicTabStatus.assessmentList[count + clinicTabStatus.assessmentDisplayStartIndex].note;
                assessNote.Visible = true;
                checkBox.Text = clinicTabStatus.assessmentList[count + clinicTabStatus.assessmentDisplayStartIndex].assessmentIndex.ToString();
                checkBox.Visible = true;
            }
            else
            {
                assessName.Text = "";
                assessName.Visible = false;
                assessNote.Text = "";
                assessNote.Visible = false;
                checkBox.Text = "";
                checkBox.Visible = false;
            }
        }

        //load existing patient in to triage;
        private void LoadCurrentPatientInClinic()
        {
			clinicTabStatus.assessmentDisplayStartIndex = 0;
            patientForm.ResetForm();
            xmlHandler.LoadCurrentPatientXML();
            label52.Text = "Patient: " + patientForm.patientData.lastName + ", " + patientForm.patientData.firstName;

            string gender = "";
            if (patientForm.patientData.genderCode == "M" || patientForm.patientData.genderCode == "m") gender = "male";
            else if (patientForm.patientData.genderCode == "F" || patientForm.patientData.genderCode == "f") gender = "female";
            else gender = patientForm.patientData.genderCode;
            label53.Text = patientForm.patientData.estimatedAge.Substring(0, 2) + "yr " + patientForm.patientData.estimatedAge.Substring(2) + "mo old " + gender;

            label54.Text = "Ht: " + patientForm.encounterData.vitalSigns[0].value + " cm Wt: " + patientForm.encounterData.vitalSigns[1].value + " kg BMI: " + patientForm.encounterData.vitalSigns[2].value + " Z-Score: " + patientForm.encounterData.vitalSigns[3].value;

            label57.Text = "Grade: " + patientForm.patientData.yearInSchool;

            string school_teacher = "";
            foreach (Support s in patientForm.patientData.Supports)
            {
                if (school_teacher.Length > 0)
                {
                    school_teacher += ", " + s.schoolName + "/" + s.firstName + " " + s.lastname;
                }
                else school_teacher = s.schoolName + "/" + s.firstName + " " + s.lastname;
            }
            label56.Text = school_teacher;

            string languages = "";
            foreach (string l in patientForm.patientData.Languages)
            {
                if (languages.Length > 0)
                {
                    languages += ", " + l;
                }
                else languages = l;
            }
            label55.Text = "Languages: " + languages;
        }

        #region notes and test results
        private void button17_Click(object sender, EventArgs e)
        {
            ClinicTabNotesEntry.Text = ClinicTabNotesEntry.Text.Replace("\n-", "\n");
            ClinicTabNotesEntry.Text = ClinicTabNotesEntry.Text.Replace("\n", "\n-");
            patientForm.encounterData.encounter.notes = ClinicTabNotesEntry.Text;
            richTextBox2.Text = patientForm.encounterData.encounter.notes;
            richTextBox3.Text = ClinicTabNotesEntry.Text;
        }

        private void comboBox22_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox22.Text == "Negative" || comboBox22.Text == "Positive" ||
                comboBox22.Text == "Equivocal")
            {
                label76.Text = "Orasure: " + comboBox22.Text;
            }
            else label76.Text = "";
        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int test = Convert.ToInt16(textBox38.Text);
                label121.Text = "Hemoglobin: " + textBox38.Text + " " + label119.Text;
            }
            catch
            {
                textBox38.Text = "";
            }
        }

        private void comboBox23_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckStoolSandP();
        }

        private void textBox39_TextChanged(object sender, EventArgs e)
        {
            CheckStoolSandP();
        }
        
        private void CheckStoolSandP()
        {
            if (comboBox23.Text == "Normal" || comboBox23.Text == "Abnormal")
            {
                if (textBox39.Text != "")
                {
                    label122.Text = "Stool O&&P: -" + comboBox23.Text + "- " +
                            textBox39.Text;
                }
            }
        }
        #endregion end notes and test results

        //REMOVE SELECTED ASSESSMENTS
        private void button18_Click(object sender, EventArgs e)
        {

            foreach (ClinicAssesment ca in clinicTabStatus.assessmentList)
            {
                if (ca.checkedForDeletion)
                {
                    Condition c = patientForm.encounterData.conditions[ca.arrayIndex];
                    c.code = "0";
                    patientForm.encounterData.conditions[ca.arrayIndex] = c;
                }
            }
            GetAssessmentList();
            UpdateAssesmentsDisplay();

        }

        private void ClinicCheckIfChecked(CheckBox checkBox)
        {
            if (checkBox.Visible)
            {
                int assessmentIndex = Convert.ToInt16(checkBox.Text);
                ClinicAssesment ca = clinicTabStatus.assessmentList[assessmentIndex - 1];
                checkBox.Checked = ca.checkedForDeletion;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            ClinicToggleRemoveChecked(checkBox6);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            ClinicToggleRemoveChecked(checkBox7);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            ClinicToggleRemoveChecked(checkBox8);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            ClinicToggleRemoveChecked(checkBox9);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            ClinicToggleRemoveChecked(checkBox10);
        }

        private void ClinicToggleRemoveChecked(CheckBox checkBox)
        {
            int assessmentIndex = Convert.ToInt16(checkBox.Text);

            ClinicAssesment c = clinicTabStatus.assessmentList[assessmentIndex - 1];
            c.checkedForDeletion = checkBox.Checked;
            clinicTabStatus.assessmentList[assessmentIndex - 1] = c;
        }

        private void textBox42_TextChanged(object sender, EventArgs e)
        {
            ClinicUpdateAssessmentNotes(textBox42, checkBox6);
        }

        private void ClinicUpdateAssessmentNotes(TextBox textBox, CheckBox checkBox)
        {
            if (checkBox.Text != "")
            {
                int assessmentIndex = Convert.ToInt16(checkBox.Text);
                ClinicAssesment c = clinicTabStatus.assessmentList[assessmentIndex-1];
                c.note = textBox.Text;
                clinicTabStatus.assessmentList[assessmentIndex-1] = c;

                Condition co = patientForm.encounterData.conditions[c.arrayIndex];
                co.comment = c.note;
                patientForm.encounterData.conditions[c.arrayIndex] = co;
            }

        }

        private void textBox43_TextChanged(object sender, EventArgs e)
        {
            ClinicUpdateAssessmentNotes(textBox43, checkBox7);
        }

        private void textBox44_TextChanged(object sender, EventArgs e)
        {
            ClinicUpdateAssessmentNotes(textBox44, checkBox8);
        }

        private void textBox45_TextChanged(object sender, EventArgs e)
        {
            ClinicUpdateAssessmentNotes(textBox45, checkBox9);
        }

        private void textBox46_TextChanged(object sender, EventArgs e)
        {
            ClinicUpdateAssessmentNotes(textBox46, checkBox10);
        }

        private void ButtonClinicSaveData_Click(object sender, EventArgs e)
        {
            patientForm.encounterData.encounter.notes = ClinicTabNotesEntry.Text;

            //write hemoglobin test
            if (textBox38.Text.Length > 0)
            {
                LabResult h = new LabResult();
                h.displayName = "Hemoglobin Test";
                h.value = textBox38.Text;
                h.referenceRange = label119.Text;
                h.comment = "Clinic Tab";
                h.id = patientForm.GetUUID();
                h.interpretCode = "NI";
                h.interpretCodeSystem = clinicData.resultsInterpretCode;
                h.referenceRange = clinicData.hemoglobinRange;
                h.status = "Completed";
                h.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                h.typeCode = "NI";
                h.typeCodeSystem = clinicData.resultTypeCode;
                h.unit = "NI";
                patientForm.encounterData.labResults.Add(h);
            }

            //write orasure test
            if (comboBox22.Text.Length > 0)
            {
                LabResult o = new LabResult();
                o.displayName = "Orasure Test";
                o.value = comboBox22.Text;
                o.referenceRange = "NI";
                o.comment = "Clinic Tab";
                o.id = patientForm.GetUUID();
                o.interpretCode = "NI";
                o.interpretCodeSystem = clinicData.resultsInterpretCode;
                o.referenceRange = "NI";
                o.status = "Completed";
                o.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                o.typeCode = "NI";
                o.typeCodeSystem = clinicData.resultTypeCode;
                o.unit = "NI";
                patientForm.encounterData.labResults.Add(o);
            }

            //write stool O&P test
            if (textBox39.Text.Length > 0 || comboBox23.Text.Length > 0)
            {
                LabResult s = new LabResult();
                s.displayName = "Stool O&P Test";
                s.value = comboBox23.Text + " Notes: " + textBox39.Text;
                s.referenceRange = "NI";
                s.comment = "Clinic Tab";
                s.id = patientForm.GetUUID();
                s.interpretCode = "NI";
                s.interpretCodeSystem = clinicData.resultsInterpretCode;
                s.referenceRange = "NI";
                s.status = "Completed";
                s.timeStamp = patientForm.GetTimeStamp() + clinicData.timeZone;
                s.typeCode = "NI";
                s.typeCodeSystem = clinicData.resultTypeCode;
                s.unit = "NI";
                patientForm.encounterData.labResults.Add(s);
            }

            foreach (string[] plan in clinicTabStatus.plansOfCareList)
            {
                string s = plan[0];
                for (int i = 1; i < plan.Length - 2; i++)
                {
                    s += "|" + plan[i];
                }
                s += "|" + plan[plan.Length - 2];

                PlanOfCare p = new PlanOfCare();
                p.text = s;
                p.id = plan[plan.Length - 1];
                p.code = "";
                p.codeSystem = "";
                p.displayName = "";
                int addIndex = Convert.ToInt16(plan[0]);
                if (patientForm.encounterData.planofCares.Count <= addIndex)
                {
                    while (patientForm.encounterData.planofCares.Count <= addIndex)
                    {
                        patientForm.encounterData.planofCares.Add(new PlanOfCare());
                    }
                }
                patientForm.encounterData.planofCares[addIndex] = p;
            }

            xmlHandler.WriteCurrentPatientXML();
            LoadExitReviewPatient();
            InitializeClinicTabData();
        }

        //add assessment
        private void button13_Click(object sender, EventArgs e)
        {
			if (textBox40.Text != "")
			{
            	Condition c = new Condition();
            	c.code = "NI";
            	c.codeSystem = clinicData.problemCode;
           	 	c.code = "1"; //DEBUG need codes!
            	c.problemName = textBox40.Text;
            	c.displayName = "Assessment";
            	c.startYear = "1900";  //DEBUG start dates???
            	c.comment = textBox41.Text;
            	patientForm.encounterData.conditions.Add(c);

            	GetAssessmentList();

            	if (clinicTabStatus.assessmentList.Count > 5)
            	{	
                	clinicTabStatus.assessmentDisplayStartIndex = clinicTabStatus.assessmentList.Count - 5;
            	}

            	UpdateAssesmentsDisplay();
				textBox40.Text = "";
				textBox41.Text = "";
			}
        }

        //Button show Assessments
        private void button16_Click(object sender, EventArgs e)
        {
            groupBox15.Visible = true;
            label125.Visible = true;
            richTextBox3.Visible = true;

            label132.Visible = false;
            label131.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;
            groupBox16.Visible = false;
            groupBox14.Visible = false;
            checkBox11.Visible = false;
            label130.Visible = false;
            ClinicTabNotesEntry.Visible = false;
            label123.Visible = false;
            button17.Visible = false;
            richTextBox2.Visible = false;
            label124.Visible = false;
            label76.Visible = false;
            label121.Visible = false;
            label122.Visible = false;
            button15.Visible = true;
            button16.Visible = false;
            button14.Visible = true;

            richTextBox3.Text = patientForm.encounterData.encounter.notes;
        }

        //Button Other Findings and Test Results
        private void button15_Click(object sender, EventArgs e)
        {
            groupBox15.Visible = false;
            groupBox16.Visible = false;
            label125.Visible = false;
            richTextBox3.Visible = false;
            label132.Visible = false;
            label131.Visible = false;
            richTextBox4.Visible = false;
            richTextBox5.Visible = false;
            checkBox11.Visible = false;

            groupBox14.Visible = true;
            label130.Visible = true;
            ClinicTabNotesEntry.Visible = true;
            label123.Visible = true;
            button17.Visible = true;
            richTextBox2.Visible = true;
            label124.Visible = true;
            label76.Visible = true;
            label121.Visible = true;
            label122.Visible = true;
            button15.Visible = false;
            button16.Visible = true;
            button14.Visible = true;
        }

        //show Plan Of Care
        private void button14_Click(object sender, EventArgs e)
        {
            groupBox16.Visible = true;
            label132.Visible = true;
            label131.Visible = true;
            richTextBox4.Visible = true;
            richTextBox5.Visible = true;
            checkBox11.Visible = true;

            groupBox15.Visible = false;
            label125.Visible = false;
            richTextBox3.Visible = false;
            groupBox14.Visible = false;
            label130.Visible = false;
            ClinicTabNotesEntry.Visible = false;
            label123.Visible = false;
            button17.Visible = false;
            richTextBox2.Visible = false;
            label124.Visible = false;
            label76.Visible = false;
            label121.Visible = false;
            label122.Visible = false;
            button15.Visible = true;
            button16.Visible = true;
            button14.Visible = false;

            richTextBox4.Text = patientForm.encounterData.encounter.notes;

            string displayNotesAndTests = patientForm.encounterData.encounter.notes;
            if (comboBox22.Text.Length > 0 || textBox38.Text.Length > 0 || textBox39.Text.Length > 0 || comboBox23.Text.Length > 0)
            {
                displayNotesAndTests += "\n-Test Results:";
            }

            if (comboBox22.Text.Length > 0)
            {
                string o = "\n-Orasure:  " + comboBox22.Text;
                displayNotesAndTests += o;
            }
            if (textBox38.Text.Length > 0)
            {
                string h = "\n-Hemoglobin Test: " + textBox38.Text + "  " + clinicData.hemoglobinRange;
                displayNotesAndTests += h;
            }

            if (textBox39.Text.Length > 0 || comboBox23.Text.Length > 0)
            {
                string s = "\n-Stool O&P:  " + comboBox23.Text + " Notes: " + textBox39.Text;
                displayNotesAndTests += s;
            }

            richTextBox4.Text = displayNotesAndTests;

            string assessmentNotes = "";
            int assessmentCount = 1;
            for (int i = 0; i < patientForm.encounterData.conditions.Count; i++)
            {
                Condition c = patientForm.encounterData.conditions[i];
                if (c.displayName == "Assessment")
                {
                    if (!clinicTabStatus.showAllAssessments)
                    {
                        if (c.code == "1")
                        {
                            if (assessmentNotes.Length > 0)
                            {
                                assessmentNotes += "\n#" + assessmentCount.ToString() + ": " + c.problemName + ". Notes: " + c.comment;
                            }
                            else assessmentNotes = "#" + assessmentCount.ToString() + ": " + c.problemName + ". Notes: " + c.comment;
                        }
                    }
                    else
                    {
                        string status = "";
                        if (c.code == "0") status = " (REMOVED)";
                        if (assessmentNotes.Length > 0)
                        {
                            assessmentNotes += "\n#" + assessmentCount.ToString() + ": " + c.problemName + ". Notes: " + c.comment + status;
                        }
                        else assessmentNotes = "#" + assessmentCount.ToString() + ": " + c.problemName + ". Notes: " + c.comment + status;
                    }
                    assessmentCount++;
                }
            }
            richTextBox5.Text = assessmentNotes;

            ClinicTabLoadPlansOfCare();
        }

        private void ClinicTabLoadPlansOfCare()
        {
            //clinicTabStatus.planDisplayStartIndex = 0;
            if (clinicTabStatus.plansOfCareList.Count > 5)
            {
                button22.Visible = true;
                button23.Visible = true;
            }
            else
            {
                button22.Visible = false;
                button23.Visible = false;
            }

            if (clinicTabStatus.plansOfCareList.Count <= 5) //checks if entire list will fits on 1 page
            {
                clinicTabStatus.planDisplayStartIndex = 0;
            }
            else if (clinicTabStatus.plansOfCareList.Count - clinicTabStatus.planDisplayStartIndex <= 5)
            {
                clinicTabStatus.planDisplayStartIndex = clinicTabStatus.plansOfCareList.Count - 5;
            }

            if (clinicTabStatus.planDisplayStartIndex == 0)
            {
                button22.Visible = false;
            }
            else button22.Visible = true;

            if (clinicTabStatus.planDisplayStartIndex + 5 == clinicTabStatus.plansOfCareList.Count)
            {
                button23.Visible = false;
            }
            else button23.Visible = true;
            UpdateClinicPlanListCells(label133, comboBox24, textBox47, 0);
            UpdateClinicPlanListCells(label134, comboBox25, textBox48, 1);
            UpdateClinicPlanListCells(label135, comboBox26, textBox49, 2);
            UpdateClinicPlanListCells(label136, comboBox27, textBox50, 3);
            UpdateClinicPlanListCells(label137, comboBox29, textBox56, 4);
        }

        public void UpdateClinicPlanListCells(Label label, ComboBox comboBox, TextBox textBox, int count)
        {
            if (clinicTabStatus.plansOfCareList.Count > count)
            {
                string[] plan = clinicTabStatus.plansOfCareList[count + clinicTabStatus.planDisplayStartIndex];   
                label.Text = plan[0];
                label.Visible = true;
                comboBox.Text = plan[1];
                comboBox.Visible = true;
                textBox.Text = plan[2];
                textBox.Visible = true;
            }
            else
            {
                label.Text = "";
                label.Visible = false;
                comboBox.Text = "";
                comboBox.Visible = false;
                textBox.Text = "";
                textBox.Visible = false;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            clinicTabStatus.showAllAssessments = checkBox11.Checked;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int index = clinicTabStatus.plansOfCareList.Count + 1;
            string newPlan = index.ToString() + "|||1||";
            newPlan += "|" + patientForm.GetUUID();
            string[] planArray = newPlan.Split('|');
            clinicTabStatus.plansOfCareList.Add(planArray);
            ClinicTabLoadPlansOfCare();

        }

        private void button22_Click(object sender, EventArgs e)
        {
            clinicTabStatus.planDisplayStartIndex--;
            ClinicTabLoadPlansOfCare();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            clinicTabStatus.planDisplayStartIndex++;
            ClinicTabLoadPlansOfCare();
        }

        private void comboBox24_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex];
            plan[1] = comboBox24.Text;
        }

        private void textBox47_TextChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex];
            plan[2] = textBox47.Text;
        }

        private void comboBox25_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 1];
            plan[1] = comboBox25.Text;
        }

        private void textBox48_TextChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 1];
            plan[2] = textBox48.Text;
        }

        private void comboBox26_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 2];
            plan[1] = comboBox26.Text;
        }

        private void textBox49_TextChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 2];
            plan[2] = textBox49.Text;
        }

        private void comboBox27_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 3];
            plan[1] = comboBox27.Text;
        }

        private void textBox50_TextChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 3];
            plan[2] = textBox50.Text;
        }

        private void comboBox29_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 4];
            plan[1] = comboBox29.Text;
        }

        private void textBox56_TextChanged(object sender, EventArgs e)
        {
            string[] plan = clinicTabStatus.plansOfCareList[clinicTabStatus.planDisplayStartIndex + 4];
            plan[2] = textBox56.Text;
        }

        #endregion ----------------------------End Clinic tab-------------------------------

        #region ---------------------------------Pharmacy tab---------------------------------
        //load existing patient in to Pharmacy;
        private void LoadCurrentPatientInLabPharmacy()
        {
            patientForm.ResetForm();
            xmlHandler.LoadCurrentPatientXML();
            label138.Text = "Patient: " + patientForm.patientData.lastName + ", " + patientForm.patientData.firstName;

            string gender = "";
            if (patientForm.patientData.genderCode == "M" || patientForm.patientData.genderCode == "m") gender = "male";
            else if (patientForm.patientData.genderCode == "F" || patientForm.patientData.genderCode == "f") gender = "female";
            else gender = patientForm.patientData.genderCode;
            label142.Text = patientForm.patientData.estimatedAge.Substring(0, 2) + "yr " + patientForm.patientData.estimatedAge.Substring(2) + "mo old " + gender;

            label140.Text = "Ht: " + patientForm.encounterData.vitalSigns[0].value + " cm Wt: " + patientForm.encounterData.vitalSigns[1].value + " kg BMI: " + patientForm.encounterData.vitalSigns[2].value + " Z-Score: " + patientForm.encounterData.vitalSigns[3].value;

            label139.Text = "Grade: " + patientForm.patientData.yearInSchool;

            string school_teacher = "";
            foreach (Support s in patientForm.patientData.Supports)
            {
                if (school_teacher.Length > 0)
                {
                    school_teacher += ", " + s.schoolName + "/" + s.firstName + " " + s.lastname;
                }
                else school_teacher = s.schoolName + "/" + s.firstName + " " + s.lastname;
            }
            label141.Text = school_teacher;

            string languages = "";
            foreach (string l in patientForm.patientData.Languages)
            {
                if (languages.Length > 0)
                {
                    languages += ", " + l;
                }
                else languages = l;
            }
            label143.Text = "Languages: " + languages;

            pharmacyTabStatus.planDisplayStartIndex = 0;
            pharmacyTabStatus.activePlanList = new List<PharmacyPlan>();
            pharmacyTabStatus.allPlanList = new List<string[]>();

            foreach (PlanOfCare plan in patientForm.encounterData.planofCares)
            {
                string text = plan.text;
                text += "|" + plan.id;
                //string[] newPlan = text.Split('|');

                //if (newPlan[1] != "") pharmacyTabStatus.allPlanList.Add(plan.text.Split('|'));
                pharmacyTabStatus.allPlanList.Add(plan.text.Split('|'));
            }
        }


        //Pharmacy Load Patient
        private void button24_Click(object sender, EventArgs e)
        {
            patientForm.ResetForm();
            xmlHandler.LoadCurrentPatientXML();
            LoadCurrentPatientInLabPharmacy();
            PharmacyTabLoadPlansOfCare();
            LoadExitReviewPatient();
        }

        private void PharmacyTabLoadPlansOfCare()
        {
            pharmacyTabStatus.activePlanList.Clear();
            for (int i = 0; i < pharmacyTabStatus.allPlanList.Count; i++)
            {
                PharmacyPlan p = new PharmacyPlan();
                p.plan = pharmacyTabStatus.allPlanList[i];
                p.index = i;
                if (checkBox25.Checked)
                {
                    pharmacyTabStatus.activePlanList.Add(p);
                }
                else
                {
                    if (p.plan[3] == "1")
                    {
                        pharmacyTabStatus.activePlanList.Add(p);
                    }
                }
            }
            //clinicTabStatus.planDisplayStartIndex = 0;
            if (pharmacyTabStatus.activePlanList.Count > 13)
            {
                button27.Visible = true;
                button28.Visible = true;
            }
            else
            {
                button27.Visible = false;
                button28.Visible = false;
            }

            if (pharmacyTabStatus.activePlanList.Count <= 13) //checks if entire list will fits on 1 page
            {
                pharmacyTabStatus.planDisplayStartIndex = 0;
            }
            else if (pharmacyTabStatus.activePlanList.Count - pharmacyTabStatus.planDisplayStartIndex <= 13)
            {
                pharmacyTabStatus.planDisplayStartIndex = pharmacyTabStatus.activePlanList.Count - 13;
            }

            if (pharmacyTabStatus.planDisplayStartIndex == 0)
            {
                button27.Visible = false;
            }
            else button27.Visible = true;

            if (pharmacyTabStatus.planDisplayStartIndex + 13 == pharmacyTabStatus.activePlanList.Count)
            {
                button28.Visible = false;
            }
            else button28.Visible = true;

            UpdatePharmacyPlanListCells(textBox96, textBox83, textBox57, textBox59, checkBox12, 0);
            UpdatePharmacyPlanListCells(textBox97, textBox84, textBox62, textBox60, checkBox13, 1);
            UpdatePharmacyPlanListCells(textBox98, textBox85, textBox63, textBox61, checkBox14, 2);
            UpdatePharmacyPlanListCells(textBox99, textBox86, textBox64, textBox73, checkBox15, 3);
            UpdatePharmacyPlanListCells(textBox100, textBox87, textBox65, textBox74, checkBox16, 4);
            UpdatePharmacyPlanListCells(textBox101, textBox88, textBox66, textBox75, checkBox17, 5);
            UpdatePharmacyPlanListCells(textBox102, textBox89, textBox67, textBox76, checkBox18, 6);
            UpdatePharmacyPlanListCells(textBox103, textBox90, textBox68, textBox77, checkBox19, 7);
            UpdatePharmacyPlanListCells(textBox104, textBox91, textBox69, textBox78, checkBox20, 8);
            UpdatePharmacyPlanListCells(textBox105, textBox92, textBox70, textBox79, checkBox21, 9);
            UpdatePharmacyPlanListCells(textBox106, textBox93, textBox71, textBox80, checkBox22, 10);
            UpdatePharmacyPlanListCells(textBox107, textBox94, textBox72, textBox81, checkBox23, 11);
            UpdatePharmacyPlanListCells(textBox108, textBox95, textBox58, textBox82, checkBox24, 12);
            
        }

        public void UpdatePharmacyPlanListCells(TextBox indexBox, TextBox order, TextBox action, TextBox comment, CheckBox checkBox, int adjustIndex)
        {

            if (pharmacyTabStatus.activePlanList.Count > adjustIndex)
            {
                PharmacyPlan p = pharmacyTabStatus.activePlanList[adjustIndex + pharmacyTabStatus.planDisplayStartIndex];
                indexBox.Text = p.plan[0];
                indexBox.Visible = true;
                order.Text = p.plan[1] + " -- " + p.plan[2];
                order.Visible = true;
                action.Text = p.plan[4];
                action.Visible = true;
                comment.Text = p.plan[5];
                comment.Visible = true;

                if (p.plan[3] == "1") checkBox.Checked = false;
                else checkBox.Checked = true;
            }
            else
            {
                indexBox.Text = "";
                indexBox.Visible = false;
                order.Text = "";
                order.Visible = false;
                action.Text = "";
                action.Visible = false;
                comment.Text = "";
                comment.Visible = false;
            }
        }

        //toggle show removed plans
        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            PharmacyTabLoadPlansOfCare();
        }

        private void PharmacyAdjustPlanActive(int indexAdjust, CheckBox checkBox)
        {
            PharmacyPlan p = pharmacyTabStatus.activePlanList[indexAdjust + pharmacyTabStatus.planDisplayStartIndex];
            if (checkBox.Checked)
            {
                p.plan[3] = "0";
            }
            else
            {
                p.plan[3] = "1";
            }
        }

        //remove selected
        private void button25_Click(object sender, EventArgs e)
        {
            PharmacyAdjustPlanActive(0, checkBox12);
            PharmacyAdjustPlanActive(1, checkBox13);
            PharmacyAdjustPlanActive(2, checkBox14);
            PharmacyAdjustPlanActive(3, checkBox15);
            PharmacyAdjustPlanActive(4, checkBox16);
            PharmacyAdjustPlanActive(5, checkBox17);
            PharmacyAdjustPlanActive(6, checkBox18);
            PharmacyAdjustPlanActive(7, checkBox19);
            PharmacyAdjustPlanActive(8, checkBox20);
            PharmacyAdjustPlanActive(9, checkBox21);
            PharmacyAdjustPlanActive(10, checkBox22);
            PharmacyAdjustPlanActive(11, checkBox23);
            PharmacyAdjustPlanActive(12, checkBox24);
        }

        //store patient in pharmacy
        private void button26_Click(object sender, EventArgs e)
        {
            patientForm.encounterData.planofCares.Clear();
            foreach (string[] plan in pharmacyTabStatus.allPlanList)
            {
                string s = plan[0];
                for (int i = 1; i < plan.Length - 2; i++)
                {
                    s += "|" + plan[i];
                }
                s += "|" + plan[plan.Length - 2];

                PlanOfCare p = new PlanOfCare();
                p.text = s;
                p.id = plan[plan.Length - 1];
                p.code = "";
                p.codeSystem = "";
                p.displayName = "";

                patientForm.encounterData.planofCares.Add(p);
            }
            xmlHandler.WriteCurrentPatientXML();
            LoadExitReviewPatient();
            InitializeClinicTabData();
        }

        private void UpdatePharmacyPlan(TextBox action, TextBox comment, int adjustIndex)
        {
            PharmacyPlan p = pharmacyTabStatus.activePlanList[adjustIndex + pharmacyTabStatus.planDisplayStartIndex];
            p.plan[4] = action.Text;
            p.plan[5] = comment.Text;
            pharmacyTabStatus.activePlanList[adjustIndex + pharmacyTabStatus.planDisplayStartIndex] = p;
            pharmacyTabStatus.allPlanList[p.index] = p.plan;
        }

        #region update text in pharmacy plans
        private void textBox57_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox57, textBox59, 0);
        }

        private void textBox59_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox57, textBox59, 0);
        }

        private void textBox62_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox62, textBox60, 1);
        }

        private void textBox60_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox62, textBox60, 1);
        }

        private void textBox61_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox63, textBox61, 2);
        }

        private void textBox63_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox63, textBox61, 2);
        }

        private void textBox64_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox64, textBox73, 3);
        }

        private void textBox73_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox64, textBox73, 3);
        }

        private void textBox65_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox65, textBox74, 4);
        }

        private void textBox74_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox65, textBox74, 4);
        }

        private void textBox66_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox66, textBox75, 5);
        }

        private void textBox75_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox66, textBox75, 5);
        }

        private void textBox67_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox67, textBox76, 6);
        }

        private void textBox76_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox67, textBox76, 6);
        }

        private void textBox68_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox68, textBox77, 7);
        }

        private void textBox77_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox68, textBox77, 7);
        }

        private void textBox69_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox69, textBox78, 8);
        }

        private void textBox78_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox69, textBox78, 8);
        }

        private void textBox70_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox70, textBox79, 9);
        }

        private void textBox79_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox70, textBox79, 9);
        }

        private void textBox71_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox71, textBox80, 10);
        }

        private void textBox80_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox71, textBox80, 10);
        }

        private void textBox72_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox72, textBox81, 11);
        }

        private void textBox81_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox72, textBox81, 11);
        }

        private void textBox58_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox58, textBox82, 12);
        }

        private void textBox82_TextChanged(object sender, EventArgs e)
        {
            UpdatePharmacyPlan(textBox58, textBox82, 12);
        }
        #endregion update text in plans

        private void button27_Click(object sender, EventArgs e)
        {
            pharmacyTabStatus.planDisplayStartIndex--;
            PharmacyTabLoadPlansOfCare();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            pharmacyTabStatus.planDisplayStartIndex++;
            PharmacyTabLoadPlansOfCare();
        }

        #endregion ----------------------------End Pharmacy tab-------------------------------

        #region ---------------------------------Exit tab---------------------------------
        protected void RunCommand()
        {

            Process p1 = Process.Start("bash", "save.bat");
            p1.WaitForExit();
        }

        private void ExitButton_Click_1(object sender, EventArgs e)
        {
            xmlHandler.WriteFinalPatientXML();
            RunCommand();
        }

        //load patient For Review Button
        private void button29_Click(object sender, EventArgs e)
        {
            xmlHandler.LoadCurrentPatientXML();
            LoadExitReviewPatient();
        }

        private void LoadExitReviewPatient()
        {
            string endLine = "\n";
            string reviewPatient = "";

            reviewPatient += "Patient ID: " + patientForm.patientData.id + endLine;
            reviewPatient += "Patient Name: " + patientForm.patientData.lastName + ", " + patientForm.patientData.firstName + endLine;
            reviewPatient += "Chief Complaint: " + patientForm.encounterData.encounter.reason;
            reviewPatient += "Patient Gender: " + patientForm.patientData.genderCode + endLine;

            if (patientForm.patientData.Languages.Count > 0)
            {
                reviewPatient += "Languages: ";
                string languages = patientForm.patientData.Languages[0];
                for (int i = 1; i < patientForm.patientData.Languages.Count; i++)
                {
                    languages += ", " + patientForm.patientData.Languages[i];
                }
                languages += endLine;
                reviewPatient += languages;
            }

            reviewPatient += "Patient DOB: " + patientForm.patientData.dateOfBirth + endLine;
            reviewPatient += "Patient Stated Age:  Years:" + patientForm.patientData.statedAge.Substring(0, 2) + " Months:" + patientForm.patientData.statedAge.Substring(2, 2) + endLine;
            reviewPatient += "Patient Estimated Age:  Years:" + patientForm.patientData.estimatedAge.Substring(0, 2) + " Months:" + patientForm.patientData.estimatedAge.Substring(2, 2) + endLine;
            reviewPatient += "Patient Status In School: " + patientForm.patientData.statusInSchool + ". Year In School: " + patientForm.patientData.yearInSchool + endLine;
            reviewPatient += "Patient Tribe: " + patientForm.patientData.tribe + ". Present Village: " + patientForm.patientData.presentVillage + ". Home Village: " + patientForm.patientData.homeVillage + endLine;
            reviewPatient += endLine;
            if (patientForm.patientData.Supports.Count > 0)
            {
                reviewPatient += "Supports: " + endLine;
                for (int i = 0; i < patientForm.patientData.Supports.Count; i++)
                {
                    Support su = patientForm.patientData.Supports[i];
                    reviewPatient += "    Support Name: " + su.lastname + ", " + su.firstName + ".    School:" + su.schoolName + ".    City:" + su.city + endLine;
                }
                reviewPatient += endLine;
            }

            if (patientForm.encounterData.labResults.Count > 0)
            {
                reviewPatient += "Lab Results: " + endLine;
                for (int i = 0; i < patientForm.encounterData.labResults.Count; i++)
                {
                    LabResult lr = patientForm.encounterData.labResults[i];
                    reviewPatient += "    Result: Name" + lr.displayName + ".    Value:" + lr.value + ".    Reference Range:" + lr.referenceRange + endLine;
                }
                reviewPatient += endLine;
            }

            if (patientForm.encounterData.medications.Count > 0)
            {
                reviewPatient += "Medications: " + endLine;
                for (int i = 0; i < patientForm.encounterData.medications.Count; i++)
                {
                    Medication m = patientForm.encounterData.medications[i];
                    reviewPatient += "    Medication: Name" + m.text + endLine;
                }
                reviewPatient += endLine;
            }

            if (patientForm.encounterData.immunizations.Count > 0)
            {
                reviewPatient += "Immunizations: " + endLine;
                for (int i = 1; i < patientForm.encounterData.immunizations.Count; i++)
                {
                    Immunization im = patientForm.encounterData.immunizations[i];
                    reviewPatient += "    Immunization: Name" + im.displayName + "   Comment:" + im.comment + "   Text:" + im.text + endLine;
                }
                reviewPatient += endLine;
            }

            if (patientForm.encounterData.vitalSigns.Count > 0)
            {
                reviewPatient += "Vitals: " + endLine;
                for (int i = 1; i < patientForm.encounterData.vitalSigns.Count; i++)
                {
                    VitalSign v = patientForm.encounterData.vitalSigns[i];
                    reviewPatient += "    Vital: Situation:" + v.comment + "   Type:" + v.displayName + "   Value:" + v.value + "   Unit:" + v.unit + endLine;
                }
                reviewPatient += endLine;
            }

            if (patientForm.encounterData.conditions.Count > 0)
            {
                reviewPatient += "Assessments: " + endLine;
                for (int i = 1; i < patientForm.encounterData.conditions.Count; i++)
                {
                    Condition c = patientForm.encounterData.conditions[i];
                    reviewPatient += "    Assessment:" + c.problemName + "   Description:" + c.comment + endLine;
                }
                reviewPatient += endLine;
            }

            if (patientForm.encounterData.planofCares.Count > 0)
            {
                reviewPatient += "Plans Of Care: " + endLine;
                for (int i = 1; i < patientForm.encounterData.planofCares.Count; i++)
                {
                    PlanOfCare p = patientForm.encounterData.planofCares[i];
                    string[] plan = p.text.Split('|');
                    reviewPatient += "    #" + plan[0] + "  Order: " + plan[1] + "  Sig: " + plan[2] + "   #/Action:" + plan[4] + "   Comment:" + plan[5] + endLine;
                }
                reviewPatient += endLine;
            }




            reviewPatient += endLine;
            reviewPatient += endLine;
            reviewPatient += "Notes: " + endLine;
            reviewPatient += patientForm.encounterData.encounter.notes;

            richTextBox1.Text = reviewPatient;
        }

        #endregion ------------------------------End Exit tab-------------------------------

        #region ---------------------------------Admin tab---------------------------------

        private void ListBoxPathType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (xmlHandler != null)
            {
                xmlHandler.SetStorageSystem(ListBoxPathType.SelectedItem.ToString());
            }
            if (xmlReader != null)
            {
                xmlReader.SetStorageSystem(ListBoxPathType.SelectedItem.ToString());
            }
        }

        private void comboBoxScribeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            stationScribe = clinicData.GetProviderStruct(comboBoxScribeList.Text);
        }

        private void InitializeAdminTab()
        {
            textBox1.Text = clinicData.stationName;
            textBox2.Text = clinicData.clinicDetails;
            textBox3.Text = clinicData.clinicName;
            textBox4.Text = clinicData.gps;
            textBox5.Text = clinicData.dateStart;
            textBox6.Text = clinicData.dateEnd;
            textBox7.Text = clinicData.timeZone;
            textBox8.Text = clinicData.minYearInSchool;
            textBox9.Text = clinicData.maxYearInSchool;
            textBox10.Text = clinicData.hemoglobinRange;
            textBox11.Text = clinicData.custodianName;
            textBox12.Text = clinicData.custodianID;
            textBox13.Text = clinicData.authorPrefix;
            textBox14.Text = clinicData.authorFirstName;
            textBox15.Text = clinicData.authorLastName;
            adminGroups = new List<GroupBox>();
            adminGroups.Add(groupBox2);
            comboBox2.Items.Add("Languages");
            AdminUpdateLanguageList();

            //Medications
            adminGroups.Add(groupBox4);
            comboBox2.Items.Add("Medications");
            AdminUpdateMedicationList();

            //Immunizations
            adminGroups.Add(groupBox18);
            comboBox2.Items.Add("Immunizations");
            AdminUpdateImmunizationList();
        }


        #region textbox changes
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            clinicData.clinicDetails = textBox2.Text;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            clinicData.authorFirstName = textBox14.Text;
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            clinicData.authorPrefix = textBox13.Text;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            clinicData.custodianID = textBox12.Text;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            clinicData.custodianName = textBox11.Text;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            clinicData.hemoglobinRange = textBox10.Text;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            clinicData.maxYearInSchool = textBox9.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            clinicData.minYearInSchool = textBox8.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            clinicData.timeZone = textBox7.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            clinicData.dateEnd = textBox6.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            clinicData.dateStart = textBox5.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            clinicData.gps = textBox4.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            clinicData.clinicName = textBox3.Text;
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            clinicData.authorLastName = textBox15.Text;
        }
        #endregion

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupBox activeGroup = null;
            if (comboBox2.Text == "Languages")
            {
                activeGroup = groupBox2;
            }
            else if (comboBox2.Text == "Medications")
            {
                activeGroup = groupBox4;
            }
            else if (comboBox2.Text == "Immunizations")
            {
                activeGroup = groupBox18;
            }

            foreach (GroupBox g in adminGroups)
            {
                if (g == activeGroup) g.Visible = true;
                else g.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox109.Text != "" && textBox110.Text != "")
            {
                clinicData.AddLanguageInfo(textBox109.Text, textBox110.Text);
                AdminUpdateLanguageList();
                textBox109.Text = textBox110.Text = "";
            }
        }

        private void AdminUpdateLanguageList()
        {
            listBox1.Items.Clear();
            string[] langs = clinicData.GetLanguageInfoLists();
            foreach (string l in langs)
            {
                listBox1.Items.Add(l);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox112.Text != "" && textBox111.Text != "" && textBox113.Text != "")
            {
                clinicData.AddMedInfo(textBox112.Text, textBox111.Text, textBox113.Text);
                AdminUpdateMedicationList();
                textBox112.Text = textBox111.Text = textBox113.Text = "";
            }
        }

        private void AdminUpdateMedicationList()
        {
            listBox2.Items.Clear();
            string[] meds = clinicData.GetMedInfoList();
            foreach (string m in meds)
            {
                listBox2.Items.Add(m);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox116.Text != "" && textBox115.Text != "" && textBox114.Text != "")
            {
                clinicData.AddImmInfo(textBox116.Text, textBox115.Text, textBox114.Text);
                AdminUpdateImmunizationList();
                textBox116.Text = textBox115.Text = textBox114.Text = "";
            }
        }

        private void AdminUpdateImmunizationList()
        {
            listBox3.Items.Clear();
            string[] imm = clinicData.GetImmInfoList();
            foreach (string i in imm)
            {
                listBox3.Items.Add(i);
            }
        }
        #endregion ------------------------------End Admin tab-------------------------------
    }
}


#region old code
//xmlHandler.LoadPatientTemplateXML();

//TextBoxIntakeTabPatientLastNameIn.Text = label46.Text = patientForm.patientData.firstName;

//TextBoxIntakeTabPatientFirstNameIn.Text = label47.Text = patientForm.patientData.lastName;

//GenderComboBox.Text = label51.Text = patientForm.patientData.genderCode;

//label88.Text = patientForm.patientData.dateOfBirth;
//TextBoxIntakeTabDOBYear.Text = patientForm.patientData.dateOfBirth.Substring(0, 4);
//TextBoxIntakeTabDOBMonth.Text = patientForm.patientData.dateOfBirth.Substring(4, 2);
//TextBoxIntakeTabDOBDay.Text = patientForm.patientData.dateOfBirth.Substring(6, 2);

//label89.Text = "Years:" + patientForm.patientData.estimatedAge.Substring(0, 2) + " Months:" + patientForm.patientData.estimatedAge.Substring(2);
//TextBoxIntakeTabEstAgeYears.Text = patientForm.patientData.estimatedAge.Substring(0, 2);
//TextBoxIntakeTabEstAgeMonths.Text = patientForm.patientData.estimatedAge.Substring(2);

//label90.Text = patientForm.patientData.statedAge;
//label90.Text = "Years:" + patientForm.patientData.statedAge.Substring(0, 2) + " Months:" + patientForm.patientData.estimatedAge.Substring(2);
//TextBoxIntakeTabStatedAgeYears.Text = patientForm.patientData.statedAge.Substring(0, 2);
//TextBoxIntakeTabStatedAgeMonths.Text = patientForm.patientData.statedAge.Substring(2);

//string languages = "";
//foreach (string l in patientForm.patientData.Languages)
//{
//    if (languages.Length > 0)
//    {
//        languages += ", ";
//        languages += l;
//    }
//    else languages = l;
//    if (l != "NI") tempLanguagesHolder.Add(l);
//}
//label91.Text = languages;
//ComboBoxIntakeTabLanguageList.Text = "Enter Language";

//string tribeVillage = patientForm.patientData.tribe + "/" + patientForm.patientData.presentVillage;
//label92.Text = tribeVillage;
//ComboBoxIntakeTabTribe.Text = patientForm.patientData.tribe;
//ComboBoxIntakeTabHomeVillage.Text = patientForm.patientData.homeVillage;
//ComboBoxIntakeTabPresentVillage.Text = patientForm.patientData.presentVillage;

//ComboBoxIntakeTabSchoolList.Text = "NI";
//ComboBoxIntakeTabStatusInSchool.Text = "NI";
//comboBox28.Text = "NI";
//label93.Text = "NI";


//label94.Text = patientForm.patientData.yearInSchool;
////ComboBoxIntakeTabYearInSchool.Text = patientForm.patientData.yearInSchool;

//if (patientForm.encounterData.vitalSigns.Count <= 1)
//{
//    CreateInitialVitals();
//}

//patientForm.clinicData.documentTimeStamp = patientForm.GetTimeStamp();
//patientForm.clinicData.docID = patientForm.GetUUID();

//label95.Text = patientForm.encounterData.vitalSigns[0].value;
//label96.Text = patientForm.encounterData.vitalSigns[1].value;
//label97.Text = patientForm.encounterData.vitalSigns[2].value;
//label98.Text = patientForm.encounterData.vitalSigns[3].value;

//textBox29.Text = patientForm.encounterData.vitalSigns[0].value; //height text box
//textBox26.Text = patientForm.encounterData.vitalSigns[1].value; //weight text box
//textBox28.Text = patientForm.encounterData.vitalSigns[2].value; //bmi text box
//textBox27.Text = patientForm.encounterData.vitalSigns[3].value; //z-score text box
#endregion end old code