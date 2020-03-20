# Boat Tours Manager
The BoatToursManager represents a boat company, offering group trips and boat rentals. It enables users to book group trips with large motor ships or rent single boats available within a specified location of the boat company.

Created for *MI138 - Web Application Development with .Net Technology* course, Fachhochschule Kiel.

### Setup

- Create a new database
- Execute the SQL script `BoatToursManager/database/boatTourManager.sql` in your database.
- Configure your DB credentials in `BoatToursManager/web.config`

### Features
* Setup: Configure your Gmail credentials in `EmailConfig` file for user registration verification and forget password.
#### User Registration
After registration user need to verify their account for login into the system. During registration, login activation code is generated and sended to the given email account. After click on the activation link user is redirected to system verification URL and activation code is compared with stored database activation code.
#### User Login
Firstly given login password is encrypted and  compare with encrypted store password on database.
#### Session Management
 After successfully login server store user information on session.
 #### Keep Me Login
 User can store their Credentials on browser  for the next time login.
 #### Forget Password
 User can reset new password if hs/she forgot their password. User should enter valid gmail account to get new password reset link.
 #### Rent a Boat

