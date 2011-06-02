import csv
#basicpatientinfo.csv
#id,timestamp,familyname,givenname,gender,sex,age,homevillage,tribe,language


def runCSV():
	lines=[]
	basicInfo = []
	commaString = ""
	aFile = open("/home/phil/hr4e/data/patient_data/patient_data.txt")
	while 1:
    		line = aFile.readline()
    		if not line:
			break
    		lines.append(line)
	aFile.close()	
	#print lines
	basicWriter = csv.writer(open('/home/phil/hr4e/data/csv/basicpatientinfo.csv','a'),quoting=csv.QUOTE_MINIMAL)
	basicInfo = lines[0].split('<|>')
	basicInfo[len(basicInfo)-1] = basicInfo[len(basicInfo)-1].strip('\n')
	print basicInfo
	#print lines
	for info in basicInfo:
		print info
                commaString = commaString + info + ","
		print commaString
		print
		print
	#strip the last comma
	commaString = commaString[0:len(commaString)-1]
	print commaString
	basicWriter.writerow(commaString)
