namespace PlatformFacade.Editor
{
    /// <summary>
    /// Editor implementation of ILeaderboardEntry representing a single entry in a leaderboard
    /// </summary>
    public class EditorLeaderboardEntry : ILeaderboardEntry
    {
        private int _rank;

        /// <summary>
        /// The user associated with this leaderboard entry
        /// </summary>
        public IUser User { get; }

        /// <summary>
        /// The rank/position of this entry in the leaderboard (1-based)
        /// </summary>
        public int Rank => _rank;

        /// <summary>
        /// The score value for this entry
        /// </summary>
        public long Score { get; }

        /// <summary>
        /// Initializes a new instance of the EditorLeaderboardEntry class
        /// </summary>
        /// <param name="user">The user associated with this entry</param>
        /// <param name="rank">The rank/position of this entry (1-based)</param>
        /// <param name="score">The score value for this entry</param>
        public EditorLeaderboardEntry(IUser user, int rank, long score)
        {
            User = user ?? throw new System.ArgumentNullException(nameof(user));
            _rank = rank;
            Score = score;
        }

        /// <summary>
        /// Updates the rank of this entry (used internally by EditorLeaderboard)
        /// </summary>
        /// <param name="newRank">The new rank value</param>
        internal void UpdateRank(int newRank)
        {
            _rank = newRank;
        }

        /// <summary>
        /// Returns a string representation of this leaderboard entry
        /// </summary>
        /// <returns>String representation of the entry</returns>
        public override string ToString()
        {
            return $"Rank {_rank}: {User.Name} - {Score} points";
        }
    }
}