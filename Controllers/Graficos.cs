using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Data;
using System.Threading.Tasks;
using Fatec_Facilities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FusionCharts.Visualization;
using FusionCharts.DataEngine;
using Microsoft.Extensions.Logging;

namespace Fatec_Facilities.Controllers
{
    public class Graficos : Controller
    {
        private readonly Contexto _context;
        private readonly LimiteGastosController limiteGastos;

        private readonly ILogger<LimiteGastosController> _logger;

        public Graficos(Contexto context, ILogger<LimiteGastosController> logger)
        {
            _context = context;
            _logger = logger;
            
            limiteGastos = new LimiteGastosController(_context, _logger);
        }

        [Authorize]
        public IActionResult Index()
        {
            this.RenderOsByStatus();
            this.RenderOsByLocal();
            this.RenderOsGastobyFornecedor();
            this.RenderGastoManutencaoPredial();
            this.RenderGastosContaMensal();
            return View();
        }

        public void RenderOsByStatus(){
           

           var quant = (from _OSGestor in _context.Set<OSGestor>()
                           // where _OSGestor.StatusId == (i + 1)
                     join _Status in _context.Set<Status>() on _OSGestor.StatusId equals _Status.Id
                     group _Status by new { _Status.Descricao,_Status.Id } into total
                     select new { total.Key.Descricao ,total = total.Count() }).ToArray();


            DataTable ChartData = new DataTable();
            ChartData.Columns.Add("NomeStatus", typeof(System.String));
			ChartData.Columns.Add("Qtd", typeof(System.Int32));
            foreach(var item in quant)
            {
                // ChartData.Rows.Add(descStatus[i], quant[i]);
                ChartData.Rows.Add(item.Descricao, item.total);
            }
			StaticSource source = new StaticSource(ChartData);
            DataModel model = new DataModel();
            model.DataSources.Add(source);
            Charts.PieChart pie = new Charts.PieChart("count_status");
            pie.Data.Source = model;
            pie.Width.Percentage(100);
			pie.Height.Pixel(500); 
            pie.Caption.Text = "Quantidade de OS por status";
			pie.ThemeName = FusionChartsTheme.ThemeName.CANDY;
			ViewData["Chart_2"] = pie.Render();
        }
        
		public void RenderOsByLocal(){
            var osloc = (
                            from _OSGestor in _context.Set<OSGestor>()
                            join _Equipamento in _context.Set<Equipamento>() on _OSGestor.EquipamentoId equals _Equipamento.Id
                            join _Local in _context.Set<Local>() on _Equipamento.LocalId equals _Local.Id
                            // join _Bloco in _context.Set<Bloco>() on _Local.BlocoID equals _Bloco.Id
                            group _Local by new { _Local.Nome, _Local.Id } into total
                            select new { total.Key.Nome, total = total.Count()}
                        ).ToArray();
            DataTable ChartData = new DataTable();
            ChartData.Columns.Add("Local", typeof(System.String));
			ChartData.Columns.Add("Qtd", typeof(System.Double));
            foreach(var item in osloc){
				ChartData.Rows.Add(item.Nome, item.total);
            }
            StaticSource source = new StaticSource(ChartData);
            DataModel model = new DataModel();
            model.DataSources.Add(source);
            Charts.ColumnChart column = new Charts.ColumnChart("incidencia_local");
            column.Scrollable = true;
            column.Width.Percentage(100);
			column.Height.Pixel(500);
            column.Data.Source = model;
            column.Caption.Text = "Locais e seu total de OS";
			column.XAxis.Text = "Local";
			column.YAxis.Text = "Total";
			column.ThemeName = FusionChartsTheme.ThemeName.CANDY;
			ViewData["Chart_3"] = column.Render();
		}
		
