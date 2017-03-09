using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class LogRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public LogRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(Log log)
        {
            chicoDoColchaoEntities.Entry(log).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();
        }
    }
}
