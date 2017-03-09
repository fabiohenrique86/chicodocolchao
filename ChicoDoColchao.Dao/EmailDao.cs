using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ChicoDoColchao.Dao
{
    public class EmailDao
    {
        public EmailDao()
        {
            Anexo = new List<Attachment>();
        }

        public string Titulo { get; set; }
        public string Assunto { get; set; }
        public string Remetente { get; set; }
        public string Destinatario { get; set; }
        public string Mensagem { get; set; }
        public List<Attachment> Anexo { get; set; }
    }
}
