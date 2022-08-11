using System.Data;
using System.Data.SqlClient;
using SQLServer_ADONET.Models;

namespace SQLServer_ADONET.Services
{
    public class ApplicationService : IApplicationService
    {
        private string connectionString { get; set; }
        private IConfiguration config;
        private SqlConnection sqlConnection { get; set; }

        public ApplicationService(IConfiguration c)
        {
            this.config = c;
            connectionString = config.GetConnectionString("cnnstr");
            sqlConnection = new SqlConnection(connectionString);
        }

        public List<StudentModel> getAllStudent()
        {
            List<StudentModel> list = new List<StudentModel>();
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); //open sqlConnection inside 'using' doesn't need to .Close() afterwards
                var command = new SqlCommand("GETALLSTUDENT", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        StudentModel student = new StudentModel();
                        student.ID = Convert.ToInt32(reader["StudentID"]);
                        student.Firstname = reader["Firstname"].ToString();
                        student.Lastname = reader["Lastname"].ToString();
                        student.ClassID = Convert.ToInt32((object)reader["ClassID"] != (object)DBNull.Value ? reader["ClassID"] : -1);
                        student.Classname = reader["Classname"].ToString();
                        student.Birthdate = Convert.ToDateTime(reader["Birthdate"]);
                        list.Add(student);
                    }
                    reader.Close();
                }
                return list;
            }
        }

        public StudentModel? getStudentByID(int id)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var command = new SqlCommand("GETSTUDENTBYID", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@input", id));
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    StudentModel student = new StudentModel();
                    student.ID = Convert.ToInt32(reader["StudentID"]);
                    student.Firstname = reader["Firstname"].ToString();
                    student.Lastname = reader["Lastname"].ToString();
                    student.ClassID = Convert.ToInt32((object)reader["ClassID"] != (object)DBNull.Value ? reader["ClassID"] : -1);
                    student.Classname = reader["Classname"].ToString();
                    student.Birthdate = Convert.ToDateTime(reader["Birthdate"]);
                    reader.Close();
                    return student;
                }
                return null;
            }
        }

        public List<StudentModel> getStudentByClass(int classID)
        {
            List<StudentModel> list = new List<StudentModel>();
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var command = new SqlCommand("GETSTUDENTBYCLASS", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (classID != -1) command.Parameters.Add(new SqlParameter("@input", classID));
                else command.Parameters.Add(new SqlParameter("@input", DBNull.Value));
                SqlDataReader reader = command.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        StudentModel student = new StudentModel();
                        student.ID = Convert.ToInt32(reader["StudentID"]);
                        student.Firstname = reader["Firstname"].ToString();
                        student.Lastname = reader["Lastname"].ToString();
                        student.Birthdate = Convert.ToDateTime(reader["Birthdate"]);
                        list.Add(student);
                    }
                    reader.Close();
                }
                return list;
            }
        }

        public bool addStudent(StudentModel student)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("ADDSTUDENT", sqlConnection, transaction);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@FN", student.Firstname == null ? DBNull.Value : student.Firstname));
                    command.Parameters.Add(new SqlParameter("@LN", student.Lastname));
                    command.Parameters.Add(new SqlParameter("@C", student.ClassID == null ? DBNull.Value : student.ClassID));
                    command.Parameters.Add(new SqlParameter("@BD", DateTime.MinValue));
                    int query = command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction commited");
                    return (query > 0);
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rollback");
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public bool updateStudent(StudentModel student)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("UPDATESTUDENT", sqlConnection, transaction);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@input", student.ID));
                    command.Parameters.Add(new SqlParameter("@FN", student.Firstname == null ? DBNull.Value : student.Firstname));
                    command.Parameters.Add(new SqlParameter("@LN", student.Lastname));
                    command.Parameters.Add(new SqlParameter("@C", student.ClassID == null ? DBNull.Value : student.ClassID));
                    int query = command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction commited");
                    return (query > 0);
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rollback");
                    Console.Error.WriteLine(e.Message);
                    return false;
                }

            }
        }

        public bool changeStudentClass(int id, int classid)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("UPDATESTUDENTCLASS", sqlConnection, transaction);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@id", id));
                    if (classid != -1)
                        command.Parameters.Add(new SqlParameter("@classid", classid));
                    else
                        command.Parameters.Add(new SqlParameter("@classid", DBNull.Value));
                    int query = command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction commited");
                    return (query > 0);
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rollback");
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public bool deleteStudent(int id)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("DELETESTUDENTBYID", sqlConnection, transaction);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@input", id));
                    int query = command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction commited");
                    return (query > 0);
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rollback");
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public List<ClassModel> getAllClass()
        {
            List<ClassModel> list = new List<ClassModel>();
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); //open sqlConnection inside 'using' doesn't need to .Close() afterward
                try
                {
                    var command = new SqlCommand("select * from Class", sqlConnection);
                    command.CommandType = System.Data.CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ClassModel classModel = new ClassModel();
                            classModel.ID = Convert.ToInt32(reader["ID"]);
                            classModel.Classname = reader["Classname"].ToString();
                            classModel.NumStudent = Convert.ToInt32(reader["NumStudent"]);
                            list.Add(classModel);
                        }
                        reader.Close();
                    }
                    return list;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    return list;
                }
            }
        }

        public ClassModel? getClassByID(int id)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open(); //open sqlConnection inside 'using' doesn't need to .Close() afterward
                try
                {
                    var command = new SqlCommand("GETCLASSBYID", sqlConnection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@input", id));
                    SqlDataReader reader = command.ExecuteReader();
                    ClassModel temp = new ClassModel();
                    if (reader.Read())
                    {
                        temp.ID = Convert.ToInt32(reader["ID"]);
                        temp.Classname = reader["Classname"].ToString();
                        temp.NumStudent = Convert.ToInt32(reader["NumStudent"]);
                        return temp;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public bool addClass(string classname)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("ADDCLASS", sqlConnection, transaction);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@input", classname));
                    int query = command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction commited");
                    return (query > 0);
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rollback");
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public bool updateClass(int classid, string name)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("UPDATECLASS", sqlConnection, transaction);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@input", classid));
                    command.Parameters.Add(new SqlParameter("@name", name));
                    int query = command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction commited");
                    return (query > 0);
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rollback");
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public bool deleteClass(int id)
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("DELETECLASSBYID", sqlConnection, transaction);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@input", id));
                    int query = command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Transaction commited");
                    return (query > 0);
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rollback");
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
        }
    }

    public interface IApplicationService
    {
        public List<StudentModel> getAllStudent();
        public StudentModel? getStudentByID(int id);
        public List<StudentModel> getStudentByClass(int classID);
        public bool addStudent(StudentModel student);
        public bool updateStudent(StudentModel student);
        public bool changeStudentClass(int id, int classid);
        public bool deleteStudent(int id);
        public List<ClassModel> getAllClass();
        public ClassModel? getClassByID(int id);
        public bool addClass(string classname);
        public bool updateClass(int classid, string name);
        public bool deleteClass(int id);
    }
}
