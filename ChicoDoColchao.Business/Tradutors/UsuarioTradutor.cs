using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class UsuarioTradutor
    {
        public static Usuario ToBd(this UsuarioDao usuarioDao)
        {
            Usuario usuario = new Usuario();

            usuario.UsuarioID = usuarioDao.UsuarioID;

            if (!string.IsNullOrEmpty(usuarioDao.Login))
            {
                usuario.Login = usuarioDao.Login.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(usuarioDao.Senha))
            {
                usuario.Senha = usuarioDao.Senha.Trim().ToLower();
            }

            usuario.Ativo = usuarioDao.Ativo;

            if (usuarioDao.TipoUsuarioDao != null)
            {
                usuario.TipoUsuario = new TipoUsuario();

                if (usuarioDao.TipoUsuarioDao.TipoUsuarioID > 0)
                {
                    usuario.TipoUsuario.TipoUsuarioID = usuarioDao.TipoUsuarioDao.TipoUsuarioID;
                }

                if (!string.IsNullOrEmpty(usuarioDao.TipoUsuarioDao.Descricao))
                {
                    usuario.TipoUsuario.Descricao = usuarioDao.TipoUsuarioDao.Descricao;
                }
            }

            return usuario;
        }

        public static UsuarioDao ToApp(this Usuario usuario)
        {
            UsuarioDao usuarioDao = new UsuarioDao();

            usuarioDao.UsuarioID = usuario.UsuarioID;
            usuarioDao.Login = usuario.Login;
            usuarioDao.Senha = usuario.Senha;
            usuarioDao.Ativo = usuario.Ativo;

            if (usuario.TipoUsuario != null)
            {
                usuarioDao.TipoUsuarioDao.TipoUsuarioID = usuario.TipoUsuario.TipoUsuarioID;
                usuarioDao.TipoUsuarioDao.Descricao = usuario.TipoUsuario.Descricao;
            }

            return usuarioDao;
        }
    }
}
