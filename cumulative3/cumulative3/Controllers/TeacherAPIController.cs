using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cumulative3.Models;
using System;
using MySql.Data.MySqlClient;
using cumulative3.Models;

namespace cumulative3.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        // dependency injection of database context
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>
        /// GET api/Teacher/ListTeachers -> [{"TeacherId":1,"TeacherFname":"Alexander", "TeacherLName":"Bennett","EmployeeNumber":"T378","HireDate":"2016-08-05 00:00:00","salary":"55.30" },{"TeacherId":2,"TeacherFname":"Caitlin", "TeacherLName":"Cummings","EmployeeNumber":"T381","HireDate":"2014-06-10 00:00:00","salary":"62.77" }..]
        /// </example>
        /// <returns>
        /// A list of Teacher objects 
        /// </returns>

        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(string SearchKey = null)
        {
            // Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from teachers";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["teacherlname"].ToString();
                        DateTime TeacherJoinDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        double Salary = Convert.ToDouble(ResultSet["salary"]);

                        //short form for setting all properties while creating the object
                        Teacher CurrentAuthor = new Teacher()
                        {
                            Id = Id,
                            FName = FirstName,
                            LName = LastName,
                            ENo = EmployeeNumber,
                            JoinDate = TeacherJoinDate,
                            Salary = Salary
                        };

                        Teachers.Add(CurrentAuthor);

                    }
                }
            }


            //Return the final list of Teachers
            return Teachers;
        }
        /// <summary>
        /// Returns an Teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Teacher/FindTeacher/1 -> {"TeacherId":1,"TeacherFname":"Alexander", "TeacherLName":"Bennett","EmployeeNumber":"T378","HireDate":"2016-08-05 00:00:00","salary":"55.30" }
        /// </example>
        /// <returns>
        /// A matching Teacher object by its ID. Empty object if teacher not found
        /// </returns>

        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {

            //Empty Teacher
            Teacher SelectedTeacher = new Teacher();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // @id is replaced with a 'sanitized' id
                Command.CommandText = "select * from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    if (ResultSet.HasRows)
                    {
                        //Loop Through Each Row the Result Set
                        while (ResultSet.Read())
                        {
                            //Access Column information by the DB column name as an index
                            int Id = Convert.ToInt32(ResultSet["teacherid"]);
                            string FirstName = ResultSet["teacherfname"].ToString();
                            string LastName = ResultSet["teacherlname"].ToString();
                            string EmployeeNumber = ResultSet["employeenumber"].ToString();
                            DateTime TeacherJoinDate = Convert.ToDateTime(ResultSet["hiredate"]);
                            double Salary = Convert.ToDouble(ResultSet["salary"]);

                            SelectedTeacher.Id = Id;
                            SelectedTeacher.FName = FirstName;
                            SelectedTeacher.LName = LastName;
                            SelectedTeacher.ENo = EmployeeNumber;
                            SelectedTeacher.JoinDate = TeacherJoinDate;
                            SelectedTeacher.Salary = Salary;

                        }

                    }

                }

                //Return the final list of Teacher names
                return SelectedTeacher;


            }
        }
        /// <summary>
        /// Adds an Teacher to the database
        /// </summary>
        /// <param name="TeacherData">Teacher Object</param>
        /// <example>
        /// POST: api/TeacherData/AddTeacher
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///	    "TeacherFname":"Christine",
        ///	    "TeacherLname":"Bittle",
        ///	    "EmployeeNumber":"T007",
        ///	    "Hiredate":"current_date"
        ///	    "Salary":"99.99"
        /// }
        /// </example>
        /// <returns>
        /// The inserted Teacher Id from the database if successful. 0 if Unsuccessful
        /// </returns>
        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // CURRENT_DATE() for the teacher join date in this context
                // Other contexts the join date may be an input criteria!
                Command.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherfname, @teacherlname, @employeenumber, CURRENT_DATE(), @salary)";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.FName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.LName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.ENo);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }


        /// <summary>
        /// Deletes an Teacher from the database
        /// </summary>
        /// <param name="TeacherId">Primary key of the Teacher to delete</param>
        /// <example>
        /// DELETE: api/TeacherData/DeleteTeacher -> 1
        /// </example>
        /// <returns>
        /// Number of rows affected by delete operation.
        /// </returns>
        [HttpDelete(template: "DeleteTeacher/{TeacherId}")]
        public int DeleteTeacher(int TeacherId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", TeacherId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }
        /// <summary>
        /// Updates an Teacher in the database. Data is Teacher object, request query contains ID
        /// </summary>
        /// <param name="TeacherData">Teacher Object</param>
        /// <param name="TeacherId">The Teacher ID primary key</param>
        /// <example>
        /// PUT: api/Teacher/UpdateTeacher/4
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///	    "TeacherFname":"Christine",
        ///	    "TeacherLname":"Bittle",
        ///	    "EmployeeNumber":"007",
        ///	    "HireDate":"2024-12-10"
        ///	    "Salary":"99.99"
        /// } -> 
        /// {
        ///     "TeacherId":20,
        ///	    "TeacherFname":"Christine",
        ///	    "TeacherLname":"Bittle",
        ///	    "EmployeeNumber":"007",
        ///	    "HireDate":"2024-12-10"
        ///	    "Salary":Teacher"
        /// }
        /// </example>
        /// <returns>
        /// The updated Teacher object
        /// </returns>
        [HttpPut(template: "UpdateTeacher/{TeacherId}")]
        public Teacher UpdateTeacher(int TeacherId, [FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
                Command.CommandText = "update teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, salary=@salary where teacherid=@id";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.FName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.LName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.ENo);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);
                

                Command.Parameters.AddWithValue("@id", TeacherId);

                Command.ExecuteNonQuery();



            }

            return FindTeacher(TeacherId);
        }
    }
}
