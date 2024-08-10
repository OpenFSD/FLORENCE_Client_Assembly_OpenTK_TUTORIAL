﻿namespace FLORENCE_Client_Assembly
{
    namespace FrameworkSpace
    {
        namespace ClientSpace
        {
            public class Execute
            {
                private static Thread listen_To_Server;
                private static Thread listen_To_User_Peripherals;
                //private static Thread do_Graphics = null;

                public Execute()
                {
                    listen_To_Server = new Thread(new ThreadStart(FLORENCE_Client_Assembly.FrameworkSpace.ClientSpace.Algorithms.Thread_Listen_To_Server));
                    while (listen_To_Server == null) { /* wait untill created */ }
                    listen_To_Server.Start();

                    listen_To_User_Peripherals = new Thread(new ThreadStart(FLORENCE_Client_Assembly.FrameworkSpace.ClientSpace.Algorithms.Thread_Listen_To_User_Peripherals));
                    while (listen_To_User_Peripherals == null) { /* wait untill created */ }
                    listen_To_User_Peripherals.Start();

                    FLORENCE_Client_Assembly.FrameworkSpace.ClientSpace.Algorithms.Thread_Do_Graphics();
                    //do_Graphics = new Thread(new ThreadStart(FLORENCE_Client_Assembly.FrameworkSpace.ClientSpace.Algorithms.Thread_Do_Graphics));
                    //while (do_Graphics == null) { /* wait untill created */ }
                    //do_Graphics.Start();
                }
            }
        }
    }
}
