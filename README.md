# Game Store
GameStore is an online game store that allows users to buy, sell, and create games. It provides a platform to interact with existing games as well as create and edit new ones. Ukrainian localization is used in the project.

### Technologies
The front-end of GameStore is built using React.js, while the back-end is built using .Net and utilizes a RESTful API. The application utilizes various services, including Braintree for transaction processing, ethereal.email as a fake SMTP client, and Google OAuth2 for authentication through Google accounts.

### Architecture
GameStore follows a client-server architecture and utilizes a multi-layered architecture on the server-side. The application interacts with a database using code-first technology, which requires no additional setup.

### Services
* Braintree: Payment processing service.
* Ethereal.email: Fake SMTP client for email functionality.
* Google OAuth2: Authentication service.

### Running the Project
To run the GameStore project on your local machine, follow these steps:
1. Clone the repository.
2. Install the necessary dependencies for both the front-end and back-end.
    - For a list of npm packages used in the frontend, see [npm packages](/listOfNpmPackages.txt).
    - For a list of NuGet packages used in the backend, see [nuget puckages](/listOfNugetPackages.txt).
3. Add necessary user secrets in the API project for the SMPT client Braintree services.
    ````
    
    "MailSettings": {
        "Mail": "YourEmail",
        "DisplayName": "YourDisplayName",
        "Password": "YourPassword",
        "Host": "smtp.ethereal.email", // Or use another host
        "Port": 587 
    },
    "BraintreeGateway": {
        "Environment": "PutEnvironmentHere",
        "MerchantId": "YourMerchantId",
        "PublicKey": "YourPublicKey",
        "PrivateKey": "YourPrivateKey"
    }
  
    ````
    
4. Add necessary fields in ClientApp/.env.development such as:
    - PORT=44458 {if you want to use another port, change this and the other one in API launchSettings.json }
    - HTTPS=true 
    - REACT_APP_GOOGLE_OAUTH2_CLIENT_ID=YourGoogleOAuthClientID
    - REACT_APP_BRAINTREE_AUTHORIZATION_CODE=YourBraintreeAuthCode
    
5. Add necessary fields in ClientApp/.env.development.local such as:
    - SSL_CRT_FILE=C:\Users\... {path to your .pem file}
    - SSL_KEY_FILE=C:\Users\... {path to your .key file}
    
6. Start the front-end and back-end servers.
7. Access the application in your web browser.

### License
This project is licensed under the [Apache-2.0 license](http://www.apache.org/licenses/LICENSE-2.0).

### Contact Information
For any questions or concerns regarding GameStore, please contact the developer:

Name: Mykolai Tymchenko

Email: nykolai.tymchenko@gmail.com
