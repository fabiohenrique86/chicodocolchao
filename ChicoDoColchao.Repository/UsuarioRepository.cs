using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class UsuarioRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public UsuarioRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public List<Usuario> Listar(Usuario usuario)
        {
            IQueryable<Usuario> query = chicoDoColchaoEntities.Usuario;

            if (usuario.UsuarioID > 0)
            {
                query = query.Where(x => x.UsuarioID == usuario.UsuarioID);
            }

            if (!string.IsNullOrEmpty(usuario.Login))
            {
                query = query.Where(x => x.Login.Equals(usuario.Login));
            }

            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                query = query.Where(x => x.Senha.Equals(usuario.Senha));
            }

            query = query.Where(x => x.Ativo);

            return query.Include(x => x.TipoUsuario).OrderBy(x => x.Login).ToList();
        }

        public void AlterarSenha(Usuario usuario)
        {
            chicoDoColchaoEntities.Entry(usuario).State = EntityState.Modified;
            chicoDoColchaoEntities.SaveChanges();
        }
    }
}