		public void RenderOsGastobyFornecedor(){
            var osloc = (
                            from _OSGestor in _context.Set<OSGestor>()
                            join _Fornecedor in _context.Set<Fornecedor>() on _OSGestor.FornecedorId equals _Fornecedor.Id
                            group _OSGestor by new { _Fornecedor.Nome,_Fornecedor.Id } into gasto
                            select new { gasto.Key.Nome, gasto = gasto.Sum(p => p.Valor) }
                        ).ToArray();
                        
            var limite = limiteGastos.GetLimiteGastos();

            DataTable ChartData = new DataTable();
            ChartData.Columns.Add("Local", typeof(System.String));
			ChartData.Columns.Add("Qtd", typeof(System.Double));
			ChartData.Columns.Add("Limite", typeof(System.Double));
            foreach(var item in osloc){
				ChartData.Rows.Add(item.Nome, item.gasto, limite.Limite);
            }
            StaticSource source = new StaticSource(ChartData);
            DataModel model = new DataModel();
            model.DataSources.Add(source);
            Charts.CombinationChart combiChart = new Charts.CombinationChart("gasto_fornecedor");
            combiChart.Scrollable = true;
            combiChart.Data.Source = model;
            combiChart.Data.ColumnPlots("Qtd");
            combiChart.Data.SplinePlots("Limite");
            combiChart.Width.Percentage(100);
			combiChart.Height.Pixel(500);
            combiChart.Caption.Text = "Gastos com terceiros em equipamentos";
			combiChart.XAxis.Text = "Terceiros";
			combiChart.PrimaryYAxis.Text = "Gastos";
			combiChart.ThemeName = FusionChartsTheme.ThemeName.CANDY;
			ViewData["Chart_4"] = combiChart.Render();
		}

        public void RenderGastoManutencaoPredial(){
            var manGasPredial = from _ManPredial in _context.Set<ManutencaoPredial>()
                           join _CatManPredial in _context.Set<CategoriaManutencaoPredial>() on _ManPredial.CategoriaManutencaoPredialId equals _CatManPredial.Id
                           group _ManPredial by new { _CatManPredial.Nome } into gasto
                           select new { gasto.Key.Nome, gasto = gasto.Sum(p => p.Valor) };
                             
            DataTable ChartData = new DataTable();
            ChartData.Columns.Add("Categoria", typeof(System.String));
			ChartData.Columns.Add("Gastos", typeof(System.Double));
            foreach(var item in manGasPredial){
				ChartData.Rows.Add(item.Nome, item.gasto);
            }
            StaticSource source = new StaticSource(ChartData);
            DataModel model = new DataModel();
            model.DataSources.Add(source);
            Charts.BarChart combiChart = new Charts.BarChart("gasto_manpredial");
            combiChart.Data.Source = model;
            combiChart.Width.Percentage(100);
			combiChart.Height.Pixel(500);
            combiChart.Caption.Text = "Gastos com manutenção predial";
			combiChart.XAxis.Text = "Categoria";
			combiChart.YAxis.Text = "Gastos";
			combiChart.ThemeName = FusionChartsTheme.ThemeName.CANDY;
			ViewData["Chart_5"] = combiChart.Render();

        }

        public void RenderGastosContaMensal(){
            var manGasPredial = from _ContaMensal in _context.Set<ContaMensal>()
                           join _CatConMensal in _context.Set<CategoriaContaMensal>() on _ContaMensal.CategoriaContaMensalId equals _CatConMensal.Id
                           group _ContaMensal by new { _CatConMensal.Id, _CatConMensal.Descricao } into gasto
                           select new { gasto.Key.Descricao, gasto = gasto.Sum(p => p.Valor) };
                             
            DataTable ChartData = new DataTable();
            ChartData.Columns.Add("Contas", typeof(System.String));
			ChartData.Columns.Add("Gastos", typeof(System.Double));
            foreach(var item in manGasPredial){
				ChartData.Rows.Add(item.Descricao, item.gasto);
            }
            StaticSource source = new StaticSource(ChartData);
            DataModel model = new DataModel();
            model.DataSources.Add(source);
            Charts.ColumnChart combiChart = new Charts.ColumnChart("gasto_conmensal");
            combiChart.Scrollable = true;
            combiChart.Data.Source = model;
            combiChart.Width.Percentage(100);
			combiChart.Height.Pixel(500);
            combiChart.Caption.Text = "Gastos com as contas mensais";
			combiChart.XAxis.Text = "Contas";
			combiChart.YAxis.Text = "Gastos";
			combiChart.ThemeName = FusionChartsTheme.ThemeName.CANDY;
			ViewData["Chart_6"] = combiChart.Render();

        }
    
    
    }
}

