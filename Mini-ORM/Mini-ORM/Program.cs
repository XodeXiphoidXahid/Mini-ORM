using System;
using System.Data.SqlClient;

namespace Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Server=DESKTOP-OTRJQNH\\SQLEXPRESS;Database=Practice;Trusted_Connection=True;";

            ORM<Employee> ormObj = new ORM<Employee>(connection);

            Console.Write("Insert press 1\nUpdate press 2\nDelete using Id press 3\nDelete using object press 4\nGetById press 5\nGetAll press 6\n");
            
            var input= Console.ReadLine();
            int choice = int.Parse(input);

            switch(choice)
            {
                case 1:
                    Console.WriteLine("Insert Name");
                    Console.WriteLine("Insert Salary");
                    Console.WriteLine("Insert Gender");
                    Console.WriteLine("Insert City");
                    
                    Employee empObj = new Employee();

                    empObj.Name = Console.ReadLine(); 
                    empObj.Salary = int.Parse(Console.ReadLine()); 
                    empObj.Gender = Console.ReadLine();
                    empObj.City = Console.ReadLine();

                    ormObj.Insert(empObj);
                    break;
                case 2:
                    Console.WriteLine("Insert Id for update");
                    Console.WriteLine("Insert Name");
                    Console.WriteLine("Insert Salary");
                    Console.WriteLine("Insert Gender");
                    Console.WriteLine("Insert City");
                 
                    Employee empObj1 = new Employee();

                    empObj1.Id = int.Parse(Console.ReadLine());
                    empObj1.Name = Console.ReadLine();
                    empObj1.Salary = int.Parse(Console.ReadLine());
                    empObj1.Gender = Console.ReadLine();
                    empObj1.City = Console.ReadLine();

                    ormObj.Update(empObj1);
                    break;
                case 3:
                    Console.WriteLine("Insert Id for Delete");

                    var deleteId = int.Parse(Console.ReadLine());

                    ormObj.Delete(deleteId);
                    break;
                case 4:
                    Console.WriteLine("Insert Id for Delete(Using Object)");

                    Employee empObj2 = new Employee();

                    empObj2.Id = int.Parse(Console.ReadLine());
                    ormObj.Delete(empObj2);
                    break;
                case 5:
                    Console.WriteLine("Insert Id for Get information");

                    var employee=ormObj.GetById(int.Parse(Console.ReadLine()));

                    Console.WriteLine(employee.Id + " " + employee.Name + " " + employee.Salary + " " + employee.City + " " + employee.Gender);
                    break;
                case 6:
                    var employeeList = ormObj.GetAll();

                    foreach(var member in employeeList)
                        Console.WriteLine(member.Id+" "+ member.Name+" "+ member.Salary+" "+ member.City+" "+ member.Gender);
                    break;
            }
        }
    }
}
