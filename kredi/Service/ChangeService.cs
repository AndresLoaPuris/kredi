using HtmlAgilityPack;
using kredi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kredi.Service
{
	public class ChangeService
	{

        public float extractNumbers(string number)
        {
            string result = string.Empty;
            for (int i = 0; i < number.Length; i++)
            {
                if (Char.IsDigit(number[i]))
                    result += number[i];
            }

            result = (result.Length > 0) ? result : "0";
            return float.Parse(result);
        }

        public Fintech getData(string url, string bancoCompra, string bancoVende, string Nombre)
        {

            List<Fintech> fintechs = new List<Fintech>();
            Fintech fintech = new Fintech();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            //SACAMOS INFORMACION DE LA PAGINA WEB Y LO GUARDAMOS EN VARIABLES DE TIPO HTMLNODE 
            List<HtmlNode> Nodes_BC = doc.DocumentNode.SelectNodes(bancoCompra).ToList();
            List<HtmlNode> Nodes_BV = doc.DocumentNode.SelectNodes(bancoVende).ToList();


            float _bc = 0;
            float _bv = 0;




            for (int i = 0; i < Nodes_BC.Count(); i++)
            {

                if (Nodes_BC[i].InnerText.ToString().Trim() != "Compra" && Nodes_BC[i].InnerText.ToString().Trim() != "")
                {
                    _bc = extractNumbers(Nodes_BC[i].InnerText.ToString().Trim());
                    fintech.buy = _bc / 10000;
                }

                if (Nodes_BV[i].InnerText.ToString().Trim() != "Venta" && Nodes_BV[i].InnerText.ToString().Trim() != "")
                {

                    _bv = extractNumbers(Nodes_BV[i].InnerText.ToString().Trim());
                    fintech.sale = _bv / 10000;
                }

                fintechs.Add(fintech);
            }
            fintech.name = Nombre;
            //Console.WriteLine("Nombre): " + fintech.name + " " + fintech.buy + " " + fintech.sale + " ");
            return fintech;

        }

    }
}