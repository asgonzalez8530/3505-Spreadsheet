Updated by Rebekah Peterson for CS3505
Last Updated: 4/6/18
Uses a new version of Formula (that includes FormatError)
Also returns CircularError objects for GetCellValue

//////////////////////////////////////////////

Spreadsheet solution implemented by Aaron Bellis U0981638 for CS3500 2017 Fall Semester
10/11/2017

Spreadsheet project is a strait forward implementation of the AbstractSpreadsheet class.
It includes an internal class Cell which holds the different types of data a Spreadsheet
can hold including a simple Enum for casting convenience as its contents are stored as a 
generic object type. 

Several method requirements require check for throwing exception based on passed parameters
but as they are protected methods, improper arguments are never passed Checks are included 
per documentation and in anticipation of further development. 

Building against the same PS2 and PS3 libraries turned in for grading on their respective
due dates

Note to grader: acheiving 97.10% code coverage of spreadsheet class for the reasons stated 
above. 

Also, extension granted per conversation with Professor Kopta
