using NSeed;
using Seeds.Model;
using System;
using System.Threading.Tasks;

namespace Seeds.Seeding.People
{
    internal class SummerSchoolStudents : ISeed<Person>
    {
        public Task<bool> HasAlreadyYielded()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OutputAlreadyExists()
        {
            throw new NotImplementedException();
        }

        public Task Seed()
        {
            throw new NotImplementedException();
        }

        public Task WeedOut()
        {
            throw new NotImplementedException();
        }
    }
}
