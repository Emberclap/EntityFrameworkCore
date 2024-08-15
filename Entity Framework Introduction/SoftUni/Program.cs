using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new SoftUniContext();
            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            return string.Join(Environment.NewLine, context.Employees
                .Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}")
                .ToList());
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            return string.Join(Environment.NewLine, context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => $"{e.FirstName} - {e.Salary:f2}")
                .ToList());
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            return string.Join(Environment.NewLine, context.Employees
                .Where(d => d.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName)
                .Select(e => $"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}")
                .ToList());
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees.
                FirstOrDefault(e => e.LastName == "Nakov");

            if (employee != null)
            {
                employee.Address = newAddress;
                context.SaveChanges();
            }

            return string.Join(Environment.NewLine, context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => $"{e.Address.AddressText}")
                .Take(10)
                .ToList());
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Employees
               .Take(10)
               .Select(e => new
               {
                   EmployeeNames = $"{e.FirstName} {e.LastName}",
                   ManagerNames = $"{e.Manager.FirstName} {e.Manager.LastName}",
                   Projects = e.EmployeesProjects
                    .Where(ep =>
                        ep.Project.StartDate.Year >= 2001 &&
                        ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ep.Project.StartDate,
                        EndDate = ep.Project.EndDate.HasValue ?
                            ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                            : "not finished"
                    })

               }).ToList();
            foreach (var e in result)
            {
                sb.AppendLine($"{e.EmployeeNames} - Manager: {e.ManagerNames}");
                if (e.Projects.Any())
                {
                    foreach (var project in e.Projects)
                    {
                        sb.AppendLine($"--{project.ProjectName} - " +
                            $"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {project.EndDate}");


                    }
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context) 
        {

            return string.Join(Environment.NewLine, context.Addresses
                .OrderByDescending(e => e.Employees.Count())
                .ThenBy(t => t.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(e => $"{e.AddressText}, {e.Town.Name} - {e.Employees.Count()} employees")
                .Take(10)
                .ToList());
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employees = context
               .Employees
               .Where(x => x.EmployeeId == 147)
               .Select(e => new
               {
                   firstName = e.FirstName,
                   lastName = e.LastName,
                   jobTitle = e.JobTitle,
                   projecs = e.EmployeesProjects
                   .Select(p => p.Project.Name)
               })
               .ToList();

            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.firstName} {emp.lastName} - {emp.jobTitle}");

                foreach (var p in emp.projecs.OrderBy(pr => pr))
                {
                    sb.AppendLine($"{p}");
                }
            }

            return sb.ToString().Trim();
        }


        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context
                .Departments
                .Where(d => d.Employees.Count() > 5)
                .OrderBy(x => x.Employees.Count())
                .ThenBy(dn => dn.Name)
                .Select(dm => new
                {
                    departmentName = dm.Name,
                    departmentManagerFirstName = dm.Manager.FirstName,
                    departmentManagerLastName = dm.Manager.LastName,
                    employees = dm.Employees
                        .Select(e => new
                        {
                            empFirstname = e.FirstName,
                            empLastName = e.LastName,
                            empJobTitle = e.JobTitle
                        })
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.departmentName} - {dep.departmentManagerFirstName} {dep.departmentManagerLastName}");

                foreach (var e in dep.employees.OrderBy(em => em.empFirstname).ThenBy(em => em.empLastName))
                {
                    sb.AppendLine($"{e.empFirstname} {e.empLastName} - {e.empJobTitle}");
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context
                .Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    projectName = x.Name,
                    projectDescription = x.Description,
                    startDate = x.StartDate
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var p in projects)
            {
                sb
                    .AppendLine($"{p.projectName}")
                    .AppendLine($"{p.projectDescription}")
                    .AppendLine($"{p.startDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString().Trim();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var desiredDepartments = new[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employees = context
                .Employees
                .Where(e => desiredDepartments.Contains(e.Department.Name));

            foreach (var e in employees)
            {
                e.Salary *= 1.12M;
            }

            context.SaveChanges();

            var employeesWithIncreasedSalary = employees
                .Select(e => new
                {
                    firstName = e.FirstName,
                    lastName = e.LastName,
                    salary = e.Salary
                })
                .OrderBy(e => e.firstName)
                .ThenBy(e => e.lastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var em in employeesWithIncreasedSalary)
            {
                sb.AppendLine($"{em.firstName} {em.lastName} (${em.salary:F2})");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
   
            return string.Join(Environment.NewLine, context.Employees
               .Where(employee => EF.Functions.Like(employee.FirstName, "sa%"))
               .OrderBy(x => x.FirstName)
               .ThenBy(x => x.LastName)
               .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})")
               .ToList());
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var idToDelete = 2;
            //First step - select and delete from the many-to-many table
            var projectToDeleteFromTable_EP = context
                .EmployeesProjects
                .Where(p => p.ProjectId == idToDelete)
                .ToList();

            context.EmployeesProjects.RemoveRange(projectToDeleteFromTable_EP);

            //Second step - delete from the Projects table
            var projectToDeleteFromTable_Projects = context
                .Projects
                .Where(x => x.ProjectId == idToDelete)
                .ToList();

            context.Projects.RemoveRange(projectToDeleteFromTable_Projects);
            context.SaveChanges();
          
            return string.Join(Environment.NewLine, context.Projects
               .Take(10)
               .Select(x => x.Name)
               .ToList());
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townName = "Seattle";

            var townToDelete = context
                .Towns
                .Where(t => t.Name == townName)
                .FirstOrDefault();

            var addresses = context
                .Addresses
                .Where(a => a.TownId == townToDelete.TownId)
                .ToList();

            foreach (var adr in addresses)
            {
                var employees = context
                    .Employees
                    .Where(e => e.AddressId == adr.AddressId)
                    .ToList();

                foreach (var emp in employees)
                {
                    emp.AddressId = null;
                }

                context.Addresses.Remove(adr);
            }

            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            return $"{addresses.Count()} addresses in {townName} were deleted";
        }
    }


}

