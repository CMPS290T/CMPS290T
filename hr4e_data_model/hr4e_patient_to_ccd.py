'''
Created on Apr 27, 2011

    Simple python script to invoke saxon to apply XSLT stylesheet to remove hr4e namespace
    from an hr4e patient data record.  
    
    The input to this script is an instance of an hr4e patient data xml document that validates
    against the hr4e_patient_data.xsd and/or was generated from hr4e_patient_template.xml.
    
    The output of this script can be validated against the green_ccd.xsd. The green_ccd.xsd 
    also includes green_cda_narrative.xsd, so you will need to have both files present in the
    current directory.
    
    This script assumes that the saxon jar is in a subdirectory called saxon. saxon can be 
    downloaded from here: http://saxon.sourceforge.net/#F9.3HE
    
    After you extract the jar file, you invoke saxon as follows:
    java -jar saxon9he.jar -s:<data file>.xml -xsl:<xslt file>.xslt
    More information on the command line options can be found here:
    http://www.saxonica.com/documentation/using-xsl/commandline.xml

@author: torkroth
'''

import StringIO, sys
import subprocess
import argparse
    
 #
 # Parse the command line
 #
 # -i <input file>: patient data file from which to remove elements from the hr4e namespace
 #    The default is hr4e_patient_template.xml in current directory
 #
 # -o <output file>: the name to give to the created file, which will be an xml file
 #    with elements from hr4e namespace removed
 #    The default is green_patient_template.xml
 #
 # -xsl <xslt file>: the name of the XSLT stylesheet to apply to remove elements from
 #    the hr4e namespace.
 #    The default is hr4e_patient_to_ccd.xslt
 #
 # Examples:
 #    hr4e_patient_to_ccd will apply style sheet hr4e_patient_to_ccd.xslt in the current
 #    directory to input file to hr4e_patient_template.xml to produce file 
 #    green_patient_template.xml in the current directory.
 #
 #    hr4e_patient_to_ccd -i hr4e_patient_1.xml -o green_patient_1.xml will apply style 
 #    sheet hr4e_patient_to_ccd.xslt in the current directory to input file to 
 #    hr4e_patient_1.xml to produce file green_patient_1.xml in the current directory.
 #       
parser = argparse.ArgumentParser()
parser.add_argument('-i', metavar='in-file', type=str, default="hr4e_patient_template.xml")
parser.add_argument('-o', metavar='out-file', type=str, default="green_patient_template.xml")
parser.add_argument('-xsl', metavar='xslt-file', type=str, default="hr4e_patient_to_ccd.xslt")

try:
    results = parser.parse_args()
    input = results.i
    output = results.o
    xslput = results.xsl
    print 'Input file:', input
    print 'Output file:', output
except IOError, msg:
    parser.error(str(msg))
   
 #
 # Build the command line from options to this script
 #     
command = "java -jar saxon/saxon9he.jar " + " -s:" + input + " -xsl:" + xslput + " > " + output
print command

 # 
 # Fire off a process to invoke java and run the the stylesheet
 #       
p = subprocess.Popen(command, shell=True)
retval = p.wait()


