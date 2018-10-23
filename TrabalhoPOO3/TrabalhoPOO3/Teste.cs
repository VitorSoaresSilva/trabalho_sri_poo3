using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestesCSharp
{
    class Teste
    {

        static void Main(string[] args)
        {
            //pego os arquivos txt originais
            string[] arrayArquivosOriginais = System.IO.Directory.GetFiles(@"C:\Users\Lekrieg\source\repos\TestesCSharp\TestesCSharp\bin\Debug\ArtigosEmTXT");
            //Passo ele para o CriaResumo que retorna o diretorio com os arquivos de resumo
            CriaResumo(arrayArquivosOriginais);

            //Passa o a localizacao dos arquivos de serumo para criar as listas de palavras
            string[] arrayArquivosResumo = System.IO.Directory.GetFiles(@"C:\Users\Lekrieg\source\repos\TestesCSharp\TestesCSharp\bin\Debug\Resumos");
            CriaListaPalavras(arrayArquivosResumo);

            //Passo a o diretorio da lista de palavras para retirar as stopwords
            //RetirarStopWord(arrayArquivosListaPalavras);

            Console.Read();
        }

        static void CriaResumo(string[] arrayArquivosOriginais)
        {
            foreach (string nomeArquivo in arrayArquivosOriginais)
            {
                System.IO.Stream entradaArquivo = System.IO.File.OpenRead(nomeArquivo);
                System.IO.StreamReader leitor = new System.IO.StreamReader(entradaArquivo);

                try
                {
                    //separo as palavras por espaco para poder tratar e pegar apenas o resumo
                    string conteudo = leitor.ReadLine();

                    string diretorioNovo = @"C:\Users\Lekrieg\source\repos\TestesCSharp\TestesCSharp\bin\Debug\Resumos";
                    //cria a pasta no diretorio, ou o diretorio, deve dar na mesma
                    System.IO.Directory.CreateDirectory(diretorioNovo);
                    System.IO.Stream textoAlvo = System.IO.File.Open(System.IO.Path.Combine(diretorioNovo, "resumo_" + System.IO.Path.GetFileNameWithoutExtension(nomeArquivo) +
                        ".txt"), System.IO.FileMode.OpenOrCreate);
                    System.IO.StreamWriter escritor = new System.IO.StreamWriter(textoAlvo);

                    while (conteudo != null)
                    {
                        if (conteudo.Contains("RESUMO:") || conteudo.Contains("RESUMO"))
                        {
                            conteudo = leitor.ReadLine();
                            while (conteudo != null)
                            {
                                if (conteudo.Contains("PALAVRAS-CHAVE"))
                                {
                                    conteudo = leitor.ReadLine();
                                }

                                char[] charAux;
                                if (conteudo != null)
                                {
                                    charAux = conteudo.ToArray();
                                    for (int i = 0; i < charAux.Length; i++)
                                    {
                                        if ((char.IsPunctuation(charAux[i]) || char.IsSymbol(charAux[i]) ||
                                            char.IsWhiteSpace(charAux[i])) && charAux[i] != '-')
                                        {
                                            escritor.Write(' ');
                                        }
                                        else
                                        {
                                            escritor.Write(charAux[i]);
                                        }
                                    }
                                }
                                //escritor.WriteLine(conteudoAux);
                                conteudo = leitor.ReadLine();

                            }
                        }
                        conteudo = leitor.ReadLine();

                    }

                    escritor.Close();
                    textoAlvo.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao pegar dados: " + ex.Message);
                }
                finally
                {
                    leitor.Close();
                    entradaArquivo.Close();
                }
            }
        }

        static void CriaListaPalavras(string[] arrayArquivosResumo)
        {

            foreach (string nomeArquivo in arrayArquivosResumo)
            {
                System.IO.Stream entradaArquivo = System.IO.File.OpenRead(nomeArquivo);
                System.IO.StreamReader leitor = new System.IO.StreamReader(entradaArquivo);

                try
                {
                    string diretorioNovo = @"C:\Users\Lekrieg\source\repos\TestesCSharp\TestesCSharp\bin\Debug\ListaPalavras";
                    //cria a pasta no diretorio, ou o diretorio, deve dar na mesma
                    System.IO.Directory.CreateDirectory(diretorioNovo);
                    System.IO.Stream textoAlvo = System.IO.File.Open(System.IO.Path.Combine(diretorioNovo, "lista_" + System.IO.Path.GetFileNameWithoutExtension(nomeArquivo) +
                        ".txt"), System.IO.FileMode.OpenOrCreate);
                    System.IO.StreamWriter escritor = new System.IO.StreamWriter(textoAlvo);

                    //separo as palavras por espaco para poder tratar e pegar apenas o resumo
                    string[] conteudo = leitor.ReadToEnd().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    //DICIONARIO--------------------
                    System.Collections.Specialized.OrderedDictionary dict = new System.Collections.Specialized.OrderedDictionary();

                    foreach (var element in conteudo.Select((word, index) => new { word, index }))
                    {
                        if (dict.Contains(element.word))
                        {
                            int count = (int)dict[element.word];
                            dict[element.word] = ++count;
                        }
                        else
                        {
                            dict[element.word] = 1;
                        }
                    }
                    //--------------------

                    foreach (System.Collections.DictionaryEntry item in dict)
                    {
                        escritor.WriteLine(item.Key + " " + item.Value);
                    }

                    escritor.Close();
                    textoAlvo.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao pegar dados: " + ex.Message);
                }
                finally
                {
                    leitor.Close();
                    entradaArquivo.Close();
                }
            }

        }

    }
}
