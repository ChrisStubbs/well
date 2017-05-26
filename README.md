# Project Setup

1. Pre-requisites
    * Install latest version of Node
    * Install ConEmu
	* run npm install -g Webpack
    
2. Git repo: [Well on P&H bitbucket](http://ho-nt-vmdevtfs1:7990/scm/mig/well.git)
    * Open ...\src\Well.All.sln
    * Delete the node_modules folder in the dashboard folder (..src\1. Layers\1.1 Presentation\Well.Dashboard\node_modules) if the folder exists
    * Do a fresh NPM install from the Dashboard Folder (..src\1. Layers\1.1 Presentation\Well.Dashboard)
        - NPM Install

2. Publish Well Database

3. Setup Well Security

4. On IIS 
	* Create a virtual directory with the name Well
	* Under this directory add the Dashboard and API sites
	* On the directory under Authentication make sure Windows Authentication is enabled
	* Run the app pool for these new sites under the following credentials:  
           User: "palmerharvey\csdevservice"  
           Pass: C5D3vservice
	* Make sure that on SQL server this user has permissions for the databases

5. Angular
    * Open a ConEmu console at the Well.Dashboard folder
    * Run npm run build
    * Change a single TypeScript file in the project to start rebuilding of all TypeScript files
    * Ensure Output javascript files have been generated to the Scripts\Angular2 folder
    