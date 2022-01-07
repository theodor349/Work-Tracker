# Work Tracker 
Aims to make it easier to log, when I'm working for a given employer and generate the invoice that I have to hand in every month.

## Key Features
 * Enter when I'm working and for which employer, with Start/Stop buttons
 * Generate an invoice for a given month and employer
 * Keep track of how many hours I have invoiced, to make sure I do not invoice too much nore too little

## Roadmap
- [x] Create Backend
- [ ] Create Frontend
  - [ ]  Setup Communication with API
  - [ ]  Draft UI
  - [ ]  Link UI to Backend

# Getting Started
## Prerequisites 
- MSSQL
- .Net 6
- Valid Active Directory Tenant - [Guide](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-create-new-tenant)

## Installation 
1. Clone the repository 
2. Pubish database project found at `WorkTracker\DB` to a database
3. Update `appsettings.json` found at `WorkTracker\Backend\API\appsettings.json`
   - Setup `connectionstring` to point to the database 
   - Setup `AzureAd` to point at a valid Azure Active Application - [Guide](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#get-tenant-and-app-id-values-for-signing-in)
4. Set the `API` project as `startup-project` and press `F5`

# Contribute
If you want to help out, just ask 😊
