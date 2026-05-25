namespace ConsoleAppHospital
{
    internal class Nurse : Employee, IPageable
    {
        public string? Level { get; set; }

        public override string GetDescription()
        {
            return $"Details for {EmployeeId}:\n{FirstName} {LastName}\nNurse ({Level})";
        }

        public override string GetDescriptionFile()
        {
            return $"nurse\n{EmployeeId}\n{FirstName}\n{LastName}\n{Level}";
        }

        public void IPage()
        {
            Console.WriteLine("Paging Nurse " + LastName);
        }
    }
}
