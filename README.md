# Project Setup

1. Pre-requisites
    * Install latest version of Node
    * Install ConEmu [from here](https://conemu.github.io/)
	* run npm install -g Webpack
    * Install Webpack Extensions for visual studio 2015

2. Git repo: [Well on P&H bitbucket](http://ho-nt-vmdevtfs1:7990/scm/mig/well.git)
    * Open ...\src\Well.All.sln
    * Right-click "Restore NuGet Packages" on Solution to restore missing/required packages (including Octopack required for database)
    * Delete the node_modules folder in the dashboard folder (..src\1. Layers\1.1 Presentation\Well.Dashboard\node_modules) if the folder exists
    * Do a fresh NPM install from the Dashboard Folder (..src\1. Layers\1.1 Presentation\Well.Dashboard)
        - npm Install

3. Publish Well Database
    * Right click on 3. Database\Well and choose "Publish"

4. Setup Well Security
    * If Security has been previousl installed just goto http://localhost/Security.Dashboard/
    * Else
      - Obtain Security project from Git repo: [Well on P&H bitbucket](http://ho-nt-vmdevtfs1:7990/projects/MIG/repos/security/browse)
      - Set Security.Dashboard as the Startup Project
      - Run Security.Dashboard (runs on http://localhost/Security.Dashboard/)
      - Search for your own first name or surname
      - Check a permission (e.g. SuperUser) under Order Well and Save

5. On IIS 
	* Create a virtual directory with the name Well
	* Under this directory add the Dashboard and API sites
	* On the directory under Authentication make sure Windows Authentication is enabled
	* Run the app pool for these new sites under the following credentials:  
           User: "palmerharvey\csdevservice"  
           Pass: C5D3vservice
	* Make sure that on SQL server this user has permissions for the databases

6. Webpack compilation
    * Open a ConEmu console at the 1. Layers\1.1 Presentation\Well.Dashboard folder
    * Run npm run build
    * Ensure Output javascript files have been generated to the Scripts\Angular2 folder

7. Obtain Route and Order Data from Adam
    * File pickup folder is specified in 1. Layers\1.5 ACL\Well.Adam.Listener\App.config
      - rootFolder points at initial folder to obtain route/order files
      - archiveLocation points at a folder to move processed files into
    * Place your Adam test data files into the rootFolder location
    * Run 1. Layers\1.5 ACL\Well.Adam.Listener to process the Adam files
    
8. Obtain Update Data from Transend
    * File pickup folder is specified in 1. Layers\1.5 ACL\Well.Transend\App.config
      - downloadFilePath points at initial folder to obtain route/order files
      - archiveLocation points at a folder to move processed files into
    * Run 1. Layers\1.5 ACL\Well.Transend to process the TranSend files

