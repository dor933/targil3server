using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary3.DTO
{
    public class RecipesDTO
    {
        public int id;
        public string name;
        public string imageurl;
        public Nullable<int> calories;
        public string cookingmethod;
        public List<IngerdientDTO> ingerdients = new List<IngerdientDTO>();
        public Nullable<int> Time;


    }
}
