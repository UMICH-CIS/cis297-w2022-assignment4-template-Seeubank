using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// author: Sarah Eubank
/// creation date: 2/13/2022
/// last modified: 2/23/2022
/// purpose: program that creates a class PatientClass that contains a patient's id number, their name, and a current balance owed
/// program writes and reads this info to a file
/// </summary>
namespace PatientRecordApplication
{
    class PatientRecordSystem
    {
        /// <summary>
        /// author: Sarah Eubank
        /// creation date: 2/13/2022
        /// last modified: 2/17/2022
        /// </summary>
        public class Patientclass
        {
            public int idNum;
            public string name;
            public decimal currentBalance;

        }

        /// <summary>
        /// author: Sarah Eubank
        /// creation date: 2/23/2022
        /// last modified: 2/23/2022
        /// class for user defined exception
        /// throws if there is no input entered by the user
        /// </summary>
        public class NoInput : Exception
        {
            public NoInput(string message) : base(message)
            {
            }
        }

        static void Main(string[] args)
        {
            bool showMenu = true;
            string path = @"C:\Users\Sarah Eubank\Desktop\CIS 297\Assingments\Assignment4\input.txt";

            FileOperation(path);
            while(showMenu)
            {
                showMenu = DisplayMenu(path);
            }
        }

        /// <summary>
        /// author: Sarah Eubank
        /// creation date: 2/15/2022
        /// last modified: 2/23/2022
        /// function to open a file at path checks that file opened properly
        /// </summary>
        static void FileOperation(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    var myfile = File.Create(path);
                    myfile.Close();
                }

                Console.WriteLine("File created at " + File.GetCreationTime(path));
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
            catch(AccessViolationException)
            {
                Console.WriteLine("ERROR: file creation/reading in protected memory. Change file path.");
            }
        }

        /// <summary>
        /// author: Sarah Eubank
        /// creation date: 2/15/2022
        /// last modified: 2/17/2022
        /// function that displays menu of program functions
        /// </summary>
        static bool DisplayMenu(string path)
        {
            Console.Clear();
            Console.WriteLine("1. Enter patient info.");
            Console.WriteLine("2. Search by ID number.");
            Console.WriteLine("3. Search by minimum balance owed.");
            Console.WriteLine("4. Exit");
            Console.Write("\r\nSelect an option: ");

            switch(Console.ReadLine())
            {
                case "1":
                    enterPatientInfo(path);
                    return (true);

                case "2":
                    searchByIDNumber(path);
                    return (true);

                case "3":
                    searchMinimumOwed(path);
                    return (true);

                case "4":
                    return (false);

                default:
                    return (true);
            }
        }

        /// <summary>
        /// author: Sarah Eubank
        /// creation date: 2/15/2022
        /// last modified: 2/23/2022
        /// function takes user entered patient info and adds it to file
        /// </summary>
        static void enterPatientInfo(string path)
        {
            Patientclass patient = new Patientclass();
            StreamWriter sw = File.AppendText(path);

            try
            {
                Console.Write("Enter patient ID number: ");
                patient.idNum = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter patient name: ");
                patient.name = Console.ReadLine();

                Console.Write("Enter balance owed: ");
                patient.currentBalance = Convert.ToDecimal(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("ERROR: Invalid entry.");
                Console.ReadLine();
            }

            sw.Write("{0}, {1}, {2}" , patient.idNum, patient.name, patient.currentBalance);
            sw.Write("\n");

            sw.Close();
        }

        /// <summary>
        /// author: Sarah Eubank
        /// creation date: 2/17/2022
        /// last modified: 2/23/2022
        /// function that allows user to search file by ID number
        /// displays any found to console
        /// </summary>
        /// <param name="path"></param>
        static void searchByIDNumber(string path)
        {
            StreamReader sr = new StreamReader(path);
            Patientclass patient = new Patientclass();
            string[] fields;
            int IDnum = 0;

            Console.Write("Enter ID number to search by: ");
            IDnum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\n");

            if (IDnum == 0)
            {
                throw (new NoInput("ERROR: Invalid ID number"));
            }
            else
            {
                string line = sr.ReadLine();

                while (line != null)
                {
                    fields = line.Split(',');
                    patient.idNum = Convert.ToInt32(fields[0]);
                    patient.name = fields[1];
                    patient.currentBalance = Convert.ToDecimal(fields[2]);

                    if (patient.idNum == IDnum)
                    {
                        Console.WriteLine("{0}, {1}, {2}", patient.idNum, patient.name, patient.currentBalance);
                    }

                    line = sr.ReadLine();
                }

                sr.Close();
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
            } 
        }

        /// <summary>
        /// author: Sarah Eubank
        /// creation date: 2/17/2022
        /// last modified: 2/23/2022
        /// function that searches the file for a minimum balanced owed
        /// displays any found to console
        /// </summary>
        /// <param name="path"></param>
        static void searchMinimumOwed(string path)
        {
            StreamReader sr = new StreamReader(path);
            Patientclass patient = new Patientclass();
            string[] fields;

            Console.Write("Enter minimum balanced owed to search: ");
            decimal owed = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("\n");

            string line = sr.ReadLine();

            while (line != null)
            {
                fields = line.Split(',');
                patient.idNum = Convert.ToInt32(fields[0]);
                patient.name = fields[1];
                patient.currentBalance = Convert.ToDecimal(fields[2]);

                if (patient.currentBalance >= owed)
                {
                    Console.WriteLine("{0}, {1}, {2}", patient.idNum, patient.name, patient.currentBalance);
                }

                line = sr.ReadLine();
            }

            sr.Close();
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
