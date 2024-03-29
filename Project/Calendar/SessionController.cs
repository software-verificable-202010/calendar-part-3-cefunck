﻿using System.Collections.Generic;

namespace Calendar
{
    public static class SessionController
    {
        #region Constants
        #endregion


        #region Fields
        private static List<User> calendarUsers;
        private static User currentUser;

        #endregion


        #region Properties
        public static User CurrenUser 
        {
            get 
            {
                return currentUser;
            }
            set 
            {
                currentUser = value;
            }
        }
        #endregion


        #region Methods
        public static User GetUserByName(string name)
        {
            LoadDefaultUsers();

            foreach (User user in calendarUsers)
            {
                bool haveTheSameName = user.Name == name;

                if (haveTheSameName)
                {
                    return user;
                }
            }
            return null;
        }

        private static void LoadDefaultUsers()
        {
            const string defaultUserName1 = "usuario1";
            const string defaultUserName2 = "usuario2";
            List<User> defaultUsers = new List<User>()
                {
                    new User(defaultUserName1),
                    new User(defaultUserName2)
                };
            calendarUsers = defaultUsers;
        }

        #endregion
    }
}
