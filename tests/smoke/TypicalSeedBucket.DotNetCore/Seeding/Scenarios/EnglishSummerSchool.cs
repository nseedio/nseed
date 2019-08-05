using NSeed;
using Seeds.Seeding.People;

namespace Seeds.Seeding.Scenarios
{
    [Requires(typeof(EnglishTeachers))]
    [Requires(typeof(SummerSchoolStudents))]
    [Requires(typeof(EnglishAdvancedCourse))]
    [Requires(typeof(EnglishBeginnersCourse))]
    [Description("English two-month summer school for beginners and advanced.")]
    internal class EnglishSummerSchool : IScenario { }
}
