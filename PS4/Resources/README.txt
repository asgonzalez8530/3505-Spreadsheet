Spreadsheet solution implemented by Aaron Bellis U0981638 for CS3500 2017 Fall Semester
9/30/2017

Spreadsheet project is a strait forward implementation of the AbstractSpreadsheet class.
It includes an internal class Cell which holds the different types of data a Spreadsheet
can hold including a simple Enum for casting convenience as its contents are stored as a 
generic object type. 

GetDirectDependents(string name) method requirements require check for throwing 
NullArgumentException if name is null, but as it is a protected method, name is never 
null as it is evaluated prior to call. Check for null name is included per documentation
and in anticipation of further development. 

Building against the same PS2 and PS3 libraries turned in for grading on their respective
due dates

Note to grader: acheiving 97.94% code coverage of spreadsheet class due to GetDirectDependents
method for the reasons stated above. 
