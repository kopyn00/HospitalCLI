namespace ConsoleAppHospital
{
    internal class Custodian : Employee
    {
        public override string GetDescription()
        {
            return $"Details for {EmployeeId}:\n{FirstName} {LastName}\nCustodian)";
        }

        public override string GetDescriptionFile()
        {
            return $"custodian\n{EmployeeId}\n{FirstName}\n{LastName}";
        }
    }
}
