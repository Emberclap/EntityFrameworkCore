// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Text;
    using Invoices.Utilities;
    using TeisterMask.DataProcessor.ImportDto;
    using System.Globalization;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var projectsDtos = XmlSerializationHelper
                .Deserialize<ImportProjectsDto[]>(xmlString, "Projects");
            var projectsToImport = new List<Project>();

            foreach (var p in projectsDtos )
            {
                bool isOpenDateValid = DateTime
                    .TryParseExact(p.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime openDate);
                bool isDueDateValid = DateTime
                    .TryParseExact(p.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

                if (!IsValid(p) || !isOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var tasksToAdd = new List<Task>();

                foreach (var t in p.Tasks)
                {
                    bool isTaskOpenDateValid = DateTime
                       .TryParseExact(t.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDate);
                    bool isTaskDueDateValid = DateTime
                        .TryParseExact(t.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDate);

                    if (!IsValid(t) || !isTaskOpenDateValid || !isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (taskOpenDate < openDate || taskDueDate > dueDate && isDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newTask = new Task()
                    {
                        Name = t.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)t.ExecutionType,
                        LabelType = (LabelType)t.LabelType,
                    };
                    tasksToAdd.Add(newTask);
                }
                var newProject = new Project()
                {
                    Name = p.Name,
                    OpenDate = openDate,
                    DueDate = dueDate,
                    Tasks = tasksToAdd,
                };
                projectsToImport.Add(newProject);
                sb.AppendLine(string.Format(SuccessfullyImportedProject , newProject.Name, tasksToAdd.Count));
            }
            context.Projects.AddRange(projectsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var employeesDtos = JsonConvert
                .DeserializeObject<ImportEmployeeDto[]>(jsonString);

            var employeesToImport = new List<Employee>();

            foreach (var e in employeesDtos)
            {
                if (!IsValid(e))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var newEmployee = new Employee()
                { 
                    Username = e.Username, 
                    Email = e.Email,
                    Phone = e.Phone,
                };

                foreach(var t in e.Tasks.Distinct())
                {
                    if (!context.Tasks.Any(task => task.Id == t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var newTask = new EmployeeTask()
                    {
                        Employee = newEmployee,
                        TaskId = t
                    };
                    newEmployee.EmployeesTasks.Add(newTask);
                }
                employeesToImport.Add(newEmployee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, newEmployee.Username , newEmployee.EmployeesTasks.Count));
            }
            context.Employees.AddRange(employeesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}