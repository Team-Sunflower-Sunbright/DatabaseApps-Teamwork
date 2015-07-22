namespace DatabaseApps.Client.DbManagers
{
    using MsSql;

    public class MsSqlDBManager
    {
        private readonly MsSqlContext context;

        public MsSqlDBManager()
        {
            this.context = new MsSqlContext();
        }

        public MsSqlContext MsSqlContext
        {
            get
            {
                return this.context;
            }
        }
    }
}
