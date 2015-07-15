namespace Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MSSQL;
    using MySQL;
    using SQLite;

    /// <summary>
    /// Currently using 3 databases:
    /// *MySQL 'job_portal' database from last db exam;
    /// *SQLite 'mydatabase' located in the solution;
    /// *MSSQL 'Softuni' database used in db course;
    /// 
    /// All needed packages and app.confing modifications are in .txt files
    /// in the solution.
    /// 
    /// For MySQL settings - http://qursaan-howto.blogspot.com/2014/07/solve-your-project-references-latest.html
    /// For SQLite settigs - http://erikej.blogspot.com/2014/11/using-sqlite-with-entity-framework-6.html and the
    /// txt in it.
    /// </summary>
    public class ConsoleClient
    {
        public static void Main(string[] args)
        {
            var mysqlContex = new job_portalEntities();
            var sqliteContex = new mydatabaseEntities();
            var mssqlContex = new SoftUniEntities();

            var mysqlUsers = mysqlContex.users.ToList();
            var sqliteUsers = sqliteContex.Users.ToList();
            var mssqlUsers = mssqlContex.Employees.Take(10).ToList();

            var users = new List<string>();

            mysqlUsers.ForEach(u => users.Add(u.fullname));
            sqliteUsers.ForEach(u => users.Add(u.Name));
            mssqlUsers.ForEach(u => users.Add(u.FirstName));

            users.Sort();
            users.ForEach(Console.WriteLine);
        }
    }
}
