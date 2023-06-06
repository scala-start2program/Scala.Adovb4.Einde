using Scala.Adovb4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.Adovb4.Core.Services
{
    public class MenuService
    {
        public List<Soort> GetSoorten()
        {
            string sql;
            sql = "select * from soorten order by soortnaam";
            List<Soort> soorten = new List<Soort>();
            DataTable dt = DBServices.ExecuteSelect(sql);
            foreach(DataRow dr in dt.Rows)
            {
                string id = dr["id"].ToString();
                string soortnaam = dr["soortnaam"].ToString();
                soorten.Add(new Soort(id, soortnaam));
            }
            return soorten;
        }
        public List<Menu> GetMenus(string soortIdFilter = null)
        {
            string sql;
            sql = "select * from menus ";
            if (soortIdFilter != null)
                sql += $" where soortid = '{soortIdFilter}' ";
            sql += " order by menunaam ";
            List<Menu> menus = new List<Menu>();
            DataTable dt = DBServices.ExecuteSelect(sql);
            foreach(DataRow dr in dt.Rows)
            {
                string id = dr["id"].ToString();
                string menunaam = dr["menunaam"].ToString();
                string ingredienten = dr["ingredienten"].ToString();
                string bereidingswijze = dr["bereidingswijze"].ToString();
                int bereidingstijd = int.Parse(dr["bereidingstijd"].ToString());
                string soortId = dr["soortid"].ToString();
                menus.Add(new Menu(id, menunaam, ingredienten, bereidingswijze,
                    bereidingstijd, soortId));
            }
            return menus;
        }
        public bool MenuToevoegen(Menu menu)
        {
            string sql;
            sql = "insert into menus ";
            sql += " (id, menunaam, ingredienten, bereidingswijze, bereidingstijd, soortid) ";
            sql += " values (";
            sql += $" '{menu.Id}' , ";
            sql += $" '{Helper.HandleQuotes(menu.Menunaam)}' , ";
            sql += $" '{Helper.HandleQuotes(menu.Ingredienten)}' , ";
            sql += $" '{Helper.HandleQuotes(menu.Bereidingswijze)}' , ";
            sql += $" {menu.Bereidingstijd} , ";
            sql += $" '{menu.SoortId}'  )";
            return DBServices.ExecuteCommand(sql);

        }
        public bool MenuWijzigen(Menu menu)
        {
            string sql;
            sql = "update menus set ";
            sql += $" menunaam = '{Helper.HandleQuotes(menu.Menunaam)}' , ";
            sql += $" ingredienten = '{Helper.HandleQuotes(menu.Menunaam)}' , ";
            sql += $" bereidingswijze = '{Helper.HandleQuotes(menu.Bereidingswijze)}' , ";
            sql += $" bereidingstijd = {menu.Bereidingstijd} , ";
            sql += $" soortid = '{menu.SoortId}' ";
            sql += $" where id = '{menu.Id}'";
            return DBServices.ExecuteCommand(sql);
        }
        public bool MenuVerwijderen(Menu menu)
        {
            string sql;
            sql = "delete from menus ";
            sql += $" where id = '{menu.Id}' ";
            return DBServices.ExecuteCommand(sql);
        }
    }
}
