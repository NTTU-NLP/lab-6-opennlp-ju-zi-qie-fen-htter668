using java.io;
using opennlp.tools.tokenize;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace HW6
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] file = Directory.GetFiles(@"..\..\..\..\Dataset\", "*.html");
            StreamWriter sw = new StreamWriter(@"..\..\..\Html.txt");
            foreach (String files in file)
                using (StreamReader sr = new StreamReader(files))
                {
                    while (sr.Peek() != -1)
                    {
                        string line = sr.ReadLine();
                        string pattern = @"(<P>|<BR />|<DIV>)";
                        string replacement = "";
                        Regex rgx = new Regex(pattern);
                        line = rgx.Replace(line, replacement);
                        pattern = @"(</P>|<p/>|</DIV>)";
                        replacement = "";
                        rgx = new Regex(pattern);
                        line = rgx.Replace(line, replacement);
                        string regFind = @"<a.*?>" + @"(?'text'.*?)" + @"</a>";
                        string regReplace = @"${text}";
                        string regFindimg = @"<img.*?title=""" + @"(?'text2'.*?)" + @""" />";
                        string regReplaceimg = @"${text2}";
                        line = Regex.Replace(line, @"&nbsp;", "");
                        line = Regex.Replace(line, regFind, regReplace);

                        String[] tokens;
                        InputStream modelIn = new FileInputStream(@"..\..\..\..\en-token.bin");
                        TokenizerModel model = new TokenizerModel(modelIn);
                        TokenizerME enTokenizer = new TokenizerME(model);
                        tokens = enTokenizer.tokenize(Regex.Replace(line, regFindimg, regReplaceimg));
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            sw.Write(tokens[i] + " ");
                            if (tokens[i].Equals("."))
                            {
                                sw.Write("\n");
                            }
                        }
                    }
                }
            sw.Close();
        }
    }
}
