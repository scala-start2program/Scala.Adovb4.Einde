using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.Adovb4.Core.Entities
{
    public class Menu
    {
        public string Id { get; private set; }
        public string Menunaam { get; set; }
        public string Ingredienten { get; set; }
        public string Bereidingswijze { get; set; }
        public int Bereidingstijd { get; set; }
        public string SoortId { get; set; }

        public Menu()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Menu(string menunaam, string ingredienten, string bereidingswijze,
            int bereidingstijd, string soortId)
        {
            Id = Guid.NewGuid().ToString();
            Menunaam = menunaam;
            Ingredienten = ingredienten;
            Bereidingswijze = bereidingswijze;
            Bereidingstijd = bereidingstijd;
            SoortId = soortId;
        }
        internal Menu(string id, string menunaam, string ingredienten, string bereidingswijze,
            int bereidingstijd, string soortId)
        {
            Id = id;
            Menunaam = menunaam;
            Ingredienten = ingredienten;
            Bereidingswijze = bereidingswijze;
            Bereidingstijd = bereidingstijd;
            SoortId = soortId;
        }
        public override string ToString()
        {
            return $"{Menunaam} ({Bereidingstijd} min)";

        }
    }
}
