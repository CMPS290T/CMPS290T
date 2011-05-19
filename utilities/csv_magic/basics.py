'''
Created on May 19, 2011

    Python script to grab basic patient information and store it into a csv file.
    Includes UUID (or something), given_name, family_name, age, city, home_village, tribe

@author: super
'''

import libxml2
#import libxslt
import os
import StringIO, sys
import subprocess
import argparse
    
 #
 # Parse the command line
 #
 # -i <input file>: patient data file to write out
 #    The default is hr4e_patient_template.xml in current directory
 #       

#Get the current working directory
currentDirectory = os.getcwd()
#Set the working directory
workingDirectory = 'utilities/csv_magic'
parser = argparse.ArgumentParser()
#File to read in (location wise).
#It is possible that I will need to change this to take in sys.argv[0] and run a perl script to take in each file in 
#/patient_data/*
# for each $ls in directory:
# run python [filename] $ls
newDirectory = currentDirectory[0:len(currentDirectory) - len(workingDirectory)] + '/data/patient_data/' + "Frodo_Baggins_00000000-0000-0000-0000-000000000000.xml"
#This will probably need to refer to the file in the thumb drive...
parser.add_argument('-i', metavar='in-file', type=str, default= newDirectory)
parser.add_argument('-d', metavar='xpath_docID', type=str, default="/gcda:greenCCD/gcda:header/gcda:documentID/@root")
parser.add_argument('-g', metavar='xpath_given_name', type=str, default="//gcda:greenCCD/gcda:header/gcda:personalInformation/gcda:patientInformation/gcda:personInformation/gcda:personName/gcda:given")
parser.add_argument('-f', metavar='xpath_family_name', type=str, default="/gcda:greenCCD/gcda:header/gcda:personalInformation/gcda:patientInformation/gcda:personInformation/gcda:personName/gcda:family")
#parser.add_argument('-a', metavar='xpath_age', type=str, default="/gcda:greenCCD/gcda:header/gcda:personalInformation/gcda:patientInformation/gcda:personInformation/gcda:hr4e:ageInformation")
parser.add_argument('-c', metavar='xpath_city', type=str, default="/gcda:greenCCD/gcda:header/gcda:personalInformation/gcda:patientInformation/gcda:personAddress/gcda:personName/gcda:city")
#parser.add_argument('-v', metavar='xpath_village', type=str, default="/gcda:greenCCD/gcda:header/gcda:personalInformation/gcda:patientInformation/gcda:hr4e:livingSituation/gcda:hr4e:homeVillage")
#parser.add_argument('-t', metavar='xpath_tribe', type=str, default="/gcda:greenCCD/gcda:header/gcda:personalInformation/gcda:patientInformation/gcda:hr4e:livingSituation/gcda:hr4e:tribe")
parser.add_argument('-n', metavar='namespace', type=str, default="AlschulerAssociates::GreenCDA")

try:
    results = parser.parse_args()
    input = results.i
    docID_xpath = results.d
    given_xpath = results.g
    family_xpath = results.f
    #age_xpath = results.a
    city_xpath = results.c
    #home_village_xpath = results.v
    #tribe_xpath = results.t
    namespace_name = results.n
#    print 'Input file:', input
#    print 'XPath to given name', given_xpath
#    print 'XPath to family name', family_xpath
#    print 'XPath to docID', docID_xpath
#    print 'Namespace', namespace_name
except IOError, msg:
    parser.error(str(msg))
    
    
#
# Parse the file
#
patient_doc = libxml2.parseFile(input)


#
# Use XPath to get given name, family name and document ID
#
# For this to work, we need to set the namespace and then prefix each element name with it
# in the XPath expression. Yuck!
#
ctxt = patient_doc.xpathNewContext()
ctxt.xpathRegisterNs("gcda", namespace_name)
result = ctxt.xpathEval(given_xpath)
for node in result:
    given_name = node.content
    
result = ctxt.xpathEval(family_xpath)
for node in result:
    family_name = node.content
    
result = ctxt.xpathEval(docID_xpath)
for node in result:
    docID = node.content

result = ctxt.xpathEval(city_xpath)
for node in result:
    city = node.content

#result = ctxt.xpathEval(age_xpath)
#for node in result:
#    age = node.content

#result = ctxt.xpathEval(home_village_xpath)
#for node in result:
#    home_village = node.content

#result = ctxt.xpathEval(tribe_xpath)
#for node in result:
#    tribe = node.content

# Construct a file name out of the pieces
#
writeFileName = 'basics.csv'
#header = 'id,given_name,family_name,age, city, home_village, tribe
writeContent = "\n" + docID + "," + given_name + "," + family_name + "," + "100" + "," + "Some PLace" + "," + "Uganda" +"," + "Shooooo"
#
# Write out the XML document
#
writeOutDirectory = currentDirectory[0:len(currentDirectory) - len(workingDirectory)] + '/data/csv/'
print "Adding to basics.csv"
print "to some place"
new_file = open( 'basics.csv', 'w')
new_file.write("fuck you")
new_file.close()
print "Done."

