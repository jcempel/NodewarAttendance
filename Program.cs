using NodewarAttendance.Attendance;

namespace NodewarAttendance
{
    internal class Program
    {
        /// <summary>
        /// m_Version: The current version of the application.
        /// </summary>
        static Version m_Version = new Version(1, 2, 0);

        /// <summary>
        /// m_ProfileList: Represents a list of each user's attendance over all attendance files.
        /// </summary>
        static Dictionary<Int32, MemberProfile> m_ProfileList = new Dictionary<Int32, MemberProfile>();

        /// <summary>
        /// Initializes the Nodewar Attendance Manager.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Nodewar Attendance Manager (v{0})", m_Version);
            Console.WriteLine();

            // Get the directory from the user.
            Console.Write("Choose Directory: ");
            String? DirectoryName = Console.ReadLine();
            Console.WriteLine();

            // Check if the directory exists.
            if (!Directory.Exists(DirectoryName))
            {
                Console.WriteLine("Error: The directory was not found.");
                return;
            }

            // Get the files in the directory.
            String[] DirectoryFiles = GetFilesInDirectory(DirectoryName);

            // Check if any files exist in the directory.
            if (DirectoryFiles.Length == 0)
            {
                Console.WriteLine("Error: The directory does not contain any files.");
                return;
            }

            // Initialize the attendance reader.
            MemberProfileReader ProfileReader = new MemberProfileReader(m_ProfileList);

            // Read the member file.
            ProfileReader.ReadMemberProfile("Members.csv");

            // Read each attendance file in the directory.
            foreach (String DirectoryFile in DirectoryFiles)
            {
                ProfileReader.ReadAttendanceProfile(DirectoryFile);
            }

            // Initialize the attendance writer.
            MemberProfileWriter ProfileWriter = new MemberProfileWriter(m_ProfileList, AttendanceSortingMethod.TotalPercentThenCan);

            // Write the attendance data.
            ProfileWriter.WriteAttendance("GuildAttendance.txt");
        }

        /// <summary>
        /// Gets a list of files in the specified directory.
        /// </summary>
        /// <param name="DirectoryName">The directory to get the files from.</param>
        static String[] GetFilesInDirectory(String DirectoryName)
        {
            Int32 FileCounter = 1;

            // Gets a list of files in the specified directory.
            String[] DirectoryFileNames = Directory.GetFiles(DirectoryName);

            // Check if any files are in the folder.
            if (DirectoryFileNames.Length > 0)
            {
                // List each file in the directory.
                Console.WriteLine("Directory Files:");
                foreach (String DirectoryFileName in DirectoryFileNames)
                {
                    Console.WriteLine("{0}.) {1}", FileCounter, DirectoryFileName);
                    FileCounter++;
                }
                Console.WriteLine();
            }

            return DirectoryFileNames;
        }
    }
}