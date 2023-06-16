namespace GradeBook.EntityData
{
    public class Student
    {
        public int Id { get; set; }
        public int Roll_No { get; set; }
        public string Name { get; set; }
        public int CSC101 { get; set; }
        public int CSC102 { get; set; }
        public int CSC103 { get; set; }
        public int CSC104 { get; set; }
        public float Total { get; set; }
        public string Grade { get; set; }
        public void CalculateTotal()
        {
            Total = (CSC101 + CSC102 + CSC103 + CSC104) / 4.0f;

        }

        public void CalculateGrade()
        {
            if (Total >= 90)
                Grade = "A+";
            else if (Total >= 80)
                Grade = "A";
            else if (Total >= 70)
                Grade = "B";
            else if (Total >= 60)
                Grade = "C";
            else if (Total >= 50)
                Grade = "D";
            else
                Grade = "F";
        }
    }
}
