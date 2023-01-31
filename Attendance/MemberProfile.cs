namespace NodewarAttendance.Attendance
{
    internal class MemberProfile
    {
        /// <summary>
        /// Represents the member's unique index.
        /// </summary>
        private Int32 m_MemberIndex;

        /// <summary>
        /// Represents the member's discord name.
        /// </summary>
        private String m_DiscordName;

        /// <summary>
        /// Represents the member's family name.
        /// </summary>
        private String m_FamilyName;

        /// <summary>
        /// Represents the member's character name.
        /// </summary>
        private String m_CharacterName;

        /// <summary>
        /// Represents the number of times the member hit 'Can' during the specified period.
        /// </summary>
        private Int32 m_Can;

        /// <summary>
        /// Represents the number of times the member hit 'Cant' during the specified period.
        /// </summary>
        private Int32 m_Cant;

        /// <summary>
        /// Represents the number of times the member remained 'Undecided' during the specified period.
        /// </summary>
        private Int32 m_Undecided;

        /// <summary>
        /// Represents the number of times the member remained 'WaitingList' during the specified period.
        /// </summary>
        private Int32 m_Waiting;

        /// <summary>
        /// Represents the number of times the member was flagged as a 'NoShow' during the specified period.
        /// </summary>
        private Int32 m_NoShow;

        /// <summary>
        /// Initializes a new member profile.
        /// </summary>
        public MemberProfile()
        {
            m_MemberIndex = 0;

            m_DiscordName = String.Empty;
            m_CharacterName = String.Empty;
            m_FamilyName = String.Empty;

            m_Can = 0;
            m_Cant = 0;
            m_Undecided = 0;
            m_Waiting = 0;
            m_NoShow = 0;
        }

        /// <summary>
        /// Gets/Sets the member index.
        /// </summary>
        public Int32 MemberIndex
        {
            get { return m_MemberIndex; }
            set { m_MemberIndex = value; }
        }

        /// <summary>
        /// Gets/Sets the discord name.
        /// </summary>
        public String DiscordName
        {
            get { return m_DiscordName; }
            set { m_DiscordName = value; }
        }

        /// <summary>
        /// Gets/Sets the character name.
        /// </summary>
        public String CharacterName
        {
            get { return m_CharacterName; }
            set { m_CharacterName = value; }
        }

        /// <summary>
        /// Gets/Sets the family name.
        /// </summary>
        public String FamilyName
        {
            get { return m_FamilyName; }
            set { m_FamilyName = value; }
        }

        /// <summary>
        /// Gets/Sets the 'can' counter.
        /// </summary>
        public Int32 Can
        {
            get { return m_Can; }
            set { m_Can = value; }
        }

        /// <summary>
        /// Gets/Sets the 'cant' counter.
        /// </summary>
        public Int32 Cant
        {
            get { return m_Cant; }
            set { m_Cant = value; }
        }

        /// <summary>
        /// Gets/Sets the 'undecided' counter.
        /// </summary>
        public Int32 Undecided
        {
            get { return m_Undecided; }
            set { m_Undecided = value; }
        }

        /// <summary>
        /// Gets/Sets the 'waiting' counter.
        /// </summary>
        public Int32 Waiting
        {
            get { return m_Waiting; }
            set { m_Waiting = value; }
        }

        /// <summary>
        /// Gets/Sets the 'noshow' counter.
        /// </summary>
        public Int32 NoShow
        {
            get { return m_NoShow; }
            set { m_NoShow = value; }
        }

        /// <summary>
        /// Gets the total participation counter.
        /// </summary>
        public Int32 Total
        {
            get
            {
                return m_Can + m_Cant + m_Undecided + m_Waiting + m_NoShow;
            }
        }

        /// <summary>
        /// Gets the total participation percentage counter.
        /// </summary>
        public Single TotalPercent
        {
            get
            {
                return (Total > 0 ? (Single)Can / (Single)Total : 0F);
            }
        }

    }
}
