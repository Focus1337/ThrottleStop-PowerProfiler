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

#### (IMPORTANT) Before using PowerProfiler

Before using PowerProfiler, in the ThrottleStop application in the TPL tab,
you need to uncheck "Disable Controls" and check the boxes for Long Power PL1 and Short Power PL2 along with their "
Clamps".

![TPL Example](https://i.imgur.com/xPDbT1k.png "T")

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

You can create an unlimited number of .bat files. For example, you can put .bat files in the ThrottleStop folder with
the following content:

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
| **Calculator Section** 	 |          	          |           	           |                             The section is responsible for calculating the HEX values for the ThrottleStop.ini file.                                                                                                                                                  	                             |  Unsafe   	  |
|   LongPowerBase     	    |    DF8000     	     |      HEX      	       |                          Base value for Long Power PL1 - POWERLIMITEAX in ThrottleStop.ini. This value means 0W in HEX. Change carefully only if your base value is different.                                                                                           	                          |  Unsafe   	  |
|   ShortPowerBase     	   |    438000      	    |       HEX    	        |                          Base value for Short Power PL1 - POWERLIMITEDX in ThrottleStop.ini. This value means 0W in HEX. Change carefully only if your base value is different.                                                                                          	                          |  Unsafe   	  |
|     Step          	      |     8         	     |     Integer     	     |          This value means the following: Step Value = 1W. That is, if you decrease or increase by 1W in ThrottleStop, then the stored value of POWERLIMITEAX and POWERLIMITEDX options in ThrottleStop.ini is changed to Step Value in HEX. It is not recommended to change this value. 	           |  Unsafe   	  |
|    HexPrefix       	     |    0x00       	     |       HEX    	        | The prefix that is added to the computed values. Change only if in your case the prefixes of the  POWERLIMITEAX and POWERLIMITEDX options in ThrottleStop.ini are different.                                                                                                                      	 |  Unsafe   	  |
|            	             |          	          |           	           |                                                                                                                                                  	                                                                                                                                                  |      	       |
|  **Process Section**  	  |          	          |           	           | The section is responsible for working with the process. Safe, because on version 1.0.0 it is only responsible for working with the ThrottleStop process.                                                                                                                                         	 |  Safe    	   |
|    ProcessName      	    |  ThrottleStop   	   |      Text      	      | Allows you to change the process name. For example, if you changed the name of the ThrottleStop.exe file and now the process  is called differently in the task manager, this option can help you.                                                                                                	 |  Safe    	   |

#### (IMPORTANT) How to calculate LongPowerBase and ShortPowerBase

Due to the fact that I do not have a huge number of CPUs of different generations on hand and have no knowledge of how
ThrottleStop sets HEX values in its configuration file, your LongPowerBase and ShortPowerBase values may not match the
preset ones.

I highly recommend calculating your base values like this:

1. Check the values of Long Power PL1 and Short Power PL2 in ThrottleStop (make sure you
   read [this](#important-before-using-powerprofiler)).
2. Open [HEX calculator](https://www.calculator.net/hex-calculator.html).
3. In `Convert Decimal Value to Hexadecimal Value` enter the result of `multiplying the Long or Short Power by 8` and
   click `Calculate`.
4. In `Hexadecimal Calculation—Add, Subtract, Multiply, or Divide`

   4.1. choose `subtract operation`

   4.2. `1st argument: POWERLIMITEAX or POWERLIMITEDX value` from ThrottleStop.ini without `0x00`

   4.3. `2nd argument is result of 3rd step`

   4.4. click `Calculate`.
5. You got the base value, you can enter it in PowerProfiler.ini.

Example step by step:

1. Long Power PL1 = 40W
2. 40 * 8 = 320. Result: 140
3. POWERLIMITEAX=0x00DF8140. So: DF8140 - 140 = DF8000
4. my LongPowerBase = DF8000

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