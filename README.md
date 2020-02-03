# SharpLocker-2.0
[SharpLocker](https://github.com/Pickfordmatt/SharpLocker) but betterer!

## What's this?
This incredibly well written program fakes a Windows 10 log-in screen to trick people with sub-average computer knowledge into ~~stealing~~typing out their password. Also, for reasons unbeknownst to mankind, this was written with windows forms.

## Features

### Automation

It dynamically loads the user avatar, wallpaper and display name of the user into the fake lock screen for maximum realism.

### Realism

For behaving like the real deal, SharpLocker 2.0 implements (and is limited to) the following features:

- Turn non-primary displays black
- Disable the task bar
- Swallow often used key combinations to close or change the current window
- Disabling the windows menu
- Looks somewhat like the regular Windows 10 Login

### Extensible

To lazy to modify the existing code? Write your own and compile it to a `.dll` file, which will automatically be picked up and used instead.

## Usage

The program itself doesn't do much on it's own. That's your job!
Therefor if you want to get access to information that this program is more or less secretly collecting and to do some *things* with it - **REJOICE** - we have implemented two ways for you to do so easily.

### Use The DefaultBadStuff - Class

You will find this class in the "Classes" directory.
This super complex piece of code contains one extremely large and hard to understand function:

```cs
public void Now(string password, string username, string domainName)  
{
  
}
```

You may think to yourself: "Why is the function called `Now`?". Well, let me tell you. Calling the function looks like this...
  
```cs
DoBadStuff.Now(PasswordTextBox.Text, Environment.UserName, Environment.UserDomainName);
```

... and that's funny.

Anyway, if you want to get the data once the absolutely real login is performed, just place your code into the `Now` function and **BOOM**, you're set.
The `Now` function gets called when the login button is pressed or the login is triggered by an enter press key event and gives you access to the users account name, domain name as well as the "password" they just entered.

### Create A .dll-File That Implements The IDoBadStuff Interface

Modifying existing code bases can be tedious. So if you're too ~~pretentious~~ to edit the poor, little `Now` function, just create your own `.dll`-file that implements the `IDoBadStuff` interface (which is available in the `ocker_2._0.Interfaces` namespace).

"Why would I ever do that?", you may ask yourself.

This implementation has two big **+** *(pros)* and one big **-** *(cons)*.

#### The Good

- Quickly change the *functionality* of the program by exchanging the `.dll` file
- Update to the latest release of SharpLocker 2.0 without reimplementing your own data ~~stealing~~borrowing process.
 
#### The Bad
 
- You'll have two files to distribute instead of just one, the executable **as well** as the `.dll` file

~~The Ugly~~

"Alright, how does it work?"

1. Create a new class library project
2. Add a reference to the `SharpLocker 2.0` project
3. Create a new class in your class library that implements the IDoBadStuff interface
4. Implement the `Now` function
5. Add your code
6. Build your class library project
7. Put the `.dll` file in the same directory as the `SharpLocker.exe`
 
On launch, SharpLocker will search through all `.dll` files in its current working directory alphabetically. The first `.dll` to implement the `IDoBadStuff` interface will be loaded into SharpLocker and replaces the `DefaultBadStuff` class. If no matching `.dll` file is found, `DefaultBadStuff` will be used.
