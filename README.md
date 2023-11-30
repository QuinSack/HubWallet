Getting Started

Prerequisites
.NET 6 SDK
Visual Studio(recommended) or Visual Studio Code for development

Installation
1. Clone the repository: git clone https://github.com/yourusername/HubWallet.git
2. Navigate to the project folder: cd HubWallet
3. Run the application: dotnet run

Configuration
Update the appsettings.json file with your database connection string and any other necessary configuration.

Usage
User Registration:
Register a new wallet user: POST /api/wallets/register

Request Body:
{
  "phoneNumber": "*************",
  "password": "**********"
}

User Login:
Login with wallet user credentials: POST /api/wallets/login

Request Body: 
{
  "phoneNumber": "*************",
  "password": "**********"
}

Add Wallet: 
Add a new wallet: POST /api/wallets/add

Request Body:
{
  "id": 1,
  "name": "",
  "type": "",
  "accountNumber": "",
  "accountScheme": "",
  "createdAt": "2023-11-30T13:23:27.145Z",
  "owner": ""
}

Remove Wallet: 
Remove a wallet by ID: DELETE /api/wallets/remove/{id}

Get Wallet By ID:
Retrieve a wallet by ID: GET /api/wallets/get/{id}

Get All Wallets:
Retrieve all wallets: GET /api/wallets/getAll

Get Wallets By Owner:
Retrieve wallets by owner's phone number: GET /api/wallets/getByOwner/{phoneNumber}

Authentication and Authorization
This API uses JWT (JSON Web Tokens) for authentication. To access protected endpoints, include a valid Bearer token in the request header.

Error Handling
The API returns appropriate HTTP status codes and error messages for different scenarios. Refer to each endpoint's documentation for details on possible responses.




