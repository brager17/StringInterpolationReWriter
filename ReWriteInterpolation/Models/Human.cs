namespace ReWriteInterpolation.Models
{
    public class Human
    {
        public Human(string name, int age)
        {
            Name = name;
            Age = age;
            SurName = "unknown";
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string SurName { get; private set; }
        public int Age { get; private set; }
    }
}