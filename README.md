
# Proxmea.ReadConsole
A .Net Core library that allows for searching and reading in an .Net consoles output  

## Demo
Comes with example Console app.  
![](Demo.gif)  
The demo draws out a box with letters in it.  
Then we search for the border coordinates and replaces it with an asterix.  
Finally we do a general search for a text existing in the console and show the coordinates.   

## Credits
Thanks to Simon Mourier and Derviş Kayımbaşıoğlu for your great input on Stackoverflow  
https://stackoverflow.com/questions/12355378/read-from-location-on-console-c-sharp  
https://stackoverflow.com/questions/54317061/redirect-output-from-running-process-visual-c/54317865  

## Methods 
**ReadConsole.GetChar**  
Gets a character given its position  
**ReadConsole.GetText**  
Gets a text given its position   
**ReadConsole.IndexOfInConsole**  
Finds all occurrences of a text in the console  
**ReadConsole.GetCursorPosition**  
Gets the current cursor position  
**ReadConsole.GetConsoleInfo**  
Gets all information about the console window  

## License
MIT License - See license file  
Copyright (c) 2020 Proxmea AB, Christoffer Järnåker