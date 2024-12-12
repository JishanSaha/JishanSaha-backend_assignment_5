namespace cumulative3.Models
{
    public class Teacher
    {
        /// <summary>
        /// the unique identifier for the teacher.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The first name of the teacher.
        /// </summary>
        public string? FName { get; set; }
        /// <summary>
        /// The last name of the teacher.
        /// </summary>
        public string? LName { get; set; }
        /// <summary>
        /// The Employee id of the teacher.
        /// </summary>
        public string? ENo { get; set; }
        /// <summary>
        /// The Joining date of the teacher.
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// The Salary of the teacher.
        /// </summary>
        public double Salary { get; set; }
    }
}
