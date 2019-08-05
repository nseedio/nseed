using NSeed;
using Seeds.Model;
using System;
using System.Threading.Tasks;

namespace Seeds.Seeding.People
{
    [Requires(typeof(RegularStudents))]
    internal class EnglishAdvancedCourse : ISeed<Course>
    {
        internal EnglishTeachers.Yield EnglishTeachers { get; private set; }

        public Task<bool> HasAlreadyYielded()
        {
            throw new NotImplementedException();
        }

        public Task Seed()
        {
            throw new NotImplementedException();
        }
    }
}
