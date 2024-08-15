namespace TeisterMask.DataProcessor
{
    using Data;
    using Invoices.Utilities;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context.Projects
                .Where(p => p.Tasks.Any())
                .Select(p => new ExportProjectDto
                {
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    TasksCount = p.Tasks.Count(),
                    Tasks = p.Tasks
                    .Select(t => new ExportTaskDto 
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString(),
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .OrderByDescending(t => t.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToList();

            return XmlSerializationHelper.Serialize(projects, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employee = context.Employees
                .Where(t => t.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(t => t.Task.OpenDate >= date)
                    .OrderByDescending(t => t.Task.DueDate)
                    .ThenBy(t => t.Task.Name)
                    .Select(t => new
                    {
                        TaskName = t.Task.Name,
                        OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = t.Task.LabelType.ToString(),
                        ExecutionType = t.Task.ExecutionType.ToString(),
                    })
                    .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Count())
                .ThenBy(e => e.Username)
                .Take(10)
                .ToList();

            return JsonConvert.SerializeObject(employee ,Formatting.Indented);
        }
    }
}