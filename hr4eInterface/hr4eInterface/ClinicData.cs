using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hr4eInterface
{
    class ClinicData
    {
        private struct ResultInterpretation
        {
            public string displayName;
            public string codename;
            public string code;
        }

        public struct LanguageInfo
        {
            public string name;
            public string code;
        }

        public struct SchoolInfo
        {
            public string name;
            public string city;
        }

        public struct RolesInfo
        {
            public string test;
            public string code;
        }

        public struct SupportInfo
        {
            public string firstName;
            public string lastName;
            public string school;
            public string city;
            public string comment;
        }

        public struct ProviderInfo
        {
            public string roleCode;
            public string roleText;
            public string startDate;
            public string endDate;
            public string id;
            public string namePrefix;
            public string nameFirst;
            public string nameLast;
            public string organization;
            public string comment;
        }

        public struct MedInfo
        {
            public string code;
            public string displayName;
            public string text;
        }

        public struct ImmInfo
        {
            public string displayName;
            public string code;
            public string text;
        }

        List<ResultInterpretation> resultInterpretationList;
        List<LanguageInfo> languageInfoList;
        List<MedInfo> medicationList;
        List<LabResult> labResultsList;
        List<ImmInfo> immunizationList;
        List<SupportInfo> supportsList;
        List<ProviderInfo> providerList;
        List<string> schoolStatusList;
        List<SchoolInfo> schoolInfoList;
        List<string> tribes;
        List<string> villages;
        List<string> confidentialityCodes;

        public string stationName;
        public string custodianName;
        public string custodianID;
        public string dateStart;
        public string dateEnd;
        public string gps;
        public string clinicName;
        public string clinicDetails;
        public string authorPrefix;
        public string authorFirstName;
        public string authorLastName;
        public string greenCCDxmlns;
        public string greenCCDxmlns_xsi;
        public string greenCCDxmlns_hr4e;
        public string greenCCDxsi_schemaLocation;
        public string docTitle;
        public string docVersion;
        public string hr4eOID;
        public string timeZone;
        public string confidentialityCodeSystem;
        public string confidentialityCode;
        public string genderCode;
        public string roleCode;
        public string resultsInterpretCode;
        public string resultTypeCode;
        public string encounterTypeCode;
        public string problemCode;
        public string medicationCode;
        public string medicationCodeSystemName;
        public string immunizationCode;
        public string vitalSignCode;
        public string minYearInSchool;
        public string maxYearInSchool;
        public string encountertype;
        public string encountertext;
        public string hemoglobinRange;


        public ClinicData()
        {
            resultInterpretationList = new List<ResultInterpretation>();
            languageInfoList = new List<LanguageInfo>();
            medicationList = new List<MedInfo>();
            labResultsList = new List<LabResult>();
            immunizationList = new List<ImmInfo>();
            supportsList = new List<SupportInfo>();
            providerList = new List<ProviderInfo>();
            schoolStatusList = new List<string>();
            schoolInfoList = new List<SchoolInfo>();
            tribes = new List<string>();
            villages = new List<string>();
            confidentialityCodes = new List<string>(); ;
        }

        #region manage language lists
        public void AddLanguageInfo(string name, string code)
        {
            LanguageInfo l = new LanguageInfo();
            l.name = name;
            l.code = code;
            languageInfoList.Add(l);
        }

        public string[] GetLanguageList()
        {
            string[] languages = new string[languageInfoList.Count];
            for (int i = 0; i < languageInfoList.Count; i++)
            {
                languages[i] = languageInfoList[i].name;
            }

            return languages;
        }

        public string GetLanguageCode(string language)
        {
            foreach (LanguageInfo li in languageInfoList)
            {
                if (language == li.name)
                {
                    return li.code;
                }
            }

            return "NI"; //if language is free typed, returns NI for code;

        }

        public string[] GetLanguageInfoLists()
        {
            string[] languages = new string[languageInfoList.Count];
            for (int i = 0; i < languageInfoList.Count; i++)
            {
                languages[i] = languageInfoList[i].name + "  :  " + languageInfoList[i].code;
            }

            return languages;
        }

        #endregion end manage language lists

        #region manage med lists
        public void AddMedInfo(string displayName, string code, string text)
        {
            MedInfo m = new MedInfo();
            m.displayName = displayName;
            m.code = code;
            m.text = text;
            medicationList.Add(m);
        }

        public string[] GetMedList()
        {
            string[] meds = new string[medicationList.Count];
            for (int i = 0; i < medicationList.Count; i++)
            {
                meds[i] = medicationList[i].displayName;
            }

            return meds;
        }

        public string[] GetMedInfoList()
        {
            string[] meds = new string[medicationList.Count];
            for (int i = 0; i < medicationList.Count; i++)
            {
                meds[i] = medicationList[i].displayName + "  :  " + medicationList[i].code + "  :  " + medicationList[i].text;
            }

            return meds;
        }

        #endregion end manage med list

        #region manage imm lists
        public void AddImmInfo(string displayName, string code, string text)
        {
            ImmInfo i = new ImmInfo();
            i.displayName = displayName;
            i.code = code;
            i.text = text;
            immunizationList.Add(i);
        }

        public string[] GetImmList()
        {
            string[] imms = new string[immunizationList.Count];
            for (int i = 0; i < immunizationList.Count; i++)
            {
                imms[i] = immunizationList[i].text;
            }

            return imms;
        }

        public string[] GetImmInfoList()
        {
            string[] imms = new string[immunizationList.Count];
            for (int i = 0; i < immunizationList.Count; i++)
            {
                imms[i] = immunizationList[i].displayName + "  :  " + immunizationList[i].code + "  :  " + immunizationList[i].text;
            }

            return imms;
        }

        #endregion end manage imm list

        public void AddSchoolStatus(string status)
        {
            schoolStatusList.Add(status);
        }

        public string[] GetSchoolStatusList()
        {
            string[] stati = new string[schoolStatusList.Count];
            for (int i = 0; i < schoolStatusList.Count; i++)
            {
                stati[i] = schoolStatusList[i];
            }

            return stati;
        }

        public int[] GetSchoolYearRange()
        {
            try
            {
                int[] returnInts = new int[2];
                returnInts[0] = Convert.ToInt16(minYearInSchool);
                returnInts[1] = Convert.ToInt16(maxYearInSchool);
                return returnInts;
            }
            catch (Exception e) { string getRidOfWarning = e.ToString(); };
            return new int[]{0,12};
        }

        public void AddTribes(string tribe)
        {
            tribes.Add(tribe);
        }

        public string[] GetTribesList()
        {
            string[] tribesList = new string[tribes.Count];
            for (int i = 0; i < tribes.Count; i++)
            {
                tribesList[i] = tribes[i];
            }

            return tribesList;
        }

        public void AddVillages(string village)
        {
            villages.Add(village);
        }

        public string[] GetVillageList()
        {
            string[] villageList = new string[villages.Count];
            for (int i = 0; i < villages.Count; i++)
            {
                villageList[i] = villages[i];
            }

            return villageList;
        }

        public void AddSchoolData(string name, string city)
        {
            SchoolInfo si = new SchoolInfo();
            si.name = name;
            si.city = city;
            schoolInfoList.Add(si);
        }

        public string[] GetSchoolNames()
        {
            string[] schools = new string[schoolInfoList.Count];
            for (int i = 0; i < schoolInfoList.Count; i++)
            {
                schools[i] = schoolInfoList[i].name;
            }

            return schools;
        }

        #region providers data
        public void AddProvider(string roleCode, string roleText, string startDate,
                    string endDate, string id, string namePrefix, string nameFirst,
                    string nameLast, string organization, string comment)
        {
            ProviderInfo p = new ProviderInfo();
            p.roleCode = roleCode;
            p.roleText = roleText;
            p.startDate = startDate;
            p.endDate = endDate;
            p.id = id;
            p.namePrefix = namePrefix;
            p.nameFirst = nameFirst;
            p.nameLast = nameLast;
            p.organization = organization;
            p.comment = comment;

            providerList.Add(p);
        }

        public string[] GetProviders()
        {
            string[] providers = new string[providerList.Count];
            for (int i = 0; i < providerList.Count; i++)
            {
                string name = providerList[i].nameLast + ", " + providerList[i].nameFirst;
                providers[i] = name;
            }

            return providers;
        }

        public string[] GetProviderData(string name)
        {
            string name1 = name.Replace(" ", "");
            string[] names = name1.Split(',');
            if (names.Length > 1)
            {
                string fname = names[1];
                string lname = names[0];
                for (int i = 0; i < providerList.Count; i++)
                {
                    if (lname == providerList[i].nameLast && fname == providerList[i].nameFirst)
                    {
                        string[] returnList = new string[10];
                        returnList[0] = providerList[i].roleCode;
                        returnList[1] = providerList[i].roleText;
                        returnList[2] = providerList[i].startDate;
                        returnList[3] = providerList[i].endDate;
                        returnList[4] = providerList[i].id;
                        returnList[5] = providerList[i].namePrefix;
                        returnList[6] = providerList[i].nameFirst;
                        returnList[7] = providerList[i].nameLast;
                        returnList[8] = providerList[i].organization;
                        returnList[9] = providerList[i].comment;
                        return returnList;
                    }
                }
            }

            return null;
        }

        public Provider GetProviderStruct(string name)
        {
            string[] info = GetProviderData(name);
            if (info != null)
            {
                Provider p = new Provider();
                p.comment = info[9];
                p.dateEnd = info[3];
                p.dateStart = info[2];
                p.firstName = info[6];
                p.id = info[4];
                p.lastName = info[7];
                p.organizationName = info[8];
                p.prefix = info[5];
                p.roleCode = info[0];
                p.roleDescription = info[1];
                p.roleCodeSystem = roleCode;
                return p;
            }

            return new Provider();
        }

        #endregion end providers data

        #region support Data

        public void AddSupport(string fname, string lname, string school, string city, string comment)
        {
            SupportInfo si = new SupportInfo();
            si.firstName = fname;
            si.lastName = lname;
            si.school = school;
            si.city = city;
            si.comment = comment;
            supportsList.Add(si);
        }

        public string[] GetSupports()
        {
            string[] supports = new string[supportsList.Count];
            for (int i = 0; i < supportsList.Count; i++)
            {
                string name = supportsList[i].lastName + ", " + supportsList[i].firstName;
                supports[i] = name;
            }

            return supports;
        }

        public string[] GetSupportData(string name)
        {
            string name1 = name.Replace(" ", "");
            string[] names = name1.Split(',');
            if (names.Length > 1)
            {
                string fname = names[1];
                string lname = names[0];
                for (int i = 0; i < supportsList.Count; i++)
                {
                    if (lname == supportsList[i].lastName && fname == supportsList[i].firstName)
                    {
                        string[] returnList = new string[5];
                        returnList[0] = supportsList[i].firstName;
                        returnList[1] = supportsList[i].lastName;
                        returnList[2] = supportsList[i].school;
                        returnList[3] = supportsList[i].city;
                        returnList[4] = supportsList[i].comment;

                        return returnList;
                    }
                }
            }

            return null;
        }

        public Support GetSupportStruct(string name)
        {
            string[] info = GetSupportData(name);
            if (info != null)
            {
                Support s = new Support();
                s.dateNull = "NI";
                s.streetNull = "NI";
                s.city = info[3];
                s.stateNull = "NI";
                s.postalCodeNull = "NI";
                s.telecomNull = "NI";
                s.firstName = info[0];
                s.lastname = info[1];
                s.comment = info[4];
                s.schoolName = info[2];
                return s;
            }

            return new Support();
        }

        #endregion end get support Data



    }
}
