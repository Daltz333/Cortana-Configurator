using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Cortana Enabler/Disabler! Written by Dalton Smith");
            Console.WriteLine("Guarenteed working as of Windows 10 OS Build 17763.55");
            Console.WriteLine("=============================================");
            Console.WriteLine();
            Console.Write("Type Enable(1) or Disable(2) Cortana? ");

            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                option = 0;
            }
;
            String enabled = "";

            if (option == 1)
            {
                enabled = "Set-ItemProperty -Path $path -Name \"AllowCortana\" -Value 1";

            } else if (option == 2)
            {
                enabled = "Set-ItemProperty -Path $path -Name \"AllowCortana\" -Value 0";

            } else if  (option == 0)
            {
                Console.WriteLine("Your input was invalid");
            }

            Console.WriteLine();

            if (enabled != "")
            {
                //create powershell instance
                PowerShell powerInstance = PowerShell.Create();
                powerInstance.AddScript("New-Item -Path \"HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\" -Name \"Windows Search\"");

                powerInstance.AddScript("$path = \"HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\Windows Search\"");
                powerInstance.AddScript(enabled);
                powerInstance.AddScript("Stop-Process -name explorer");
                powerInstance.Invoke();
                if (option == 1)
                {
                    Console.WriteLine("Enabled Cortana");
                }
                else
                {
                    Console.WriteLine("Disabled Cortana");
                }
           
            }
            else
            {
                Console.WriteLine("There was an error, please restart the application");
                Console.Read();
            }

            if (option == 2)
            {
                Console.WriteLine();
                Console.WriteLine("=============================================");
                Console.WriteLine("Would you also like to uninstall the Cortana Process?");
                Console.WriteLine("Please note that this is not possible on later Windows versions");
                Console.WriteLine("Uninstalling Cortana Process will also disable Windows Search");
                Console.Write("Uninstall y/n? ");
                String uninstall = Console.ReadLine();

                if (uninstall == "y")
                {
                    PowerShell powerInstance = PowerShell.Create();
                    powerInstance.AddScript("Get-AppxPackage | Where-Object {$_.Name -eq ‘Microsoft.Windows.Cortana’} | Select-String “Cortana”");
                    var results = powerInstance.Invoke();

                    StringBuilder stringBuilder = new StringBuilder();


                    foreach (PSObject obj in results)
                    {
                        stringBuilder.AppendLine(obj.ToString());
                    }

                    String delete = stringBuilder.ToString();
                    powerInstance.AddScript("Remove-AppxPackage " + delete);
                    powerInstance.Invoke();
                    Console.WriteLine("Uninstalled Cortana");
                }
                else
                {
                    //do nothing
                }

            }

            Console.WriteLine();
            Console.WriteLine("You may now close the program");
            Console.Read();

        }

    }
}
