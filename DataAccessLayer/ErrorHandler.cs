using System.Data.SqlClient;
using System;

namespace CujoPasswordManager.DataAccessLayer
{
    public class ErrorHandler
    {
        public const string empty = "Reqired field is empty, try again!",
            wrongPass = "Username or Password is incorrect, please try again!",
            failed = "Registration Failed! An unknown error has occured!",
            acctIssue = "There is an issue with your account, contact sitemaster for assistance!",
            invalidLoginToken = "Something went wrong, redirecting back to the login page...",
            notPermitted = "You do not have permission to access this page!";

        public static string SQL(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                    return "Account Already Exists!";

                case 11001:
                    return "SQL Server unavailable, contact DB admin for assistance!";

                case 40613:
                    return "SQL Server is still starting up, try again in a few seconds!";

                case 40615:
                    //This runs if the web server's IP address has not been whitelisted by the SQL server
                    return "You do not have permission to access the database!  Contact the DB admin for assistance!";

                case 18456:
                    //This runs if the login attempt fails, possibly means the Users database doesn't exist!  Replace this if so
                    return "Login failed!";
            }

            //This runs if a new code is found that has not been handled yet
            throw new Exception(ex.Message);
        }
    }
}