using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary3;
using ClassLibrary3.DTO;
using System.Web.Http.Cors;

namespace WebApplication1.Controllers
{
    public class DefaultController : ApiController
    {

        // GET: api/Default
        public List<RecipesDTO> Get()
        {

            List<Recipes> tochek = new List<Recipes>();
            Kitchen1Dbcontext db = new Kitchen1Dbcontext();
            List<Recipes> recilist = db.Recipes.Include(x => x.Ingredients).ToList();
            List<RecipesDTO> reclistdto = new List<RecipesDTO>();
            Kitchen1Dbcontext recandingr = new Kitchen1Dbcontext();
            foreach (Recipes recipes in recilist)
            {
                recipes.calories = 0;
                RecipesDTO recipe = new RecipesDTO();
                recipe.id = recipes.Id;
                recipe.name = recipes.Name;
                recipe.imageurl = recipes.Image;
                recipe.Time = recipes.Time;
                recipe.cookingmethod = recipes.CookingMethod;



                foreach (Ingredients ingerd in recipes.Ingredients)
                {

                    recipes.calories += ingerd.Calories;


                    IngerdientDTO ingerdto = new IngerdientDTO();
                    ingerdto.id = ingerd.Id;
                    ingerdto.name = ingerd.Name;
                    ingerdto.imageurl = ingerd.Image;
                    ingerdto.calories = ingerd.Calories;
                    recipe.ingerdients.Add(ingerdto);

                }

                recipe.calories = recipes.calories;
                reclistdto.Add(recipe);



            }


            return reclistdto;

        }
        // GET: api/Default/5

        [HttpGet]
        [Route("api/Default/ingerd")]
        public List<IngerdientDTO> Get2()
        {
            Kitchen1Dbcontext db = new Kitchen1Dbcontext();
            List<IngerdientDTO> ingerdtolist = new List<IngerdientDTO>();
            List<Ingredients> ingerdlist = db.Ingredients.ToList();
            foreach (Ingredients inger in ingerdlist)
            {
                IngerdientDTO ingerdto = new IngerdientDTO();
                ingerdto.id = inger.Id;
                ingerdto.name = inger.Name;
                ingerdto.imageurl = inger.Image;
                ingerdto.calories = inger.Calories;
                ingerdtolist.Add(ingerdto);

            }

            return ingerdtolist;
        }
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default/ingerd
        [HttpPost]
        [Route("api/Default/ingerd")]
        public HttpStatusCode Posting([FromBody] IngerdientDTO value)
        {
            try
            {
                Kitchen1Dbcontext db = new Kitchen1Dbcontext();
                Ingredients ingerdient = new Ingredients();
                int ingerid = db.Recipes.OrderByDescending(x => x.Id).First().Id + 1;
                ingerdient.Id = ingerid;
                ingerdient.Name = value.name;
                ingerdient.Image = value.imageurl;
                ingerdient.Calories = value.calories;
                db.Ingredients.Add(ingerdient);
                db.SaveChanges();
                return HttpStatusCode.OK;

            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }

        }

        [HttpPost]
        [Route("api/Default/recipe")]
        public HttpStatusCode Postrec([FromBody] RecipesDTO value)
        {
            try
            {
                Kitchen1Dbcontext db = new Kitchen1Dbcontext();
                Recipes newrec = new Recipes();
                int numberlastrec = db.Recipes.OrderByDescending(x => x.Id).First().Id + 1;
                newrec.Id = numberlastrec;
                newrec.Name = value.name;
                newrec.Image = value.imageurl;
                newrec.CookingMethod = value.cookingmethod;
                newrec.calories = 0;
                newrec.Time = value.Time;
                foreach (IngerdientDTO item in value.ingerdients)
                {
                    Ingredients ing = db.Ingredients.Where(x => x.Id == item.id).FirstOrDefault();
                    if (ing != null)
                    {
                        newrec.calories += ing.Calories;
                        ing.Recipes.Add(newrec);
                    }
                }

                db.Recipes.Add(newrec);
                db.SaveChanges();
                return HttpStatusCode.OK;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }



        }






        // DELETE: api/Default/deleterec/{id}
        [Route("api/Default/deleterec/{id}")]
        [HttpDelete]
        public HttpStatusCode Deleterec(int id)
        {
            try
            {
                Kitchen1Dbcontext db = new Kitchen1Dbcontext();
                Recipes rectodel = db.Recipes.Where(x => x.Id == id).First();
                List<Ingredients> ingtlist = db.Ingredients.ToList();
                for (int i = 0; i < ingtlist.Count(); i++)
                {

                    if (ingtlist[i].Recipes.Contains(rectodel))
                    {
                        ingtlist[i].Recipes.Remove(rectodel);
                    }


                }






                db.Recipes.Remove(rectodel);
                db.SaveChanges();
                return HttpStatusCode.OK;



            }
            catch
            {

                return HttpStatusCode.BadRequest;
            }



        }
    }
}
