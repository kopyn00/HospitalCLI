namespace ConsoleAppHospital
{
    internal class Doctor : Employee, IPageable
    {
        public string? Specialty { get; set; }

        public override string GetDescription()
        {
            return $"Details for {EmployeeId}:\n{FirstName} {LastName}\nDoctor ({Specialty})";
        }

        public override string GetDescriptionFile()
        {
            return $"doctor\n{EmployeeId}\n{FirstName}\n{LastName}\n{Specialty}";
        }

        public void IPage()
        {
            Console.WriteLine("Paging Doctor " + LastName);
        }
    }
}
