namespace NodewarAttendance.Attendance
{
    internal class MemberProfileReader
    {
        /// <summary>
        /// A reference to the user list.
        /// </summary>
        private Dictionary<Int32, MemberProfile> m_ProfileList;

        /// <summary>
        /// Switches the 'WaitingList' state to the 'Can' state when compiling the results.
        /// </summary>
        private Boolean m_SwitchWaitingToCan = true;

        /// <summary>
        /// Initializes a new MemberProfileReader.
        /// </summary>
        /// <param name="UserList">The user list associated with the attendance reader.</param>
        public MemberProfileReader(Dictionary<Int32, MemberProfile> ProfileList)
        {
            m_ProfileList = ProfileList;
        }

        /// <summary>
        /// Reads the member profile list.
        /// </summary>
        /// <param name="FileName">The file name of the member profile list.</param>
        public void ReadMemberProfile(String FileName)
        {
            // Check if the member file exists before opening it.
            if (!File.Exists(FileName))
            {
                Console.WriteLine("Error: The member file ({0}) could not be found.", FileName);
                Environment.Exit(0);
            }

            // Open the member file.
            using (StreamReader MemberFile = new StreamReader(FileName))
            {
                String? Line;
                String[] LineData;

                // Skip the first line of data (just identifiers).
                MemberFile.ReadLine();

                // Read the member file line-by-line.
                while ((Line = MemberFile.ReadLine()) != null)
                {
                    // Check if the line contains any data.
                    if (Line.Length == 0)
                        continue;

                    // Check if the line is a comment.
                    if (Line[0] == '/' && Line[1] == '/')
                        continue;

                    // Split the line into segments used by the CSV filetype.
                    LineData = Line.Split(',');

                    // Check if the member id exists before parsing the data.
                    if (LineData[0].Length == 0)
                        continue;

                    // Parse the line data.
                    MemberProfile_ParseLine(LineData);
                }
            }
        }

        /// <summary>
        /// Reads the attendance profile list.
        /// </summary>
        /// <param name="FileName">The file name of the attendance profile list.</param>
        public void ReadAttendanceProfile(String FileName)
        {
            // Check if the member file exists before opening it.
            if (!File.Exists(FileName))
            {
                Console.WriteLine("Error: The attendance file ({0}) could not be found.", FileName);
                Environment.Exit(0);
            }

            // Open the attendance file.
            using (StreamReader AttendanceFile = new StreamReader(FileName))
            {
                String? Line;
                String[] LineData;

                // Skip the first line of data (just identifiers).
                AttendanceFile.ReadLine();

                // Read the member file line-by-line.
                while ((Line = AttendanceFile.ReadLine()) != null)
                {
                    // Check if the line contains any data.
                    if (Line.Length == 0)
                        continue;

                    // Check if the line is a comment.
                    if (Line[0] == '/' && Line[1] == '/')
                        continue;

                    // Split the line into segments used by the CSV filetype.
                    LineData = Line.Split(',');

                    // Check if the member id exists before parsing the data.
                    if (LineData[0].Length == 0)
                        continue;

                    // Parse the line data.
                    AttendanceProfile_ParseLine(LineData);
                }
            }
        }

        /// <summary>
        /// Parses the user data on the current line for the member profile.
        /// </summary>
        /// 
        /// [0] = MemberId
        /// [1] = DiscordName
        /// [2] = FamilyName
        /// [3] = CharacterName
        ///
        /// Additional data is available but not used by this application.
        private void MemberProfile_ParseLine(String[] LineData)
        {
            // Get the member index.
            Int32 MemberIndex = Convert.ToInt32(LineData[0]);

            // Check if the member id already exists in the database.
            if (m_ProfileList.ContainsKey(MemberIndex))
            {
                // Update an existing entry.
                Console.WriteLine("Duplicate account found with the member index '{0}' in the member file.", MemberIndex);
                return;
            }

            // Initialize a new member profile.
            MemberProfile NewProfile = new MemberProfile();

            // Set the member index.
            NewProfile.MemberIndex = MemberIndex;

            // Set the discord handle.
            NewProfile.DiscordName = LineData[1];

            // Set the family name.
            NewProfile.FamilyName = LineData[2];

            // Set the character name.
            NewProfile.CharacterName = LineData[3];

            // Add the profile to the member profile list.
            m_ProfileList.Add(MemberIndex, NewProfile);
        }

        /// <summary>
        /// Parses the user data on the current line for the attendance profile. 
        /// </summary>
        /// 
        /// [0] = MemberId
        /// [1] = NodewarId
        /// [2] = FamilyName
        /// [3] = CharacterName
        /// [4] = Attendance (Can, Cant, Undecided, WaitingList, NoShow)
        ///
        /// Additional data is available but not used by this application.
        private void AttendanceProfile_ParseLine(String[] LineData)
        {
            // Get the member index.
            Int32 MemberIndex = Convert.ToInt32(LineData[0]);

            // Check if the member index exists in the database.
            if (!m_ProfileList.ContainsKey(MemberIndex))
            {
                Console.WriteLine("Removing character '{0}' with family name '{1}' from final results (no longer in guild).", LineData[3], LineData[2]);
                return;
            }

            // Determine the attendance state.
            switch (LineData[4])
            {
                // 'Can' attendance state: The player participated in the node watr.
                case "Can":
                    m_ProfileList[MemberIndex].Can += 1;
                    break;

                // 'Cant' attendance state: The player didn't participate in the node war.
                case "Cant":
                    m_ProfileList[MemberIndex].Cant += 1;
                    break;

                // 'Undecided' attendance state: The player didn't choose any choice for the node war.
                case "Undecided":
                    m_ProfileList[MemberIndex].Undecided += 1;
                    break;

                // 'WaitingList' attendance state: The player was moved by Canute to the waiting list for the node war.
                case "WaitingList":
                    // Members who sign up 15 minutes prior to node war or during the node war will automatically
                    // be moved to the 'WaitingList' state on Canute. By modifying this configuration settings, you
                    // can control how the attendance manager compiles this data.
                    if (m_SwitchWaitingToCan)
                        m_ProfileList[MemberIndex].Can += 1;
                    else
                        m_ProfileList[MemberIndex].Waiting += 1;
                    break;

                // 'NoShow' attendance state: The player was manually moved to this state for no showing in the node war.
                case "NoShow":
                    m_ProfileList[MemberIndex].NoShow += 1;
                    break;

                default:
                    Console.WriteLine("Unknown attendance state '{0}' for characer name '{1}'.", LineData[4], LineData[3]);
                    break;
            }
        }
    }
}
