using System.Globalization;

namespace NodewarAttendance.Attendance
{
    internal enum AttendanceSortingMethod
    {
        /// <summary>
        /// Sort by total percent.
        /// </summary>
        TotalPercent = 0,

        /// <summary>
        /// Sort by total percent, then by can participation.
        /// </summary>
        TotalPercentThenCan,

        /// <summary>
        /// Sort by total percent, then by can't participation.
        /// </summary>
        TotalPercentThenCant,

        /// <summary>
        /// Sort by character name in ascending order.
        /// </summary>
        CharacterNameAscending,

        /// <summary>
        /// Sort by character name in descending order.
        /// </summary>
        CharacterNameDescending,

        /// <summary>
        /// Sort by family name in ascending order.
        /// </summary>
        FamilyNameAscending,

        /// <summary>
        /// Sort by family name in descending order.
        /// </summary>
        FamilyNameDescending,

        /// <summary>
        /// Sort by undecided.
        /// </summary>
        Undecided,

        /// <summary>
        /// Sort by member index.
        /// </summary>
        MemberIndex
    }

    internal class MemberProfileWriter
    {
        /// <summary>
        /// A reference to the user list.
        /// </summary>
        private Dictionary<Int32, MemberProfile> m_ProfileList;

        /// <summary>
        /// The sorting method used by the profile writer.
        /// </summary>
        private AttendanceSortingMethod m_SortMethod;

        /// <summary>
        /// Initializes a new MemberProfileWriter.
        /// </summary>
        /// <param name="UserList">The user list associated with the attendance writer.</param>
        /// <param name="SortMethod">The sorting method used to write the attendance.</param>
        public MemberProfileWriter(Dictionary<Int32, MemberProfile> UserList, AttendanceSortingMethod SortMethod)
        {
            m_ProfileList = UserList;
            m_SortMethod = SortMethod;
        }

        /// <summary>
        /// Writes the attendance data in a readable format.
        /// </summary>
        /// <param name="FileName">The file name of the attendance log.</param>
        public void WriteAttendance(String FileName)
        {
            // Open the output file.
            using (StreamWriter Writer = new StreamWriter(FileName))
            {
                String FormatString;

                // Begin writing the attendance sheet.
                Writer.WriteLine("Nodewar/Siege Attendance Sheet");
                Writer.WriteLine();

                // Write the header.
                Writer.WriteLine("CAN\tCANT\tWAITING\tUNDECIDED\tTOTAL\t\tCANUTE\t  FAMILY NAME (CHARACTER NAME)");

                // v1.3 WIP
                m_SortMethod = AttendanceSortingMethod.TotalPercentThenCan;
                switch (m_SortMethod)
                {
                    case AttendanceSortingMethod.TotalPercent:
                        m_ProfileList = m_ProfileList.OrderBy(Key => Key.Value.TotalPercent).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.TotalPercentThenCan:
                        m_ProfileList = m_ProfileList.OrderBy(Key => Key.Value.TotalPercent).ThenBy(Key => Key.Value.Can).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.TotalPercentThenCant:
                        m_ProfileList = m_ProfileList.OrderBy(Key => Key.Value.TotalPercent).ThenBy(Key => Key.Value.Cant).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.CharacterNameAscending:
                        m_ProfileList = m_ProfileList.OrderBy(Key => Key.Value.CharacterName).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.CharacterNameDescending:
                        m_ProfileList = m_ProfileList.OrderByDescending(Key => Key.Value.CharacterName).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.FamilyNameAscending:
                        m_ProfileList = m_ProfileList.OrderBy(Key => Key.Value.FamilyName).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.FamilyNameDescending:
                        m_ProfileList = m_ProfileList.OrderByDescending(Key => Key.Value.FamilyName).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.Undecided:
                        m_ProfileList = m_ProfileList.OrderBy(Key => Key.Value.Undecided).ToDictionary(X => X.Key, X => X.Value);
                        break;
                    case AttendanceSortingMethod.MemberIndex:
                        m_ProfileList = m_ProfileList.OrderBy(Key => Key.Value.MemberIndex).ToDictionary(X => X.Key, X => X.Value);
                        break;
                }

                // Loop through each member in the member profile list and sort the participation by total percentage, then by number of can participation.
                foreach (var User in m_ProfileList)
                {
                    // Format the attendance string.
                    FormatString = String.Format("{0}\t{1}\t{2}\t{3}\t\t{4}\t\t#{5}\t  {6} ({7})",
                                    User.Value.Can,
                                    User.Value.Cant,
                                    User.Value.Waiting,
                                    User.Value.Undecided,
                                    User.Value.TotalPercent.ToString("P1", CultureInfo.InvariantCulture),
                                    User.Value.MemberIndex,
                                    User.Value.FamilyName,
                                    User.Value.CharacterName
                    );

                    // Write the member's attendance.
                    Writer.WriteLine(FormatString);
                }

                // Write the total number of parsed accounts.
                Writer.WriteLine();
                Writer.WriteLine("Successfully parsed '{0}' node war accounts.", m_ProfileList.Count);
            }

            // Output the total number of parsed accounts.
            Console.WriteLine();
            Console.WriteLine("Successfully parsed '{0}' node war accounts.", m_ProfileList.Count);
        }
    }
}