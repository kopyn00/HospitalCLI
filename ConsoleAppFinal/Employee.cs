namespace ConsoleAppHospital
{
    // base class
    abstract internal class Employee
    {
        public string? EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        abstract public string GetDescription();
        abstract public string GetDescriptionFile();
    }
}
