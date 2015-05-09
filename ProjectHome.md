# Brief Description #
This tool is designed to automatically populate your TV show library with the appropriate assets for XBMC to recognise. It uses the configuration settings of TVRename, and scrapes the relevant data from the TVDB.
It will download, and place in the correct location, the following files:

  * fanart.jpg
  * folder.jpg
  * season-all.tbn
  * tvshow.nfo
  * seasonXX.tbn (per season)
  * folder.jpg (per season optionally)
  * {episodename}.nfo (per episode)
  * {episdoename}.tbn (per episode)

# Setup Instructions #
During installation you should be prompted for the path to your TVRename settings file.
This is an XML file created by TVRename, usually found in your user application data folder,
e.g. `C:\Users\{USERNAME}\AppData\Roaming\TVRename\TVRename\2.1\TvRenameSettings.xml`

You should locate this file and copy and paste the path, including the file name, into
the dialog box when prompted. You can alter the path at any time by editing the config file
in the installation directory. This tool will not modify your TVRename settings XML file.
You can create a copy of your settings file and use that instead.
## Other options ##
There are some other options you can alter in the config file in the installation directory:

### Logging ###
Logs of your last ten days of activity are recorded in your application data folder
### Disabling actions ###
You can stop this tool from creating any assets in your library by setting the `DisableAllActions` value to `true`. The tool will function the same as
normal, but will not actually save any images of NFO files to your collection, so it can be used as a test to see what the outcome of running the tool would be.
### Delays ###
You can add either a delay at the end of the program, or after each series has been processed, to give you time to examine the console output. You can edit the values of the `PostCompletionDelay` and `PostSeriesDelay` keys.
They should be values in milliseconds (0 for none).
### folder.jpg For Each Season ###
You can change the value of `GetFolderJpgForSeasons` to `true` to download folder.jpg file into each season folder (thanks to originallinuxguy for the request).

# Detailed Description #

In February 2010, LifeHacker published an article describing one way to automatically download TV episodes and import them into your XBMC media library:
http://lifehacker.com/5475649/set-up-a-fully-automated-media-center

For the last couple of years I have been manually organising my TV library with TVRename, and Ember Media Manager. I decided to try to automate this process.

I found TVRename can be run from the command line to automatically copy/move episodes to the correct location and rename the episode to the correct format.

However, Ember Media Manager cannot scrape the correct assets from The TVDB using a command line prompt - it's a manual process.

Therefore I created this scraper to download the correct assets from the command line.

More information can be found on the wiki for this project.

# Licences and Acknowledgements #
This tool uses SharpZipLibrary:

SharpZipLibrary samples
Copyright (c) 2007, AlphaSierraPapa
All rights reserved.
//
Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:
//
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials  provided with the distribution.
//
Neither the name of the SharpDevelop team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written  permission.
//
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS &AS IS& AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

This tool uses NLog, which is open source software, distributed under the terms of BSD license.

All assets created by this tool are download from http://www.thetvdb.com, using
their excellent API. Please support their good work by contributing
images via their website!

This tool would, of course, not be possible without TV Rename,
which is an excellent piece of software and should be supported at
http://www.tvrename.com.