using ConsoleAppHospital;

namespace ConsoleAppHospital
{
    class Program
    {
        public static bool running = true;
        public static List<Employee> employees = new List<Employee>();
        public static string employeesFilePath = @"employeesFiles";
        static void Main(string[] args)
        {
            Headers();
            Directory.CreateDirectory(employeesFilePath);
            LoadEmployee(employees, employeesFilePath);
            while (running)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                ProcessInput(input);
            }
        }

        static void Headers()
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(" Welcome to the Hospital Management Portal!");
            Console.WriteLine(" Type \"help\" for available commands.");
            Console.WriteLine("--------------------------------------------");
        }

        static void ProcessInput(string? input)
        {
            switch (input)
            {
                case "help":
                    PrintHelp();
                    break;
                case "add":
                    AddEmployee(employees, employeesFilePath);
                    break;
                case "remove":
                    RemoveEmployee(employees, employeesFilePath);
                    break;
                case "load":
                    LoadEmployee(employees, employeesFilePath);
                    break;
                case "view":
                    ViewEmployee(employees);
                    break;
                case "page":
                    PageEmployees(employees);
                    break;
                case "exit":
                    ExitProgram();
                    break;
                default:
                    PrintDefault(input);
                    break;
            }

        }

        private static void PrintDefault(string? input)
        {
            Console.Write($"  Incorrect command");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" \"{input}\"");
            Console.ResetColor();
            Console.WriteLine(".");
            Console.WriteLine( "  Type \"help\" to learn more.");
        }

        private static void ExitProgram()
        {
            running = false;
            Console.WriteLine("See you next time!");
        }

        private static void PageEmployees(List<Employee> employees)
        {
            foreach (Employee employee in employees)
            {
                if (employee is IPageable pageable)
                {
                    pageable.IPage();
                }
            }
        }

        private static void ViewEmployee(List<Employee> employees)
        {
            Console.Write("Employee Id: ");
            string? input = Console.ReadLine();
            Employee? matchingIdEmployee = employees.Find(e => e.EmployeeId == input);
            if (matchingIdEmployee != null)
                Console.WriteLine(matchingIdEmployee.GetDescription());
            else 
                Console.WriteLine("There is no matching Id in Hospital");
        }

        private static void LoadEmployee(List<Employee> employees, string employeesFilePath)
        {
            string[] files = Directory.GetFiles(employeesFilePath);
            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);
                if (lines.Length == 5)
                    AddEmployee(employees, employeesFilePath, lines[1], lines[2], lines[3], lines[0], lines[4], firstUse: true);
                else
                    AddEmployee(employees, employeesFilePath, lines[1], lines[2], lines[3], lines[0], firstUse: true);
            }
            Console.WriteLine($"Loaded {files.Length} employees");
        }

        private static void RemoveEmployee(List<Employee> employees, string employeesFilePath)
        {
            Console.Write("Employee Id: ");
            string? input = Console.ReadLine();
            Employee? matchingIdEmployee = employees.Find(e => e.EmployeeId == input);
            if (matchingIdEmployee != null)
            {
                employees.Remove(matchingIdEmployee);
                string filePath = CreateTextFilePath(matchingIdEmployee, employeesFilePath);
                File.Delete(filePath);
                Console.WriteLine($"Employee {input} deleted.");
            }
            else
                Console.WriteLine("There is no matching Id in Hospital");
        }

        private static void PrintHelp()
        {
            Console.WriteLine("\tAvailable Commands:");
            Console.WriteLine("\tadd -  Add a new employee from portal.");
            Console.WriteLine("\tremove - Remove an employee from portal.");
            Console.WriteLine("\tload - Load existing employees from file.");
            Console.WriteLine("\tview - View an employee.");
            Console.WriteLine("\tpage - Page all medical employees.");
        }

        private static void AddEmployee(List<Employee> employees, string employeesFilePath,
        string? employeeId = null, string? firstName = null, string? lastName = null,
        string? jobTitle = null, string? extra = null, bool firstUse = false)
        {
            if (employeeId == null) { Console.Write("Employee Id: "); employeeId = Console.ReadLine(); }
            if (firstName == null) { Console.Write("First Name: "); firstName = Console.ReadLine(); }
            if (lastName == null) { Console.Write("Last Name: "); lastName = Console.ReadLine(); }
            if (jobTitle == null) { Console.Write("Job Title(doctor, nurse, custodian): "); jobTitle = Console.ReadLine(); }

            Employee employee;

            switch (jobTitle)
            {
                case "doctor":
                    if (extra == null) { Console.Write("Speciality: "); extra = Console.ReadLine(); }
                    employee = new Doctor
                    {
                        EmployeeId = employeeId,
                        FirstName = firstName,
                        LastName = lastName,
                        Specialty = extra
                    };
                    break;
                case "nurse":
                    if (extra == null) { Console.Write("Level: "); extra = Console.ReadLine(); }
                    employee = new Nurse
                    {
                        EmployeeId = employeeId,
                        FirstName = firstName,
                        LastName = lastName,
                        Level = extra
                    };
                    break;
                case "custodian":
                    employee = new Custodian
                    {
                        EmployeeId = employeeId,
                        FirstName = firstName,
                        LastName = lastName,
                    };
                    break;
                default:
                    Console.WriteLine("You typed wrong job title.");
                    return;
            }

            employees.Add(employee);
            SaveEmployee(employee, employeesFilePath);
            if (!firstUse)
                Console.WriteLine($"Employee {employeeId} added successfully.");
        }

        private static void SaveEmployee(Employee employee, string employeesFilePath)
        {
            string filePath = CreateTextFilePath(employee, employeesFilePath);
            if (File.Exists(filePath))
                return;
            try
            {
                File.AppendAllText(filePath, employee.GetDescriptionFile());
            }
            catch(Exception ex){
                if (ex is not FileNotFoundException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error: ");
                    Console.ResetColor();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press any key to exit...");
                }
            }
        }

        private static string CreateTextFilePath(Employee employee, string employeesFilePath)
        {
            string fileFormat = ".txt";
            string fileName = employee.EmployeeId + fileFormat;
            string filePath = Path.Combine(employeesFilePath, fileName);
            return filePath;
        }
    }
}