using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.CoreModal.DTO;
using RPG.Models.RulebookModal.AbstractClasses;

namespace RPG.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Home");
        }

        [HttpGet]
        public ActionResult GetAbilities(string searchTerm, int pageSize, int pageNum)
        {
            /*List<ElementId> elms = ar.GetAttendees(searchTerm, pageSize, pageNum);
            int elmsCount = ar.GetAttendeesCount(searchTerm, pageSize, pageNum);

            //Translate the elms into a format the select2 dropdown expects
            Select2PagedResult pagedAttendees = AttendeesToSelect2Format(elms, elmsCount);*/

            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = new Select2PagedResult
                {
                    Results = new List<Select2Result>(new []{ new Select2Result
                    {
                        id = Guid.NewGuid().ToString(),
                        text = "test"
                    }, }),
                    Total = 1
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        private Select2PagedResult AttendeesToSelect2Format(List<ElementId> elms, int totalElms)
        {
            Select2PagedResult jsonElms = new Select2PagedResult();
            jsonElms.Results = new List<Select2Result>();

            //Loop through our elms and translate it into a text value and an id for the select list
            foreach (ElementId a in elms)
            {
                jsonElms.Results.Add(new Select2Result { id = a.ID.ToString(), text = a.Name });
            }
            //Set the total count of the results from the query.
            jsonElms.Total = totalElms;

            return jsonElms;
        }
    }
    //Extra classes to format the results the way the select2 dropdown wants them
    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2Result> Results { get; set; }
    }

    public class Select2Result
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}
