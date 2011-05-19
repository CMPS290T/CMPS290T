import webbrowser
import sys, os

#This is just running files off of the thumb drive
#the drive in the default browser.  The current file location
# for jig.html is on my ubuntu thumb drive path.  We can set
# this to match our needs.
working = os.getcwd()
#Gosh this is an ugly way to do this.  If you execute the script in a directory
#different from utilities...you have to change it below...sigh*
new_working = working[0:len(working)-len('utilities')] + 'html/jig.html'
print("Hello and welcome to the prototype HR4E Jig(sp?)")
webbrowser.open(new_working)
