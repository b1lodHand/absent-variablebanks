#define ABSENT_VARIABLEBANKS
#define ABSENT_VARIABLEBANKS_1_3_0

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor-firstpass")]

namespace com.absence.variablebanks.internals
{
    public static class Package
    {
        public class PackageVersion
        {
            public int Major;
            public int Minor;
            public int Patch;

            public string Text => $"{Major}.{Minor}.{Patch}";
        }

        public static readonly PackageVersion Version = new PackageVersion()
        {
            Major = 1,
            Minor = 3,
            Patch = 0,
        };
    }
}
