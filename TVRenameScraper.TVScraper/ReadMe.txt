﻿TV Rename TVDB Scraper

This tool is designed for TV Rename users. It will use you TV Rename settings
to scan your collection of TV shows, and download relevant assets from The TVDB.

It will download:
	For each series:
		folder.jpg
		fanart.jpg
		season-all.tbn
		tvshow.nfo
	For each season:
		seasonXX.tbn
        	folder.jpg (optionally)
	For each episode:
		EPISODE_NAME.nfo
		EPISODE_NAME.tbn
		
The tool requires you to have a folder for each TV series, and within that a folder for each Season.

It will respect the settings you have in TV Rename, including "Use DVD Order". 
You can force the use of DVD order by using the command line option "/dvd".

I have developed this tool for my own use, but if people find it useful
I am willing to improve on it or release the source code.


SETUP:

During installation you should be prompted for the path to your TVRename settings file.
This is an XML file created by TVRename, usually found in your user application data folder,
e.g. C:\Users\{USERNAME}\AppData\Roaming\TVRename\TVRename\2.1\TvRenameSettings.xml

You should locate this file and copy and paste the path, including the file name, into
the dialog box when prompted. You can alter the path at any time by editing the config file
in the installation directory. This tool will not modify your TVRename settings XML file.
You can create a copy of your settings file and use that instead.

OTHER OPTIONS:

There are some other options you can alter in the config file in the installation directory:

	- Logging - logs of your last ten days of activity are recorded in your application data folder
	- Disabling actions - you can stop this tool from creating any assets in your
		library by setting the DisableAllActions value to true.
	- Delays - you can add either a delay at the end of the program, or after each series
		has been processed, to give you time to examine the console output. You can
		edit the values of the PostCompletionDelay and PostSeriesDelay keys.
		They should be values in milliseconds (0 for none).
   	- Folder.jpg for seasons - Change the value GetFolderJpgForSeasons to "true" to download
        	folder.jpg into each season folder


History:
v1.0.4.0 - Added option to download "folder.jpg" into each season folder, at request from originallinuxguy - thanks for using!
           Added facility to check for a new version of this program - if a new version is available it will
           be highlighted in the log and console output

v1.0.3.0 - Added ability to scan specials folders for each show

v1.0.2.0 - Added facility to remove existing .nfo files that aren't valid for XBMC

v1.0.0.0 - Released


This tool uses SharpZipLibrary:

SharpZipLibrary samples
Copyright (c) 2007, AlphaSierraPapa
All rights reserved.
//
Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:
//
- Redistributions of source code must retain the above copyright notice, this list
  of conditions and the following disclaimer.
//
- Redistributions in binary form must reproduce the above copyright notice, this list
  of conditions and the following disclaimer in the documentation and/or other materials
  provided with the distribution.
//
- Neither the name of the SharpDevelop team nor the names of its contributors may be used to
  endorse or promote products derived from this software without specific prior written
  permission.
//
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS &AS IS& AND ANY EXPRESS
OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

This tool uses NLog, which is open source software, distributed under the terms of BSD license.

All assets created by this tool are download from http://www.thetvdb.com, using
their excellent API. Please support their good work by contributing
images via their website!

This tool would, of course, not be possible without TV Rename,
which is an excellent piece of software and should be supported at
http://www.tvrename.com.