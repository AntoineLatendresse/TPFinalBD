using MainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TPFinalBD.Controllers
{
    public class ProgrammeController : Controller
    {
        public ActionResult Ajouter()
        {
            return View(new Programme());
        }

        public ActionResult Lister()
        {
            Programmes programmes = new Programmes(Session["DB_PROGRAMME"]);

            String orderBy = "";
            if (Session["Programme_SortBy"] != null)
                orderBy = (String)Session["Programme_SortBy"] + " " + (String)Session["Programme_SortOrder"];

            programmes.SelectAll(orderBy);
            return View(programmes.ToList());
        }

        public ActionResult Effacer(String Id)
        {
            Programmes programmes = new Programmes(Session["DB_PROGRAMME"]);
            programmes.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "Programme");
        }


        public ActionResult Editer(String Id)
        {
            Programmes programmes = new Programmes(Session["DB_PROGRAMME"]);
            if (programmes.SelectByID(Id))
                return View(programmes.programme);
            else
                return RedirectToAction("Lister", "Programme");
        }
	}
}