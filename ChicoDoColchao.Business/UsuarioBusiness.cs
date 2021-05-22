using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class UsuarioBusiness
    {
        UsuarioRepository usuarioRepository;
        LogRepository logRepository;

        public UsuarioBusiness()
        {
            usuarioRepository = new UsuarioRepository();
            logRepository = new LogRepository();
        }

        private void ValidarAlterarSenha(UsuarioDao usuarioDao, out Usuario usuario)
        {
            if (usuarioDao == null)
            {
                throw new BusinessException("Usuário é obrigatório");
            }

            if (usuarioDao.UsuarioID <= 0)
            {
                throw new BusinessException("UsuarioID é obrigatório");
            }

            if (string.IsNullOrEmpty(usuarioDao.Senha))
            {
                throw new BusinessException("Senha é obrigatório");
            }

            usuario = usuarioRepository.Listar(new Usuario() { UsuarioID = usuarioDao.UsuarioID }).FirstOrDefault();

            if (usuario == null)
            {
                throw new BusinessException(string.Format("Usuário {0} não encontrado", usuarioDao.UsuarioID));
            }
        }

        private void ValidarIncluir(UsuarioDao usuarioDao)
        {
            if (usuarioDao == null)
            {
                throw new BusinessException("Usuário é obrigatório");
            }

            if (usuarioDao.UsuarioID <= 0)
            {
                throw new BusinessException("UsuarioID é obrigatório");
            }

            if (string.IsNullOrEmpty(usuarioDao.Login))
            {
                throw new BusinessException("Login é obrigatório");
            }

            if (string.IsNullOrEmpty(usuarioDao.Senha))
            {
                throw new BusinessException("Senha é obrigatório");
            }

            if (usuarioDao.TipoUsuarioID <= 0)
            {
                throw new BusinessException("TipoUsuarioID é obrigatório");
            }

            var usuario = usuarioRepository.Listar(new Usuario() { Login = usuarioDao.Login }).FirstOrDefault();

            if (usuario != null)
            {
                throw new BusinessException(string.Format("Usuário {0} já cadastrado", usuarioDao.Login));
            }
        }

        private void ValidarLogin(UsuarioDao usuarioDao)
        {
            if (usuarioDao == null)
            {
                throw new BusinessException("Usuário é obrigatório");
            }

            if (string.IsNullOrEmpty(usuarioDao.Login))
            {
                throw new BusinessException("Login é obrigatório");
            }

            if (string.IsNullOrEmpty(usuarioDao.Senha))
            {
                throw new BusinessException("Senha é obrigatório");
            }
        }

        public List<UsuarioDao> Login(UsuarioDao usuarioDao)
        {
            try
            {
                ValidarLogin(usuarioDao);

                return usuarioRepository.Listar(usuarioDao.ToBd()).Select(x => x.ToApp()).ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public void AlterarSenha(UsuarioDao usuarioDao)
        {
            try
            {
                Usuario usuario;

                ValidarAlterarSenha(usuarioDao, out usuario);

                usuario.Senha = usuarioDao.Senha;

                usuarioRepository.AlterarSenha(usuario);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public void Incluir(UsuarioDao usuarioDao)
        {
            try
            {
                ValidarIncluir(usuarioDao);
                
                usuarioRepository.Incluir(new Usuario() { UsuarioID = usuarioDao.UsuarioID, Login = usuarioDao.Login, Senha = usuarioDao.Senha, Ativo = usuarioDao.Ativo, TipoUsuarioID = usuarioDao.TipoUsuarioID });
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public List<UsuarioDao> Listar(UsuarioDao usuarioDao)
        {
            try
            {
                return usuarioRepository.Listar(usuarioDao.ToBd()).Select(x => x.ToApp()).ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }
    }
}
