using System.Text;

namespace lesson_13_pattern_builder
{
    class Program
    {
        static void Main()
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee() {Name = "Артем", Salary = 100},
                new Employee() {Name = "Борис", Salary = 50},
                new Employee() {Name = "Федор", Salary = 200}
            };

            EmployeeReportBuilder builder = new EmployeeReportBuilder(employees);
            EmployeeReportDirector director = new EmployeeReportDirector(builder);

            director.Build();

            var report = builder.GetReport();

            Console.WriteLine(report);

            Console.ReadLine();
        }
    }

    class EmployeeReport
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }

        public override string ToString() =>
            new StringBuilder()
            .Append(Header)
            .Append(Body)
            .Append(Footer)
            .ToString();
    }

    class Employee
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }
    }

    interface IEmployeeReportBuilder
    {
        void BuildHeader();
        void BuildBody();
        void BuildFooter();

        EmployeeReport GetReport();
    }

    class EmployeeReportBuilder : IEmployeeReportBuilder
    {
        private EmployeeReport _employeeReport;

        private readonly IEnumerable<Employee> _employees;

        public EmployeeReportBuilder(IEnumerable<Employee> employees)
        {
            _employees = employees;
            _employeeReport = new EmployeeReport();
        }

        public void BuildHeader()
        {
            _employeeReport.Header =
                $"Отчет по сотрудникам на дату: {DateTime.Now}\n";

            _employeeReport.Header +=
                $"\n{new string('-', 100)}\n\n";
        }

        public void BuildBody()
        {
            _employeeReport.Body =
                string.Join(Environment.NewLine,
                       _employees.Select(e =>
                       $"Сотрудник: {e.Name}\t\tЗарплата: {e.Salary}"));
        }

        public void BuildFooter()
        {
            _employeeReport.Footer =
                $"\n{new string('-', 100)}\n\n";

            _employeeReport.Footer +=
                string.Join(Environment.NewLine,
                $"Всего сотрудников: {_employees.Count()}, " +
                $"Суммарная зарплата: {_employees.Sum(e => e.Salary)}");
        }

        public EmployeeReport GetReport()
        {
            EmployeeReport employeeReport = _employeeReport;

            _employeeReport = new();

            return employeeReport;
        }
    }

    class EmployeeReportDirector
    {
        private readonly IEmployeeReportBuilder _builder;

        public EmployeeReportDirector(IEmployeeReportBuilder builder)
        {
            _builder = builder;
        }

        public void Build()
        {
            _builder.BuildHeader();

            _builder.BuildBody();

            _builder.BuildFooter();
        }
    }
}


/// <summary>
/// Builder - пораждающий паттерн проектирования
/// Делает создание новых объектов максимально дуобно
/// Задача:
/// - пошаговое создание сложного объекта
/// 
/// Таким образом мы выносим логику создание объекта за пределы собственного класса
/// поручив её специальному объекту строителю
/// 
/// Анатомия паттерна:
/// Ключевым является класс продукта (сложный объект)
/// Далее нам нужен интрерфейс (IBuilder) которым будет пользоваться строитель
/// В котором будет определены шаги создания нашего объекта
/// Кроме того нам понадобится метод с помощью которого мы получим результат конструирования
/// 
/// Кроме того нам нужен будет строитель (Builder) - кто будет способен эти шаги выполнять
/// Строителей может быть +100500
/// 
/// Ключевым является продукт который нужно создать
/// 
/// Далее нужен класс Director
/// Метод в котором будет строиться объект
/// 
/// И пользуется всем этим клиентский код (например Main)
/// </summary>