# ThrottleStop PowerProfiler

## About PowerProfiler

This is an application that allows you to create an unlimited number of TPL tab profiles for the ThrottleStop
application.

## For users

### Where to download the application

You can download the latest version of the application
from [releases page](https://github.com/Focus1337/ThrottleStop-PowerProfiler/releases).

### How to install the application

You need to extract the files from the archive to the ThrottleStop folder.

An example with an image below.

![Extracted Example](https://i.imgur.com/S5NSd3T.png "Title")

### How to use the application

#### The application is launched with a terminal like this:

```
.\PowerProfiler longPower shortPower
```

* `longPower = Long Power PL1`, `shortPower = Short Power PL2`
* Both variables accept only positive integers.
* And also Long Power should be greater than Short Power.

Example using with a terminal:

   ```
   .\PowerProfiler 60 95
   ```

#### The best way to use the application is to launch it in `.bat files`

The archive will contain some .bat files as an example.

You can create an unlimited number of .bat files. For example, you can put .bat files in the ThrottleStop folder with the following content:

````
@echo off
cd /d "."
PowerProfiler.exe 60 95

REM You can use "pause" if you don't want the console to close automatically.
pause
````
![Bat Files Example](https://i.imgur.com/Ni75mLt.png "Bat files")

For example, further you can create shortcuts to these files and place them anywhere.

#### How to use configuration file

In version 1.0.0, the [PowerProfiler.ini](PowerProfiler.ini) file with the program configuration consists of 3
sections: `General`, `Calculator`, `Process`.

| **Options & Sections** 	 | **Default Value** 	 | **Accepted Values** 	 |                                                                     **Description**                                                                                                                                          	                                                                      | **Safety** 	 |
|:------------------------:|:-------------------:|:---------------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|:------------:|
|  **General Section**  	  |          	          |           	           | The section responsible for the main settings of the program. Changes to option values are safe.                                                                                                                                                                                                  	 |  Safe    	   |
|   CalculatePowerLimits   |        True         |      True/False       |                                                                                                                               If true, calculates Power Limits in HEX                                                                                                                               |     Safe     |
|   SetPowerLimits     	   |    True       	     |   True/False     	    |           If true, sets calculated Power Limits Values in ThrottleStop.ini file.                                                                                                                                                                                                       	            |  Safe    	   |
|  RestartThrottleStop  	  |    True       	     |   True/False     	    | If true, ThrottleStop will be restarted.                                                                                                                                                                                                                                                          	 |  Safe    	   |
|            	             |          	          |           	           |                                                                                                                                                  	                                                                                                                                                  |      	       |
| **Calculator Section** 	 |          	          |           	           | The section is responsible for calculating the HEX values for the ThrottleStop.ini file. Danger zone, better not to change anything.                                                                                                                                                              	 |  Unsafe   	  |
|   LongPowerBase     	    |   DF8000     	    |      HEX      	       |                          Base value for Long Power PL1 - POWERLIMITEAX in ThrottleStop.ini. This value means 0W in HEX. Change carefully only if your base value is different.                                                                                           	                          |  Unsafe   	  |
|   ShortPowerBase     	   |   438000      	    |       HEX    	        |                          Base value for Short Power PL1 - POWERLIMITEDX in ThrottleStop.ini. This value means 0W in HEX. Change carefully only if your base value is different.                                                                                          	                          |  Unsafe   	  |
|     Step          	      |     8         	     |     Integer     	     |          This value means the following: Step Value = 1W. That is, if you decrease or increase by 1W in ThrottleStop, then the stored value of POWERLIMITEAX and POWERLIMITEDX options in ThrottleStop.ini is changed to Step Value in HEX. It is not recommended to change this value. 	           |  Unsafe   	  |
|    HexPrefix       	     |    0x00       	     |       HEX    	        | The prefix that is added to the computed values. Change only if in your case the prefixes of the  POWERLIMITEAX and POWERLIMITEDX options in ThrottleStop.ini are different.                                                                                                                      	 |  Unsafe   	  |
|            	             |          	          |           	           |                                                                                                                                                  	                                                                                                                                                  |      	       |
|  **Process Section**  	  |          	          |           	           | The section is responsible for working with the process. Safe, because on version 1.0.0 it is only responsible for working with the ThrottleStop process.                                                                                                                                         	 |  Safe    	   |
|    ProcessName      	    |  ThrottleStop   	   |      Text      	      | Allows you to change the process name. For example, if you changed the name of the ThrottleStop.exe file and now the process  is called differently in the task manager, this option can help you.                                                                                                	 |  Safe    	   |

## For developers

### Documentation

So far documentation with examples and descriptions of functions is in progress, but you can look at the documentation
of methods
[here](https://github.com/Focus1337/ThrottleStop-PowerProfiler/tree/main/PowerProfiler).

### How to publish the application

The [PowerProfiler.csproj](PowerProfiler/PowerProfiler.csproj)
file already has some properties configured for application publishing:

```
<PublishSingleFile>true</PublishSingleFile>
<PublishTrimmed>true</PublishTrimmed>
```

You just need to write the following command in the CLI:

``
dotnet publish -c Release -r win-x64
``

If you are interested in other options, use the Microsoft documentation.