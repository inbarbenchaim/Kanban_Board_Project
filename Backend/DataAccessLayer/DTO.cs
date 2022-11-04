namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DTO
    {
        public const string IDColumnName = "ID";
        protected AbstractMapper mapper;
        protected DTO(AbstractMapper mapper)
        {
            this.mapper = mapper;
        }

    }
}