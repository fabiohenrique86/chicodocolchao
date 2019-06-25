using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business
{
    public class ColchaoIdealBusiness
    {
        LogRepository logRepository;

        public ColchaoIdealBusiness()
        {
            logRepository = new LogRepository();
        }

        private void ValidarVerificar(ColchaoIdealDao colchaoIdealDao)
        {
            if (colchaoIdealDao == null)
            {
                throw new BusinessException("Colchao Ideal é obrigatório");
            }
            
            if (string.IsNullOrEmpty(colchaoIdealDao.Altura))
            {
                throw new BusinessException("Altura é obrigatório");
            }

            if (string.IsNullOrEmpty(colchaoIdealDao.Peso))
            {
                throw new BusinessException("Peso é obrigatório");
            }            
        }
        
        public string Verificar(ColchaoIdealDao colchaoIdealDao)
        {
            string retorno = "";

            try
            {
                
                ValidarVerificar(colchaoIdealDao);

                short peso = Convert.ToInt16(colchaoIdealDao.Peso);
                double altura = Convert.ToDouble(colchaoIdealDao.Altura.Replace(".",","));

                if (peso <= 50)
                {
                    if (altura < 1.5)
                    {
                        retorno = "D23";
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = "D23*/20";
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = "D23*/20";
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D20";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                }
                else if (peso >= 51 && peso <= 60)
                {
                    if (altura < 1.5)
                    {
                        retorno = "D26";
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = "D26*/23";
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = "D26*/23";
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D23";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                }
                else if (peso >= 61 && peso <= 70)
                {
                    if (altura < 1.5)
                    {
                        retorno = "D28";
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = "D26*/28";
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = "D26*/28";
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D26*/28";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = "D26";
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                }
                else if (peso >= 71 && peso <= 80)
                {
                    if (altura < 1.5)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = "D33";
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = "D28*/33";
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D28*/33";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = "D28";
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                }
                else if (peso >= 81 && peso <= 90)
                {
                    if (altura < 1.5)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = "D33";
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D33*/28";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = "D33*/28";
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = "D28";
                    }
                }
                else if (peso >= 91 && peso <= 100)
                {
                    if (altura < 1.5)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = "D40";
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D40*/33";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = "D33";
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = "D33";
                    }
                }
                else if (peso >= 101 && peso <= 120)
                {
                    if (altura < 1.5)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = "D45";
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D40";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = "D40";
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = "D40/33*";
                    }
                }
                else if (peso >= 121 && peso <= 150)
                {
                    if (altura < 1.5)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.51 && altura <= 1.6)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.61 && altura <= 1.7)
                    {
                        retorno = string.Format("Padrão não cadastrado para as medidas: Peso {0} e Altura {1}", peso, altura);
                    }
                    else if (altura >= 1.71 && altura <= 1.8)
                    {
                        retorno = "D45";
                    }
                    else if (altura >= 1.81 && altura <= 1.9)
                    {
                        retorno = "D45*/40";
                    }
                    else if (altura >= 1.9)
                    {
                        retorno = "D40";
                    }
                }

                return retorno;
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
