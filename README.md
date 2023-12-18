The project is set up with .NET Core 8 with Docker support.
This is a sample .NET Core Web API project using an in-memory Entity Framework database. 
The architecture follows the Onion architecture pattern. 
The application uses NLog for logging application exceptions.
Uses Unit of work with Generic Repository Pattern for accessing data.
Written test cases using XUnit and Moq framework.
Application has support for Swagger UI.

Getting Started
To run this project, you'll need the following installed on your machine:

.NET Core SDK
Docker

Clone the Repository

_bash_
git clone https://github.com/Pavankuamr-Mara/BankingSystem.git
cd BankingSystem

Build and Run the Docker Container
_bash_
docker build -t my-web-api
docker run -p 8080:80 my-web-api

Endpoints
1. Get Users and Associated Accounts
http GET /Admin/ViewAccounts

3. Add New Account to Existing User
http POST /Accounts/AddAccounts
Request Body:
{
  "UserId": "_GUID_"
}

3. Remove Account of a User
http DELETE /Admin/RemoveAccount
Request Body:
{
  "UserId": "_GUID_"
}

4. Check Account Balance
http GET /Banking/ViewBalance/{AccountNumber}

6. Deposit Money to an Account
http POST /api/accounts/deposit
Request Body:
{
  "AccountId": "_GUID_",
  "Amount": "_100_"
}

6. Withdraw Money from an Account
http POST /Banking/WithdrawMoney
Request Body:
{
  "AccountId": "_GUID_",
  "Amount": "_100_"
}

Transaction Conditions
A user can have as many accounts as they want.
A user can create and delete accounts.
A user can deposit and withdraw from accounts.
An account cannot have less than $100 at any time.
A user cannot withdraw more than 90% of their total balance in a single transaction.
A user cannot deposit more than $10,000 in a single transaction.
Logging
Application exceptions are logged using NLog.

Onion Architecture
The project is structured using the Onion architecture pattern.

Domain: Contains entities
DAL: Unit of work patern to update entities using Generic repositry.
Infrastructure: Business logic.
WebAPI: The entry point for the application, contains controllers.
InfrastructureTest : XUnit test cases with Moq framework

Dependencies
In-Memeory Entity Framework Core
NLog
XUnit
Moq
FuentAssertion
Swashbuckle

License
This project is licensed under the MIT License.
